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

    // LeNS�R���o�[�g�����{��
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

        // �����{��
        public void Run()
        {
            try
            {
                outputLog("LeNSConv Ver " + Assembly.GetEntryAssembly().GetName().Version.ToString(3));
                outputLog("Programed by RaTTiE.");
                outputLog("");
                _state = LeNSConvState.Execute;

                // �R���o�[�g�Ώۂŏ�������
                switch (Option.conv)
                {
                    case LeNSConvMode.MODE_SZ:
                        outputLog("����NScripter�`���ɕϊ����܂�", true);
                        convSizuku();
                        break;
                    case LeNSConvMode.MODE_KZ:
                        outputLog("����NScripter�`���ɕϊ����܂�", true);
                        convKizuato();
                        break;
                    case LeNSConvMode.MODE_TH:
                        outputLog("To Heart��NScripter�`���ɕϊ����܂�", true);
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
                // ��O����(���ʂȏ������s��Ȃ��ꍇ�͂����ŏ�������)
                outputLog(ex.Message, true);
                outputLog("�ϊ����ُ�I�����܂����B", true);
                _errorDescription = ex.ToString();
                _processingTask = "";
                _state = LeNSConvState.Error;
                return;
            }

            outputLog("�ϊ����������܂���", true);
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

        // ���ϊ�����
        private void convSizuku()
        {
            convertOther("MAX_DATA.PAK");
            convertScenario("MAX_DATA.PAK");
            createBmp("BLACK.PNG", new Size(640, 400), Color.Black);
            createBmp("WHITE.PNG", new Size(640, 400), Color.White);
        }

        // ���ϊ�����
        private void convKizuato()
        {
            convertOther("MAX2DATA.PAK");
            convertScenario("MAX2DATA.PAK");
            createBmp("BLACK.PNG", new Size(640, 400), Color.Black);
            createBmp("WHITE.PNG", new Size(640, 400), Color.White);
        }

        // To Heart�ϊ�����
        private void convToHeart()
        {
            convertOther("LVNS3DAT.PAK");
            convertScenario("LVNS3SCN.PAK");
            createBmp("BLACK.PNG", new Size(640, 480), Color.Black);
            createBmp("WHITE.PNG", new Size(640, 480), Color.White);
        }

        // PAK�t�@�C���ϊ��W�J(�f�ރt�@�C��)
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
                outputLog(fileName + "�̑f�ރt�@�C���ϊ������݂܂�", false);
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

                        // �g���q���ƂɑΉ�����t�@�C���ւƃR���o�[�g����
                        if (info.Type == LeafPack.LeNSFileType.SpecialFile)
                        {
                            // ����t�@�C���ϊ�
                        }
                        else
                        {
                            switch (ext)
                            {
                                // �V�i���I�t�@�C���̓X�L�b�v
                                case ".SCN":
                                    break;
                                // �摜�t�@�C��(LFG)
                                case ".LFG":
                                    _processingTask += " -> " + conv_lfg.GetSaveName(info.Name);
                                    result = conv_lfg.Conv(info, pak.Get(info.Name));
                                    if (result == LeNSConvResult.warning)
                                    {
                                        outputLog(conv_lfg.Message, true);
                                    }

                                    break;
                                // �摜�t�@�C��(SDT)
                                case ".SDT":
                                    _processingTask += " -> " + conv_sdt.GetSaveName(info.Name);
                                    result = conv_sdt.Conv(info, pak.Get(info.Name));
                                    if (result == LeNSConvResult.warning)
                                    {
                                        outputLog(conv_sdt.Message, true);
                                    }

                                    break;
                                // �摜�t�@�C��(LF2)
                                case ".LF2":
                                    //outputLog(info.Name + ":�������쐬�ł�", true);
                                    _processingTask += " -> " + Path.GetFileNameWithoutExtension(info.Name) + ".PNG";
                                    break;
                                // �����t�@�C��
                                case ".P16":
                                    _processingTask += " -> " + conv_p16.GetSaveName(info.Name);
                                    result = conv_p16.Conv(info, pak.Get(info.Name));
                                    if (result == LeNSConvResult.warning) outputLog(conv_p16.Message, true);
                                    soundCache.Add(info.Name, info.PlayTime);

                                    // ������Đ����K�v�ȃt�@�C���𐶐�
                                    // �����ɏ�肢�������Ȃ��c�B
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
                                // ����`
                                default:
                                    outputLog(info.Name + ":����`�t�@�C���`���̂��߃X�L�b�v����܂���", true);
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

        // PAK�t�@�C���ϊ��W�J(�V�i���I�t�@�C��)
        private void convertScenario(String fileName)
        {
            _progressValue = 0;
            _progressMax = 0;
            LeafPack pak = new LeafPack();

            LeNSConvResult result;
            LeNSConvSCN conv_scn;

            // �R���o�[�^�I�u�W�F�N�g�𐶐�
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
                    // �����ɂ͗��Ȃ��͂�
                    return;
            }

            conv_scn.SoundCache = soundCache;

            try
            {
                outputLog(fileName + "�̃V�i���I�t�@�C���ϊ������݂܂�", false);
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

                        // �ϊ������R�[��
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
            // �߂�ǂ����Graphics��
            Bitmap bmp = new Bitmap(newSize.Width, newSize.Height, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(bmp);

            g.Clear(color);
            g.Dispose();

            bmp.Save(Option.outputPath + "\\" + GraphicPath + "\\" + fileName);
            bmp.Dispose();
        }
    }
}
