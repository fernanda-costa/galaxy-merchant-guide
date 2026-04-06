using Localiza.MerchantGuide.Enums;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Localiza.MerchantGuide
{
    public class MerchantGuideService(IConsole console)
    {

        public Dictionary<string, string> intergalactiNumbers = [];
        public Dictionary<string, decimal> materialValues = [];

        private readonly IConsole _console = console;

        public void Run()
        {
            var optionSelected = -1;

            do
            {
                try
                {
                    _console.WriteLine("\n");
                    _console.WriteLine("--- Bem vindo ao seu conversor de unidades intergalácticas! ---");
                    _console.WriteLine("1 - Mapear numerais intergalácticos");
                    _console.WriteLine("2 - Calcular valor de metais a partir de transações");
                    _console.WriteLine("3 - Responder consultas");
                    _console.WriteLine("0 - Sair");
                    _console.WriteLine("\n");

                    var menuOptionSelected = _console.ReadLine();

                    if (!int.TryParse(menuOptionSelected, out optionSelected))
                    {
                        _console.WriteLine("Opção inválida. Favor inserir algo entre 0 e 4");
                        continue;
                    }

                    switch (optionSelected)
                    {
                        case (int)MenuOptionsEnum.ReadIntergalacticNumber:
                            ReadIntergalacticNumber();
                            break;

                        case (int)MenuOptionsEnum.ReceiveMaterialValues:
                            ReceiveMaterialValues();
                            break;

                        case (int)MenuOptionsEnum.GetTransactionValue:
                            GetTransactionValue();
                            break;

                        case (int)MenuOptionsEnum.Leave:
                            break;

                        default:
                            _console.WriteLine("Opção inválida. Favor inserir algo entre 0 e 4");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _console.WriteLine($"Erro: {ex.Message}");
                }

            } while (optionSelected != (int)MenuOptionsEnum.Leave);
        }
        private Dictionary<string, string> ReadIntergalacticNumber()
        {
            intergalactiNumbers.TryAdd(ReadInput("Qual valor você quer atribuir ao I?", ValidateIntergalactNumbersInput), "I");
            intergalactiNumbers.TryAdd(ReadInput("Qual valor você quer atribuir ao V?", ValidateIntergalactNumbersInput), "V");
            intergalactiNumbers.TryAdd(ReadInput("Qual valor você quer atribuir ao X?", ValidateIntergalactNumbersInput), "X");
            intergalactiNumbers.TryAdd(ReadInput("Qual valor você quer atribuir ao L?", ValidateIntergalactNumbersInput), "L");
            //intergalactiNumbers.TryAdd(ReadInput("Qual valor você quer atribuir ao C?", ValidateIntergalactNumbersInput), "C");
            //intergalactiNumbers.TryAdd(ReadInput("Qual valor você quer atribuir ao D?", ValidateIntergalactNumbersInput), "D");
            //intergalactiNumbers.TryAdd(ReadInput("Qual valor você quer atribuir ao M?", ValidateIntergalactNumbersInput), "M");

            return intergalactiNumbers;
        }

        void ReceiveMaterialValues()
        {
            if(intergalactiNumbers.Count == 0)
            {
                _console.WriteLine("\nPrimeiro, precisamos mapear os numerais intergalácticos para depois receber os valores dos materiais. Por favor, mapeie os numerais intergalácticos.");
                ReadIntergalacticNumber();
            }

            string? inputValue;

            _console.WriteLine("\nAgora, por favor vamos descobrir os valores correspondentes aos materiais");

            do
            {
                inputValue = ReadInput("Insira a transação?", ValidateIntergalactMaterialsInput);

                if (inputValue == "SAIR") break;

                var transactionWords = inputValue.Split(' ').ToList();
                var material = transactionWords[^4];
                var materialValue = CalculateMaterialValue(transactionWords);

                materialValues.TryAdd(material.ToLower(), materialValue);

                _console.WriteLine($"\nO Material {material} custa {materialValue}");


            } while (inputValue != "SAIR");
        }

        decimal CalculateMaterialValue(List<string> inputValue)
        {
            var creditsValue = inputValue[inputValue.Count() - 2];
            var romanValues = inputValue.Slice(0, inputValue.Count() - 4);
            var romanValuesCalculated = CalculateIntergalactValue(romanValues);
            var materialValue = decimal.Parse(creditsValue) / romanValuesCalculated;

            return materialValue;
        }


        decimal CalculateIntergalactValue(List<string> intergalactValuesInput)
        {
            decimal result = 0;
            decimal plusvalue;
            var numbersCount = new Dictionary<string, decimal>();

            for (int index = 0; index < intergalactValuesInput.Count; index++)
            {
                intergalactiNumbers.TryGetValue(intergalactValuesInput[index], out var romanKey);
                Constants.ROMAN_NUMBERS.TryGetValue(romanKey!, out var romanNumber);

                var hasNextValue = index + 1 < intergalactValuesInput.Count;

                if (numbersCount.ContainsKey(romanKey))
                {
                    numbersCount[romanKey]++;
                }
                else
                {
                    numbersCount.Add(romanKey!, 1);
                }

                if (hasNextValue)
                {
                    intergalactiNumbers.TryGetValue(intergalactValuesInput[index + 1], out var nextRomanKey);
                    Constants.ROMAN_NUMBERS.TryGetValue(nextRomanKey!, out var nextRomanNumber);

                    bool mustSubtract = romanNumber < nextRomanNumber;

                    ValidateAllowedRepetead(numbersCount, romanKey, mustSubtract);

                    if (mustSubtract)
                    {
                        plusvalue = nextRomanNumber - romanNumber;

                        ValidateNextNumber(romanKey, romanNumber, nextRomanKey, nextRomanNumber);

                        index++;
                    }
                    else
                    {
                        plusvalue = romanNumber;
                    }
                }
                else
                {
                    plusvalue = romanNumber;
                }

                result += plusvalue;
            }

            _console.WriteLine($"O valor total em números romanos é {result}");

            return result;
        }

        private static void ValidateAllowedRepetead(Dictionary<string, decimal> numbersCount, string romanKey, bool mustSubtract)
        {
            if (numbersCount[romanKey] == 3 && !mustSubtract)
            {
                throw new Exception($"Valor romano inválido. O valor {romanKey} não pode se repetir mais que 4 vezes!");
            }
        }

        private void ValidateNextNumber(string? romanKey, decimal romanNumber, string? nextRomanKey, decimal nextRomanNumber)
        {

            var romanNumbersList = Constants.ROMAN_NUMBERS.ToList();
            var romanKeyIndex = romanNumbersList.FindIndex(x => x.Key == romanKey);

            if (romanKey == "V" || romanKey == "L" || romanKey == "M")
            {
                throw new Exception($"Valor romano inválido. O valor {romanKey} não pode ser subtraido");
            }
            else if (nextRomanKey != romanNumbersList[romanKeyIndex + 1].Key && romanNumbersList[romanKeyIndex + 2].Key != nextRomanKey) throw new Exception("Valor romano inválido");


            if (nextRomanNumber < romanNumber) throw new Exception($"Valor romano inválido. O valor {nextRomanNumber} é maior que o {romanNumber}");

        }

        public decimal GetTransactionValue()
        {
            if (materialValues.Count == 0)
            {
                _console.WriteLine("Primeiro, precisamos mapear os numerais intergalácticos para depois receber os valores dos materiais. Por favor, mapeie os numerais intergalácticos.");
                ReceiveMaterialValues();
            }

            string? value = ReadInput("Qual valor você quer descobrir?");

            bool hasMaterial = value
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Any(word => word.Equals("creditos", StringComparison.OrdinalIgnoreCase));

            var formattedValues = value.Split('é')[1]
                .Split(' ').Where(e => !string.IsNullOrWhiteSpace(e))
                .Select(w => w.Trim().ToLower()).ToList();

            var result = hasMaterial
                ? CalculateTransactionValue(formattedValues)
                : CalculateIntergalactValue(formattedValues);

            _console.WriteLine($"\n{string.Join(' ', formattedValues)} é {result}");

            return result;
        }


        decimal CalculateTransactionValue(List<string> words)
        {
            var materialValueInput = words[words.Count() - 1];
            var romanValues = words.ToList().Slice(0, words.Count() - 1);
            var romanValuesCalculated = CalculateIntergalactValue(romanValues);

            materialValues.TryGetValue(materialValueInput.ToLower(), out decimal materialValue);

            var finalValue = romanValuesCalculated * materialValue;

            return finalValue;
        }

        public string ReadInput(string message, Func<string, bool>? inputValidation = null)
        {
            string? inputValue = string.Empty;
            bool isValid;

            do
            {
                _console.WriteLine(message);
                inputValue = _console.ReadLine();

                isValid = inputValidation != null ? inputValidation(inputValue!) : !string.IsNullOrEmpty(inputValue);

            } while (!isValid);

            return inputValue!.Trim();
        }

        public bool ValidateIntergalactNumbersInput(string? inputValue)
        {

            if (string.IsNullOrEmpty(inputValue))
            {
                _console.WriteLine("Entrada inválida. Por favor, insira um valor.");
                return false;
            }

            var valueAlreadyUsed = intergalactiNumbers.Values
                .Any(attributedValue => attributedValue.Equals(inputValue, StringComparison.OrdinalIgnoreCase));

            if (valueAlreadyUsed)
            {
                _console.WriteLine("Esse valor já foi usado. Por favor, insira um valor.");
                return false;
            }

            return true;
        }

        public bool ValidateIntergalactMaterialsInput(string? inputValue)
        {

            if (string.IsNullOrEmpty(inputValue))
            {
                _console.WriteLine("Entrada inválida. Por favor, insira um valor.");
                return false;
            }

            var valueAlreadyUsed = materialValues.Keys
                .Any(attributedValue => attributedValue.Equals(inputValue, StringComparison.OrdinalIgnoreCase));

            if (valueAlreadyUsed)
            {
                _console.WriteLine("Esse valor já foi usado. Por favor, insira um valor.");
                return false;
            }

            if (inputValue == "SAIR") return true;

            return true;
        }

    }
}
