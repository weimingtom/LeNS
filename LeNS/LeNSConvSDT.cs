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

        // �R���X�g���N�^
        public LeNSConvSDT(LeNSConvOption Option) : base(Option)
        {
            createSDTTable();
        }

        // SDT�t�@�C���e�[�u���쐬
        private void createSDTTable()
        {
            // �摜�T�C�Y�ƃp���b�g���͊��S���ߑł�
            // �ǂ����t�@�C�����ɏ�񂪂Ȃ��炵��
            Color[] pal;

            LeNSSDTInfo sdt;

            // ���
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

            // ��
            sdt = new LeNSSDTInfo();
            sdt.Width = 448; sdt.Height = 512;
            sdt.TranslateColor = 0;
            sdt.Pallete = pal;
            sdtTable.Add("OP2W.SDT", sdt);

            // ��
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

            // ����
            sdt = new LeNSSDTInfo();
            sdt.Width = 336; sdt.Height = 400;
            sdt.TranslateColor = 0;
            sdt.Pallete = pal;
            sdtTable.Add("OP4W.SDT", sdt);

            // �_
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

        // �ϊ�����
        public override LeNSConvResult Conv(LeafPack.LeafFileInfo SrcInfo, byte[] SrcData)
        {
            byte[] img_data;

            // �p���b�g���
            palette = new Color[16];

            LeNSSDTInfo sdt_info;
            if (sdtTable.ContainsKey(SrcInfo.Name))
            {
                sdt_info = sdtTable[SrcInfo.Name];
            }
            else
            {
                _message = SrcInfo.Name + ":����`��SDT�t�@�C���ł��B";
                return LeNSConvResult.warning;
            }

            // �擾����SDT�����Z�b�g
            palette = sdt_info.Pallete;
            int translate_color = sdt_info.TranslateColor;

            // �𓀌�T�C�Y�̎擾
            int size = SrcData[0] | (SrcData[1] << 8) | (SrcData[2] << 16) | (SrcData[3] << 24);

            // ���k�W�J
            img_data = lzs2(SrcData, size, 4);

            // �r�b�g�}�b�v�擾
            // SDT�t�@�C���͖������Ń��T�C�Y�Ȃ��̏o��
            Rectangle create_size = new Rectangle(0, 0, sdt_info.Width, sdt_info.Height);
            Rectangle real_size = new Rectangle(0, 0, sdt_info.Width, sdt_info.Height);

            Bitmap wk = getBitmap(img_data, translate_color, create_size, real_size);

            // PNG�ŕۑ�
            writeFile(SrcInfo.Name, wk);
            wk.Dispose();

            return LeNSConvResult.ok;
        }

        // �r�b�g�}�b�v����
        protected override Bitmap getBitmap(byte[] imgData, int translateColor, Rectangle createSize, Rectangle realSize)
        {
            int p;
            int img_size = imgData.Length;
            Bitmap bmp;
            int rwidth;

            // ���ߐF�L�̏ꍇ�͕�x2��Bitmap�쐬
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

            // �C���[�W�f�[�^�W�J
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
