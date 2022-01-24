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
        public static async Task<Response<string>> UploadFileAsync(string bucket)
        {
            try
            {
                var client = new AmazonS3Client();

                FileInfo file = new FileInfo(@".\imgs\football.jpg");
                string path = "demo/football.jpg";

                PutObjectRequest request = new PutObjectRequest()
                {
                    InputStream = file.OpenRead(),
                    BucketName = bucket,
                    Key = path // <-- in S3 key represents a path  
                };

                PutObjectResponse response = await client.PutObjectAsync(request);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    return Response<string>.Successful(response.HttpStatusCode.ToString());
                else
                    return Response<string>.Error(response.HttpStatusCode.ToString());

            }
            catch (Exception e)
            {
                return Response<string>.Error(e.Message);
            }
            //GetObjectRequest getRequest = new GetObjectRequest()
            //{
            //    BucketName = bucket,
            //    Key = path // <-- in S3 key represents a path  
            //};
            //GetObjectResponse getResponse = await client.GetObjectAsync(getRequest);
            //await getResponse.WriteResponseStreamToFileAsync($"download/{getResponse.Key}", false,new System.Threading.CancellationToken());

            //TODO Response
        }
    }
}
