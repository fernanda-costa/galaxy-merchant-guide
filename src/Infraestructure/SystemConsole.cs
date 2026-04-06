using System;
using System.Collections.Generic;
using System.Text;

namespace Localiza.MerchantGuide.Infraestructure
{
    public interface IConsole
    {
        string? ReadLine();
        void WriteLine(object message);
    }

    public class SystemConsole : IConsole
    {
        public string? ReadLine() => Console.ReadLine();
        public void WriteLine(object message) => Console.WriteLine(message);
    }
}
