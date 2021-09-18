using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using PhotoGallery.Services;
using System;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace PhotoGallery
{
    public class PhotoFunction
    {
        private readonly IServiceCollection serviceCollection;
        private readonly App app;

        public PhotoFunction()
        {
            serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            app = serviceProvider.GetService<App>();
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> PhotoFunctionHandler(S3Event input, ILambdaContext context)
        {
            var sourceFolder = Environment.GetEnvironmentVariable("sourceFolder") ?? "original/";
            var destFolder = Environment.GetEnvironmentVariable("destFolder") ?? "processed/";

            Console.WriteLine(input.Records[0].S3.Object.Key);

            var sourceBucket = input.Records[0].S3.Bucket.Name;
            var objectKey = input.Records[0].S3.Object.Key.Replace("+", " ");

            var destBucket = Environment.GetEnvironmentVariable("destBucket") ?? sourceBucket;
            var destObjectKey = objectKey.Replace(sourceFolder, destFolder);

            var result = await app.RunAsync(sourceBucket, objectKey, destBucket, destObjectKey);

            return result;
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IPhotoProvider, PhotoProvider>();
            serviceCollection.AddSingleton<IAmazonDynamoDB>(s => new AmazonDynamoDBClient());

            serviceCollection.AddTransient<App>();

            serviceCollection.AddScoped<IAmazonS3>((sc) => new AmazonS3Client());
            serviceCollection.AddScoped<IStorageService, SimpleStorageService>();
            serviceCollection.AddScoped<IResizeService, ImageResizeService>();
        }
    }
}
