using Newtonsoft.Json;
using PhotoGallery.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PhotoGallery
{
    public class App
    {
        private readonly IPhotoProvider _photoProvider;
        private readonly IStorageService _storageService;
        private readonly IResizeService _resizeService;

        private readonly int height; 
        private readonly int width;

        public App(IPhotoProvider photoProvider, IStorageService storageService, IResizeService resizeService)
        {
            _photoProvider = photoProvider;
            _storageService = storageService;
            _resizeService = resizeService;

            if (!int.TryParse(Environment.GetEnvironmentVariable("height"), out height))
            {
                height = 10;
            }
            if (!int.TryParse(Environment.GetEnvironmentVariable("width"), out width))
            {
                width = 10;
            }
        }

        public async Task<string> RunAsync(string sourceBucket, string objectKey, string destBucket, string destObjectKey)
        {
            ImageModel image;
            try
            {
                var stream = await _storageService.GetObjectAsync(sourceBucket, objectKey);
                var resizedStream = _resizeService.ResizeImage(stream, width, height, out string format);
                await _storageService.SaveObjectAsync(destBucket, destObjectKey, resizedStream, format);

                //upsert dynamoDB
                image = new ImageModel()
                {
                    GalleryId = new Random().Next(1, 10),
                    FullName = $"s3://{sourceBucket}/{objectKey}",
                    Thumbnail = $"s3://{destBucket}/{destObjectKey}",
                    UploadTime = DateTime.UtcNow
                };

                await _photoProvider.UpsertImageAsync(image);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                return ex.Message;
            }

            return JsonConvert.SerializeObject(image);
        }
    }
}
