using GradDemo.Api;
using GradDemo.Api.Models;
using GradDemo.Api.Models.Auth;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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
            
            var login = await SuccessLogin();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", login.Token);

            shouldFail = await CallHelper.GetAndDeserialize<Response<string>>(_httpClient, "/Demo");

            Assert.IsTrue(shouldFail.httpResponse.IsSuccessStatusCode);

            var niceer = shouldFail.content;

            Assert.NotNull(niceer);
            Assert.IsTrue(niceer.Success);
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

            Assert.IsFalse(tooFewRepeatsRequest.httpResponse.IsSuccessStatusCode);

            var postTestResponse = await CallHelper.PostAndDeserialize<Response<string>>(_httpClient, $"demo/greet-many/repeat/{5}", input);

            Assert.IsTrue(postTestResponse.httpResponse.IsSuccessStatusCode);
            Assert.IsTrue(postTestResponse.content.Success);

            var resultContent = postTestResponse.content.Payload;

            Assert.NotNull(resultContent);
        }

        public async System.Threading.Tasks.Task<TokenResult> SuccessLogin()
        {
            var creds = await CallHelper.PostAndDeserialize<DeviceCredentials>(_httpClient, "/auth/register", null);

            Assert.IsTrue(creds.httpResponse.IsSuccessStatusCode);
            Assert.IsNotNull(creds.content);

            var token = await CallHelper.PostAndDeserialize<TokenResult>(_httpClient, "/auth/token", creds.content);

            Assert.IsTrue(token.httpResponse.IsSuccessStatusCode);
            Assert.IsNotNull(token.content?.Token);

            return token.content;
        }
    }
}