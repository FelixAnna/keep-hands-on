using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace PhotoGallery.Services
{
    public class ImageResizeService : IResizeService
    {
        public Stream ResizeImage(Stream stream, int width, int height, out string format)
        {
            var image = Image.Load(stream);
            image.Mutate(x => x.Resize(width, height));
            var imageStream = new MemoryStream();
            var encoder = new PngEncoder();
            format = "image/png";
            image.Save(imageStream, encoder);
            return imageStream;
        }
    }
}
