using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace LeNS
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }

    public struct LeNSConvOption
    {
        public String pakPath;
        public String outputPath;
        public LeNSConvMode conv;
        public LeNSBgmMode bgm;
    }

    public enum LeNSConvMode
    {
        MODE_SZ,
        MODE_KZ,
        MODE_TH
    }

    public enum LeNSBgmMode
    {
        MODE_CD,
        MODE_MP3,
        MODE_Ogg
    }

}
