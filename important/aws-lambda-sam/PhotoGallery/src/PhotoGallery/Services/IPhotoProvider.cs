using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoGallery.Services
{
    public interface IPhotoProvider
    {
        Task<ImageModel> GetImageAsync(int gallaryId, string fullName);
        Task<List<ImageModel>> GetImagesAsync();
        Task UpsertImageAsync(ImageModel image);
        Task DeleteImageAsync(int galleryId, string fullName);
    }
}
