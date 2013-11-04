using LeNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

// �ϊ��_�C�A���O
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
            // �i����Ԃ��t�H�[���ɕ\��
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

            // ���O�����X�g�ɔ��f
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
                        MessageBox.Show("�����͐���I�����܂����B",
                                        "�ϊ�����",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                        */
                        this.Text = "�ϊ�����";
                        btnClose.Text = "����";
                    }

                    break;
                case LeNSConvState.Error:
                    ((System.Windows.Forms.Timer)sender).Enabled = false;

                    MessageBox.Show("�������ɃG���[���������܂����B\n" + 
                                    conv.ErrorDescription,
                                    "�G���[", 
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Error);
                    this.Text = "�ُ�I��";
                    btnClose.Text = "����";

                    break;
            }
        }
    }
}
