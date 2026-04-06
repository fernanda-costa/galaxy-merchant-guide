using Localiza.MerchantGuide.Application.Services;
using Localiza.MerchantGuide.Domain.Services;
using Xunit;

public class MaterialServiceTests
{
    private readonly MaterialService _service;

    public MaterialServiceTests()
    {
        var intergalactic = new IntergalacticService();

        intergalactic.Add("glob", "I");
        intergalactic.Add("prok", "V");

        var roman = new RomanCalculatorService(intergalactic);

        _service = new MaterialService(roman);
    }

    [Fact]
    public void Add_And_Get_ShouldWork()
    {
        _service.Add("prata", 34);

        var result = _service.Get("prata");

        Assert.Equal(34, result);
    }

    [Fact]
    public void Get_ShouldThrow_WhenNotFound()
    {
        Assert.Throws<InvalidOperationException>(() =>
            _service.Get("ouro"));
    }

    [Fact]
    public void CalculateMaterialValue_ShouldWork()
    {
        var words = new List<string>
        {
            "glob", "glob", "prata", "é", "34", "creditos"
        };

        var result = _service.CalculateMaterialValue(words);

        Assert.Equal(17, result); 
    }

    [Fact]
    public void CalculateTransactionValue_ShouldWork()
    {
        _service.Add("prata", 17);

        var words = new List<string>
        {
            "glob", "glob", "prata"
        };

        var result = _service.CalculateTransactionValue(words);

        Assert.Equal(34, result);
    }
}