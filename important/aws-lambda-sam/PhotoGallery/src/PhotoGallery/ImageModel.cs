using System;
using System.Text.Json.Serialization;

namespace PhotoGallery
{
    public class ImageModel
    {
        public int GalleryId { get; set; }
        public string FullName { get; set; }
        public string Thumbnail { get; set; }
        [JsonIgnore]
        public DateTime UploadTime { get; set; }
    }
}
