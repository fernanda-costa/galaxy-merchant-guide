using Localiza.MerchantGuide.Application.Services;
using Xunit;

public class IntergalacticServiceTests
{
    [Fact]
    public void Add_ShouldStoreMapping()
    {
        var service = new IntergalacticService();

        service.Add("glob", "I");

        var result = service.GetRoman("glob");

        Assert.Equal("I", result);
    }

    [Fact]
    public void Add_ShouldThrow_WhenDuplicateKey()
    {
        var service = new IntergalacticService();

        service.Add("glob", "I");

        Assert.Throws<InvalidOperationException>(() =>
            service.Add("glob", "V"));
    }

    [Fact]
    public void GetRoman_ShouldThrow_WhenNotExists()
    {
        var service = new IntergalacticService();

        Assert.Throws<InvalidOperationException>(() =>
            service.GetRoman("unknown"));
    }
}