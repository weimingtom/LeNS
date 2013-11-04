using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace LeNS
{
    abstract class LeNSConvSCN : LeNSConvFile
    {
        // ＢＧＭのフェード時間
        public int BgmFadeTime = 0;
        public Dictionary<String, int> SoundCache;

        protected char[] fontTable;
        protected Dictionary<byte, int> flagTable = new Dictionary<byte, int>();

        protected Dictionary<String, String> labelCache;
        protected String includeFile = "";

        public LeNSConvSCN(LeNSConvOption Option)
            : base(Option)
        {
            labelCache = new Dictionary<String, String>();
        }

        // アドレスラベル生成
        protected String getLabelString(int no, int address)
        {
            return "*scn" + no.ToString("000") + "_0x" + address.ToString("x4");
        }

        // フラグ番号取得
        protected int getFragNo(byte no)
        {
            if (!flagTable.ContainsKey(no)) {
                throw new InvalidDataException("フラグテーブルに存在しないフラグです。[" + no.ToString("x") + "]");
            }
            return flagTable[no];
        }


        protected enum LeNSBgmOperation
        {
            Play,
            FadeOut,
            Stop,
            FadeStart
        }

        protected String getBgmString(int no, LeNSBgmOperation mode)
        {
            String bgm_src = "";
            switch (option.bgm)
            {
                case LeNSBgmMode.MODE_MP3:
                    bgm_src = String.Format("{0}AudioTrack_{1:00}.MP3", LeNSMain.BgmPath, no);
                    break;
                case LeNSBgmMode.MODE_Ogg:
                    bgm_src = String.Format("{0}AudioTrack_{1:00}.OGG", LeNSMain.BgmPath, no);
                    break;
                case LeNSBgmMode.MODE_CD:
                default:
                    bgm_src = "*" + no.ToString();
                    break;
            }

            switch (mode)
            {
                case LeNSBgmOperation.Play:
                    if (option.bgm == LeNSBgmMode.MODE_CD)
                    {
                        return String.Format("play \"{0}\"\r\n", bgm_src);
                    }
                    else
                    {
                        return String.Format("bgmfadein 0\r\nbgm \"{0}\"\r\n", bgm_src);
                    }
                case LeNSBgmOperation.FadeOut:
                    if (option.bgm == LeNSBgmMode.MODE_CD)
                    {
                        return String.Format("cdfadeout {0}\r\nplaystop\r\n", BgmFadeTime);
                    }
                    else
                    {
                        return String.Format("bgmfadeout {0}\r\nbgmstop\r\n", BgmFadeTime);
                    }
                case LeNSBgmOperation.FadeStart:
                    if (option.bgm == LeNSBgmMode.MODE_CD)
                    {
                        return String.Format("cdfadeout {0}\r\nplaystop\r\nplay \"{1}\"\r\n",BgmFadeTime, bgm_src);
                    }
                    else
                    {
                        return String.Format("bgmfadeout {0}\r\nbgmstop\r\nbgm \"{1}\"\r\n",BgmFadeTime, bgm_src);
                    }
                case LeNSBgmOperation.Stop:
                default:
                    if (option.bgm == LeNSBgmMode.MODE_CD)
                    {
                        return String.Format("cdfadeout 0\r\nplaystop\r\n");
                    }
                    else
                    {
                        return String.Format("bgmfadeout 0\r\nbgmstop\r\n");
                    }
            }
        }

        public override LeNSConvResult Conv(LeafPack.LeafFileInfo SrcInfo, byte[] SrcData)
        {
            int p_scn;
            int p_txt;
            int scn_size;
            int txt_size;
            byte[] scn_data;
            byte[] txt_data;

            // シナリオ、テキストデータの開始位置を取得
            p_scn = getShort(SrcData, 0) * 16;
            p_txt = getShort(SrcData, 2) * 16;

            // それぞれのサイズを取得
            scn_size = getLong(SrcData, p_scn);
            txt_size = getLong(SrcData, p_txt);

            int tmp_len;
            byte[] tmp_data;

            // シナリオデータの解凍
            tmp_len = p_txt - p_scn;
            tmp_data = new byte[tmp_len];
            for (int p = 0; p < tmp_len; p++)
            {
                tmp_data[p] = SrcData[p_scn + p];
            }
            scn_data = lzs2(tmp_data, scn_size, 4);

            // テキストデータの解凍
            tmp_len = SrcData.Length - p_txt;
            tmp_data = new byte[tmp_len];
            for (int p = 0; p < tmp_len; p++)
            {
                tmp_data[p] = SrcData[p_txt + p];
            }
            txt_data = lzs2(tmp_data, txt_size, 4);

            // 解凍後のダンプデータ出力(解析用)
            //dumpData(srcInfo, scnData, txtData);
            return decodeEvent(SrcInfo, scn_data, txt_data);
        }

        // イベントデコーダ(継承先で実装)
        protected abstract LeNSConvResult decodeEvent(LeafPack.LeafFileInfo srcInfo, byte[] scnData, byte[] txtData);

        // セーブパス取得
        protected override string getSavePath(string fileName)
        {
            return getSavePath() + "\\" + GetSaveName(fileName);
        }

        // 変換後ファイル名取得
        public override string GetSaveName(string FileName)
        {
            return "0.TXT";
        }

        // ダンプデータ出力(デバッグ用)
        protected void dumpData(LeafPack.LeafFileInfo srcInfo, byte[] scnData, byte[] txtData)
        {
            FileStream fs;
            fs = new FileStream(getSavePath() + "\\" + Path.GetFileNameWithoutExtension(srcInfo.Name) + ".EVT", FileMode.Create);
            try
            {
                BinaryWriter bw = new BinaryWriter(fs);
                try
                {
                    bw.Write(scnData);
                }
                finally
                {
                    bw.Close();
                }
            }
            finally
            {
                fs.Close();
            }

            fs = new FileStream(getSavePath() + "\\" + Path.GetFileNameWithoutExtension(srcInfo.Name) + ".MSG", FileMode.Create);
            try
            {
                BinaryWriter bw = new BinaryWriter(fs);
                try
                {
                    bw.Write(txtData);
                }
                finally
                {
                    bw.Close();
                }
            }
            finally
            {
                fs.Close();
            }
        }

        // 書き込み用ファイル作成
        public void CreateFile()
        {
            // インクルードファイルを開く
            String include_text;
            String include_path = AppDomain.CurrentDomain.BaseDirectory + "\\include\\" + includeFile;
            if (!File.Exists(include_path))
            {
                throw new FileNotFoundException("インクルードファイルが見つかりません。[" + includeFile + "]");
            }

            // インクルードファイルを読み込む
            FileStream in_file = new FileStream(include_path, FileMode.Open);
            try
            {
                StreamReader sr = new StreamReader(in_file, Encoding.GetEncoding(932));
                try
                {
                    include_text = sr.ReadToEnd();
                }
                finally
                {
                    sr.Close();
                }
            }
            finally
            {
                in_file.Close();
            }

            // 置換文字列を変換する
            // <$LENSVER$>
            include_text = include_text.Replace("<$LENSVER$>", Assembly.GetEntryAssembly().GetName().Version.ToString(3));
            // <$PLAY_XX$>
            for (int bgm = 2; bgm <= 99; bgm++)
            {
                String label = String.Format("<$PLAY_{0}$>", bgm.ToString("00"));
                include_text = include_text.Replace(label, getBgmString(bgm, LeNSBgmOperation.Play));
            }
            // <$BGMSTOP$>
            include_text = include_text.Replace("<$BGMSTOP$>", getBgmString(0, LeNSBgmOperation.Stop));

            // インクルードファイルを書き込み用ファイルに書き込む
            FileStream out_file = new FileStream(getSavePath(includeFile), FileMode.Create);
            try
            {
                StreamWriter sw = new StreamWriter(out_file, Encoding.GetEncoding(932));
                try
                {
                    sw.Write(include_text);
                }
                finally
                {
                    sw.Close();
                }
            }
            finally
            {
                out_file.Close();
            }
        }

        // ファイル書き込み
        protected void writeFile(LeafPack.LeafFileInfo srcInfo, StringBuilder str)
        {
            // 書き込みはアペンドで行う
            FileStream fs = new FileStream(getSavePath(srcInfo.Name), FileMode.Append);
            try
            {
                StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding(932));
                try
                {
                    sw.Write(str.ToString());
                }
                finally
                {
                    sw.Close();
                }
            }
            finally
            {
                fs.Close();
            }
        }
    }
}
