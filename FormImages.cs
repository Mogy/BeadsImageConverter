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

        FormMain Main;
        List<string> FileNames;
        int FileCount { get { return lvImages.Items.Count; } }

        public FormImages()
        {
            InitializeComponent();
            FileNames = new List<string>();
            lvImages.SmallImageList = ilThumbnail;
        }

        private void FormImages_Shown(object sender, EventArgs e)
        {
            Main = (FormMain)Owner;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ilThumbnail.Images.Clear();
            FileNames.Clear();
            lvImages.Items.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Main.openImage();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvImages.SelectedItems)
            {
                ilThumbnail.Images.RemoveAt(lvImages.Items.IndexOf(item));
                FileNames.Remove(item.SubItems[PATH].Text);
                lvImages.Items.Remove(item);
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

        internal string Bool2String(bool b) { return b ? "Yes" : "No"; }

        internal bool String2Bool(string s) { return s == "Yes"; }

        /// <summary>
        ///     状態を反映する
        /// </summary>
        /// <param name="sub">サブアイテム</param>
        private void setState(ListViewItem.ListViewSubItemCollection sub)
        {
            string fileName = sub[PATH].Text;
            string palette = sub[PALETTE].Text;
            bool special = String2Bool(sub[SPECIAL].Text);
            bool discontinue = String2Bool(sub[DISCONTINUE].Text);
            Main.setState(fileName, palette, special, discontinue);
        }

        /// <summary>
        ///     アイテムを追加する
        /// </summary>
        /// <param name="fileName">ファイルパス</param>
        /// <param name="image">画像</param>
        /// <param name="palette">パレット名</param>
        /// <param name="special">特殊ビーズ</param>
        /// <param name="discontinue">廃盤ビーズ</param>
        public void AddItem(string fileName, Image image, string palette, bool special, bool discontinue)
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
            item.SubItems.Add(Bool2String(special));
            item.SubItems.Add(Bool2String(discontinue));
            item.ImageIndex = lvImages.SmallImageList.Images.Count;
            item.Selected = true;
            lvImages.Items.Add(item);
            lvImages.SmallImageList.Images.Add(createThumbnailImage(image));
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
                item.SubItems[SPECIAL].Text = Bool2String(special);
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
                item.SubItems[DISCONTINUE].Text = Bool2String(discontinue);
            }
        }

    }
}
