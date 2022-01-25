using GradDemo.Api.Models;
using GradDemo.Api.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GradDemo.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class S3Controller : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly S3Provider _provider;
        private readonly ILogger<S3Controller> _logger;

        public S3Controller(IConfiguration iConfig, S3Provider provider, ILogger<S3Controller> logger)
        {
            _configuration = iConfig;
            _provider = provider;
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<Response<string>> UploadFileAsync(UploadFileRequest fileDetail)
         {
            if (string.IsNullOrEmpty(fileDetail?.FileName))
            {
                return Response<string>.Error($"fileDetail.FileName must be supplied");
            }

            try
            {
                await _provider.UploadFileAsync(fileDetail.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, fileDetail);
                return Response<string>.Error($"Failed to upload file. Reason: {ex.Message}");
            }

            return Response<string>.Successful($"Upload success for {fileDetail.FileName}");
        }

        [HttpPost("download")]
        public async Task<Response<string>> DownloadFileAsync(UploadFileRequest fileDetail)
        {
            if (string.IsNullOrEmpty(fileDetail?.FileName))
            {
                return Response<string>.Error($"fileDetail.FileName must be supplied");
            }

            await _provider.DownloadFileAsync(fileDetail.FileName);

            return Response<string>.Successful($"Added");
        }
    }
}
