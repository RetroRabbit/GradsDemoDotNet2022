using GradDemo.Api.Models;
using GradDemo.Api.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly DemoProvider _demo;

        public DemoController(ILogger<DemoController> logger, DemoProvider demo)
        {
            _logger = logger;
            _demo = demo;
        }

        [HttpGet]
        public Response<string> Demo()
        {
            return Response<string>.Successful("Hello"/*_demo.Greeting*/);
        }

        [HttpGet("should-fail")]
        public Response<string> ShouldFail()
        {
            return Response<string>.Error("Fails for test purposes");
        }

        [HttpPost("say-hello")]
        public Response<string> SayHello([FromBody] string name)
        {
            var reallyOld = "Hello " + name;
            var lessOld = string.Format("Hello {0}", name);

            return Response<string>.Successful($"Hello, {name}!");
        }

        [HttpPost("greet-many/repeat/{number}")]
        public Response<string> GreetManyPeople(int number, [FromBody] ComplexRequest inputs)
        {
            // validate things
            // consider api standards - exceptions or error responses
            if (number < 1) throw new ArgumentException("I must perform at least one greeting!");
            if (inputs == null)
            {
                return Response<string>.Error($"Insufficient inputs");
            }

            // chunky code in controller
            StringBuilder builder = new StringBuilder("Hello");

            for (int i = 0; i < number; i++)
            {
                if (inputs.UseNewLines)
                {
                    builder.AppendLine($"Hello {inputs.Name}");
                }
                else
                {
                    builder.Append($", hello {inputs.Name}");
                }
            }

            builder.Append("!");

            return Response<string>.Successful(builder.ToString());
        }
    }
}
