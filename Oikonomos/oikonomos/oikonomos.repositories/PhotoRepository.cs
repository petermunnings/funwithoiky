using System.Linq;
using oikonomos.common;
using oikonomos.common.DTOs;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class PhotoRepository : RepositoryBase, IPhotoRepository
    {
        public void SavePhoto(int personId, byte[] photoContent, string contentType, string fileName)
        {
            SavePhoto(personId, photoContent, contentType, fileName, ImageSize.FullSize);
        }

        public void SavePhoto(int personId, byte[] photoContent, string contentType, string fileName, string imageSize)
        {
            var currentPhoto = Context.UploadPhotoes.FirstOrDefault(p => p.PersonId == personId && p.ImageSize == imageSize);
            if (currentPhoto != null)
            {
                Context.DeleteObject(currentPhoto);
                Context.SaveChanges();
            }

            var newPhoto = new UploadPhoto()
            {
                PersonId = personId,
                Photo = photoContent,
                ContentType = contentType,
                FileName = fileName,
                ImageSize = imageSize
            };
            Context.UploadPhotoes.AddObject(newPhoto);
            Context.SaveChanges();

        }

        public UploadPhotoDto FetchPhoto(int personId, string imageSize)
        {
            var currentPhoto =
                Context.UploadPhotoes.FirstOrDefault(p => p.PersonId == personId && p.ImageSize == imageSize);
            if (currentPhoto == null) return new UploadPhotoDto {FileContent = new byte[0], FileType = "image/png"};
            return new UploadPhotoDto
            {
                FileContent = currentPhoto.Photo,
                FileType = currentPhoto.ContentType
            };
        }

        public void DeleteDefaultImage(int personId)
        {
            var currentPhoto = Context.UploadPhotoes.FirstOrDefault(p => p.PersonId == personId && p.ImageSize == ImageSize.FullSize);
            if (currentPhoto == null) return;
            Context.UploadPhotoes.DeleteObject(currentPhoto);
            Context.SaveChanges();
        }

        public bool ImageExists(int personId)
        {
            return Context.UploadPhotoes.FirstOrDefault(p => p.PersonId == personId && p.ImageSize == ImageSize.Small) != null;
        }
    }
}