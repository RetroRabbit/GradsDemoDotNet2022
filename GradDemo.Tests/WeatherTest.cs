using GradDemo.Api;
using GradDemo.Api.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GradDemo.Tests
{
    public class WeatherTests
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
        public async Task GetTheWeather()
        {
            var shouldFail = await CallHelper.GetAndDeserialize<IList<WeatherForecast>>(_httpClient, "/WeatherForecast");

            Assert.IsTrue(shouldFail.httpResponse.IsSuccessStatusCode);

            var niceer = shouldFail.content;

            Assert.NotNull(niceer);
            Assert.IsTrue(niceer.Count > 0);
        }
    }
}