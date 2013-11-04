using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace LeNS
{
    class LeNSConvSDT : LeNSConvGraphic
    {
        private struct LeNSSDTInfo
        {
            public int Width;
            public int Height;
            public byte TranslateColor;
            public Color[] Pallete;
        }
        private Dictionary<String, LeNSSDTInfo> sdtTable = new Dictionary<string,LeNSSDTInfo>();

        // コンストラクタ
        public LeNSConvSDT(LeNSConvOption Option) : base(Option)
        {
            createSDTTable();
        }

        // SDTファイルテーブル作成
        private void createSDTTable()
        {
            // 画像サイズとパレット情報は完全決め打ち
            // どうもファイル内に情報がないらしい
            Color[] pal;

            LeNSSDTInfo sdt;

            // 千鶴
            sdt = new LeNSSDTInfo();
            sdt.Width = 336; sdt.Height= 400;
            sdt.TranslateColor = 0;

            pal = new Color[16];
            pal[0] = Color.FromArgb(255, 0, 0, 0);
            pal[1] = Color.FromArgb(255, 0, 192, 192);
            pal[2] = Color.FromArgb(255, 0, 128, 128);
            pal[3] = Color.FromArgb(255, 0, 0, 0);
            pal[4] = Color.FromArgb(255, 192, 0, 0);
            pal[5] = Color.FromArgb(255, 192, 192, 192);
            pal[6] = Color.FromArgb(255, 192, 128, 128);
            pal[7] = Color.FromArgb(255, 192, 64, 64);
            pal[8] = Color.FromArgb(255, 128, 0, 0);
            pal[9] = Color.FromArgb(255, 128, 192, 192);
            pal[10] = Color.FromArgb(255, 128, 128, 128);
            pal[11] = Color.FromArgb(255, 128, 64, 64);
            pal[12] = Color.FromArgb(255, 0, 0, 0);
            pal[13] = Color.FromArgb(255, 64, 192, 192);
            pal[14] = Color.FromArgb(255, 64, 128, 128);
            pal[15] = Color.FromArgb(255, 64, 64, 64);

            sdt.Pallete = pal;
            sdtTable.Add("OP1W.SDT", sdt);

            // 梓
            sdt = new LeNSSDTInfo();
            sdt.Width = 448; sdt.Height = 512;
            sdt.TranslateColor = 0;
            sdt.Pallete = pal;
            sdtTable.Add("OP2W.SDT", sdt);

            // 楓
            sdt = new LeNSSDTInfo();
            sdt.Width = 448; sdt.Height = 512;
            sdt.TranslateColor = 0;

            pal = new Color[16];
            pal[0] = Color.FromArgb(255, 0, 0, 0); 
            pal[1] = Color.FromArgb(255, 192, 0, 0);
            pal[2] = Color.FromArgb(255, 128, 0, 0);
            pal[3] = Color.FromArgb(255, 0, 0, 0);
            pal[4] = Color.FromArgb(255, 0, 96, 192);
            pal[5] = Color.FromArgb(255, 192, 96, 192);
            pal[6] = Color.FromArgb(255, 64, 96, 192);
            pal[7] = Color.FromArgb(255, 128, 96, 192);
            pal[8] = Color.FromArgb(255, 0, 64, 128);
            pal[9] = Color.FromArgb(255, 192, 64, 128);
            pal[10] = Color.FromArgb(255, 128, 64, 128);
            pal[11] = Color.FromArgb(255, 64, 64, 128);
            pal[12] = Color.FromArgb(255, 0, 0, 0);
            pal[13] = Color.FromArgb(255, 192, 32, 64);
            pal[14] = Color.FromArgb(255, 128, 32, 64);
            pal[15] = Color.FromArgb(255, 64, 32, 64);

            sdt.Pallete = pal;
            sdtTable.Add("OP3W.SDT", sdt);

            // 初音
            sdt = new LeNSSDTInfo();
            sdt.Width = 336; sdt.Height = 400;
            sdt.TranslateColor = 0;
            sdt.Pallete = pal;
            sdtTable.Add("OP4W.SDT", sdt);

            // 雲
            sdt = new LeNSSDTInfo();
            sdt.Width = 1280; sdt.Height = 400;
            sdt.TranslateColor = 0;

            pal = new Color[16];
            pal[0] = Color.FromArgb(255, 51, 0, 85);
            pal[1] = Color.FromArgb(255, 34, 0, 51);
            pal[2] = Color.FromArgb(255, 17, 0, 34);
            pal[3] = Color.FromArgb(255, 0, 0, 17);
            pal[4] = Color.FromArgb(255, 153, 102, 187);
            pal[5] = Color.FromArgb(255, 85, 51, 119);
            pal[6] = Color.FromArgb(255, 85, 51, 119);
            pal[7] = Color.FromArgb(255, 85, 51, 119);
            pal[8] = Color.FromArgb(255, 119, 68, 170);
            pal[9] = Color.FromArgb(255, 68, 34, 102);
            pal[10] = Color.FromArgb(255, 68, 34, 102);
            pal[11] = Color.FromArgb(255, 68, 34, 102);
            pal[12] = Color.FromArgb(255, 85, 34, 153);
            pal[13] = Color.FromArgb(255, 34, 17, 85);
            pal[14] = Color.FromArgb(255, 34, 17, 85);
            pal[15] = Color.FromArgb(255, 34, 17, 85);

            sdt.Pallete = pal;
            sdtTable.Add("OP2_KM_W.SDT", sdt);
        }

        // 変換処理
        public override LeNSConvResult Conv(LeafPack.LeafFileInfo SrcInfo, byte[] SrcData)
        {
            byte[] img_data;

            // パレット情報
            palette = new Color[16];

            LeNSSDTInfo sdt_info;
            if (sdtTable.ContainsKey(SrcInfo.Name))
            {
                sdt_info = sdtTable[SrcInfo.Name];
            }
            else
            {
                _message = SrcInfo.Name + ":未定義のSDTファイルです。";
                return LeNSConvResult.warning;
            }

            // 取得したSDT情報をセット
            palette = sdt_info.Pallete;
            int translate_color = sdt_info.TranslateColor;

            // 解凍後サイズの取得
            int size = SrcData[0] | (SrcData[1] << 8) | (SrcData[2] << 16) | (SrcData[3] << 24);

            // 圧縮展開
            img_data = lzs2(SrcData, size, 4);

            // ビットマップ取得
            // SDTファイルは無条件でリサイズなしの出力
            Rectangle create_size = new Rectangle(0, 0, sdt_info.Width, sdt_info.Height);
            Rectangle real_size = new Rectangle(0, 0, sdt_info.Width, sdt_info.Height);

            Bitmap wk = getBitmap(img_data, translate_color, create_size, real_size);

            // PNGで保存
            writeFile(SrcInfo.Name, wk);
            wk.Dispose();

            return LeNSConvResult.ok;
        }

        // ビットマップ生成
        protected override Bitmap getBitmap(byte[] imgData, int translateColor, Rectangle createSize, Rectangle realSize)
        {
            int p;
            int img_size = imgData.Length;
            Bitmap bmp;
            int rwidth;

            // 透過色有の場合は幅x2でBitmap作成
            if (translateColor != 0xff)
            {
                rwidth = createSize.Width * 2;
            }
            else
            {
                rwidth = createSize.Width;
            }

            bmp = new Bitmap(rwidth, createSize.Height, PixelFormat.Format24bppRgb);

            Graphics g = Graphics.FromImage(bmp);
            Brush b;
            g.Clear(Color.White);
            if (translateColor != 0xff)
            {
                b = new SolidBrush(palette[translateColor]);
            }
            else
            {
                b = new SolidBrush(palette[0]);
            }
            g.FillRectangle(b, createSize);
            g.Dispose();

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmp_lock = bmp.LockBits(rect,
                                              ImageLockMode.ReadWrite,
                                              PixelFormat.Format24bppRgb);
            int bmp_length = bmp.Width * bmp.Height * 3;
            byte[] bmp_byte = new byte[bmp.Width * bmp.Height * 3];
            Marshal.Copy(bmp_lock.Scan0, bmp_byte, 0, bmp_length);

            // イメージデータ展開
            int x = 0, y = realSize.Height - 1;
                int i;
                int c;
                p = 0;

                for (i = 0; i < img_size; i++, p++)
                {
                    c = imgData[p];
                    setPixel(ref bmp_byte, palette, translateColor, c, x + realSize.X, y + realSize.Y, rwidth);

                    if (++x >= realSize.Width)
                    {
                        x = 0;
                        y--;
                    }
                }

                Marshal.Copy(bmp_byte, 0, bmp_lock.Scan0, bmp_length);
                bmp.UnlockBits(bmp_lock);

                return bmp;
        }
    }
}
