using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGallery.Services
{
    public interface IStorageService
    {
        Task<Stream> GetObjectAsync(string bucket, string key);
        Task SaveObjectAsync(string bucket, string key, Stream stream, string contentType);
    }
}
