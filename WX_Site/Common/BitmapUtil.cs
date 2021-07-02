using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace WX_Site.Common
{
    public static class BitmapUtil
    {
        public static Bitmap RotatingBitmap(this Bitmap img)
        {
            var exif = img.PropertyItems;
            byte orien = 0;
            var item = exif.Where(m => m.Id == 274).ToArray();
            if (item.Length > 0)
                orien = item[0].Value[0];
            switch (orien)
            {
                case 2:
                    img.RotateFlip(RotateFlipType.RotateNoneFlipX);//horizontal flip
                    break;
                case 3:
                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);//right-top
                    break;
                case 4:
                    img.RotateFlip(RotateFlipType.RotateNoneFlipY);//vertical flip
                    break;
                case 5:
                    img.RotateFlip(RotateFlipType.Rotate90FlipX);
                    break;
                case 6:
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);//right-top
                    break;
                case 7:
                    img.RotateFlip(RotateFlipType.Rotate270FlipX);
                    break;
                case 8:
                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);//left-bottom
                    break;
                default:
                    break;
            }
            return img;
        }

        public static Bitmap RotateImage(this Image img)
        {
            var exif = img.PropertyItems;
            byte orien = 0;
            var item = exif.Where(m => m.Id == 274).ToArray();
            if (item.Length > 0)
                orien = item[0].Value[0];
            switch (orien)
            {
                case 2:
                    img.RotateFlip(RotateFlipType.RotateNoneFlipX);//horizontal flip
                    break;
                case 3:
                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);//right-top
                    break;
                case 4:
                    img.RotateFlip(RotateFlipType.RotateNoneFlipY);//vertical flip
                    break;
                case 5:
                    img.RotateFlip(RotateFlipType.Rotate90FlipX);
                    break;
                case 6:
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);//right-top
                    break;
                case 7:
                    img.RotateFlip(RotateFlipType.Rotate270FlipX);
                    break;
                case 8:
                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);//left-bottom
                    break;
                default:
                    break;
            }
            return (Bitmap)img;
        }


        /// <summary> 
        /// 生成缩略图重载方法1，返回缩略图的Image对象 
        /// </summary> 
        /// <param name="Width">缩略图的宽度</param> 
        /// <param name="Height">缩略图的高度</param> 
        /// <returns>缩略图的Image对象</returns> 
        public static Image GetReducedImage(this Image ResourceImage, int Width, int Height)
        {
            try
            {

                //用指定的大小和格式初始化Bitmap类的新实例 
                Bitmap bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                //从指定的Image对象创建新Graphics对象 
                Graphics graphics = Graphics.FromImage(bitmap);
                //清除整个绘图面并以透明背景色填充 
                graphics.Clear(Color.Transparent);
                //在指定位置并且按指定大小绘制原图片对象 
                graphics.DrawImage(ResourceImage, new Rectangle(0, 0, Width, Height));

                return bitmap;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
