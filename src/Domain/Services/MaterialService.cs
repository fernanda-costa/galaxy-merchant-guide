using Localiza.MerchantGuide.Application.Services;
using Localiza.MerchantGuide.Domain.Services;

namespace Localiza.MerchantGuide.Application.Services
{
    public class MaterialService
    {
        private readonly Dictionary<string, decimal> _materials = [];
        private readonly RomanCalculatorService _romanCalculator;

        public MaterialService(RomanCalculatorService romanCalculator)
        {
            _romanCalculator = romanCalculator;
        }

        public void Add(string material, decimal value)
        {
            _materials.TryAdd(material.ToLower(), value);
        }

        public decimal Get(string material)
        {
            if (!_materials.TryGetValue(material.ToLower(), out var value))
                throw new InvalidOperationException($"Material '{material}' não encontrado");

            return value;
        }

        public bool IsEmpty() => !_materials.Any();

        public decimal CalculateMaterialValue(List<string> words)
        {
            var creditsText = words[^2];

            if (!decimal.TryParse(creditsText, out var credits))
                throw new InvalidOperationException("Valor de créditos inválido");

            var romanWords = words.Take(words.Count - 4).ToList();
            var quantity = _romanCalculator.Calculate(romanWords);

            return credits / quantity;
        }

        public bool Exists(string material)
        {
            return _materials.ContainsKey(material.ToLower());
        }

        public decimal CalculateTransactionValue(List<string> words)
        {
            var material = words.Last();
            var romanWords = words.Take(words.Count - 1).ToList();

            var quantity = _romanCalculator.Calculate(romanWords);
            var value = Get(material);

            return quantity * value;
        }
    }
}