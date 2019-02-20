﻿using CoreTest.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Services
{
    public interface IResizeService
    {
        byte[] GetImage(byte[] photobyte, int width);
    }

    public class ResizeService : IResizeService
    {
        public byte[] GetImage(byte[] photobyte, int width)
        {
            if (width != 0)
            {
                Bitmap bmp;
                MemoryStream memoryStream = new MemoryStream();
                const long quality = 50;
                using (var ms = new MemoryStream(photobyte))
                {
                    bmp = new Bitmap(ms);
                    int imageHeight = bmp.Height;
                    int imageWidth = bmp.Width;
                    if (imageWidth > width)
                    {
                        float ratio = (float)imageWidth / (float)imageHeight;
                        var resized_Bitmap = new Bitmap((int)width, (int)(width / ratio));
                        using (var graphics = Graphics.FromImage(resized_Bitmap))
                        {
                            graphics.CompositingQuality = CompositingQuality.HighSpeed;
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.CompositingMode = CompositingMode.SourceCopy;
                            graphics.DrawImage(bmp, 0, 0, width, width / ratio);

                            var qualityParamId = Encoder.Quality;
                            var encoderParameters = new EncoderParameters(1);
                            encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
                            var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);
                            resized_Bitmap.Save(memoryStream, ImageFormat.Jpeg);
                        }
                    }
                    else
                    {
                        bmp.Save(memoryStream, ImageFormat.Jpeg);
                    }
                }
                return memoryStream.ToArray();
            }
            else {
                return photobyte;
            }
        }
    }
}
