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
        FormMain Main;

        public FormImages()
        {
            InitializeComponent();
            lvImages.SmallImageList = ilThumbnail;
        }

        private void FormImages_Shown(object sender, EventArgs e)
        {
            Main = (FormMain)Owner;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lvImages.Items.Clear();
            ilThumbnail.Images.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Main.openImage();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvImages.SelectedItems)
            {
                lvImages.Items.Remove(item);
            }
        }

        private void lvImages_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected && lvImages.SelectedItems.Count == 1)
            {
                Main.showPreview(e.Item.SubItems[2].Text);
            }
        }

        public bool AddImage(string fileName, Image image)
        {
            lvImages.SelectedItems.Clear();

            // 存在チェック
            foreach (ListViewItem i in lvImages.Items)
            {
                if (fileName == i.SubItems[2].Text)
                {
                    i.Selected = true;
                    return false;
                }
            }

            ListViewItem item = new ListViewItem();
            item.SubItems.Add(Path.GetFileName(fileName));
            item.SubItems.Add(fileName);
            item.ImageIndex = lvImages.SmallImageList.Images.Count;
            item.Selected = true;
            lvImages.Items.Add(item);
            lvImages.SmallImageList.Images.Add(createThumbnailImage(image));
            lvImages.Focus();
            return true;
        }

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


    }
}
