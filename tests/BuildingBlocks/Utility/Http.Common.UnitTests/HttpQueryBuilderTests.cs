using FluentAssertions;
using Http.Common.Builders;
using Xunit;

namespace Http.Common.UnitTests;

public class HttpQueryBuilderTest
{
    [Fact]
    public void Clear_ShouldNotThrowException_WhenInvoked()
    {
        HttpQueryBuilder builder = new();
        
        try
        {
            builder.Clear();
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData(" ")]
    public void Remove_ShouldThrowException_WhenKeyIsNullOrEmpty(string? key)
    {
        HttpQueryBuilder builder = new();
        Assert.ThrowsAny<ArgumentException>(() => builder.Remove(key));
    }
    
    [Theory]
    [InlineData(null, "123")]
    [InlineData(" ", "")]
    [InlineData(null, null)]
    public void Add_ShouldThrowException_WhenKeyOrValueIsNullOrEmpty(string? key, string? value)
    {
        HttpQueryBuilder builder = new();
        Assert.ThrowsAny<ArgumentException>(() => builder.Add(key, value));
    }
    
    [Fact]
    public void Build_ShouldReturnUrl_WhenInvoked()
    {
        var queries = new Dictionary<string, string?>
        {
            { "asc", "1" },
            { "desc", "3" },
        };
        HttpQueryBuilder builder = new();
        foreach (var item in queries)
        {
            builder.Add(item.Key, item.Value);
        }
        
        string result = builder.Build();

        result.Should().StartWith("?");
        result.Should().Contain("asc=1");
        result.Should().Contain("desc=3");
    }
}