using System;
using System.Collections.Generic;
using System.Text;

namespace Localiza.MerchantGuide
{
    public static class Constants
    {
        public const string CREDITS = "Créditos";

        public static readonly Dictionary<string, decimal> ROMAN_NUMBERS = new() { 
            { "I", 1 }, 
            { "V", 5 },
            { "X", 10 }, 
            { "L", 50 },
            { "C", 100 },
            { "D", 500 },
            { "M", 1000 },
        };

    }
}
