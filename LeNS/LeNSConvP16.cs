using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LeNS
{
    class LeNSConvP16 : LeNSConvFile
    {
        public LeNSConvP16(LeNSConvOption Option) : base(Option) { }

        // 変換処理
        public override LeNSConvResult Conv(LeafPack.LeafFileInfo SrcInfo, byte[] SrcData)
        {
            return Conv(SrcInfo, SrcData, 1);
        }

        // 変換処理
        public LeNSConvResult Conv(LeafPack.LeafFileInfo SrcInfo, byte[] SrcData, int Number)
        {
            // 復号化すると11MHzStereoの生データになるのでヘッダ付けて出力。
            // 一部データが奇数バイトになっているので、0書き込んでサイズを合わせる。
            // 合わせないと複数回再生でバグる。
            FileStream fs;
            BinaryWriter bw;
            int length = SrcData.Length;
            if (length % 2 == 1)
            {  
                // 奇数サイズの場合はサイズを+1バイト
                length++;
            }
            length *= Number;

            fs = new FileStream(getSavePath(SrcInfo.Name, Number), FileMode.Create);

            try
            {
                bw = new BinaryWriter(fs);
                try
                {
                    //　ごりごりっと書き出す
                    bw.Write('R');                          // RIFFヘッダ
                    bw.Write('I');
                    bw.Write('F');
                    bw.Write('F');
                    bw.Write((Int32)(length + 36));         // データサイズ + 36byte
                    bw.Write('W');                          // WAVEヘッダ
                    bw.Write('A');
                    bw.Write('V');
                    bw.Write('E');
                    bw.Write('f');                          // fmtチャンク
                    bw.Write('m');
                    bw.Write('t');
                    bw.Write(' ');
                    bw.Write((Int32)(16));                  // fmtチャンクサイズ(16)
                    bw.Write((Int16)(1));                   // フォーマット(リニアPCM=1)
                    bw.Write((Int16)(2));                   // チャネル数(2)
                    bw.Write((Int32)(11025));               // サンプリングレート(11025)
                    bw.Write((Int32)(44100));               // 44100Byte/sec)
                    bw.Write((Int16)(4));                   // Byte/sample×チャンネル数
                    bw.Write((Int16)(16));                  // 16bit
                    bw.Write('d');                          // dataチャンク
                    bw.Write('a');
                    bw.Write('t');
                    bw.Write('a');
                    bw.Write((Int32)(length));              // データサイズ

                    // 再生回数分出力
                    for (int i = 1; i <= Number; i++ )
                    {
                        bw.Write(SrcData);                  // 波形データ
                        if (SrcData.Length % 2 == 1)
                        {
                            bw.Write((byte)0);
                        }
                    }
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

            return LeNSConvResult.ok;
        }

        // セーブパス取得
        protected override String getSavePath()
        {
            String path = base.getSavePath() + "\\se";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        protected String getSavePath(String fileName, int Number)
        {
            return getSavePath() + "\\" + GetSaveName(fileName, Number);
        }

        // 変換後ファイル名取得
        public override string GetSaveName(string FileName)
        {
            return GetSaveName(FileName, 1);
        }

        // 変換後ファイル名取得
        public string GetSaveName(string FileName, int Number)
        {
            if (Number == 1)
            {
                return Path.GetFileNameWithoutExtension(FileName) + ".WAV";
            }
            else
            {
                return Path.GetFileNameWithoutExtension(FileName) + "_" + Number.ToString("0") + ".WAV";
            }
        }
    }
}
