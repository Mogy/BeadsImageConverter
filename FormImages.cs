using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace BeadsImageConverter
{
    public partial class FormImages : Form
    {
        private const int NAME = 1;
        private const int PATH = 2;
        private const int PALETTE = 3;
        private const int SPECIAL = 4;
        private const int DISCONTINUE = 5;
        private const int RESULT = 6;

        FormMain FormMain;
        List<string> FileNames;
        IProgress<int> ImageProgress;
        bool Cancel;
        int FileCount { get { return lvImages.Items.Count; } }

        public FormImages()
        {
            InitializeComponent();
            FileNames = new List<string>();
            ImageProgress = new Progress<int>(new Action<int>(showProgress));
            lvImages.SmallImageList = ilThumbnail;
        }

        private void FormImages_Shown(object sender, EventArgs e)
        {
            FormMain = (FormMain)Owner;
        }

        private void lvImages_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = FormMain.getDropEffect(e);
            // 移動をコピーに置き換える
            if (e.Effect == DragDropEffects.Move) 
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void lvImages_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
            FormMain.loadImage(fileNames);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearItem();
            FormMain.clearImage();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FormMain.openImage();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvImages.SelectedItems)
            {
                ilThumbnail.Images.RemoveAt(lvImages.Items.IndexOf(item));
                FileNames.Remove(item.SubItems[PATH].Text);
                lvImages.Items.Remove(item);
            }
            // インデックスを再設定
            for (int i = 0; i < lvImages.Items.Count; i++)
            {
                lvImages.Items[i].ImageIndex = i;
            }
            if (lvImages.Items.Count > 0)
            {
                cbConvert.Enabled = true;
                lvImages.Items[0].Selected = true;
            }
            else
            {
                cbConvert.Enabled = false;
                FormMain.clearImage();
            }
        }

        private void lvImages_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected && lvImages.SelectedItems.Count == 1)
            {
                setState(e.Item.SubItems);
            }
        }

        private void lvImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvImages.SelectedItems.Count == 1)
            {
                setState(lvImages.SelectedItems[0].SubItems);
            }
        }

        private async void cbConvert_CheckedChanged(object sender, EventArgs e)
        {
            if (cbConvert.Checked)
            {
                cbConvert.Text = "キャンセル";
                setEnabled(false);
                lvImages.SelectedItems.Clear();
                Cancel = false;
                foreach (ListViewItem item in lvImages.Items)
                {
                    item.Selected = true;
                    await FormMain.convert(ImageProgress, item.SubItems[PALETTE].Text);
                    item.Selected = false;
                    if (Cancel) break;
                }
                FormMain.setProgress(0);
                cbConvert.Checked = false;
                if (Cancel)
                {
                    MessageBox.Show(this, "変換をキャンセルしました", "キャンセル", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(this, "変換が完了しました", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                cbConvert.Text = "一括変換";
                setEnabled(true);
                FormMain.cancel();
            }
        }

        internal string bool2String(bool b) { return b ? "Yes" : "No"; }

        internal bool string2Bool(string s) { return s == "Yes"; }

        /// <summary>
        ///     コントロールの有効状態を設定する
        /// </summary>
        /// <param name="enabled">有効状態</param>
        private void setEnabled(bool enabled)
        {
            FormMain.Enabled = enabled;
            lvImages.Enabled = enabled;
            btnClear.Enabled = enabled;
            btnAdd.Enabled = enabled;
        }

        /// <summary>
        ///     状態を反映する
        /// </summary>
        /// <param name="sub">サブアイテム</param>
        private void setState(ListViewItem.ListViewSubItemCollection sub)
        {
            string fileName = sub[PATH].Text;
            string palette = sub[PALETTE].Text;
            bool special = string2Bool(sub[SPECIAL].Text);
            bool discontinue = string2Bool(sub[DISCONTINUE].Text);
            FormMain.setState(fileName, palette, special, discontinue);
        }

        /// <summary>
        ///     アイテムを追加する
        /// </summary>
        /// <param name="fileName">ファイルパス</param>
        /// <param name="image">画像</param>
        /// <param name="palette">パレット名</param>
        /// <param name="special">特殊ビーズ</param>
        /// <param name="discontinue">廃盤ビーズ</param>
        public void addItem(string fileName, Image image, string palette, bool special, bool discontinue)
        {
            // 存在チェック
            int idx = FileNames.IndexOf(fileName);
            if (idx >= 0)
            {
                lvImages.Items[idx].Selected = true;
                return;
            }

            FileNames.Add(fileName);
            lvImages.SelectedItems.Clear();
            ListViewItem item = new ListViewItem();
            item.SubItems.Add(Path.GetFileName(fileName));
            item.SubItems.Add(fileName);
            item.SubItems.Add(palette);
            item.SubItems.Add(bool2String(special));
            item.SubItems.Add(bool2String(discontinue));
            item.SubItems.Add("");
            item.UseItemStyleForSubItems = false;
            item.ImageIndex = lvImages.Items.Count;
            item.Selected = true;
            lvImages.Items.Add(item);
            lvImages.SmallImageList.Images.Add(createThumbnailImage(image));
            cbConvert.Enabled = lvImages.Items.Count > 0;
        }

        /// <summary>
        ///     全てのアイテムを削除する
        /// </summary>
        public void clearItem()
        {
            ilThumbnail.Images.Clear();
            FileNames.Clear();
            lvImages.Items.Clear();
            cbConvert.Enabled = lvImages.Items.Count > 0;
        }

        /// <summary>
        ///     サムネイル画像を作成する
        /// </summary>
        /// <param name="image">元画像</param>
        /// <returns>サムネイル画像</returns>
        private Image createThumbnailImage(Image image)
        {
            Size size = ilThumbnail.ImageSize;
            Bitmap bitmap = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                Bitmap bmp = new Bitmap(image);
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.DrawImage(bmp, 0, 0, bitmap.Width, bitmap.Height);
            }
            return bitmap;
        }

        /// <summary>
        ///     パレット名を設定する
        /// </summary>
        /// <param name="palette">パレット名</param>
        public void setPalette(string palette)
        {
            foreach (ListViewItem item in lvImages.SelectedItems)
            {
                item.SubItems[PALETTE].Text = palette;
            }
        }

        /// <summary>
        ///     特殊ビーズを設定する
        /// </summary>
        /// <param name="special">特殊ビーズ</param>
        public void setSpecial(bool special)
        {
            foreach (ListViewItem item in lvImages.SelectedItems)
            {
                item.SubItems[SPECIAL].Text = bool2String(special);
            }
        }

        /// <summary>
        ///     廃盤ビーズを設定する
        /// </summary>
        /// <param name="discontinue">廃盤ビーズ</param>
        public void setDiscontinue(bool discontinue)
        {
            foreach (ListViewItem item in lvImages.SelectedItems)
            {
                item.SubItems[DISCONTINUE].Text = bool2String(discontinue);
            }
        }

        /// <summary>
        ///     プログレスバーを更新する
        /// </summary>
        /// <param name="progress">更新値</param>
        public void showProgress(int progress)
        {
            if (lvImages.SelectedItems.Count == 0) return;
            if (0 < progress && progress <= 100)
            {
                lvImages.SelectedItems[0].SubItems[RESULT].Text = progress + "%";
                lvImages.SelectedItems[0].SubItems[RESULT].ForeColor = Color.DarkBlue;
            }
            else if (progress <= 0)
            {
                lvImages.SelectedItems[0].SubItems[RESULT].Text = "NG";
                lvImages.SelectedItems[0].SubItems[RESULT].ForeColor = Color.Red;
                if (progress == FormMain.ERROR_CANCEL)
                {
                    Cancel = true;
                }
            }
            else
            {
                lvImages.SelectedItems[0].SubItems[RESULT].Text = "OK";
                lvImages.SelectedItems[0].SubItems[RESULT].ForeColor = Color.Green;
            }
        }

    }
}
