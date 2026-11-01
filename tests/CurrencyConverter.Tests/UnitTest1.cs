namespace CurrencyConverter.Tests;

public class CurrencyConverterTests
{
    [Theory]
    [InlineData("0", "zero dollars")]
    [InlineData("1", "one dollar")]
    [InlineData("25,1", "twenty-five dollars and ten cents")]
    [InlineData("0,01", "zero dollars and one cent")]
    [InlineData("45100", "forty-five thousand one hundred dollars")]
    [InlineData(
        "999999999,99",
        "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents"
    )]
    public void Convert_English_ShouldReturnExpectedWords(string amount, string expected)
    {
        var converter = new CurrencyConverter.Core.CurrencyConverter();

        string result = converter.Convert(amount, "en");

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("0", "null Dollar")]
    [InlineData("1", "ein Dollar")]
    [InlineData("25,1", "fünfundzwanzig Dollar und zehn Cent")]
    [InlineData("0,01", "null Dollar und ein Cent")]
    [InlineData("45100", "fünfundvierzigtausendeinhundert Dollar")]
    [InlineData(
        "999999999,99",
        "neunhundertneunundneunzig Millionen neunhundertneunundneunzigtausendneunhundertneunundneunzig Dollar und neunundneunzig Cent"
    )]
    public void Convert_German_ShouldReturnExpectedWords(string amount, string expected)
    {
        var converter = new CurrencyConverter.Core.CurrencyConverter();

        string result = converter.Convert(amount, "de");

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("", "Please enter an amount.")]
    [InlineData("abc", "Please enter a valid amount.")]
    [InlineData("-1", "Please enter a valid amount.")]
    [InlineData("25.50", "Please enter a valid amount.")]
    [InlineData("25,,1", "Please enter a valid amount.")]
    [InlineData("25,", "Please enter a valid amount.")]
    [InlineData("45 100", "Please enter a valid amount.")]
    [InlineData("25,123", "Please enter no more than 2 decimal places.")]
    [InlineData("0,124", "Please enter no more than 2 decimal places.")]
    [InlineData("1000000000", "Amount must not exceed 999,999,999.99 USD.")]
    [InlineData("999999999999", "Amount must not exceed 999,999,999.99 USD.")]
    public void Convert_InvalidAmount_ShouldThrowExpectedError(
        string amount,
        string expectedMessage)
    {
        var converter = new CurrencyConverter.Core.CurrencyConverter();

        var exception = Assert.Throws<ArgumentException>(
            () => converter.Convert(amount, "en")
        );

        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Convert_UnsupportedLanguage_ShouldThrowArgumentException()
    {
        var converter = new CurrencyConverter.Core.CurrencyConverter();

        var exception = Assert.Throws<ArgumentException>(
            () => converter.Convert("1", "ko")
        );
        Assert.Equal("Unsupported language", exception.Message);
    }
}