using System.Globalization;
using System.Resources;
using System.Text.RegularExpressions;
using CurrencyConverter.Core.Converters;

namespace CurrencyConverter.Core;

public class CurrencyConverter
{
    private static readonly ResourceManager ResourceManager = new("CurrencyConverter.Core.Resources.CurrencyWords", typeof(CurrencyConverter).Assembly);

    private static string GetWord(string key, CultureInfo culture)
    {
        string? word = ResourceManager.GetString(key, culture);

        if (word == null)
        {
            throw new InvalidOperationException($"Missing resource: {key}");
        }

        return word;
    }

    public string Convert(string amount, string language)
    {
        // Validate the input amount 
        if (string.IsNullOrWhiteSpace(amount))
        {
            throw new ArgumentException("Please enter an amount.");
        }

        if (!Regex.IsMatch(amount, @"\A[0-9]+(,[0-9]+)?\z"))
        {
            throw new ArgumentException("Please enter a valid amount.");
        }

        string[] parts = amount.Split(',');

        if (parts.Length == 2 && parts[1].Length > 2)
        {
            throw new ArgumentException("Please enter no more than 2 decimal places.");
        }

        if (parts[0].Length > 9)
        {
            throw new ArgumentException("Amount must not exceed 999,999,999.99 USD.");
        }

        int dollars = int.Parse(parts[0]);
        int cents = 0;

        if (parts.Length == 2)
        {
            // Pad to two digits: ",1" means 10 cents
            string centPart = parts[1].PadRight(2, '0');
            cents = int.Parse(centPart);
        }

        INumberConverter converter;

        switch (language)
        {
            case "en":
                converter = new EnglishNumberConverter();
                break;

            case "de":
                converter = new GermanNumberConverter();
                break;

            default:
                throw new ArgumentException("Unsupported language");
        }

        CultureInfo culture = CultureInfo.GetCultureInfo(language);

        string dollarWords = converter.Convert(dollars);
        string dollarKey; 

        // use "dollar" for 1, otherwise "dollars"
        if (dollars == 1)
        {
            dollarKey = "DollarSingular"; 
        }
        else
        {
            dollarKey = "DollarPlural";  
        }

        string dollarUnit = GetWord(dollarKey, culture);

        string result = $"{dollarWords} {dollarUnit}";

        if (cents > 0)
        {
            string centWords = converter.Convert(cents);
            string centKey;

            // use "cent" for 1, otherwise "cents"
            if (cents == 1)
            {
                centKey = "CentSingular";
            }
            else
            {
                centKey = "CentPlural";
            }

            string centUnit = GetWord(centKey, culture);

            string conjunction = GetWord("Conjunction", culture);

            result += $" {conjunction} {centWords} {centUnit}";
        }

        return result;
    }
}