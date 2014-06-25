using System.Drawing;

namespace oikonomos.services.interfaces
{
    public interface IPhotoServices
    {
        Image ByteArrayToImage(byte[] byteArrayIn);
        byte[] ImageToByteArray(Image imageIn, string imageFormatString);
        Image CropImage(Image img, Rectangle cropArea);
        Image ResizeImage(Image imgToResize, Size size);
    }
}