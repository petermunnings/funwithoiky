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
using oikonomos.services;
using oikonomos.services.interfaces;
using oikonomos.web.Helpers;

namespace oikonomos.web.Controllers
{
    public class ImagesController : Controller
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IPhotoServices _photoServices;

        public ImagesController()
        {
            _photoRepository = new PhotoRepository();
            _photoServices = new PhotoServices();
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
            var croppedImage = _photoServices.CropImage(_photoServices.ByteArrayToImage(oldImage.FileContent), cropArea);

            var newSize = new Size(200, 200);
            var largeImage = _photoServices.ResizeImage(croppedImage, newSize);

            var extension = oldImage.FileType.Split('/').Count()>1 ? oldImage.FileType.Split('/')[1] : "bmp";
            _photoRepository.SavePhoto(picCrop.PersonId, _photoServices.ImageToByteArray(largeImage, oldImage.FileType), oldImage.FileType, string.Format("200.{0}", extension), ImageSize.Large);

            newSize = new Size(50,50);
            var smallImage = _photoServices.ResizeImage(croppedImage, newSize);
            _photoRepository.SavePhoto(picCrop.PersonId, _photoServices.ImageToByteArray(smallImage, oldImage.FileType), oldImage.FileType, string.Format("100.{0}", extension), ImageSize.Small);

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
