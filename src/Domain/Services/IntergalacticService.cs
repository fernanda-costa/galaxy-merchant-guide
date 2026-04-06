namespace Localiza.MerchantGuide.Application.Services
{
    public class IntergalacticService
    {
        private readonly Dictionary<string, string> _map = [];

        public void Add(string intergalactic, string roman)
        {
            if (string.IsNullOrWhiteSpace(intergalactic))
                throw new ArgumentException("Valor intergaláctico inválido");

            if (_map.ContainsKey(intergalactic.ToLower()))
                throw new InvalidOperationException($"'{intergalactic}' já foi mapeado");

            if (_map.Values.Any(v => v == roman))
                throw new InvalidOperationException($"'{roman}' já foi utilizado");

            _map.Add(intergalactic.ToLower(), roman.ToUpper());
        }

        public string GetRoman(string intergalactic)
        {
            if (!_map.TryGetValue(intergalactic.ToLower(), out var roman))
                throw new InvalidOperationException($"Valor '{intergalactic}' não encontrado");

            return roman;
        }

        public bool IsEmpty() => !_map.Any();
    }
}