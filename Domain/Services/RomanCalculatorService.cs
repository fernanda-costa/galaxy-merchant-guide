using Localiza.MerchantGuide.Application.Services;
using Localiza.MerchantGuide.Infraestructure;

namespace Localiza.MerchantGuide.Domain.Services
{
    public class RomanCalculatorService
    {
        private readonly IntergalacticService _intergalacticService;
        private static readonly Dictionary<string, decimal> RomanValues = new()
        {
            { "I", 1 }, { "V", 5 }, { "X", 10 },
            { "L", 50 }, { "C", 100 }, { "D", 500 }, { "M", 1000 }
        };

        private static readonly Dictionary<string, string[]> ValidSubtractions = new()
        {
            { "I", new[] { "V", "X" } },
            { "X", new[] { "L", "C" } },
            { "C", new[] { "D", "M" } }
        };

        private static readonly HashSet<string> NonSubtractable = ["V", "L", "D"];

        public RomanCalculatorService(IntergalacticService intergalacticService)
        {
            _intergalacticService = intergalacticService;
        }

        public decimal Calculate(List<string> values)
        {
            decimal result = 0;
            decimal lastAddedValue = decimal.MaxValue;
            int repeatCount = 1;

            for (int i = 0; i < values.Count; i++)
            {
                var currentRoman = GetRoman(values[i]);
                var currentValue = GetValue(currentRoman);

                var nextRoman = HasNext(values, i) ? GetRoman(values[i + 1]) : null;
                var nextValue = nextRoman != null ? GetValue(nextRoman) : 0;

                var mustSubstract = nextRoman != null && currentValue < nextValue;

                if (mustSubstract)
                {
                    ValidateSubtraction(currentRoman, nextRoman, repeatCount);

                    var subtractionValue = nextValue - currentValue;

                    if (subtractionValue > lastAddedValue)
                        throw new InvalidOperationException("Ordem romana inválida");

                    result += subtractionValue;
                    lastAddedValue = subtractionValue;

                    i++;
                    repeatCount = 1;
                    continue;
                }

                repeatCount = UpdateRepeatCount(values, i, repeatCount);
                ValidateRepetition(currentRoman, repeatCount, mustSubstract);

                if (currentValue > lastAddedValue)
                    throw new InvalidOperationException("Ordem romana inválida");

                result += currentValue;
                lastAddedValue = currentValue;
            }

            return result;
        }

        private string GetRoman(string intergalactic)
        {
            return _intergalacticService.GetRoman(intergalactic);
        }

        private decimal GetValue(string roman)
        {
            if (!RomanValues.TryGetValue(roman, out var value))
                throw new InvalidOperationException($"Número romano inválido: {roman}");

            return value;
        }

        private static bool HasNext(List<string> values, int index)
        {
            return index < values.Count - 1;
        }

        private static int UpdateRepeatCount(List<string> values, int index, int count)
        {
            if (index > 0)
            {
                var previous = values[index - 1];
                if (previous == values[index])
                    return count + 1;
            }

            return 1;
        }
        private void ValidateRepetition(string roman, int count, bool mustSubtract)
        {
            if (count > 3 && !mustSubtract)
                throw new InvalidOperationException($"'{roman}' não pode repetir mais que 3 vezes");
        }

        private void ValidateSubtraction(string current, string next, int repeatCount)
        {

            if (NonSubtractable.Contains(current))
                throw new InvalidOperationException($"'{current}' não pode ser subtraído");

            if (!ValidSubtractions.TryGetValue(current, out var allowed) ||
                !allowed.Contains(next))
                throw new InvalidOperationException($"Subtração inválida: {current}{next}");
        }
    }
}