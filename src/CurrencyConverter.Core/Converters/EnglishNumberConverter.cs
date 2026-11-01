using System.Globalization;
using System.Resources;

namespace CurrencyConverter.Core.Converters;

public class EnglishNumberConverter: INumberConverter
{
    private static readonly ResourceManager ResourceManager = new(
        "CurrencyConverter.Core.Resources.CurrencyWords",
        typeof(EnglishNumberConverter).Assembly
    );

    private static string GetWord(string key)
    {
        return ResourceManager.GetString(
            key,
            CultureInfo.GetCultureInfo("en")
        ) ?? throw new InvalidOperationException($"Missing resource: {key}");
    }

    public string Convert(int number)
    {
        if (number == 0)
        {
            return GetWord("Unit0");
        }

        var parts = new List<string>();

        if (number / 1_000_000 > 0)
        {
            parts.Add($"{Convert(number / 1_000_000)} {GetWord("MillionSingular")}");
            number %= 1_000_000;
        }

        if (number / 1_000 > 0)
        {
            parts.Add($"{Convert(number / 1_000)} {GetWord("Thousand")}");
            number %= 1_000;
        }

        if (number / 100 > 0)
        {
            parts.Add($"{Convert(number / 100)} {GetWord("Hundred")}");
            number %= 100;
        }

        if (number > 0)
        {
            if (number < 20)
            {
                parts.Add(GetWord($"Unit{number}"));
            }
            else
            {
                string tens = GetWord($"Ten{number / 10}");
                int units = number % 10;

                if (units > 0)
                {
                    parts.Add($"{tens}-{GetWord($"Unit{units}")}");
                }
                else
                {
                    parts.Add(tens);
                }
            }
        }

        return string.Join(" ", parts);
    }
    
}