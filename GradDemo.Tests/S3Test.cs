using GradDemo.Api;
using GradDemo.Api.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GradDemo.Tests
{
    public class S3Tests
    {
        private APIWebApplicationFactory? _apiWebApplicationFactory;
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _apiWebApplicationFactory = new APIWebApplicationFactory();
            _httpClient = _apiWebApplicationFactory.CreateClient();
        }

        [Test]
        public async Task UploadAndFetchFile()
        {
            var filename = $"david/{Guid.NewGuid().ToString()}.jpg";

            var postTestResponse = await CallHelper.PostAndDeserialize<Response<string>>(_httpClient, "/s3/upload", new UploadFileRequest()
            {
                FileName = filename
            });

            Assert.IsTrue(postTestResponse.httpResponse.IsSuccessStatusCode);

            var resultContent = postTestResponse.content;

            Assert.NotNull(resultContent);
            Assert.IsTrue(resultContent.Success);

            var downloadResponse = await CallHelper.PostAndDeserialize<Response<string>>(_httpClient, "/s3/download", new UploadFileRequest()
            {
                FileName = filename
            });
            
            Assert.IsTrue(downloadResponse.httpResponse.IsSuccessStatusCode);
            Assert.NotNull(downloadResponse.content);
            Assert.IsTrue(downloadResponse.content.Success);
        }
    }
}