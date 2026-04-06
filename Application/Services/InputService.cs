using Localiza.MerchantGuide.Infraestructure;

namespace Localiza.MerchantGuide.Application.Services
{
    public class InputService
    {
        private readonly IConsole _console;

        public InputService(IConsole console)
        {
            _console = console;
        }

        public string ReadInput(string message)
        {
            string? input;

            do
            {
                _console.WriteLine(message);
                input = _console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(input));

            return input.Trim();
        }
    }
}