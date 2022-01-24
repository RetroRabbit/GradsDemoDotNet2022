using GradDemo.Api.Models;
using System.Text;

namespace GradDemo.Api.Providers
{
    public class DemoProvider
    {
        private string _greeting;

        public DemoProvider(string greetingWord)
        {
            _greeting = greetingWord;
        }

        public string GetGreeting()
        {
            return _greeting;
        }

        public string Greeting { get { return _greeting; } }

        public StringBuilder BuildGreetingString(int number, ComplexRequest inputs)
        {
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
            return builder;
        }
    }
}
