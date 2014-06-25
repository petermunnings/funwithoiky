using oikonomos.common.DTOs;

namespace oikonomos.repositories.interfaces
{
    public interface IUploadPhotoRepository
    {
        void SavePhoto(int personId, byte[] photoContent, string contentType, string fileName);
        void SavePhoto(int personId, byte[] photoContent, string contentType, string fileName, string imageSize);
        UploadPhotoDto FetchPhoto(int personId, string imageSize);
        
    }
}
