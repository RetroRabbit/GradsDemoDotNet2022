using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using System.Threading.Tasks;

namespace GradDemo.Api.Providers
{
    public class S3Provider
    {
        public static async Task UploadFileAsync(string bucket)
        {
            var client = new AmazonS3Client();

            FileInfo file = new FileInfo(@".\imgs\ball.jpg");
            string path = "demo/ball3.jpg";

            PutObjectRequest request = new PutObjectRequest()
            {
                InputStream = file.OpenRead(),
                BucketName = bucket,
                Key = path // <-- in S3 key represents a path  
            };

            PutObjectResponse response = await client.PutObjectAsync(request);

            //TODO Response
        }
    }
}
