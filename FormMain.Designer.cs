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

        private System.Windows.Forms.ComboBox cbPallet = null;

        private System.Windows.Forms.Label lblPallet = null;

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
            this.cbPallet = new System.Windows.Forms.ComboBox();
            this.lblPallet = new System.Windows.Forms.Label();
            this.pbWork = new System.Windows.Forms.ProgressBar();
            this.cbConvert = new System.Windows.Forms.CheckBox();
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
            this.lbLoad.AutoSize = true;
            this.lbLoad.Location = new System.Drawing.Point(55, 121);
            this.lbLoad.Name = "lbLoad";
            this.lbLoad.Size = new System.Drawing.Size(175, 24);
            this.lbLoad.TabIndex = 2;
            this.lbLoad.Text = "クリックして画像ファイルを読み込むか\r\n画像ファイルをD&&Dして下さい";
            this.lbLoad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbLoad.Click += new System.EventHandler(this.lbLoad_Click);
            this.lbLoad.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbLoad_DragDrop);
            this.lbLoad.DragEnter += new System.Windows.Forms.DragEventHandler(this.lbLoad_DragEnter);
            // 
            // cbPallet
            // 
            this.cbPallet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPallet.FormattingEnabled = true;
            this.cbPallet.Location = new System.Drawing.Point(57, 256);
            this.cbPallet.Name = "cbPallet";
            this.cbPallet.Size = new System.Drawing.Size(215, 20);
            this.cbPallet.TabIndex = 4;
            // 
            // lblPallet
            // 
            this.lblPallet.AutoSize = true;
            this.lblPallet.Location = new System.Drawing.Point(12, 259);
            this.lblPallet.Name = "lblPallet";
            this.lblPallet.Size = new System.Drawing.Size(39, 12);
            this.lblPallet.TabIndex = 5;
            this.lblPallet.Text = "パレット";
            // 
            // pbWork
            // 
            this.pbWork.Location = new System.Drawing.Point(14, 287);
            this.pbWork.Maximum = 101;
            this.pbWork.Name = "pbWork";
            this.pbWork.Size = new System.Drawing.Size(181, 22);
            this.pbWork.TabIndex = 6;
            // 
            // cbConvert
            // 
            this.cbConvert.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbConvert.Enabled = false;
            this.cbConvert.Location = new System.Drawing.Point(201, 287);
            this.cbConvert.Name = "cbConvert";
            this.cbConvert.Size = new System.Drawing.Size(71, 22);
            this.cbConvert.TabIndex = 7;
            this.cbConvert.Text = "変換";
            this.cbConvert.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbConvert.UseVisualStyleBackColor = true;
            this.cbConvert.CheckedChanged += new System.EventHandler(this.cbConvert_CheckedChanged);
            // 
            // FormMain
            // 
            this.ClientSize = new System.Drawing.Size(284, 317);
            this.Controls.Add(this.cbConvert);
            this.Controls.Add(this.pbWork);
            this.Controls.Add(this.lblPallet);
            this.Controls.Add(this.cbPallet);
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
    }
}

