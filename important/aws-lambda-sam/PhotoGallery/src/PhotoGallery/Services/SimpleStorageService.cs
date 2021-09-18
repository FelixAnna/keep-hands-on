using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using System.Threading.Tasks;

namespace PhotoGallery.Services
{
    public class SimpleStorageService : IStorageService
    {
        private readonly IAmazonS3 _s3Client;

        public SimpleStorageService(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        public async Task<Stream> GetObjectAsync(string bucket, string key)
        {
            var response = await _s3Client.GetObjectAsync(bucket, key);
            return response.ResponseStream;
        }

        public async Task SaveObjectAsync(string bucket, string key, Stream stream, string contentType)
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = bucket,
                Key = key,
                InputStream = stream,
                ContentType = contentType
            };
            await _s3Client.PutObjectAsync(putRequest);
        }
    }
}
