using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.Image.Model
{
    public abstract class ImageBase
    {
        public ImageBase()
        {
            FileName = Guid.NewGuid().ToString();
        }

        public abstract string Folder { get; }

        public string FileName { get; private set; }

        public abstract string EmptyImage();

        public abstract string FilePath();

        [Required(ErrorMessage = "A imagem é obrigatório.")]
        public IFormFile Value { get; set; }

        public abstract int Width { get; }

        public abstract int Height { get; }



        private System.Drawing.Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);

            using var ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            return image;
        }

        public string ConvertImageFromBase64(System.Drawing.Image image, ImageFormat format)
        {
            try
            {
                using MemoryStream m = new MemoryStream();
                Bitmap bmp = new Bitmap(image);

                bmp.Save(m, format);
                byte[] imageBytes = m.ToArray();

                string base64String = Convert.ToBase64String(imageBytes);
                return $"data:image/{format.ToString()};base64,{base64String}";
            }
            catch (Exception)
            {
                return null;
            }
        }

        public byte[] ResizeImage(String[] base64, ImageFormat format)
        {
            var image = Base64ToImage(base64[1]);

            var destRect = new Rectangle(0, 0, Width, Height);
            var destImage = new Bitmap(Width, Height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighSpeed;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using var wrapMode = new ImageAttributes();
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }

            string fileString = ConvertImageFromBase64(destImage, format);

            var file = Convert.FromBase64String(fileString.Split(',').LastOrDefault());
            return file;
        }
    }
}
