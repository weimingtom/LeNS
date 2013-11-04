using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security;
using System.IO;
using Microsoft.Win32;

// ���C���t�H�[��
namespace LeNS
{
    public partial class frmMain : Form
    {
        // �R���X�g���N�^
        public frmMain()
        {
            InitializeComponent();
        }

        // ���[�h
        private void frmMain_Load(object sender, EventArgs e)
        {
            setCaption();

            rdoLVNS2.Checked = true;
        }

        // �L���v�V�����Z�b�g
        private void setCaption()
        {
            this.Text = Application.ProductName + " " + getVersionString();
        }

        // PAK�p�X�����ݒ�
        private void setPakPath()
        {
            // ���W�X�g���̃C���X�g�[����񂩂�PAK�p�X���w��
            RegistryKey regkey;
            try
            {
                // ��
                if (rdoLVNS1.Checked)
                {
                    // ���C���X�R���ĂȂ��̂łЂ���Ƃ�����Ⴄ�\�����c�B
                    regkey = Registry.CurrentUser.OpenSubKey(@"Software\Leaf\Sizuku", false);
                }
                // ��
                else if (rdoLVNS2.Checked)
                {
                    regkey = Registry.CurrentUser.OpenSubKey(@"Software\Leaf\Kizuato", false);
                }
                // To Heart
                else if (rdoLVNS3.Checked)
                {
                    regkey = Registry.CurrentUser.OpenSubKey(@"Software\Leaf\ToHeart", false);
                }
                else
                {
                    return;
                }

                try
                {
                    // ���W�X�g�������݂��Ȃ��Ƃ�NULL
                    if (regkey == null)
                    {
                        txtPakPath.Text = "";
                        return;
                    }
                    // ���W�X�g������p�X�擾
                    String path = (String)regkey.GetValue("DataDir");

                    // �擾�����p�X�𕡍�(�Z�k����Ă邽��)
                    path = Path.GetFullPath(path);
                    txtPakPath.Text = path;
                }
                finally
                {
                    if (regkey != null)
                    {
                        regkey.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
                MessageBox.Show("��O���������܂����B\n" + ex.ToString(),
                                "Exception",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        // �o�[�W����������擾
        private String getVersionString()
        {
            Version ver = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            return "Ver " + ver.ToString(3);
        }

        // �t�H���_�Q�ƃ{�^��
        private void btnPath_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnAlias = (Button)sender;
                TextBox txtAlias = (TextBox)btnAlias.Tag;
                switch (btnAlias.Name)
                {
                    case "btnPakPath":
                        fbDialog.Description = "PAK�t�@�C���̑��݂���p�X(�C���X�g�[���t�H���_)��I�����ĉ�����";
                        fbDialog.ShowNewFolderButton = false;
                        break;
                    case "btnOutputPath":
                        fbDialog.Description = "�ϊ��t�@�C���̊i�[���I�����ĉ�����(�󂫃t�H���_����)";
                        fbDialog.ShowNewFolderButton = true;
                        break;
                }

                fbDialog.SelectedPath = txtAlias.Text;
                if (fbDialog.ShowDialog() == DialogResult.OK)
                {
                    txtAlias.Text = fbDialog.SelectedPath;
                }
            }
            // �l�b�g���[�N�z���Ɏ��s����ƃZ�L�����e�B��O����������̂�Catch
            catch (SecurityException ex)
            {
                Debug.Print(ex.ToString());
                MessageBox.Show("�Z�L�����e�B��O���������܂����B\n�s�K�؂ȃp�X������s�����\��������܂��B",
                                "SecurityException",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
                MessageBox.Show("��O���������܂����B\n" + ex.ToString(),
                                "Exception",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        // �I���{�^��
        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // �ϊ��J�n�{�^��
        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (!checkConvert())
            {
                return;
            }

            frmProgress convDialog = new frmProgress();
            
            // �N���I�v�V�����̐ݒ�
            LeNSConvOption option = new LeNSConvOption();

            if (rdoLVNS1.Checked) option.conv = LeNS.LeNSConvMode.MODE_SZ;
            else if (rdoLVNS2.Checked) option.conv = LeNS.LeNSConvMode.MODE_KZ;
            else if (rdoLVNS3.Checked) option.conv = LeNS.LeNSConvMode.MODE_TH;

            option.pakPath = txtPakPath.Text;
            option.outputPath = txtOutputPath.Text;

            if (rdoBgmCd.Checked) option.bgm = LeNS.LeNSBgmMode.MODE_CD;
            if (rdoBgmMp3.Checked) option.bgm = LeNS.LeNSBgmMode.MODE_MP3;
            if (rdoBgmOgg.Checked) option.bgm = LeNS.LeNSBgmMode.MODE_Ogg;

            convDialog.Option = option;
            if (convDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("�����͐���I�����܂����B", 
                                "�ϊ�����", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Information);
            }
        }

        // �ϊ��O�`�F�b�N
        private Boolean checkConvert()
        {
            /*
            // �܂��͍����������Ȃ̂ŁA���J����Ȃ�`�F�b�N������
            if (!rdoLVNS2.Checked)
            {
                MessageBox.Show("���o�[�W�����͍��ȊO���Ή��ł��B", "�`�F�b�N�G���[", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            */

            // PAK�p�X�`�F�b�N
            if (txtPakPath.Text.Equals(""))
            {
                MessageBox.Show("PAK�t�@�C���p�X�����w��ł��B", "�`�F�b�N�G���[", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else if (!Directory.Exists(txtPakPath.Text))
            {
                MessageBox.Show("PAK�t�@�C���p�X�����݂��܂���B", "�`�F�b�N�G���[", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
            {
                // �e�X�K�v�t�@�C���̃`�F�b�N
                // ��(MAX_DATA.PAK)
                if (rdoLVNS1.Checked)
                {
                    if (!File.Exists(txtPakPath.Text + "\\MAX_DATA.PAK"))
                    {
                        MessageBox.Show("MAX_DATA.PAK�����݂��܂���B", "�`�F�b�N�G���[", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                // ��(MAX2DATA.PAK)
                else if (rdoLVNS2.Checked)
                {
                    if (!File.Exists(txtPakPath.Text + "\\MAX2DATA.PAK")) {
                        MessageBox.Show("MAX2DATA.PAK�����݂��܂���B", "�`�F�b�N�G���[", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                // To Heart(LVNS3DAT.PAK�ALVNS3SCN.PAK�ALVNS3.EXE)
                else if (rdoLVNS3.Checked)
                {
                    if (!File.Exists(txtPakPath.Text + "\\LVNS3DAT.PAK"))
                    {
                        MessageBox.Show("LVNS3DAT.PAK�����݂��܂���B", "�`�F�b�N�G���[", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    else if (!File.Exists(txtPakPath.Text + "\\LVNS3SCN.PAK"))
                    {
                        MessageBox.Show("LVNS3SCN.PAK�����݂��܂���B", "�`�F�b�N�G���[", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    else if (!File.Exists(txtPakPath.Text + "\\LVNS3.EXE"))
                    {
                        MessageBox.Show("LVNS3.EXE�����݂��܂���B", "�`�F�b�N�G���[", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
            }

            // �o�̓p�X�`�F�b�N
            if (txtOutputPath.Text.Equals(""))
            {
                MessageBox.Show("�o�̓p�X�����w��ł��B", "�`�F�b�N�G���[", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else if (!Directory.Exists(txtOutputPath.Text))
            {
                MessageBox.Show("�o�̓p�X�����݂��܂���B", "�`�F�b�N�G���[", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            // �m�F
            if (MessageBox.Show("�ϊ����s���܂����H", "�m�F", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return false;
            }

            return true;
        }

        private void comLVNS_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGetRegPath.Checked)
            {
                setPakPath();
            }
        }
    }
}
