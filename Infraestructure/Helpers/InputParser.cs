using System;
using System.Collections.Generic;
using System.Text;

namespace Localiza.MerchantGuide.Infraestructure.Helpers
{
    public static class InputParser
    {
        public static List<string> GetWords(string input)
        {
            return input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(w => w.Trim().ToLower())
                .ToList();
        }

        public static List<string> ExtractValuesAfterIs(string input)
        {
            var parts = input.Split("é", StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 2)
                throw new Exception("Formato inválido");

            return GetWords(parts[1]);
        }
    }
}