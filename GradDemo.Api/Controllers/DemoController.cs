using GradDemo.Api.Entities;
using GradDemo.Api.Models;
using GradDemo.Api.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ApplicationDbContext _db;


        public DemoController(ILogger<DemoController> logger, DemoProvider demo, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _demo = demo;
            _db = dbContext;
        }

        [HttpGet]
        public async Task<Response<string>> Demo()
        {
            // add a thing
            await _db.Contacts.AddAsync(new Contact()
            {
                ContactNumber = "0821231234",
                Name = "Test",
                LastName = "LastName"
            });

            await _db.SaveChangesAsync();
            
            // count of a thing
            var countofContacts = await _db.Contacts.CountAsync();

            var contact = await _db.Contacts.Where(x => x.Id == 3).FirstOrDefaultAsync();
            contact.Name = "New name for test";

            await _db.SaveChangesAsync();

            return Response<string>.Successful($"{_demo.Greeting} everyone, there are now {countofContacts} contacts. 3 = {contact.Name}");
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
