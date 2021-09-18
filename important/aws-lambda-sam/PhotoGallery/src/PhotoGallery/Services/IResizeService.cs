using System.IO;

namespace PhotoGallery.Services
{
    public interface IResizeService
    {
        Stream ResizeImage(Stream imageStream, int width, int height, out string format);
    }
}
