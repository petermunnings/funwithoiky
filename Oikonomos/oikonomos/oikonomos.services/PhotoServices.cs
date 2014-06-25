using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class PhotoServices : IPhotoServices
    {
        public Image ByteArrayToImage(byte[] byteArrayIn)
        {
            var ms = new MemoryStream(byteArrayIn);
            return Image.FromStream(ms);
        }

        public byte[] ImageToByteArray(Image imageIn, string imageFormatString)
        {
            var ms = new MemoryStream();
            var imageFormat = getImageFormat(imageFormatString);
            imageIn.Save(ms, imageFormat);
            return ms.ToArray();
        }

        private ImageFormat getImageFormat(string imageFormatString)
        {
            switch (imageFormatString)
            {
                case "image/jpeg":
                    {
                        return ImageFormat.Jpeg;
                    }
                case "image/png":
                    {
                        return ImageFormat.Png;
                    }
                case "image/bmp":
                    {
                        return ImageFormat.Bmp;
                    }
            }
            return ImageFormat.Bmp;
        }

        public Image CropImage(Image img, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(img);
            var bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return bmpCrop;
        }

        public Image ResizeImage(Image imgToResize, Size size)
        {
            var sourceWidth = imgToResize.Width;
            var sourceHeight = imgToResize.Height;

            var nPercentW = (size.Width / (float)sourceWidth);
            var nPercentH = (size.Height / (float)sourceHeight);

            var nPercent = nPercentH < nPercentW ? nPercentH : nPercentW;

            var destWidth = (int)(sourceWidth * nPercent);
            var destHeight = (int)(sourceHeight * nPercent);

            var b = new Bitmap(destWidth, destHeight);
            var g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return b;
        }
    }
}