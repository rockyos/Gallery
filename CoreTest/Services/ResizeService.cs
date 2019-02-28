using CoreTest.Models;
using CoreTest.Repository;
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
    public interface IResizeService
    {
        Task<byte[]> GetImageAsync(List<Photo> photosfromsession, string id, int width);
    }

    public class ResizeService : BaseService, IResizeService
    {
        public ResizeService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<byte[]> GetImageAsync(List<Photo> photosfromsession, string id, int width)
        {
            var photo = await (await UnitOfWork.PhotoRepository.GetAllAsync()).FirstOrDefaultAsync(m => m.Guid == id);

            if (photo == null)
            {
                foreach (var item in photosfromsession)
                {
                    if (item.Guid == id)
                    {
                        photo = item;
                        break;
                    }
                }
            }

            if (width != 0)
            {
                Bitmap bmp;
                MemoryStream memoryStream = new MemoryStream();
                const long quality = 50;
                using (var ms = new MemoryStream(photo.ImageContent))
                {
                    bmp = new Bitmap(ms);
                    int imageHeight = bmp.Height;
                    int imageWidth = bmp.Width;
                    if (imageWidth > width)
                    {
                        float ratio = imageWidth / (float)imageHeight;
                        var resized_Bitmap = new Bitmap(width, (int)(width / ratio));
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
            else
            {
                return photo.ImageContent;
            }
        }
    }
}
