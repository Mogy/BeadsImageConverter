using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace BeadsImageConverter
{
    public partial class FormMain : Form
    {
        private const int ERROR_CANCEL = 0;
        private const int ERROR_LOAD_PALLET = -1;
        private const int ERROR_CREATE_WORKBOOK = -2;
        private const int ERROR_CREATE_RESULT = -3;

        private string FileName;
        private string PalletName;
        private Bitmap Image;
        private Dictionary<string, string> PalletMap;
        private Dictionary<Color, int> ColorCashe;
        private List<Bead> Pallet;
        private IProgress<int> Progress;
        private int ProgressCount;
        private CancellationTokenSource Cancel;

        public FormMain()
        {
            InitializeComponent();
            PalletMap = new Dictionary<string, string>();
            ColorCashe = new Dictionary<Color, int>();
            Pallet = new List<Bead>();
            Progress = new Progress<int>(new Action<int>(showProgress));
            AssemblyName name = Assembly.GetExecutingAssembly().GetName();
            Text = string.Format("{0} v{1}", name.Name, name.Version);
            pbImage.AllowDrop = true;
            loadPalletName();
        }

        private void pbImage_Click(object sender, EventArgs e)
        {
            loadImage();
        }

        private void pbImage_DragEnter(object sender, DragEventArgs e)
        {
            // 単一の画像ファイルのみD&Dを許可する
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fileName = (string[])e.Data.GetData(DataFormats.FileDrop);
                string ext = Path.GetExtension(fileName[0]);
                if (fileName.Length == 1 && (ext == ".png" || ext == ".bmp" || ext == ".gif"))
                {
                    e.Effect = DragDropEffects.Move;
                    return;
                }
            }
            e.Effect = DragDropEffects.None;
        }

        private void pbImage_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileName = (string[])e.Data.GetData(DataFormats.FileDrop);
            showPreview(fileName[0]);
        }

        private void lbLoad_Click(object sender, EventArgs e)
        {
            pbImage_Click(sender, e);
        }

        private void lbLoad_DragEnter(object sender, DragEventArgs e)
        {
            pbImage_DragEnter(sender, e);
        }

        private void lbLoad_DragDrop(object sender, DragEventArgs e)
        {
            pbImage_DragDrop(sender, e);
        }

        private void cbConvert_CheckedChanged(object sender, EventArgs e)
        {
            if (cbConvert.Checked)
            {
                cbConvert.Text = "キャンセル";
                convert(cbPallet.Text);
            }
            else
            {
                cbConvert.Text = "変換";
                if (Cancel != null)
                {
                    Cancel.Cancel();
                }
            }
        }

        /// <summary>
        ///     パレット名一覧を読み込む
        /// </summary>
        private void loadPalletName()
        {
            try
            {
                string[] files = Directory.GetFiles(".\\pallet", "*.plt", SearchOption.TopDirectoryOnly);
                for (int i = 0; i < files.Length; i++)
                {
                    string fileName = files[i];
                    string palletName = getPalletName(fileName);
                    if (palletName != null)
                    {
                        cbPallet.Items.Add(palletName);
                        PalletMap.Add(palletName, fileName);
                    }
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (cbPallet.Items.Count == 0)
            {
                cbPallet.Items.Add("パレットが見つかりません");
                cbPallet.Enabled = false;
            }
            cbPallet.SelectedIndex = 0;
        }

        /// <summary>
        ///     パレット名を取得する
        /// </summary>
        /// <param name="fileName">ファイルパス</param>
        /// <returns>パレット名</returns>
        private string getPalletName(string fileName)
        {
            string ret = null;
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    ret = sr.ReadLine();
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ret;
        }

        /// <summary>
        ///     画像ファイルを読み込む
        /// </summary>
        private void loadImage()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "画像 ファイル(*.bmp, *.png, *.gif)|*.bmp;*.png;*.gif";
                ofd.FilterIndex = 2;
                if (FileName != null)
                {
                    ofd.InitialDirectory = FileName;
                }
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    showPreview(ofd.FileName);
                }
            }
        }

        /// <summary>
        ///     プログレスバーを更新する
        /// </summary>
        /// <param name="progress">更新値</param>
        private void showProgress(int progress)
        {
            if (0 < progress && progress <= pbWork.Maximum)
            {
                pbWork.Value = progress;
            }
            else
            {
                switch (progress)
                {
                    case ERROR_CANCEL:
                        MessageBox.Show(this, "変換をキャンセルしました", "キャンセル", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case ERROR_LOAD_PALLET:
                        MessageBox.Show(this, "パレットの読込に失敗しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case ERROR_CREATE_WORKBOOK:
                        MessageBox.Show(this, "ワークブックの作成に失敗しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case ERROR_CREATE_RESULT:
                        MessageBox.Show(this, "リザルトの作成に失敗しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    default:
                        MessageBox.Show(this, "変換が完了しました", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }
                pbWork.Value = 0;
                cbConvert.Checked = false;
            }
        }

        /// <summary>
        ///     プレビューを表示する
        /// </summary>
        /// <param name="fileName">ファイルパス</param>
        private void showPreview(string fileName)
        {
            if (!File.Exists(fileName))
            {
                MessageBox.Show("ファイルが存在しません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            FileName = fileName;
            lbLoad.Visible = false;
            Bitmap bitmap = new Bitmap(pbImage.Width, pbImage.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                Image = new Bitmap(FileName);
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.DrawImage(Image, 0, 0, bitmap.Width, bitmap.Height);
            }
            pbImage.Image = bitmap;
            cbConvert.Enabled = cbPallet.Enabled;
        }

        /// <summary>
        ///     画像ファイルの変換を行なう
        /// </summary>
        /// <param name="palletName">パレット名</param>
        private async void convert(string palletName)
        {
            ProgressCount = 0;
            await Task.Run(() =>
            {
                try
                {
                    Cancel = new CancellationTokenSource();
                    if (!loadPallet(palletName))
                    {
                        Progress.Report(ERROR_LOAD_PALLET);
                    }
                    else if (!createWorkBook(Path.ChangeExtension(FileName, ".xlsx")))
                    {
                        Progress.Report(ERROR_CREATE_WORKBOOK);
                    }
                    else if (!createResult(".\\result.xlsx"))
                    {
                        Progress.Report(ERROR_CREATE_RESULT);
                    }
                }
                catch (OperationCanceledException)
                {
                    Progress.Report(ERROR_CANCEL);
                }
                finally
                {
                    Cancel.Dispose();
                    Cancel = null;
                }
            });
        }

        /// <summary>
        ///     パレットを読み込む
        /// </summary>
        /// <param name="palletName">パレット名</param>
        /// <returns>読込が成功したか否か</returns>
        private bool loadPallet(string palletName)
        {
            Progress.Report(++ProgressCount);

            // パレットが読み込み済みの場合はスキップ
            if (PalletName == palletName) return true;

            string fileName = PalletMap[palletName];
            try
            {
                ColorCashe.Clear();
                Pallet.Clear();
                using (StreamReader sr = new StreamReader(fileName))
                {
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        Cancel.Token.ThrowIfCancellationRequested();
                        string line = sr.ReadLine();
                        string[] value = line.Split(new char[] { ',' });
                        string name = value[0];
                        byte r = byte.Parse(value[1]);
                        byte g = byte.Parse(value[2]);
                        byte b = byte.Parse(value[3]);
                        Pallet.Add(new Bead(Pallet.Count + 1, name, r, g, b));
                    }
                }
                PalletName = palletName;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        ///     ワークブックを作成する
        /// </summary>
        /// <param name="fileName">ファイルパス</param>
        /// <returns>作成が成功したか否か</returns>
        private bool createWorkBook(string fileName)
        {
            try
            {
                using (XLWorkbook workbook = new XLWorkbook())
                {
                    Cancel.Token.ThrowIfCancellationRequested();
                    using (IXLWorksheet canvas = workbook.AddWorksheet("Canvas", 2))
                    {
                        setCanvas(canvas);
                    }
                    Cancel.Token.ThrowIfCancellationRequested();
                    using (IXLWorksheet info = workbook.AddWorksheet("Info", 1))
                    {
                        setInfo(info);
                    }
                    Cancel.Token.ThrowIfCancellationRequested();
                    workbook.SaveAs(fileName);
                }
            }
            catch (OutOfMemoryException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            Progress.Report(++ProgressCount);
            return true;
        }

        /// <summary>
        ///     Infoシートに情報を書き込む
        /// </summary>
        /// <param name="info">Infoシート</param>
        private void setInfo(IXLWorksheet info)
        {
            setInfoHeader(info);

            // 画像情報
            info.Cell(3, 2).SetValue(FileName);
            info.Cell(4, 2).SetValue(string.Format("{0} x {1}", Image.Width, Image.Height));
            info.Cell(5, 2).SetValue(PalletName);

            // ビーズ情報
            int i = 0;
            Pallet.Where(x => x.Count > 0).ForEach((bead) =>
            {
                int row = 8 + i++;
                info.Cell(row, 1).SetValue(bead.No)
                    .Style.Fill.SetBackgroundColor(bead.Color)
                    .Font.SetFontColor(bead.FontColor);
                info.Cell(row, 2).SetValue(bead.Name);
                info.Cell(row, 3).Value = bead.Count;
                bead.Total += bead.Count;
                bead.Count = 0;
            });

            info.Range(info.Cell(7, 1), info.LastCellUsed())
                .Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                .Border.SetOutsideBorder(XLBorderStyleValues.Dashed)
                .Border.SetInsideBorder(XLBorderStyleValues.Dashed);
        }

        /// <summary>
        ///     Infoシートにヘッダ情報を書き込む
        /// </summary>
        /// <param name="info">Infoシート</param>
        private void setInfoHeader(IXLWorksheet info)
        {
            // タイトル
            info.Cell(1, 1).SetValue(Text)
                .Style.Font.SetFontSize(20.0)
                .Font.SetBold();

            // 画像情報
            info.Cell(3, 1).SetValue("ディレクトリ");
            info.Cell(4, 1).SetValue("サイズ");
            info.Cell(5, 1).SetValue("パレット");
            info.Range(3, 1, 5, 1).Style.Font.SetBold()
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                .Border.SetOutsideBorder(XLBorderStyleValues.Dashed)
                .Border.SetInsideBorder(XLBorderStyleValues.Dashed)
                .Fill.SetBackgroundColor(XLColor.LightGray);

            // ビーズ情報
            info.Cell(7, 1).SetValue("No");
            info.Cell(7, 2).SetValue("名前");
            info.Cell(7, 3).SetValue("個数");
            info.Range(7, 1, 7, 3).Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.LightGray);

            // 行列幅
            info.Columns(1, 1).Width = 16.43;
            info.Columns(2, 2).Width = 23.57;
            info.Rows(1, 1).Height = 26.25;
        }

        /// <summary>
        ///     Canvasシートに情報を書き込む
        /// </summary>
        /// <param name="canvas">Canvasシート</param>
        private void setCanvas(IXLWorksheet canvas)
        {
            BitmapData data = null;
            try
            {
                data = Image.LockBits(new Rectangle(
                    0, 0, Image.Width, Image.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppArgb);
                int size = Image.Width * Image.Height;
                byte[] buf = new byte[size * 4];
                Marshal.Copy(data.Scan0, buf, 0, buf.Length);
                for (int i = 0; i < Image.Height; i++)
                {
                    for (int j = 0; j < Image.Width; j++)
                    {
                        Cancel.Token.ThrowIfCancellationRequested();

                        // プログレスバー更新
                        int progress = (j + Image.Width * i) * 100 / size + 1;
                        if (ProgressCount != progress)
                        {
                            ProgressCount = progress;
                            Progress.Report(progress);
                        }

                        IXLCell cell = canvas.Cell(i + 1, j + 1);
                        cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Dashed);

                        int idx = (j + Image.Width * i) * 4;
                        byte a = buf[idx + 3];
                        byte r = buf[idx + 2];
                        byte g = buf[idx + 1];
                        byte b = buf[idx];

                        // アルファ値が0のピクセルはスキップ
                        if (a == 0) continue;

                        int colorIdx = getNearestColorIndex(r, g, b);
                        Bead bead = Pallet[colorIdx];
                        bead.Count++;
                        cell.SetValue(colorIdx + 1)
                            .Style.Fill.SetBackgroundColor(bead.Color)
                            .Font.SetFontColor(bead.FontColor)
                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                            .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    }
                }
            }
            finally
            {
                if (data != null)
                {
                    Image.UnlockBits(data);
                }
            }

            // 行列幅
            canvas.Columns(1, Image.Width).Width = 3.57;
            canvas.Rows(1, Image.Height).Height = 22.5;
        }

        /// <summary>
        ///     パレットから最も近い色を選択する
        /// </summary>
        /// <param name="r">R</param>
        /// <param name="g">G</param>
        /// <param name="b">B</param>
        /// <returns>カラーインデックス</returns>
        private int getNearestColorIndex(byte r, byte g, byte b)
        {
            int colorIdx = 0;
            Color color = Color.FromArgb(r, g, b);
            if (ColorCashe.ContainsKey(color))
            {
                colorIdx = ColorCashe[color];
            }
            else
            {
                double min = double.MaxValue;
                Pallet.Select((bead, index) => new { bead, index }).ForEach((e) =>
                {
                    double diff = CIELAB.Difference(CIELAB.FromColor(e.bead.Color.Color), CIELAB.FromColor(color));
                    if (diff < min)
                    {
                        min = diff;
                        colorIdx = e.index;
                    }
                });
                ColorCashe.Add(color, colorIdx);
            }
            return colorIdx;
        }

        /// <summary>
        ///     リザルトを作成する
        /// </summary>
        /// <param name="fileName">ファイルパス</param>
        /// <returns>作成が成功したか否か</returns>
        private bool createResult(string fileName)
        {
            try
            {
                using (XLWorkbook workbook = new XLWorkbook())
                {
                    Cancel.Token.ThrowIfCancellationRequested();
                    using (IXLWorksheet result = workbook.AddWorksheet("Result"))
                    {
                        setResult(result);
                    }
                    Cancel.Token.ThrowIfCancellationRequested();
                    workbook.SaveAs(fileName);
                }
            }
            catch (OutOfMemoryException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            Progress.Report(++ProgressCount);
            return true;
        }

        /// <summary>
        ///     Resultシートに情報を書き込む
        /// </summary>
        /// <param name="result">Resultシート</param>
        public void setResult(IXLWorksheet result)
        {
            // タイトル
            result.Cell(1, 1).SetValue(Text)
                .Style.Font.SetFontSize(20.0)
                .Font.SetBold();

            // パレット情報
            result.Cell(3, 1).SetValue("パレット")
                .Style.Font.SetBold()
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                .Border.SetOutsideBorder(XLBorderStyleValues.Dashed)
                .Border.SetInsideBorder(XLBorderStyleValues.Dashed)
                .Fill.SetBackgroundColor(XLColor.LightGray);
            result.Cell(3, 2).SetValue(PalletName);

            // ビーズ情報
            result.Cell(5, 1).SetValue("No");
            result.Cell(5, 2).SetValue("名前");
            result.Cell(5, 3).SetValue("個数");
            result.Range(5, 1, 5, 3).Style.Font.SetBold()
                .Fill.SetBackgroundColor(XLColor.LightGray);

            int i = 0;
            Pallet.Where((x) => x.Total > 0).ForEach((bead) =>
            {
                int row = 6 + i++;
                result.Cell(row, 1).SetValue(bead.No)
                    .Style.Fill.SetBackgroundColor(bead.Color)
                    .Font.SetFontColor(bead.FontColor);
                result.Cell(row, 2).SetValue(bead.Name);
                result.Cell(row, 3).Value = bead.Total;
            });

            result.Range(result.Cell(5, 1), result.LastCellUsed())
                .Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                .Border.SetOutsideBorder(XLBorderStyleValues.Dashed)
                .Border.SetInsideBorder(XLBorderStyleValues.Dashed);

            // 行列幅
            result.Columns(1, 1).Width = 16.43;
            result.Columns(2, 2).Width = 23.57;
            result.Rows(1, 1).Height = 26.25;
        }
    }
}
