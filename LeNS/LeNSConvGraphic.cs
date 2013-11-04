using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace LeNS
{
    abstract class LeNSConvGraphic : LeNSConvFile
    {
        public LeNSConvGraphic(LeNSConvOption Option) : base(Option) { }

        protected Color[] palette = null;

        protected abstract Bitmap getBitmap(byte[] imgData, int translateColor, Rectangle createSize, Rectangle realSize);

        protected override String getSavePath()
        {
            String path = base.getSavePath() + "\\graphics";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        // 変換後ファイル名取得
        public override String GetSaveName(string FileName)
        {
            return Path.GetFileNameWithoutExtension(FileName) + ".PNG";
        }

        // ファイル出力
        protected void writeFile(String fileName, Bitmap bmp)
        {
            bmp.Save(getSavePath(fileName), ImageFormat.Png);
        }

        // ピクセルセット
        protected void setPixel(ref byte[] bmp, Color[] pal, int tc, int c, int x, int y, int w)
        {
            int ptr = (x + (w * y)) * 3;

            bmp[ptr] = pal[c].B;
            bmp[ptr + 1] = pal[c].G;
            bmp[ptr + 2] = pal[c].R;

            if (tc != 0xff)
            {
                ptr += (w / 2) * 3;

                if (c == tc)
                {
                    bmp[ptr] = 255;
                    bmp[ptr + 1] = 255;
                    bmp[ptr + 2] = 255;
                }
                else
                {
                    bmp[ptr] = 0;
                    bmp[ptr + 1] = 0;
                    bmp[ptr + 2] = 0;
                }
            }
        }
    }
}
