namespace LeNS
{
    partial class frmMain
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblPakPath = new System.Windows.Forms.Label();
            this.txtPakPath = new System.Windows.Forms.TextBox();
            this.lblOutputPath = new System.Windows.Forms.Label();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.lnlLvns = new System.Windows.Forms.Label();
            this.lblBgm = new System.Windows.Forms.Label();
            this.rdoBgmCd = new System.Windows.Forms.RadioButton();
            this.pnlBgm = new System.Windows.Forms.Panel();
            this.rdoBgmOgg = new System.Windows.Forms.RadioButton();
            this.rdoBgmMp3 = new System.Windows.Forms.RadioButton();
            this.pnlLvns = new System.Windows.Forms.Panel();
            this.rdoLVNS3 = new System.Windows.Forms.RadioButton();
            this.rdoLVNS1 = new System.Windows.Forms.RadioButton();
            this.rdoLVNS2 = new System.Windows.Forms.RadioButton();
            this.btnPakPath = new System.Windows.Forms.Button();
            this.btnOutputPath = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.fbDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.chkGetRegPath = new System.Windows.Forms.CheckBox();
            this.pnlBgm.SuspendLayout();
            this.pnlLvns.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPakPath
            // 
            this.lblPakPath.AutoSize = true;
            this.lblPakPath.Location = new System.Drawing.Point(6, 41);
            this.lblPakPath.Name = "lblPakPath";
            this.lblPakPath.Size = new System.Drawing.Size(86, 12);
            this.lblPakPath.TabIndex = 0;
            this.lblPakPath.Text = "PAKファイルパス：";
            // 
            // txtPakPath
            // 
            this.txtPakPath.Location = new System.Drawing.Point(8, 56);
            this.txtPakPath.Name = "txtPakPath";
            this.txtPakPath.Size = new System.Drawing.Size(303, 19);
            this.txtPakPath.TabIndex = 1;
            // 
            // lblOutputPath
            // 
            this.lblOutputPath.AutoSize = true;
            this.lblOutputPath.Location = new System.Drawing.Point(6, 80);
            this.lblOutputPath.Name = "lblOutputPath";
            this.lblOutputPath.Size = new System.Drawing.Size(105, 12);
            this.lblOutputPath.TabIndex = 2;
            this.lblOutputPath.Text = "変換ファイル出力先：";
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Location = new System.Drawing.Point(8, 95);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(303, 19);
            this.txtOutputPath.TabIndex = 3;
            // 
            // lnlLvns
            // 
            this.lnlLvns.AutoSize = true;
            this.lnlLvns.Location = new System.Drawing.Point(6, 6);
            this.lnlLvns.Name = "lnlLvns";
            this.lnlLvns.Size = new System.Drawing.Size(65, 12);
            this.lnlLvns.TabIndex = 4;
            this.lnlLvns.Text = "ゲーム選択：";
            // 
            // lblBgm
            // 
            this.lblBgm.AutoSize = true;
            this.lblBgm.Location = new System.Drawing.Point(6, 120);
            this.lblBgm.Name = "lblBgm";
            this.lblBgm.Size = new System.Drawing.Size(234, 12);
            this.lblBgm.TabIndex = 8;
            this.lblBgm.Text = "BGM再生方式（変換スクリプトに埋め込みます）：";
            // 
            // rdoBgmCd
            // 
            this.rdoBgmCd.AutoSize = true;
            this.rdoBgmCd.Checked = true;
            this.rdoBgmCd.Location = new System.Drawing.Point(0, 0);
            this.rdoBgmCd.Name = "rdoBgmCd";
            this.rdoBgmCd.Size = new System.Drawing.Size(61, 16);
            this.rdoBgmCd.TabIndex = 9;
            this.rdoBgmCd.Text = "CD-DA";
            this.rdoBgmCd.UseVisualStyleBackColor = true;
            // 
            // pnlBgm
            // 
            this.pnlBgm.Controls.Add(this.rdoBgmOgg);
            this.pnlBgm.Controls.Add(this.rdoBgmMp3);
            this.pnlBgm.Controls.Add(this.rdoBgmCd);
            this.pnlBgm.Location = new System.Drawing.Point(8, 135);
            this.pnlBgm.Name = "pnlBgm";
            this.pnlBgm.Size = new System.Drawing.Size(354, 20);
            this.pnlBgm.TabIndex = 10;
            // 
            // rdoBgmOgg
            // 
            this.rdoBgmOgg.AutoSize = true;
            this.rdoBgmOgg.Location = new System.Drawing.Point(118, 0);
            this.rdoBgmOgg.Name = "rdoBgmOgg";
            this.rdoBgmOgg.Size = new System.Drawing.Size(76, 16);
            this.rdoBgmOgg.TabIndex = 11;
            this.rdoBgmOgg.Text = "OggVorvis";
            this.rdoBgmOgg.UseVisualStyleBackColor = true;
            // 
            // rdoBgmMp3
            // 
            this.rdoBgmMp3.AutoSize = true;
            this.rdoBgmMp3.Location = new System.Drawing.Point(67, 0);
            this.rdoBgmMp3.Name = "rdoBgmMp3";
            this.rdoBgmMp3.Size = new System.Drawing.Size(45, 16);
            this.rdoBgmMp3.TabIndex = 10;
            this.rdoBgmMp3.Text = "MP3";
            this.rdoBgmMp3.UseVisualStyleBackColor = true;
            // 
            // pnlLvns
            // 
            this.pnlLvns.Controls.Add(this.rdoLVNS3);
            this.pnlLvns.Controls.Add(this.rdoLVNS1);
            this.pnlLvns.Controls.Add(this.rdoLVNS2);
            this.pnlLvns.Location = new System.Drawing.Point(8, 21);
            this.pnlLvns.Name = "pnlLvns";
            this.pnlLvns.Size = new System.Drawing.Size(169, 20);
            this.pnlLvns.TabIndex = 3;
            // 
            // rdoLVNS3
            // 
            this.rdoLVNS3.AutoSize = true;
            this.rdoLVNS3.Location = new System.Drawing.Point(82, 0);
            this.rdoLVNS3.Name = "rdoLVNS3";
            this.rdoLVNS3.Size = new System.Drawing.Size(68, 16);
            this.rdoLVNS3.TabIndex = 10;
            this.rdoLVNS3.Text = "To Heart";
            this.rdoLVNS3.UseVisualStyleBackColor = true;
            this.rdoLVNS3.CheckedChanged += new System.EventHandler(this.comLVNS_CheckedChanged);
            // 
            // rdoLVNS1
            // 
            this.rdoLVNS1.AutoSize = true;
            this.rdoLVNS1.Location = new System.Drawing.Point(0, 0);
            this.rdoLVNS1.Name = "rdoLVNS1";
            this.rdoLVNS1.Size = new System.Drawing.Size(35, 16);
            this.rdoLVNS1.TabIndex = 1;
            this.rdoLVNS1.Text = "雫";
            this.rdoLVNS1.UseVisualStyleBackColor = true;
            this.rdoLVNS1.CheckedChanged += new System.EventHandler(this.comLVNS_CheckedChanged);
            // 
            // rdoLVNS2
            // 
            this.rdoLVNS2.AutoSize = true;
            this.rdoLVNS2.Location = new System.Drawing.Point(41, 0);
            this.rdoLVNS2.Name = "rdoLVNS2";
            this.rdoLVNS2.Size = new System.Drawing.Size(35, 16);
            this.rdoLVNS2.TabIndex = 2;
            this.rdoLVNS2.Text = "痕";
            this.rdoLVNS2.UseVisualStyleBackColor = true;
            this.rdoLVNS2.CheckedChanged += new System.EventHandler(this.comLVNS_CheckedChanged);
            // 
            // btnPakPath
            // 
            this.btnPakPath.Location = new System.Drawing.Point(317, 52);
            this.btnPakPath.Name = "btnPakPath";
            this.btnPakPath.Size = new System.Drawing.Size(44, 23);
            this.btnPakPath.TabIndex = 12;
            this.btnPakPath.TabStop = false;
            this.btnPakPath.Tag = this.txtPakPath;
            this.btnPakPath.Text = "参照";
            this.btnPakPath.UseVisualStyleBackColor = true;
            this.btnPakPath.Click += new System.EventHandler(this.btnPath_Click);
            // 
            // btnOutputPath
            // 
            this.btnOutputPath.Location = new System.Drawing.Point(317, 91);
            this.btnOutputPath.Name = "btnOutputPath";
            this.btnOutputPath.Size = new System.Drawing.Size(44, 23);
            this.btnOutputPath.TabIndex = 13;
            this.btnOutputPath.TabStop = false;
            this.btnOutputPath.Tag = this.txtOutputPath;
            this.btnOutputPath.Text = "参照";
            this.btnOutputPath.UseVisualStyleBackColor = true;
            this.btnOutputPath.Click += new System.EventHandler(this.btnPath_Click);
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(206, 176);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 14;
            this.btnConvert.Text = "変換開始";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Location = new System.Drawing.Point(287, 176);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(75, 23);
            this.btnQuit.TabIndex = 15;
            this.btnQuit.Text = "終了";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // chkGetRegPath
            // 
            this.chkGetRegPath.AutoSize = true;
            this.chkGetRegPath.Checked = true;
            this.chkGetRegPath.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGetRegPath.Location = new System.Drawing.Point(203, 22);
            this.chkGetRegPath.Name = "chkGetRegPath";
            this.chkGetRegPath.Size = new System.Drawing.Size(159, 16);
            this.chkGetRegPath.TabIndex = 4;
            this.chkGetRegPath.TabStop = false;
            this.chkGetRegPath.Text = "PAKパスをレジストリから取得";
            this.chkGetRegPath.UseVisualStyleBackColor = true;
            this.chkGetRegPath.CheckedChanged += new System.EventHandler(this.comLVNS_CheckedChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 206);
            this.Controls.Add(this.chkGetRegPath);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.btnOutputPath);
            this.Controls.Add(this.btnPakPath);
            this.Controls.Add(this.pnlLvns);
            this.Controls.Add(this.pnlBgm);
            this.Controls.Add(this.lblBgm);
            this.Controls.Add(this.lnlLvns);
            this.Controls.Add(this.txtOutputPath);
            this.Controls.Add(this.lblOutputPath);
            this.Controls.Add(this.txtPakPath);
            this.Controls.Add(this.lblPakPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.pnlBgm.ResumeLayout(false);
            this.pnlBgm.PerformLayout();
            this.pnlLvns.ResumeLayout(false);
            this.pnlLvns.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPakPath;
        private System.Windows.Forms.TextBox txtPakPath;
        private System.Windows.Forms.Label lblOutputPath;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Label lnlLvns;
        private System.Windows.Forms.Label lblBgm;
        private System.Windows.Forms.RadioButton rdoBgmCd;
        private System.Windows.Forms.Panel pnlBgm;
        private System.Windows.Forms.Panel pnlLvns;
        private System.Windows.Forms.RadioButton rdoLVNS3;
        private System.Windows.Forms.RadioButton rdoLVNS1;
        private System.Windows.Forms.RadioButton rdoLVNS2;
        private System.Windows.Forms.RadioButton rdoBgmMp3;
        private System.Windows.Forms.RadioButton rdoBgmOgg;
        private System.Windows.Forms.Button btnPakPath;
        private System.Windows.Forms.Button btnOutputPath;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.FolderBrowserDialog fbDialog;
        private System.Windows.Forms.CheckBox chkGetRegPath;

    }
}

