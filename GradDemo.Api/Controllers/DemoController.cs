using GradDemo.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GradDemo.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DemoController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<DemoController> _logger;

        public DemoController(ILogger<DemoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Response<string> Demo()
        {
            return Response<string>.Successful("Hello");
        }

        [HttpPost("say-hello")]
        public Response<string> SayHello([FromBody] string name)
        {
            var reallyOld = "Hello " + name;
            var lessOld = string.Format("Hello {0}", name);

            return Response<string>.Successful($"Hello, {name}!");
        }

        [HttpGet("should-fail")]
        public Response<string> ShouldFail()
        {
            return Response<string>.Error("Fails for test purposes");
        }

        [HttpPost("say-hello-to-more-people/{number}")]
        public Response<string> SayHelloToLotsOfPeople(int number, [FromBody] HelloRequest name)
        {
            return Response<string>.Successful($"[{number}] Hello {name.Name} and {name.OtherName} and especially you {name.LastName}");
        }
    }
}
