using LeNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

// 変換ダイアログ
namespace LeNS
{
    public partial class frmProgress : Form
    {
        public LeNS.LeNSConvOption Option;

        private Thread conv_thread;
        private LeNSMain conv;

        public frmProgress()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

            if (conv.State == LeNSConvState.Execute) {
                conv_thread.Abort();
            }
        }

        private void frmProgress_Load(object sender, EventArgs e)
        {
            conv = new LeNSMain();
            conv.Option = Option;
            conv_thread = new Thread(conv.Run);
            conv_thread.Start();
            timWatcher.Enabled = true;
        }

        private void timWatcher_Tick(object sender, EventArgs e)
        {
            // 進捗状態をフォームに表示
            lblTarget.Text = conv.ProcessingTask;
            prgProgress.Maximum = conv.ProgressMax;
            prgProgress.Value = conv.ProgressValue;
            if (prgProgress.Maximum != 0)
            {
                lblProgress.Text = prgProgress.Value.ToString() + "/" + prgProgress.Maximum;
            }
            else
            {
                lblProgress.Text = "";
            }

            // ログをリストに反映
            String[] log;
            log = conv.FlushLog();
            foreach (String text in log)
            {
                lstLog.Items.Add(text);
            }

            switch (conv.State)
            {
                case LeNSConvState.Canceled:
                    ((System.Windows.Forms.Timer)sender).Enabled = false;

                    break;
                case LeNSConvState.Complete:
                    ((System.Windows.Forms.Timer)sender).Enabled = false;

                    if (chkAutoClose.Checked)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        /*
                        MessageBox.Show("処理は正常終了しました。",
                                        "変換完了",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                        */
                        this.Text = "変換完了";
                        btnClose.Text = "閉じる";
                    }

                    break;
                case LeNSConvState.Error:
                    ((System.Windows.Forms.Timer)sender).Enabled = false;

                    MessageBox.Show("処理中にエラーが発生しました。\n" + 
                                    conv.ErrorDescription,
                                    "エラー", 
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Error);
                    this.Text = "異常終了";
                    btnClose.Text = "閉じる";

                    break;
            }
        }
    }
}
