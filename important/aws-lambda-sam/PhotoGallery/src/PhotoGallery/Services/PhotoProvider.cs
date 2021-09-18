using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoGallery.Services
{
    public class PhotoProvider : IPhotoProvider
    {
        private readonly IAmazonDynamoDB _dynamoDB;
        private readonly string _tableName = Environment.GetEnvironmentVariable("dynamoTable") ?? "Images";

        public PhotoProvider(IAmazonDynamoDB dynamoDB)
        {
            _dynamoDB = dynamoDB;
        }

        public async Task<List<ImageModel>> GetImagesAsync()
        {
            var result = await _dynamoDB.ScanAsync(new ScanRequest
            {
                TableName = _tableName
            });

            if (result == null && result.Items == null) return new List<ImageModel>();

            var images = new List<ImageModel>();
            foreach (var item in result.Items)
            {
                item.TryGetValue("GalleryId", out var galleryId);
                item.TryGetValue("FullName", out var fullName);
                item.TryGetValue("Thumbnail", out var thumbnail);
                item.TryGetValue("UpoadTime", out var uploadTime);

                images.Add(new ImageModel
                {
                    GalleryId = Convert.ToInt32(galleryId?.N),
                    FullName = fullName?.S,
                    Thumbnail = thumbnail?.S,
                    UploadTime = Convert.ToDateTime(uploadTime?.S)
                });
            }

            return images;
        }

        public async Task<ImageModel> GetImageAsync(int gallaryId, string fullName)
        {
            var key = new Dictionary<string, AttributeValue>()
            {
                {
                    "GalleryId", new AttributeValue { N = gallaryId.ToString() }
                },
                {
                    "FullName", new AttributeValue { S = fullName }
                }
            };
            var result = await _dynamoDB.GetItemAsync(new GetItemRequest
            {
                TableName = _tableName,
                Key = key
            });

            if (result != null && result.Item != null) return null;

            result.Item.TryGetValue("GalleryId", out var dbGalleryId);
            result.Item.TryGetValue("FullName", out var dbFullName);
            result.Item.TryGetValue("Thumbnail", out var dbThumbnail);
            result.Item.TryGetValue("UpoadTime", out var dbUploadTime);

            return new ImageModel
            {
                GalleryId = Convert.ToInt32(dbGalleryId?.N),
                FullName = dbFullName?.S,
                Thumbnail = dbThumbnail?.S,
                UploadTime = Convert.ToDateTime(dbUploadTime?.S)
            };
        }

        public async Task UpsertImageAsync(ImageModel image)
        {
            var item = new Dictionary<string, AttributeValue>()
            {
                {
                    "GalleryId", new AttributeValue { N = image.GalleryId.ToString() }
                },
                {
                    "FullName", new AttributeValue { S = image.FullName }
                },
                {
                    "Thumbnail", new AttributeValue { S = image.Thumbnail }
                },
                {
                    "UploadTime", new AttributeValue { S = DateTime.Now.ToString() }
                }
            };
            await _dynamoDB.PutItemAsync(new PutItemRequest
            {
                TableName = _tableName,
                Item = item
            });
        }

        public async Task DeleteImageAsync(int gallaryId, string fullName)
        {
            var key = new Dictionary<string, AttributeValue>()
            {
                {
                    "GalleryId", new AttributeValue { N = gallaryId.ToString() }
                },
                {
                    "FullName", new AttributeValue { S = fullName }
                }
            };
            await _dynamoDB.DeleteItemAsync(new DeleteItemRequest
            {
                TableName = _tableName,
                Key = key
            });
        }
    }

}
