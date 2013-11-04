using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Drawing;
using System.Reflection;
using System.Drawing.Imaging;

namespace LeNS
{
    public enum LeNSConvState
    {
        Init,
        Ready,
        Execute,
        Canceled,
        Error,
        Complete
    }

    // LeNSコンバート処理本体
    class LeNSMain
    {
        public static String BgmPath = "bgm\\";
        public static String GraphicPath = "graphics\\";
        public static String SoundPath = "se\\";

        private LeNSConvState _state = LeNSConvState.Init;
        private String _errorDescription;
        private String _processingTask;
        private String _processingPack;

        private int _progressValue;
        private int _progressMax;
        private List<String> _log;

        private Dictionary<String, int> soundCache = new Dictionary<String,int>();
        public LeNSConvOption Option;
        public LeNSConvState State { get { return _state; } }
        public String ErrorDescription { get { return _errorDescription; } }
        public String ProcessingTask { get { return _processingTask; } }
        public String ProcessingPack { get { return _processingPack; } }
        public int ProgressValue { get { return _progressValue; } }
        public int ProgressMax { get { return _progressMax; } }

        public LeNSMain()
        {
            _state = LeNSConvState.Ready;

            _errorDescription = "";
            _processingTask = "";
            _processingPack = "";

            _progressValue = 0;
            _progressMax = 0;

            _log = new List<string>();
        }

        // 処理本体
        public void Run()
        {
            try
            {
                outputLog("LeNSConv Ver " + Assembly.GetEntryAssembly().GetName().Version.ToString(3));
                outputLog("Programed by RaTTiE.");
                outputLog("");
                _state = LeNSConvState.Execute;

                // コンバート対象で処理分岐
                switch (Option.conv)
                {
                    case LeNSConvMode.MODE_SZ:
                        outputLog("雫をNScripter形式に変換します", true);
                        convSizuku();
                        break;
                    case LeNSConvMode.MODE_KZ:
                        outputLog("痕をNScripter形式に変換します", true);
                        convKizuato();
                        break;
                    case LeNSConvMode.MODE_TH:
                        outputLog("To HeartをNScripter形式に変換します", true);
                        convToHeart();
                        break;
                }
            }
            catch (ThreadAbortException ex)
            {
                _errorDescription = ex.ToString();
                _processingTask = "Canceled.";
                _state = LeNSConvState.Canceled;
                Thread.ResetAbort();

                return;
            }
            catch (Exception ex)
            {
                // 例外処理(特別な処理を行わない場合はここで処理する)
                outputLog(ex.Message, true);
                outputLog("変換が異常終了しました。", true);
                _errorDescription = ex.ToString();
                _processingTask = "";
                _state = LeNSConvState.Error;
                return;
            }

            outputLog("変換が完了しました", true);
            _processingTask = "Complete.";
            _state = LeNSConvState.Complete;
        }

        public String[] FlushLog()
        {
            String[] ret;
            lock (_log)
            {
                ret = new String[_log.Count];
                _log.CopyTo(ret);
                _log.Clear();
            }
            return ret;
        }

        private void outputLog(String text, Boolean drawTimeStamp)
        {
            String buf = "";
            if (drawTimeStamp)
            {
                buf += DateTime.Now.ToString("yy/MM/dd HH:mm:ss ");
            }

            buf += text;

            lock (_log)
            {
                _log.Add(buf);
            }
        }

        private void outputLog(String text)
        {
            outputLog(text, false);
        }

        // 雫変換処理
        private void convSizuku()
        {
            convertOther("MAX_DATA.PAK");
            convertScenario("MAX_DATA.PAK");
            createBmp("BLACK.PNG", new Size(640, 400), Color.Black);
            createBmp("WHITE.PNG", new Size(640, 400), Color.White);
        }

        // 痕変換処理
        private void convKizuato()
        {
            convertOther("MAX2DATA.PAK");
            convertScenario("MAX2DATA.PAK");
            createBmp("BLACK.PNG", new Size(640, 400), Color.Black);
            createBmp("WHITE.PNG", new Size(640, 400), Color.White);
        }

        // To Heart変換処理
        private void convToHeart()
        {
            convertOther("LVNS3DAT.PAK");
            convertScenario("LVNS3SCN.PAK");
            createBmp("BLACK.PNG", new Size(640, 480), Color.Black);
            createBmp("WHITE.PNG", new Size(640, 480), Color.White);
        }

        // PAKファイル変換展開(素材ファイル)
        private void convertOther(String fileName)
        {
            _progressValue = 0;
            _progressMax = 0;
            LeafPack pak = new LeafPack();

            LeNSConvResult result;
            LeNSConvLFG conv_lfg = new LeNSConvLFG(Option);
            LeNSConvSDT conv_sdt = new LeNSConvSDT(Option);
            LeNSConvP16 conv_p16 = new LeNSConvP16(Option);

            switch (Option.conv)
            {
                case LeNSConvMode.MODE_KZ:
                    conv_lfg.LoadPaletteMap("kizuato.pal");
                    break;
            }

            try
            {
                outputLog(fileName + "の素材ファイル変換を試みます", false);
                _processingPack = fileName;
                _processingTask = "Extracting... " + fileName;
                pak.Open(Option.pakPath + "\\" + fileName, System.IO.FileMode.Open);
                _progressValue = 0;
                _progressMax = (int)pak.OtherNum;
                outputLog(fileName + " = " + _progressMax.ToString() + "files.");

                foreach (LeafPack.LeafFileInfo info in pak.Files.Values)
                {
                    String ext = Path.GetExtension(info.Name);

                    if (info.Type != LeafPack.LeNSFileType.ScenarioFile)
                    {
                        _processingTask = fileName + "@" + info.Name;
                        _progressValue++;

                        // 拡張子ごとに対応するファイルへとコンバートする
                        if (info.Type == LeafPack.LeNSFileType.SpecialFile)
                        {
                            // 特殊ファイル変換
                        }
                        else
                        {
                            switch (ext)
                            {
                                // シナリオファイルはスキップ
                                case ".SCN":
                                    break;
                                // 画像ファイル(LFG)
                                case ".LFG":
                                    _processingTask += " -> " + conv_lfg.GetSaveName(info.Name);
                                    result = conv_lfg.Conv(info, pak.Get(info.Name));
                                    if (result == LeNSConvResult.warning)
                                    {
                                        outputLog(conv_lfg.Message, true);
                                    }

                                    break;
                                // 画像ファイル(SDT)
                                case ".SDT":
                                    _processingTask += " -> " + conv_sdt.GetSaveName(info.Name);
                                    result = conv_sdt.Conv(info, pak.Get(info.Name));
                                    if (result == LeNSConvResult.warning)
                                    {
                                        outputLog(conv_sdt.Message, true);
                                    }

                                    break;
                                // 画像ファイル(LF2)
                                case ".LF2":
                                    //outputLog(info.Name + ":処理未作成です", true);
                                    _processingTask += " -> " + Path.GetFileNameWithoutExtension(info.Name) + ".PNG";
                                    break;
                                // 音声ファイル
                                case ".P16":
                                    _processingTask += " -> " + conv_p16.GetSaveName(info.Name);
                                    result = conv_p16.Conv(info, pak.Get(info.Name));
                                    if (result == LeNSConvResult.warning) outputLog(conv_p16.Message, true);
                                    soundCache.Add(info.Name, info.PlayTime);

                                    // 複数回再生が必要なファイルを生成
                                    // ※他に上手いやり方がない…。
                                    switch (Option.conv)
                                    {
                                        case LeNSConvMode.MODE_KZ:
                                            switch(info.Name.ToUpper())
                                            {
                                                case "KZ_VD04.P16":
                                                    result = conv_p16.Conv(info, pak.Get(info.Name), 2);
                                                    if (result == LeNSConvResult.warning) outputLog(conv_p16.Message, true);
                                                    break;
                                                case "KZ_VD08.P16":
                                                    result = conv_p16.Conv(info, pak.Get(info.Name), 2);
                                                    if (result == LeNSConvResult.warning) outputLog(conv_p16.Message, true);
                                                    result = conv_p16.Conv(info, pak.Get(info.Name), 3);
                                                    if (result == LeNSConvResult.warning) outputLog(conv_p16.Message, true);
                                                    break;
                                                case "KZ_VD12.P16":
                                                    result = conv_p16.Conv(info, pak.Get(info.Name), 2);
                                                    if (result == LeNSConvResult.warning) outputLog(conv_p16.Message, true);
                                                    break;
                                                case "KZ_VD19.P16":
                                                    result = conv_p16.Conv(info, pak.Get(info.Name), 2);
                                                    if (result == LeNSConvResult.warning) outputLog(conv_p16.Message, true);
                                                    break;
                                            }
                                            break;
                                    }

                                    break;
                                // 未定義
                                default:
                                    outputLog(info.Name + ":未定義ファイル形式のためスキップされました", true);
                                    _processingTask += " : Undefined Type.";
                                    break;
                            }
                        }
                    }
                    else
                    {
                        _processingTask = "searching...";
                    }
                }
            }
            finally
            {
                pak.Close();
                _processingPack = "";
                _progressValue = 0;
                _progressMax = 0;
            }
        }

        // PAKファイル変換展開(シナリオファイル)
        private void convertScenario(String fileName)
        {
            _progressValue = 0;
            _progressMax = 0;
            LeafPack pak = new LeafPack();

            LeNSConvResult result;
            LeNSConvSCN conv_scn;

            // コンバータオブジェクトを生成
            switch (Option.conv)
            {
                case LeNSConvMode.MODE_SZ:
                    conv_scn = new LeNSConvSCNSZ(Option);
                    break;
                case LeNSConvMode.MODE_KZ:
                    conv_scn = new LeNSConvSCNKZ(Option);
                    break;
                case LeNSConvMode.MODE_TH:
                    conv_scn = new LeNSConvSCNTH(Option);
                    break;
                default:
                    // ここには来ないはず
                    return;
            }

            conv_scn.SoundCache = soundCache;

            try
            {
                outputLog(fileName + "のシナリオファイル変換を試みます", false);
                conv_scn.CreateFile();
                _processingPack = fileName;
                _processingTask = "Extracting... " + fileName;
                pak.Open(Option.pakPath + "\\" + fileName, System.IO.FileMode.Open);
                _progressValue = 0;
                _progressMax = (int)pak.ScenarioNum;
                outputLog(fileName + " = " + _progressMax.ToString() + "files.");

                foreach (LeafPack.LeafFileInfo info in pak.Files.Values)
                {
                    String ext = Path.GetExtension(info.Name);

                    if (info.Type == LeafPack.LeNSFileType.ScenarioFile)
                    {
                        _processingTask = fileName + "@" + info.Name;
                        _progressValue++;

                        // 変換処理コール
                        _processingTask += " -> " + conv_scn.GetSaveName(info.Name);
                        result = conv_scn.Conv(info, pak.Get(info.Name));
                    }
                    else
                    {
                        _processingTask = "searching...";
                    }
                }
            }
            finally
            {
                pak.Close();
                _processingPack = "";
                _progressValue = 0;
                _progressMax = 0;
            }
        }

        private void createBmp(String fileName, Size newSize, Color color)
        {
            // めんどいんでGraphicsで
            Bitmap bmp = new Bitmap(newSize.Width, newSize.Height, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(bmp);

            g.Clear(color);
            g.Dispose();

            bmp.Save(Option.outputPath + "\\" + GraphicPath + "\\" + fileName);
            bmp.Dispose();
        }
    }
}
