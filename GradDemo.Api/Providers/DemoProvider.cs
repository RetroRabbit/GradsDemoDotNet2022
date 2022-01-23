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
    }
}
