namespace BeadsImageConverter
{
    partial class FormMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.PictureBox pbImage = null;

        private System.Windows.Forms.Label lbLoad = null;

        private System.Windows.Forms.ComboBox cbPalette = null;

        private System.Windows.Forms.Label lblPalette = null;

        private System.Windows.Forms.ProgressBar pbWork = null;

        private System.Windows.Forms.CheckBox cbConvert = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.lbLoad = new System.Windows.Forms.Label();
            this.cbPalette = new System.Windows.Forms.ComboBox();
            this.lblPalette = new System.Windows.Forms.Label();
            this.pbWork = new System.Windows.Forms.ProgressBar();
            this.cbConvert = new System.Windows.Forms.CheckBox();
            this.cbSpecial = new System.Windows.Forms.CheckBox();
            this.cbDiscontinue = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pbImage
            // 
            this.pbImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbImage.Location = new System.Drawing.Point(12, 12);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(260, 238);
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbImage.TabIndex = 0;
            this.pbImage.TabStop = false;
            this.pbImage.Click += new System.EventHandler(this.pbImage_Click);
            this.pbImage.DragDrop += new System.Windows.Forms.DragEventHandler(this.pbImage_DragDrop);
            this.pbImage.DragEnter += new System.Windows.Forms.DragEventHandler(this.pbImage_DragEnter);
            // 
            // lbLoad
            // 
            this.lbLoad.AllowDrop = true;
            this.lbLoad.AutoSize = true;
            this.lbLoad.Location = new System.Drawing.Point(55, 121);
            this.lbLoad.Name = "lbLoad";
            this.lbLoad.Size = new System.Drawing.Size(175, 24);
            this.lbLoad.TabIndex = 2;
            this.lbLoad.Text = "クリックして画像ファイルを読み込むか\r\n画像ファイルをD&&Dして下さい";
            this.lbLoad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbLoad.Click += new System.EventHandler(this.pbImage_Click);
            this.lbLoad.DragDrop += new System.Windows.Forms.DragEventHandler(this.pbImage_DragDrop);
            this.lbLoad.DragEnter += new System.Windows.Forms.DragEventHandler(this.pbImage_DragEnter);
            // 
            // cbPalette
            // 
            this.cbPalette.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPalette.FormattingEnabled = true;
            this.cbPalette.Location = new System.Drawing.Point(57, 256);
            this.cbPalette.Name = "cbPalette";
            this.cbPalette.Size = new System.Drawing.Size(215, 20);
            this.cbPalette.TabIndex = 4;
            this.cbPalette.SelectedIndexChanged += new System.EventHandler(this.cbPalette_SelectedIndexChanged);
            // 
            // lblPalette
            // 
            this.lblPalette.AutoSize = true;
            this.lblPalette.Location = new System.Drawing.Point(12, 259);
            this.lblPalette.Name = "lblPalette";
            this.lblPalette.Size = new System.Drawing.Size(39, 12);
            this.lblPalette.TabIndex = 5;
            this.lblPalette.Text = "パレット";
            // 
            // pbWork
            // 
            this.pbWork.Location = new System.Drawing.Point(12, 309);
            this.pbWork.Maximum = 101;
            this.pbWork.Name = "pbWork";
            this.pbWork.Size = new System.Drawing.Size(181, 22);
            this.pbWork.TabIndex = 6;
            // 
            // cbConvert
            // 
            this.cbConvert.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbConvert.Enabled = false;
            this.cbConvert.Location = new System.Drawing.Point(201, 309);
            this.cbConvert.Name = "cbConvert";
            this.cbConvert.Size = new System.Drawing.Size(71, 22);
            this.cbConvert.TabIndex = 7;
            this.cbConvert.Text = "変換";
            this.cbConvert.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbConvert.UseVisualStyleBackColor = true;
            this.cbConvert.CheckedChanged += new System.EventHandler(this.cbConvert_CheckedChanged);
            // 
            // cbSpecial
            // 
            this.cbSpecial.AutoSize = true;
            this.cbSpecial.Location = new System.Drawing.Point(12, 287);
            this.cbSpecial.Name = "cbSpecial";
            this.cbSpecial.Size = new System.Drawing.Size(116, 16);
            this.cbSpecial.TabIndex = 8;
            this.cbSpecial.Text = "特殊ビーズを含める";
            this.cbSpecial.UseVisualStyleBackColor = true;
            this.cbSpecial.CheckedChanged += new System.EventHandler(this.cbSpecial_CheckedChanged);
            // 
            // cbDiscontinue
            // 
            this.cbDiscontinue.AutoSize = true;
            this.cbDiscontinue.Location = new System.Drawing.Point(134, 287);
            this.cbDiscontinue.Name = "cbDiscontinue";
            this.cbDiscontinue.Size = new System.Drawing.Size(116, 16);
            this.cbDiscontinue.TabIndex = 9;
            this.cbDiscontinue.Text = "廃盤ビーズを含める";
            this.cbDiscontinue.UseVisualStyleBackColor = true;
            this.cbDiscontinue.CheckedChanged += new System.EventHandler(this.cbDiscontinue_CheckedChanged);
            // 
            // FormMain
            // 
            this.ClientSize = new System.Drawing.Size(284, 342);
            this.Controls.Add(this.cbDiscontinue);
            this.Controls.Add(this.cbSpecial);
            this.Controls.Add(this.cbConvert);
            this.Controls.Add(this.pbWork);
            this.Controls.Add(this.lblPalette);
            this.Controls.Add(this.cbPalette);
            this.Controls.Add(this.lbLoad);
            this.Controls.Add(this.pbImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.Text = "BeadsImageConverter";
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbSpecial;
        private System.Windows.Forms.CheckBox cbDiscontinue;
    }
}

