using System.Globalization;
using System.Resources;

namespace CurrencyConverter.Core.Converters;

public class GermanNumberConverter: INumberConverter
{
    private static readonly ResourceManager ResourceManager = new(
        "CurrencyConverter.Core.Resources.CurrencyWords",
        typeof(GermanNumberConverter).Assembly
    );

    private static string GetWord(string key)
    {
        return ResourceManager.GetString(
            key,
            CultureInfo.GetCultureInfo("de")
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
            int millions = number / 1_000_000;

            if (millions == 1)
            {
                parts.Add($"{GetWord("Unit1Feminine")} {GetWord("MillionSingular")}");
            }
            else
            {
                parts.Add($"{Convert(millions)} {GetWord("MillionPlural")}");
            }

            number %= 1_000_000;

            if (number > 0)
            {
                parts.Add(" ");
            }
        }

        // Thousand
        if (number / 1_000 > 0)
        {
            int thousands = number / 1_000;

            parts.Add($"{Convert(thousands)}{GetWord("Thousand")}");
            number %= 1_000;
        }

        // Hundred
        if (number / 100 > 0)
        {
            int hundreds = number / 100;

            parts.Add($"{Convert(hundreds)}{GetWord("Hundred")}");
            number %= 100;
        }

        // 1 ~ 99
        if (number > 0)
        {
            if (number < 20)
            {
                parts.Add(GetWord($"Unit{number}"));
            }
            else
            {
                int units = number % 10;
                int tens = number / 10;

                if (units > 0)
                {
                    parts.Add($"{GetWord($"Unit{units}")}{GetWord("Conjunction")}");
                }

                parts.Add(GetWord($"Ten{tens}"));
            }
        }

        return string.Join("", parts);
    }
}