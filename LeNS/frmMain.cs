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

// メインフォーム
namespace LeNS
{
    public partial class frmMain : Form
    {
        // コンストラクタ
        public frmMain()
        {
            InitializeComponent();
        }

        // ロード
        private void frmMain_Load(object sender, EventArgs e)
        {
            setCaption();

            rdoLVNS2.Checked = true;
        }

        // キャプションセット
        private void setCaption()
        {
            this.Text = Application.ProductName + " " + getVersionString();
        }

        // PAKパス自動設定
        private void setPakPath()
        {
            // レジストリのインストール情報からPAKパスを指定
            RegistryKey regkey;
            try
            {
                // 雫
                if (rdoLVNS1.Checked)
                {
                    // 雫インスコしてないのでひょっとしたら違う可能性も…。
                    regkey = Registry.CurrentUser.OpenSubKey(@"Software\Leaf\Sizuku", false);
                }
                // 痕
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
                    // レジストリが存在しないときNULL
                    if (regkey == null)
                    {
                        txtPakPath.Text = "";
                        return;
                    }
                    // レジストリからパス取得
                    String path = (String)regkey.GetValue("DataDir");

                    // 取得したパスを複合(短縮されてるため)
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
                MessageBox.Show("例外が発生しました。\n" + ex.ToString(),
                                "Exception",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        // バージョン文字列取得
        private String getVersionString()
        {
            Version ver = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            return "Ver " + ver.ToString(3);
        }

        // フォルダ参照ボタン
        private void btnPath_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnAlias = (Button)sender;
                TextBox txtAlias = (TextBox)btnAlias.Tag;
                switch (btnAlias.Name)
                {
                    case "btnPakPath":
                        fbDialog.Description = "PAKファイルの存在するパス(インストールフォルダ)を選択して下さい";
                        fbDialog.ShowNewFolderButton = false;
                        break;
                    case "btnOutputPath":
                        fbDialog.Description = "変換ファイルの格納先を選択して下さい(空きフォルダ推奨)";
                        fbDialog.ShowNewFolderButton = true;
                        break;
                }

                fbDialog.SelectedPath = txtAlias.Text;
                if (fbDialog.ShowDialog() == DialogResult.OK)
                {
                    txtAlias.Text = fbDialog.SelectedPath;
                }
            }
            // ネットワーク越しに実行するとセキュリティ例外が発生するのでCatch
            catch (SecurityException ex)
            {
                Debug.Print(ex.ToString());
                MessageBox.Show("セキュリティ例外が発生しました。\n不適切なパスから実行した可能性があります。",
                                "SecurityException",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
                MessageBox.Show("例外が発生しました。\n" + ex.ToString(),
                                "Exception",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        // 終了ボタン
        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // 変換開始ボタン
        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (!checkConvert())
            {
                return;
            }

            frmProgress convDialog = new frmProgress();
            
            // 起動オプションの設定
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
                MessageBox.Show("処理は正常終了しました。", 
                                "変換完了", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Information);
            }
        }

        // 変換前チェック
        private Boolean checkConvert()
        {
            /*
            // まずは痕からやるつもりなので、公開するならチェックかける
            if (!rdoLVNS2.Checked)
            {
                MessageBox.Show("現バージョンは痕以外未対応です。", "チェックエラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            */

            // PAKパスチェック
            if (txtPakPath.Text.Equals(""))
            {
                MessageBox.Show("PAKファイルパスが未指定です。", "チェックエラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else if (!Directory.Exists(txtPakPath.Text))
            {
                MessageBox.Show("PAKファイルパスが存在しません。", "チェックエラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
            {
                // 各々必要ファイルのチェック
                // 雫(MAX_DATA.PAK)
                if (rdoLVNS1.Checked)
                {
                    if (!File.Exists(txtPakPath.Text + "\\MAX_DATA.PAK"))
                    {
                        MessageBox.Show("MAX_DATA.PAKが存在しません。", "チェックエラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                // 痕(MAX2DATA.PAK)
                else if (rdoLVNS2.Checked)
                {
                    if (!File.Exists(txtPakPath.Text + "\\MAX2DATA.PAK")) {
                        MessageBox.Show("MAX2DATA.PAKが存在しません。", "チェックエラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                // To Heart(LVNS3DAT.PAK、LVNS3SCN.PAK、LVNS3.EXE)
                else if (rdoLVNS3.Checked)
                {
                    if (!File.Exists(txtPakPath.Text + "\\LVNS3DAT.PAK"))
                    {
                        MessageBox.Show("LVNS3DAT.PAKが存在しません。", "チェックエラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    else if (!File.Exists(txtPakPath.Text + "\\LVNS3SCN.PAK"))
                    {
                        MessageBox.Show("LVNS3SCN.PAKが存在しません。", "チェックエラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    else if (!File.Exists(txtPakPath.Text + "\\LVNS3.EXE"))
                    {
                        MessageBox.Show("LVNS3.EXEが存在しません。", "チェックエラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
            }

            // 出力パスチェック
            if (txtOutputPath.Text.Equals(""))
            {
                MessageBox.Show("出力パスが未指定です。", "チェックエラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else if (!Directory.Exists(txtOutputPath.Text))
            {
                MessageBox.Show("出力パスが存在しません。", "チェックエラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            // 確認
            if (MessageBox.Show("変換を行いますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
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
