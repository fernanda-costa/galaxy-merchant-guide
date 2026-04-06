using Localiza.MerchantGuide.Application.Services;
using Localiza.MerchantGuide.Domain.Services;
using Xunit;

public class RomanCalculatorServiceTests
{
    private readonly RomanCalculatorService _service;

    public RomanCalculatorServiceTests()
    {
        var intergalactic = new IntergalacticService();

        intergalactic.Add("glob", "I");
        intergalactic.Add("prok", "V");
        intergalactic.Add("pish", "X");
        intergalactic.Add("tegj", "L");

        _service = new RomanCalculatorService(intergalactic);
    }

    [Fact]
    public void Should_Return_2_For_II()
    {
        var input = new List<string> { "glob", "glob" };

        var result = _service.Calculate(input);

        Assert.Equal(2, result);
    }

    [Fact]
    public void Should_Return_6_For_VI()
    {
        var input = new List<string> { "prok", "glob" };

        var result = _service.Calculate(input);

        Assert.Equal(6, result);
    }

    [Fact]
    public void Should_Return_4_For_IV()
    {
        var input = new List<string> { "glob", "prok" };

        var result = _service.Calculate(input);

        Assert.Equal(4, result);
    }

    [Fact]
    public void Should_Return_9_For_IX()
    {
        var input = new List<string> { "glob", "pish" };

        var result = _service.Calculate(input);

        Assert.Equal(9, result);
    }

    [Fact]
    public void Should_Return_14_For_XIV()
    {
        var input = new List<string> { "pish", "glob", "prok" };

        var result = _service.Calculate(input);

        Assert.Equal(14, result);
    }


    [Fact]
    public void Should_Throw_When_Repeating_More_Than_3_Times()
    {
        var input = new List<string> { "glob", "glob", "glob", "glob" };

        Assert.Throws<InvalidOperationException>(() =>
            _service.Calculate(input));
    }

    [Fact]
    public void Should_Throw_When_Invalid_Subtraction_IL()
    {
        var input = new List<string> { "glob", "tegj" }; 

        Assert.Throws<InvalidOperationException>(() =>
            _service.Calculate(input));
    }

    [Fact]
    public void Should_Throw_When_Invalid_Subtraction_IC()
    {
        var intergalactic = new IntergalacticService();
        intergalactic.Add("glob", "I");
        intergalactic.Add("cent", "C");

        var service = new RomanCalculatorService(intergalactic);

        var input = new List<string> { "glob", "cent" };

        Assert.Throws<InvalidOperationException>(() =>
            service.Calculate(input));
    }

    [Fact]
    public void Should_Throw_When_V_Is_Subtracted()
    {
        var input = new List<string> { "prok", "pish" }; 

        Assert.Throws<InvalidOperationException>(() =>
            _service.Calculate(input));
    }

    [Fact]
    public void Should_Throw_When_Invalid_Order_IIV()
    {
        var input = new List<string> { "glob", "glob", "prok" };

        Assert.Throws<InvalidOperationException>(() =>
            _service.Calculate(input));
    }
}