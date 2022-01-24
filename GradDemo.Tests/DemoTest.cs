using GradDemo.Api;
using GradDemo.Api.Models;
using GradDemo.Api.Providers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GradDemo.Tests
{
    public class DemoTests
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
        public async Task NotFoundTest()
        {
            var shouldFail = await CallHelper.GetAndDeserialize<IList<WeatherForecast>>(_httpClient, "/bad-path-doesnt-exist");

            Assert.IsTrue(shouldFail.httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [Test]
        public async Task ErrorFromApi()
        {
            var shouldFail = await CallHelper.GetAndDeserialize<Response<string>>(_httpClient, "/Demo/should-fail");

            Assert.IsTrue(shouldFail.httpResponse.IsSuccessStatusCode && !shouldFail.content.Success);
        }

        [Test]
        public async Task BasicApiTest()
        {
            var shouldFail = await CallHelper.GetAndDeserialize<Response<string>>(_httpClient, "/Demo");

            Assert.IsTrue(shouldFail.httpResponse.IsSuccessStatusCode);

            var niceer = shouldFail.content;

            Assert.NotNull(niceer);
            Assert.IsTrue(niceer.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(niceer.Payload));
        }

        [Test]
        public async Task TestAPost()
        {
            var inputName = "Tommy";

            var postTestResponse = await CallHelper.PostAndDeserialize<Response<string>>(_httpClient, "/Demo/say-hello", inputName);

            Assert.IsTrue(postTestResponse.httpResponse.IsSuccessStatusCode);

            var resultContent = postTestResponse.content.Payload;

            Assert.NotNull(resultContent);
            Assert.IsTrue($"Hello, {inputName}!" == resultContent);
        }

        [Test]
        public async Task ComplexTest()
        {
            var input = new ComplexRequest()
            {
                Name = "Tommy",
                UseNewLines = true
            };

            var tooFewRepeatsRequest = await CallHelper.PostAndDeserialize<Response<string>>(_httpClient, $"demo/greet-many/repeat/{-5}", input);

            Assert.IsTrue(tooFewRepeatsRequest.httpResponse.IsSuccessStatusCode);
            Assert.IsFalse(tooFewRepeatsRequest.content.Success);

            var postTestResponse = await CallHelper.PostAndDeserialize<Response<string>>(_httpClient, $"demo/greet-many/repeat/{5}", input);

            Assert.IsTrue(postTestResponse.httpResponse.IsSuccessStatusCode);
            Assert.IsTrue(postTestResponse.content.Success);

            var resultContent = postTestResponse.content.Payload;

            Assert.NotNull(resultContent);
        }
    }
}