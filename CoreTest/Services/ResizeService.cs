using CoreTest.Models;
using CoreTest.Repository;
using CoreTest.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Services
{
    public class ResizeService : BaseService, IResizeService
    {
        public ResizeService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<byte[]> GetImageAsync(List<Photo> photosInSession, string id, int photoWidthInPixel)
        {
            Photo photoDB = await (await UnitOfWork.PhotoRepository.GetAllAsync()).FirstOrDefaultAsync(m => m.Guid == id);
            if (photoDB == null)
            {
                foreach (var item in photosInSession)
                {
                    if (item.Guid == id)
                    {
                        photoDB = item;
                        break;
                    }
                }
            }

            if (photoWidthInPixel != 0)
            {
                Bitmap bmp;
                MemoryStream memoryStream = new MemoryStream();
                const long quality = 50; // picture quality max 100
                using (var ms = new MemoryStream(photoDB.ImageContent))
                {
                    bmp = new Bitmap(ms);
                    int imageHeight = bmp.Height;
                    int imageWidth = bmp.Width;
                    if (imageWidth > photoWidthInPixel)
                    {
                        float ratio = imageWidth / (float)imageHeight;
                        var resized_Bitmap = new Bitmap(photoWidthInPixel, (int)(photoWidthInPixel / ratio));
                        using (var graphics = Graphics.FromImage(resized_Bitmap))
                        {
                            graphics.CompositingQuality = CompositingQuality.HighSpeed;
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.CompositingMode = CompositingMode.SourceCopy;
                            graphics.DrawImage(bmp, 0, 0, photoWidthInPixel, photoWidthInPixel / ratio);

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
            else
            {
                return photoDB.ImageContent;
            }
        }
    }
}
