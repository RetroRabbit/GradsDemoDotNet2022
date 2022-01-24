using GradDemo.Api.Models;
using GradDemo.Api.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace GradDemo.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class S3Controller : ControllerBase
    {
        private IConfiguration _configuration;

        public S3Controller(IConfiguration iConfig)
        {
            _configuration = iConfig;
        }

        [HttpGet("UploadFile")]
        public async Task<Response<string>> UploadFileAsync()
        {
            var response = await S3Provider.UploadFileAsync(_configuration.GetValue<string>("AWS:S3Bucket"));

            return response;
        }
    }
}
