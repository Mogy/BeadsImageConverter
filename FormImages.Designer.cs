namespace BeadsImageConverter
{
    partial class FormImages
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lvImages = new System.Windows.Forms.ListView();
            this.chThumb = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ilThumbnail = new System.Windows.Forms.ImageList(this.components);
            this.btnClear = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvImages
            // 
            this.lvImages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvImages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chThumb,
            this.chFileName,
            this.chPath});
            this.lvImages.FullRowSelect = true;
            this.lvImages.GridLines = true;
            this.lvImages.Location = new System.Drawing.Point(12, 12);
            this.lvImages.Name = "lvImages";
            this.lvImages.Size = new System.Drawing.Size(444, 242);
            this.lvImages.TabIndex = 0;
            this.lvImages.UseCompatibleStateImageBehavior = false;
            this.lvImages.View = System.Windows.Forms.View.Details;
            this.lvImages.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvImages_ItemSelectionChanged);
            // 
            // chThumb
            // 
            this.chThumb.Text = "";
            this.chThumb.Width = 40;
            // 
            // chFileName
            // 
            this.chFileName.Text = "ファイル名";
            this.chFileName.Width = 100;
            // 
            // chPath
            // 
            this.chPath.Text = "パス";
            this.chPath.Width = 300;
            // 
            // ilThumbnail
            // 
            this.ilThumbnail.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.ilThumbnail.ImageSize = new System.Drawing.Size(32, 32);
            this.ilThumbnail.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.Location = new System.Drawing.Point(12, 260);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "クリア";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(300, 260);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "追加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(381, 260);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "削除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // FormImages
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 295);
            this.ControlBox = false;
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lvImages);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormImages";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "画像";
            this.Shown += new System.EventHandler(this.FormImages_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvImages;
        private System.Windows.Forms.ColumnHeader chFileName;
        private System.Windows.Forms.ColumnHeader chPath;
        private System.Windows.Forms.ColumnHeader chThumb;
        private System.Windows.Forms.ImageList ilThumbnail;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
    }
}