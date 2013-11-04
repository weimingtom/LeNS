using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace LeNS
{
    class LeNSConvLFG : LeNSConvGraphic
    {
        public LeNSConvLFG(LeNSConvOption Option) : base(Option) { }
        protected int direction = 0;

        // 変換処理
        public override LeNSConvResult Conv(LeafPack.LeafFileInfo SrcInfo, byte[] SrcData)
        {
            int p;
            byte[] img_data;

            // パレット情報
            palette = new Color[16];

            // マジックコードチェック
            String strBuf = "";
            for (p = 0; p < 8; p++)
            {
                strBuf += ((char)SrcData[p]).ToString();
            }
            if (!strBuf.Equals("LEAFCODE"))
            {
                throw new InvalidDataException("LFGDataではありません。");
            }

            // パレット情報の取得
            int palidx = 0;
            byte[] palbuf = new byte[48];

            for (p = 8; p < 32; p++)
            {
                // 取得した数値を4bitシフトして設定(16→256階調)
                // upper
                palbuf[palidx] = (byte)(SrcData[p] & 0xf0);
                palbuf[palidx] |= (byte)(palbuf[palidx] >> 4);
                palidx++;
                // lower
                palbuf[palidx] = (byte)(SrcData[p] << 4);
                palbuf[palidx] |= (byte)(palbuf[palidx] >> 4);
                palidx++;
            }
            for (p = 0; p < 16; p++)
            {
                palette[p] = Color.FromArgb(palbuf[p * 3],
                                            palbuf[(p * 3) + 1],
                                            palbuf[(p * 3) + 2]);
            }

            // ジオメトリ情報の取得
            int xoffset = (SrcData[33] << 8 | SrcData[32]) * 8;
            int yoffset = SrcData[35] << 8 | SrcData[34];
            int width = ((SrcData[37] << 8 | SrcData[36]) + 1) * 8;
            int height = ((SrcData[39] << 8 | SrcData[38]) + 1);

            // 展開方向の取得
            direction = SrcData[40];

            // 透過色の取得
            int translate_color = SrcData[41];

            // OP2_MN_W.LFGは色変更
            if (option.conv == LeNSConvMode.MODE_KZ && SrcInfo.Name.Equals("OP2_MN_W.LFG"))
            {
                palette[15] = Color.Black;
            }

            // 解凍後サイズの取得
            int size = SrcData[44] | (SrcData[45] << 8) | (SrcData[46] << 16) | (SrcData[47] << 24);

            // 圧縮展開
            img_data = lzs(SrcData, size, 48);

            // 画像実サイズ取得
            int rwidth = width - xoffset;
            int rheight = height - yoffset;

            // ビットマップ取得
            Rectangle create_size;
            Rectangle real_size = new Rectangle(xoffset, yoffset, rwidth, rheight);

            // 画像を640x400で作成すると、セーブ時に不具合が出るっぽい(bgaliaで解決可能？)
            // ので、画像を640x480に変更、yオフセットを+40する
            switch (SrcInfo.Type)
            {
                case LeafPack.LeNSFileType.BGGraphic:
                    real_size.Y += 40;
                    create_size = new Rectangle(0, 0, 640, 480);
                    translate_color = 0xff;
                    // 雫オープニング画像のyオフセットをずらす
                    if (option.conv == LeNSConvMode.MODE_SZ &&
                        SrcInfo.Name.Substring(0, 4).Equals("OP_S"))
                    {
                        real_size.Y += 157;

                        // パレットも変なので修正(色が暗い…)
                        //palette[1] = Color.FromArgb(255, 64, 48, 208);
                    }
                    else if (width > 320)
                    {
                        create_size.Width = 640;
                    }

                    break;
                case LeafPack.LeNSFileType.CharaGraphic:
                    real_size.Y += 40;

                    if (rwidth <= 320)
                    {
                        create_size = new Rectangle(0, 0, 320, 480);

                        // 雫の一部画像で不具合が出るため対応(背景差分系)
                        if (option.conv == LeNSConvMode.MODE_SZ &&
                            (SrcInfo.Name.Equals("MAX_C37.LFG") ||
                             SrcInfo.Name.Equals("MAX_C6A.LFG"))) 
                        {
                            create_size.Width = 640;
                        }
                        else if (width > 320)
                        {
                            create_size.Width = 640;
                        }
                    }
                    else
                    {
                        create_size = new Rectangle(0, 0, 640, 480);
                    }

                    break;
                case LeafPack.LeNSFileType.EtcGraphic:
                default:
                    create_size = new Rectangle(0, 0, rwidth, rheight);
                    real_size.X = 0; real_size.Y = 0;

                    break;
            }


            String[] file_suffix = { "", "B", "C", "D", "E" };

            // 立ち絵は夜のパターンがあればＯＫ
            if (SrcInfo.Type == LeafPack.LeNSFileType.CharaGraphic)
            {
                file_suffix[1] = "";
                file_suffix[2] = "N";
                file_suffix[3] = "";
                file_suffix[4] = "";
            }
            
            if (paletteMap.ContainsKey(SrcInfo.Name))
            {
                String[] map = paletteMap[SrcInfo.Name].Split(',');
                for(int i = 0; i < 5; i++)
                {
                    if (map[i + 1].Equals("1"))
                    {
                        setPalette(i);
                        writeBitmap(Path.GetFileNameWithoutExtension(SrcInfo.Name) + file_suffix[i] + ".LFG", img_data, translate_color, create_size, real_size);
                    }
                }
            }
            else
            {
                writeBitmap(SrcInfo.Name, img_data, translate_color, create_size, real_size);
            }

            return LeNSConvResult.ok;
        }

        private void writeBitmap(String fileName, byte[] imgData, int translateColor, Rectangle createSize, Rectangle realSize)
        {
            Bitmap wk = getBitmap(imgData, translateColor, createSize, realSize);

            // PNGで保存
            writeFile(fileName, wk);
            wk.Dispose();
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

            // 画像の初期クリア
            // こっちはめんどいんでGraphicsでやっとく
            Graphics g = Graphics.FromImage(bmp);
            Brush b;
            g.Clear(Color.White);
            if (translateColor != 0xff)
            {
                // 透過色があるときは透過色で塗りつぶす
                b = new SolidBrush(palette[translateColor]);
            }
            else
            {
                // 透過色がないときは０番で塗りつぶす
                b = new SolidBrush(palette[0]);
            }
            g.FillRectangle(b, createSize);
            g.Dispose();

            // Bitmapロック
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmp_lock = bmp.LockBits(rect, 
                                               ImageLockMode.ReadWrite, 
                                               PixelFormat.Format24bppRgb);
            int bmp_length = bmp.Width * bmp.Height * 3;
            byte[] bmp_byte = new byte[bmp.Width * bmp.Height * 3];
            Marshal.Copy(bmp_lock.Scan0, bmp_byte, 0, bmp_length);

            // イメージデータ展開
            int x = 0, y = 0;
            int i;
            int c;
            p = 0;

            for (i = 0; i < img_size; i++, p++)
            {
                c = (imgData[p] & 0x80) >> 4 |
                    (imgData[p] & 0x20) >> 3 |
                    (imgData[p] & 0x08) >> 2 |
                    (imgData[p] & 0x02) >> 1;
                setPixel(ref bmp_byte, palette, translateColor, c, x + realSize.X, y + realSize.Y, rwidth);

                x++;

                c = (imgData[p] & 0x40) >> 3 |
                    (imgData[p] & 0x10) >> 2 |
                    (imgData[p] & 0x04) >> 1 |
                    (imgData[p] & 0x01);
                setPixel(ref bmp_byte, palette, translateColor, c, x + realSize.X, y + realSize.Y, rwidth);

                if (direction == 0)
                {
                    x--;

                    if (++y >= realSize.Height)
                    {
                        y = 0;
                        x += 2;
                    }
                }
                else
                {
                    if (++x >= realSize.Width)
                    {
                        x = 0;
                        y++;
                    }
                }
            }

            // Bitmapアンロック
            Marshal.Copy(bmp_byte, 0, bmp_lock.Scan0, bmp_length);
            bmp.UnlockBits(bmp_lock);

            return bmp;
        }

        protected Dictionary<String, String> paletteMap = new Dictionary<String, String>();
        public void LoadPaletteMap(String FileName)
        {
            FileStream fs;
            StreamReader sr;

            fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\" + FileName, FileMode.Open);
            try
            {
                sr = new StreamReader(fs, Encoding.GetEncoding(932));
                try
                {
                    while(sr.Peek() > -1)
                    {
                        String line = sr.ReadLine();
                        paletteMap.Add(line.Split(',')[0], line);
                    }
                }
                finally
                {
                    sr.Dispose();
                }
            }
            finally
            {
                fs.Close();
            }
        }

        private void setPalette(int no)
        {
            switch (no)
            {
                case 0:
                default:

                    break;
                case 1:
                    palette[0] = Color.FromArgb(255, 0x00, 0x00, 0x00);
                    palette[1] = Color.FromArgb(255, 0x66, 0x33, 0x22);
                    palette[2] = Color.FromArgb(255, 0xbb, 0x77, 0x33);
                    palette[3] = Color.FromArgb(255, 0xff, 0xcc, 0x55);
                    palette[4] = Color.FromArgb(255, 0xff, 0xff, 0xff);
                    palette[5] = Color.FromArgb(255, 0x22, 0x33, 0x55);
                    palette[6] = Color.FromArgb(255, 0x66, 0x66, 0xbb);
                    palette[7] = Color.FromArgb(255, 0xbb, 0xcc, 0xff);
                    palette[8] = Color.FromArgb(255, 0xff, 0xee, 0x77);
                    palette[9] = Color.FromArgb(255, 0xff, 0x33, 0x44);
                    palette[10] = Color.FromArgb(255, 0x55, 0x33, 0x22);
                    palette[11] = Color.FromArgb(255, 0x99, 0x66, 0x55);
                    palette[12] = Color.FromArgb(255, 0xee, 0x99, 0x77);
                    palette[13] = Color.FromArgb(255, 0xff, 0xbb, 0xaa);
                    palette[14] = Color.FromArgb(255, 0xff, 0xdd, 0xcc);
                    palette[15] = Color.FromArgb(255, 0x88, 0xaa, 0x99);

                    break;
                case 2:
                    palette[0] = Color.FromArgb(255, 0x00, 0x00, 0x00);
                    palette[1] = Color.FromArgb(255, 0x33, 0x22, 0x55);
                    palette[2] = Color.FromArgb(255, 0x44, 0x44, 0x88);
                    palette[3] = Color.FromArgb(255, 0x66, 0x66, 0xcc);
                    palette[4] = Color.FromArgb(255, 0xff, 0xff, 0xff);
                    palette[5] = Color.FromArgb(255, 0x22, 0x33, 0x55);
                    palette[6] = Color.FromArgb(255, 0x66, 0x66, 0xbb);
                    palette[7] = Color.FromArgb(255, 0xbb, 0xcc, 0xff);
                    palette[8] = Color.FromArgb(255, 0xff, 0xee, 0xaa);
                    palette[9] = Color.FromArgb(255, 0xdd, 0x33, 0x77);
                    palette[10] = Color.FromArgb(255, 0x33, 0x33, 0x55);
                    palette[11] = Color.FromArgb(255, 0x77, 0x66, 0x88);
                    palette[12] = Color.FromArgb(255, 0xbb, 0x99, 0xaa);
                    palette[13] = Color.FromArgb(255, 0xdd, 0xbb, 0xff);
                    palette[14] = Color.FromArgb(255, 0xff, 0xdd, 0xff);
                    palette[15] = Color.FromArgb(255, 0x66, 0x66, 0xaa);

                    break;
                case 3:
                    palette[0] = Color.FromArgb(255, 0x00, 0x00, 0x00);
                    palette[1] = Color.FromArgb(255, 0x33, 0x44, 0x22);
                    palette[2] = Color.FromArgb(255, 0x77, 0x88, 0x55);
                    palette[3] = Color.FromArgb(255, 0xee, 0xdd, 0x88);
                    palette[4] = Color.FromArgb(255, 0xff, 0xff, 0xff);
                    palette[5] = Color.FromArgb(255, 0x22, 0x33, 0x55);
                    palette[6] = Color.FromArgb(255, 0x66, 0x66, 0xbb);
                    palette[7] = Color.FromArgb(255, 0xbb, 0xcc, 0xff);
                    palette[8] = Color.FromArgb(255, 0xff, 0xee, 0x77);
                    palette[9] = Color.FromArgb(255, 0xff, 0x33, 0x44);
                    palette[10] = Color.FromArgb(255, 0x55, 0x33, 0x22);
                    palette[11] = Color.FromArgb(255, 0x99, 0x66, 0x55);
                    palette[12] = Color.FromArgb(255, 0xee, 0x99, 0x77);
                    palette[13] = Color.FromArgb(255, 0xff, 0xbb, 0xaa);
                    palette[14] = Color.FromArgb(255, 0xff, 0xdd, 0xcc);
                    palette[15] = Color.FromArgb(255, 0x88, 0xaa, 0x99);

                    break;
                case 4:
                    palette[0] = Color.FromArgb(255, 0xff, 0xff, 0xff);
                    palette[1] = Color.FromArgb(255, 0xff, 0xff, 0xff);
                    palette[2] = Color.FromArgb(255, 0xff, 0xff, 0xff);
                    palette[3] = Color.FromArgb(255, 0xff, 0xff, 0xff);
                    palette[4] = Color.FromArgb(255, 0xff, 0xff, 0xff);
                    palette[5] = Color.FromArgb(255, 0x22, 0x33, 0x55);
                    palette[6] = Color.FromArgb(255, 0x66, 0x66, 0xbb);
                    palette[7] = Color.FromArgb(255, 0xbb, 0xcc, 0xff);
                    palette[8] = Color.FromArgb(255, 0xff, 0xee, 0x77);
                    palette[9] = Color.FromArgb(255, 0xff, 0x33, 0x44);
                    palette[10] = Color.FromArgb(255, 0x55, 0x33, 0x22);
                    palette[11] = Color.FromArgb(255, 0x99, 0x66, 0x55);
                    palette[12] = Color.FromArgb(255, 0xee, 0x99, 0x77);
                    palette[13] = Color.FromArgb(255, 0xff, 0xbb, 0xaa);
                    palette[14] = Color.FromArgb(255, 0xff, 0xdd, 0xcc);
                    palette[15] = Color.FromArgb(255, 0x88, 0xaa, 0x99);

                    break;
            }
        }
    }
}
