using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using oikonomos.common;
using oikonomos.data;
using oikonomos.repositories;
using oikonomos.repositories.interfaces;
using oikonomos.web.Helpers;

namespace oikonomos.web.Controllers
{
    public class ImagesController : Controller
    {
        private readonly IPhotoRepository _photoRepository;

        public ImagesController()
        {
            _photoRepository = new PhotoRepository();
        }

        public FileContentResult Index(string dateTime)
        {
            Person currentPerson = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
            if (currentPerson == null)
            {
                return null;
            }

            var uploadedImage = _photoRepository.FetchPhoto(currentPerson.PersonId, ImageSize.FullSize);
            return new FileContentResult(uploadedImage.FileContent, uploadedImage.FileType);
        }

        public FileContentResult GetImage(int personId, string imageSize)
        {
            var uploadedImage = _photoRepository.FetchPhoto(personId, imageSize);
            return new FileContentResult(uploadedImage.FileContent, uploadedImage.FileType);
        }

        public void DeleteDefaultImage()
        {
            Person currentPerson = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
            if (currentPerson == null)
            {
                return;
            }
            _photoRepository.DeleteDefaultImage(currentPerson.PersonId);
        }

        public void SaveCroppedImage(PictureCrop picCrop)
        {
            Person currentPerson = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
            if (currentPerson == null)
            {
                return;
            }
            var oldImage = _photoRepository.FetchPhoto(currentPerson.PersonId, ImageSize.FullSize);
            var cropArea = new Rectangle(picCrop.X1, picCrop.Y1, picCrop.Width, picCrop.Height);
            var croppedImage = CropImage(ByteArrayToImage(oldImage.FileContent), cropArea);

            var newSize = new Size(200, 200);
            var largeImage = ResizeImage(croppedImage, newSize);

            var extension = oldImage.FileType.Split('/').Count()>1 ? oldImage.FileType.Split('/')[1] : "bmp";
            _photoRepository.SavePhoto(picCrop.PersonId, ImageToByteArray(largeImage, oldImage.FileType), oldImage.FileType, string.Format("200.{0}", extension), ImageSize.Large);

            newSize = new Size(50,50);
            var smallImage = ResizeImage(croppedImage, newSize);
            _photoRepository.SavePhoto(picCrop.PersonId, ImageToByteArray(smallImage, oldImage.FileType), oldImage.FileType, string.Format("100.{0}", extension), ImageSize.Small);

        }

        private static Image ByteArrayToImage(byte[] byteArrayIn)
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

        private static Image CropImage(Image img, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(img);
            var bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return bmpCrop;
        }

        private static Image ResizeImage(Image imgToResize, Size size)
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

    public class PictureCrop
    {
        public int PersonId { get; set; }
        public int X1 { get; set; }
        public int X2 { get; set; }
        public int Y1 { get; set; }
        public int Y2 { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}
