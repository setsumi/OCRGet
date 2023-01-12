using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace OCRGet
{

    static class Jpeg
    {

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static void Save(Bitmap bmp, string path, long quality)
        {
            // Get a bitmap. The using statement ensures objects  
            // are automatically disposed from memory after use.  
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

            // Create an Encoder object based on the GUID  
            // for the Quality parameter category.  
            System.Drawing.Imaging.Encoder myEncoder =
                System.Drawing.Imaging.Encoder.Quality;

            // Create an EncoderParameters object.  
            // An EncoderParameters object has an array of EncoderParameter  
            // objects. In this case, there is only one  
            // EncoderParameter object in the array.  
            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, quality);
            myEncoderParameters.Param[0] = myEncoderParameter;
            bmp.Save(path, jpgEncoder, myEncoderParameters);
        }
    } // Jpeg

    static class ImageHelper
    {
        private const InterpolationMode DefaultInterpolationMode = InterpolationMode.HighQualityBicubic;

        public static Bitmap ResizeImage(Bitmap bmp, int width, int height, InterpolationMode interpolationMode = DefaultInterpolationMode)
        {
            if (width < 1 || height < 1 || (bmp.Width == width && bmp.Height == height))
            {
                return bmp;
            }

            Bitmap bmpResult = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            bmpResult.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);

            using (bmp)
            using (Graphics g = Graphics.FromImage(bmpResult))
            {
                g.InterpolationMode = interpolationMode;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.CompositingMode = CompositingMode.SourceOver;

                using (ImageAttributes ia = new ImageAttributes())
                {
                    ia.SetWrapMode(WrapMode.TileFlipXY);
                    g.DrawImage(bmp, new Rectangle(0, 0, width, height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, ia);
                }
            }

            return bmpResult;
        }
    } // ImageHelper
}
