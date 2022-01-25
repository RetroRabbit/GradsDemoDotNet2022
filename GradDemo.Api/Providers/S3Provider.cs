using Amazon.S3;
using Amazon.S3.Model;
using GradDemo.Api.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GradDemo.Api.Providers
{
    public class S3Provider
    {
        private string _bucket;
        private string _defaultImagePath;
        private AmazonS3Client _client = new AmazonS3Client();


        public S3Provider(string bucket, string defaultImagePath)
        {
            _bucket = bucket;
            _defaultImagePath = defaultImagePath;
        }

        public async Task UploadFileAsync(string s3Path)
        {
            FileInfo file = new FileInfo(_defaultImagePath);

            if (!file.Exists) throw new Exception($"Default File could not be found at {file.FullName}");

            string path = s3Path;

            PutObjectRequest request = new PutObjectRequest()
            {
                InputStream = file.OpenRead(),
                BucketName = _bucket,
                Key = path // <-- in S3 key represents a path  
            };

            PutObjectResponse response = await _client.PutObjectAsync(request);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception($"S3 failure with status code {response.HttpStatusCode}.");
        }

        public async Task DownloadFileAsync(string s3Path)
        {
            GetObjectRequest getRequest = new GetObjectRequest()
            {
                BucketName = _bucket,
                Key = s3Path // <-- in S3 key represents a path  
            };
            GetObjectResponse getResponse = await _client.GetObjectAsync(getRequest);

            await getResponse.WriteResponseStreamToFileAsync($"download/{getResponse.Key}", false, new System.Threading.CancellationToken());
        }
    }
}
