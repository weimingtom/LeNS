using System;
using System.Collections.Generic;
using System.Text;

namespace LeNS
{
    public enum LeNSConvResult
    {
        ok,
        warning,
        error
    }

    abstract class LeNSConvFile
    {
        protected LeNSConvOption option;

        protected String _message = "";
        public String Message { get { return _message; } }

        public abstract LeNSConvResult Conv(LeafPack.LeafFileInfo SrcInfo, byte[] SrcData);

        public LeNSConvFile(LeNSConvOption Option)
        {
            option = Option;
        }

        protected virtual String getSavePath()
        {
            return option.outputPath;
        }

        protected virtual String getSavePath(String fileName)
        {
            return getSavePath() + "\\" + GetSaveName(fileName);
        }

        public virtual String GetSaveName(String FileName)
        {
            return FileName;
        }

        // ないとめんどいので作成
        // シナリオ以外でも使えるとこあるかも。
        protected static Int16 getShort(byte[] data, int p)
        {
            return (Int16)(data[p] | data[p + 1] << 8);
        }

        protected static Int32 getLong(byte[] data, int p)
        {
            return (Int32)(data[p] | data[p + 1] << 8 | data[p + 2] << 16 | data[p + 3] << 24);
        }

        // 圧縮展開(LFG)
        protected static byte[] lzs(byte[] LoadBuffer, int Size, int p)
        {
            byte[] save_buf = new byte[Size];
            byte[] ring = new byte[0x1000];

            int i, j;
            int c, m;
            int flag = 0;
            int pos, len;

            // データの展開
            for (i = 0, c = 0, m = 0xfee; i < Size; )
            {
                // フラグの取得
                if (--c < 0)
                {
                    flag = LoadBuffer[p++];
                    c = 7;
                }

                if (!((flag & 0x80) == 0))
                {
                    // データとしてセット
                    save_buf[i++] = ring[m++] = LoadBuffer[p++];
                    m &= 0xfff;
                }
                else
                {
                    // リングバッファにコピー
                    int data = LoadBuffer[p] + (LoadBuffer[p + 1] << 8);
                    p += 2;

                    len = (data & 0x0f) + 3;
                    pos = data >> 4;

                    for (j = 0; j < len; j++)
                    {
                        save_buf[i++] = ring[m++] = ring[pos++];
                        m &= 0xfff;
                        pos &= 0xfff;
                    }
                }
                flag = flag << 1;
            }

            return save_buf;
        }

        // 圧縮展開(SDT・シナリオデータ)
        protected static byte[] lzs2(byte[] LoadBuffer, int SaveSize, int p)
        {
            byte[] save_buf = new byte[SaveSize];
            byte[] ring = new byte[0x1011];

            int i;
            int c, m;
            byte flag = 0;
            int pos, len;
            UInt16 data;

            i = 0; c = 0; m = 0xfee;

            while (SaveSize > 0)
            {
                // フラグの取得
                if (c-- > 0)
                {
                    flag <<= 1;
                }
                else
                {
                    flag = (byte)(~(LoadBuffer[p++]));
                    c = 7;
                }

                if (!((flag & 0x80) == 0))
                {
                    // データとしてセット
                    ring[m++] = save_buf[i++] = (byte)(~(LoadBuffer[p++]));
                    m &= 0x0fff;
                    SaveSize--;
                }
                else
                {
                    // 以前に出現した場所と長さの情報の取得(2 byte)
                    data = (UInt16)(~((LoadBuffer[p]) + (LoadBuffer[p + 1] << 8)));
                    p += 2;

                    // 長さ
                    len = (data & 0xf) + 3;
                    SaveSize -= len;

                    // 以前出現した位置
                    pos = data >> 4;

                    // 以前出現した位置からコピー
                    while (len-- > 0)
                    {
                        ring[m++] = save_buf[i++] = ring[pos++];
                        pos &= 0x0fff;
                        m &= 0x0fff;
                    }
                }
            }

            return save_buf;
        }
    }
}
