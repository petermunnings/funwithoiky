using System.Configuration;
using System.Linq;
using System.Net.Mime;
using oikonomos.common;
using oikonomos.common.DTOs;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class UploadPhotoRepository : IUploadPhotoRepository
    {
        public void SavePhoto(int personId, byte[] photoContent, string contentType, string fileName)
        {
            SavePhoto(personId, photoContent, contentType, fileName, ImageSize.FullSize);
        }

        public void SavePhoto(int personId, byte[] photoContent, string contentType, string fileName, string imageSize)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var currentPhoto = context.UploadPhotoes.FirstOrDefault(p => p.PersonId == personId && p.ImageSize == imageSize);
                if (currentPhoto != null)
                {
                    context.DeleteObject(currentPhoto);
                    context.SaveChanges();
                }

                var newPhoto = new UploadPhoto()
                {
                    PersonId = personId,
                    Photo = photoContent,
                    ContentType = contentType,
                    FileName = fileName,
                    ImageSize = imageSize
                };
                context.UploadPhotoes.AddObject(newPhoto);
                context.SaveChanges();
            }
        }

        public UploadPhotoDto FetchPhoto(int personId, string imageSize)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var currentPhoto = context.UploadPhotoes.FirstOrDefault(p => p.PersonId == personId);
                return new UploadPhotoDto
                {
                    FileContent = currentPhoto.Photo,
                    FileType = currentPhoto.ContentType
                };
            }
        }
    }
}