using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Windows.Forms;

// LeafPackファイルクラス
namespace LeNS
{
    class LeafPack
    {
        // タイプ
        public enum LeafPackType
        {
            LPTYPE_SIZUWIN,
            LPTYPE_KIZUWIN,
            LPTYPE_TOHEART,
            LPTYPE_SAORIN,
            LPTYPE_UNKNOWN
        }

        public enum LeNSFileType
        {
            ScenarioFile,   // シナリオファイル
            BGGraphic,      // 背景、ビジュアル等(640x400にリサイズ)
            CharaGraphic,   // 立ち絵(320or640 x 400にリサイズ)
            EtcGraphic,     // その他グラフィック(リサイズなし)
            SpecialFile,    // 特殊(要専用処理)
            SoundFile       // 効果音
        }

        // ファイル情報
        public struct LeafFileInfo
        {
            public String Name;                 /* ファイル名           */
            public Int32 Pos;                   /* 先頭ポインタ         */
            public Int32 Length;                /* ファイル長           */
            public LeNSFileType Type;           /* ファイルタイプ       */
            public Int32 PlayTime;              /* 再生時間(P16のみ)    */
        }

        private FileStream fs;                  /* ファイルストリーム   */
        private BinaryReader br;

        private const int LP_KEY_LEN = 11;      /* 展開用キー長         */
        private Int16[] key;                    /* 展開用キー           */

        private LeafPackType _type;             /* パックの種別         */
        private long _size;                     /* サイズ               */
        private Int16 _fileNum;                 /* パック中のファイル数 */

        private Int16 _scenarioNum;
        private Int16 _otherNum;

        /* ファイルテーブル     */
        private Dictionary<String, LeafFileInfo> _files;

        // プロパティ
        public LeafPackType Type { get { return _type; } }
        public long Size { get { return _size; } }
        public long FileNum { get { return (long)_fileNum; } }
        public Dictionary<String, LeafFileInfo> Files { get { return _files; } }
        public long ScenarioNum { get { return (long)_scenarioNum; } }
        public long OtherNum { get { return (long)_otherNum; } }

        // オープン
        public void Open(String Path, FileMode Mode)
        {
            key = new Int16[LP_KEY_LEN];
            _type = LeafPackType.LPTYPE_UNKNOWN;
            _size = -1;
            _fileNum = -1;
            _scenarioNum = 0;
            _otherNum = 0;
            _files = new Dictionary<String, LeafFileInfo>();


            // ファイルストリームオープン
            fs = new FileStream(Path, Mode);

            // バイナリリーダオープン
            br = new BinaryReader(fs);

            if (Mode == FileMode.Open)
            {
                // ファイルサイズ
                _size = fs.Length;

                // マジックコードのチェック
                String strBuf = new String(br.ReadChars(8));
                if (!strBuf.Equals("LEAFPACK"))
                {
                    throw new InvalidDataException("LeafPackではありません。");
                }

                // ファイル数の取得
                _fileNum = br.ReadInt16();

                // タイプチェック
                switch (_fileNum)
                {
                    case 0x0248:
                        // LVNS3DAT.PAK
                        _type = LeafPackType.LPTYPE_TOHEART;
                        break;
                    case 0x03e1:
                        // LVNS3SCN.PAK
                        _type = LeafPackType.LPTYPE_TOHEART;
                        break;
                    case 0x01fb:
                        // MAX2DATA.PAK
                        _type = LeafPackType.LPTYPE_KIZUWIN;
                        break;
                    case 0x0193:
                        // MAX_DATA.PAK
                        _type = LeafPackType.LPTYPE_SIZUWIN;
                        break;
                    case 0x0072:
                        _type = LeafPackType.LPTYPE_SAORIN;
                        break;
                    default:
                        _type = LeafPackType.LPTYPE_UNKNOWN;
                        break;
                }

                // KEYの取得
                guessKey();

                // ファイルテーブルの取得
                extractTable();
            }
            else
            {
                throw new InvalidOperationException("無効なモードでオープンされました。");
            }
        }

        // クローズ
        public void Close()
        {
            if (fs != null)
            {
                fs.Close();
            }
            if (br != null)
            {
                br.Close();
            }
        }

        // KEYの取得
        private void guessKey()
        {
            byte[] buf;

            // ファイルテーブル先頭にシーク
            fs.Seek(-(24 * _fileNum), SeekOrigin.End);

            // 64バイト分読み取る
            buf = br.ReadBytes(64);

            // キー生成
            // AND 0xffは3バイト以降切り捨ての意味
            key[0] = buf[11];
            key[1] = (Int16)((buf[12] - 0x0a) & 0xff);
            key[2] = buf[13];
            key[3] = buf[14];
            key[4] = buf[15];
            key[5] = (Int16)((buf[38] - buf[22] + key[0]) & 0xff);
            key[6] = (Int16)((buf[39] - buf[23] + key[1]) & 0xff);
            key[7] = (Int16)((buf[62] - buf[46] + key[2]) & 0xff);
            key[8] = (Int16)((buf[63] - buf[47] + key[3]) & 0xff);
            key[9] = (Int16)((buf[20] - buf[36] + key[3]) & 0xff);
            key[10] = (Int16)((buf[21] - buf[37] + key[4]) & 0xff);
        }

        // ファイルテーブルの取得
        private void extractTable()
        {
            int i, j, k = 0;
            char[] filename;
            byte[] b;
            LeafFileInfo info;

            // ファイルテーブル先頭にシーク
            fs.Seek(-(24 * _fileNum), SeekOrigin.End);

            // ファイルテーブル取得
            for (i = 0; i < _fileNum; i++)
            {
                info = new LeafFileInfo();

                filename = new char[12];
                // ファイル名
                for (j = 0; j < 12; j++)
                {
                    filename[j] = (char)((br.ReadByte() - key[k]) & 0xff);
                    k = (++k) % LP_KEY_LEN;
                }
                info.Name = regularizeName(filename);

                // ファイル位置
                b = br.ReadBytes(4);
                for (j = 0; j < 4; j++)
                {
                    b[j] = (byte)(b[j] - key[k] & 0xff);
                    k = (++k) % LP_KEY_LEN;
                }
                info.Pos = (int)(b[3] << 24 | b[2] << 16 | b[1] << 8 | b[0]);

                // ファイル長
                b = br.ReadBytes(4);
                for (j = 0; j < 4; j++)
                {
                    b[j] = (byte)(b[j] - key[k] & 0xff);
                    k = (++k) % LP_KEY_LEN;
                }
                info.Length = (int)(b[3] << 24 | b[2] << 16 | b[1] << 8 | b[0]);

                // 次ファイル位置(未使用？)
                b = br.ReadBytes(4);
                for (j = 0; j < 4; j++)
                {
                    b[j] = (byte)(b[j] - key[k] & 0xff);
                    k = (++k) % LP_KEY_LEN;
                }

                // ファイルタイプ(主に画像変換で使用)
                info.Type = getFileType(_type, info.Name);

                // ＰＣＭ再生時間(msec)
                if (info.Type == LeNSFileType.SoundFile)
                {
                    info.PlayTime = info.Length * 1000 / 44100;
                }
                else
                {
                    info.PlayTime = -1;
                }

                _files.Add(info.Name, info);
            }
        }

        // ファイル名正規化
        private String regularizeName(char[] name)
        {
            int i;
            char[] buf = new char[12];

            // スペースまで取得
            for (i = 0; i < 8 && name[i] != 0x20; i++)
            {
                buf[i] = name[i];
            }

            // 拡張子
            buf[i++] = '.';
            buf[i++] = name[8];
            buf[i++] = name[9];
            buf[i++] = name[10];

            // 正規化したファイル名を返す(NULL文字は切り捨て)
            return (new String(buf)).TrimEnd('\0').ToUpper();
        }

        // ファイルを得る
        public byte[] Get(String FileName)
        {
            byte[] ret;
            LeafFileInfo info;

            if (_files.ContainsKey(FileName))
            {
                info = _files[FileName];
                // 該当するファイルをbyte配列で返す
                fs.Seek(info.Pos, SeekOrigin.Begin);
                ret = br.ReadBytes(info.Length);

                // 読み出しデータの復号化
                int i;
                for (i = 0; i < info.Length; i++)
                {
                    int buf = (int)ret[i];
                    buf = (buf - key[i % LP_KEY_LEN]) & 0xff;
                    ret[i] = (byte)buf;
                }

                // 結果を返す
                return ret;
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        // ファイル存在確認
        public Boolean Exists(String FileName)
        {
            return _files.ContainsKey(FileName);
        }

        // ファイルタイプの判別処理
        private LeNSFileType getFileType(LeafPackType packType, String fileName)
        {
            LeNSFileType file_type = LeNSFileType.EtcGraphic;
            String ext = Path.GetExtension(fileName);
            String typeString;

            switch (packType)
            {
                case LeafPackType.LPTYPE_SIZUWIN:
                    if (fileName.Equals("LEAF.LFG"))
                    {
                        // リーフロゴ(雫以前)
                        file_type = LeNSFileType.EtcGraphic;
                    }
                    else if (fileName.Equals("KNJ_ALL.KNJ"))
                    {
                        // 漢字ファイル
                        file_type = LeNSFileType.SpecialFile;
                    }
                    else if (fileName.Substring(0, 3).Equals("SCN"))
                    {
                        // シナリオファイル
                        file_type = LeNSFileType.ScenarioFile;
                    }
                    else if (ext.Equals(".P16"))
                    {
                        // 効果音ファイル
                        file_type = LeNSFileType.SoundFile;
                    }
                    else if (fileName.Substring(0, 3).Equals("HVS"))
                    {
                        // Ｈシーン
                        file_type = LeNSFileType.BGGraphic;
                    }
                    else if (fileName.Substring(0, 5).Equals("MAX_C"))
                    {
                        // 立ち絵
                        file_type = LeNSFileType.CharaGraphic;
                    }
                    else if (fileName.Substring(0, 4).Equals("NEXT"))
                    {
                        // 痕デモ用？
                        file_type = LeNSFileType.BGGraphic;
                    }
                    else if (fileName.Substring(0, 5).Equals("MAX_S"))
                    {
                        // 背景
                        file_type = LeNSFileType.BGGraphic;
                    }
                    else if (fileName.Substring(0, 2).Equals("OP"))
                    {
                        // オープニング
                        file_type = LeNSFileType.BGGraphic;
                    }
                    else if (fileName.Substring(0, 5).Equals("TITLE"))
                    {
                        // タイトル
                        file_type = LeNSFileType.EtcGraphic;
                    }
                    else if (fileName.Substring(0, 3).Equals("VIS"))
                    {
                        // ビジュアルシーン
                        file_type = LeNSFileType.BGGraphic;
                    }
                    else
                    {
                        throw new InvalidDataException("未定義のファイルを検出しました。");
                    }
                    break;
                case LeafPackType.LPTYPE_KIZUWIN:
                    if (fileName.Equals("LEAF.LFG"))
                    {
                        // リーフロゴ(痕以降)
                        file_type = LeNSFileType.SpecialFile;
                    }
                    else if (fileName.Equals("KNJ_ALL.KNJ"))
                    {
                        // 漢字ファイル
                        file_type= LeNSFileType.SpecialFile;
                    }
                    else if (ext.Equals(".SCN"))
                    {
                        // シナリオファイル
                        file_type = LeNSFileType.ScenarioFile;
                    }
                    else if (ext.Equals(".P16"))
                    {
                        // 効果音ファイル
                        file_type = LeNSFileType.SoundFile;
                    }
                    else
                    {
                        typeString = fileName.Substring(0, 4);
                        switch (typeString)
                        {
                            case "BLDW":    // 血しぶき
                                file_type = LeNSFileType.BGGraphic;
                                break;
                            case "CLAW":    // 爪
                                file_type = LeNSFileType.BGGraphic;
                                break;
                            case "TITL":    // タイトル
                                file_type = LeNSFileType.EtcGraphic;
                                break;
                            default:
                                typeString = fileName.Substring(0, 1);
                                switch (typeString)
                                {
                                    case "C":   // 立ち絵
                                        file_type = LeNSFileType.CharaGraphic;
                                        break;
                                    case "H":   // Ｈシーン
                                        file_type = LeNSFileType.BGGraphic;
                                        break;
                                    case "O":   // オープニング
                                        file_type = LeNSFileType.EtcGraphic;
                                        break;
                                    case "S":   // 背景
                                        file_type = LeNSFileType.BGGraphic;
                                        break;
                                    case "V":   // ビジュアルシーン
                                        file_type = LeNSFileType.BGGraphic;
                                        break;
                                    default:
                                        throw new InvalidDataException("未定義のファイルを検出しました。");
                                }
                                break;
                        }
                    }

                    break;
                case LeafPackType.LPTYPE_TOHEART:
                    break;
                default:    // さおりん、その他は判別未対応
                    break;
            }

            if (file_type == LeNSFileType.ScenarioFile)
            {
                _scenarioNum++;
            }
            else
            {
                _otherNum++;
            }
            return file_type;
        }
    }
}
