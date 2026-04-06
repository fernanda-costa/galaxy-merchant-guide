using Localiza.MerchantGuide.Application.Services;
using Localiza.MerchantGuide.Domain.Enums;
using Localiza.MerchantGuide.Domain.Services;
using Localiza.MerchantGuide.Infraestructure;

namespace Localiza.MerchantGuide.Application
{
    public class MerchantGuideService
    {
        private const string ExitCommand = "SAIR";

        private static readonly HashSet<string> IgnoredWords =
            ["quanto", "quantos", "creditos", "créditos", "é", "custa", "vale"];

        private readonly IConsole _console;
        private readonly InputService _inputService;
        private readonly RomanCalculatorService _romanCalculator;
        private readonly MaterialService _materialService;
        private readonly IntergalacticService _intergalacticService;

        public MerchantGuideService(
            IConsole console,
            InputService inputService,
            RomanCalculatorService romanCalculator,
            MaterialService materialService,
            IntergalacticService intergalacticService)
        {
            _console = console;
            _inputService = inputService;
            _romanCalculator = romanCalculator;
            _materialService = materialService;
            _intergalacticService = intergalacticService;
        }

        public void Run()
        {
            int option = -1;

            _console.WriteLine("\n--- Bem vindo ao seu conversor de unidades intergalácticas! ---");
            Console.WriteLine(@"
                    /\
                   /  \
                  |----|
                  |    |
                 /| || |\
                /_|_||_|_\
                   /||\
                  /_||_\
            ");

            do
            {
                try
                {
                    ShowMenu();

                    var input = _console.ReadLine();

                    if (!int.TryParse(input, out option))
                        continue;

                    switch (option)
                    {
                        case (int)MenuOptionsEnum.ReadIntergalacticNumber:
                            ReadNumbers();
                            break;

                        case (int)MenuOptionsEnum.ReceiveMaterialValues:
                            ReadMaterials();
                            break;

                        case (int)MenuOptionsEnum.GetTransactionValue:
                            AnswerQuestion();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _console.WriteLine($"\nErro: {ex.Message}");
                }

            } while (option != (int)MenuOptionsEnum.Leave);
        }

        private void ShowMenu()
        {

            _console.WriteLine("1 - Mapear numerais");
            _console.WriteLine("2 - Inserir materiais");
            _console.WriteLine("3 - Fazer pergunta");
            _console.WriteLine("0 - Sair\n");
        }

        private void ReadNumbers()
        {
            _intergalacticService.Add(_inputService.ReadInput("Valor de I:"), "I");
            _intergalacticService.Add(_inputService.ReadInput("Valor de V:"), "V");
            _intergalacticService.Add(_inputService.ReadInput("Valor de X:"), "X");
            _intergalacticService.Add(_inputService.ReadInput("Valor de L:"), "L");
        }

        private void ReadMaterials()
        {
            string input;
            _console.WriteLine("Vamos identificar os valores dos materiais! \nInforme a transação no formato: <medidas> <material> é <valor> créditos\nExemplo: glob glob prata é 34 créditos");

            do
            {
                input = _inputService.ReadInput("Digite sua transação aqui (ou SAIR para voltar ao menu):");

                if (input == ExitCommand)
                    break;

                var words = GetWords(input);

                var material = ExtractMaterial(words);
                var value = _materialService.CalculateMaterialValue(words);

                _materialService.Add(material, value);

                _console.WriteLine($"{material} = {value}");
            }
            while (true);
        }

        private void AnswerQuestion()
        {
            _console.WriteLine("\nAgora vamos descobrir os valores e quantidade! \nInforme a transação no formato: quanto é <medidas> <material> créditos? ou quanto é <medidas>?");
            var input = _inputService.ReadInput("Digite sua pergunta sobre quantidade e valores:");

            var words = ExtractQuery(input);

            var result = IsMaterialQuery(words)
                ? _materialService.CalculateTransactionValue(words)
                : _romanCalculator.Calculate(words);

            _console.WriteLine($"{string.Join(" ", words)} = {result}");
        }

        private List<string> GetWords(string input)
        {
            return input
                .Replace("?", "")
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim().ToLower())
                .ToList();
        }

        private List<string> ExtractQuery(string input)
        {
            var words = GetWords(input);

            var relevantWords = words
                .Where(w => !IgnoredWords.Contains(w.ToLower()))
                .ToList();

            if (relevantWords.Count == 0)
                throw new InvalidOperationException("Pergunta inválida");

            return relevantWords;
        }

        private string ExtractMaterial(List<string> words)
        {
            if (words.Count < 4)
                throw new InvalidOperationException("Formato inválido");

            return words[^4];
        }

        private bool IsMaterialQuery(List<string> words)
        {
            if (words.Count == 0)
                return false;

            return _materialService.Exists(words.Last());
        }
    }
}