using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;            
using System.Drawing.Drawing2D;  
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Http;

namespace CoreTest
{
    public static class PhotoResizer
    {
        public static bool Image_resize(string input_Image_Path, string output_Image_Path, float w_pixels)
        {
            const long quality = 50;
            Image img;
            using (var bmp = new Bitmap(input_Image_Path))
            {
                img = new Bitmap(bmp);
                int imageHeight = img.Height;
                int imageWidth = img.Width;
                if (imageWidth > w_pixels)
                {
                    float ratio = (float)imageWidth / (float)imageHeight;
                    var resized_Bitmap = new Bitmap((int)w_pixels, (int)(w_pixels / ratio));
                    using (var graphics = Graphics.FromImage(resized_Bitmap))
                    {
                        graphics.CompositingQuality = CompositingQuality.HighSpeed;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.CompositingMode = CompositingMode.SourceCopy;
                        graphics.DrawImage(img, 0, 0, w_pixels, w_pixels / ratio);

                        using (var output = File.Open(output_Image_Path, FileMode.Create))
                        {
                            var qualityParamId = Encoder.Quality;
                            var encoderParameters = new EncoderParameters(1);
                            encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
                            var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);
                            resized_Bitmap.Save(output, codec, encoderParameters);
                           // output.Flush();
                        }
                        //graphics.Flush();
                    }
                   // resized_Bitmap.Dispose();
                    return true;
                }
                return false;
            }
        }

    }
}

