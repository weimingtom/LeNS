namespace LeNS
{
    partial class frmProgress
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
            this.components = new System.ComponentModel.Container();
            this.lblTarget = new System.Windows.Forms.Label();
            this.prgProgress = new System.Windows.Forms.ProgressBar();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.chkAutoClose = new System.Windows.Forms.CheckBox();
            this.timWatcher = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lblTarget
            // 
            this.lblTarget.Location = new System.Drawing.Point(3, 6);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(312, 13);
            this.lblTarget.TabIndex = 0;
            // 
            // prgProgress
            // 
            this.prgProgress.Location = new System.Drawing.Point(5, 27);
            this.prgProgress.Name = "prgProgress";
            this.prgProgress.Size = new System.Drawing.Size(404, 14);
            this.prgProgress.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(334, 180);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "キャンセル";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblProgress
            // 
            this.lblProgress.Location = new System.Drawing.Point(321, 6);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(88, 13);
            this.lblProgress.TabIndex = 3;
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lstLog
            // 
            this.lstLog.FormattingEnabled = true;
            this.lstLog.HorizontalScrollbar = true;
            this.lstLog.ItemHeight = 12;
            this.lstLog.Location = new System.Drawing.Point(3, 48);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(406, 124);
            this.lstLog.TabIndex = 4;
            this.lstLog.TabStop = false;
            // 
            // chkAutoClose
            // 
            this.chkAutoClose.AutoSize = true;
            this.chkAutoClose.Location = new System.Drawing.Point(3, 186);
            this.chkAutoClose.Name = "chkAutoClose";
            this.chkAutoClose.Size = new System.Drawing.Size(157, 16);
            this.chkAutoClose.TabIndex = 5;
            this.chkAutoClose.TabStop = false;
            this.chkAutoClose.Text = "変換完了時に自動で閉じる";
            this.chkAutoClose.UseVisualStyleBackColor = true;
            // 
            // timWatcher
            // 
            this.timWatcher.Interval = 10;
            this.timWatcher.Tick += new System.EventHandler(this.timWatcher_Tick);
            // 
            // frmProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 209);
            this.ControlBox = false;
            this.Controls.Add(this.chkAutoClose);
            this.Controls.Add(this.lstLog);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.prgProgress);
            this.Controls.Add(this.lblTarget);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmProgress";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "変換処理中・・・";
            this.Shown += new System.EventHandler(this.frmProgress_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTarget;
        private System.Windows.Forms.ProgressBar prgProgress;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.CheckBox chkAutoClose;
        private System.Windows.Forms.Timer timWatcher;
    }
}