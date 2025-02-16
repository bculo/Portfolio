using Queryable.Common.Extensions;
using Queryable.Common.Models;

namespace Queryable.Common.Tests.Extensions;

internal record Person(string Name, int Age);

public class QueryableExtensionsTest
{
    [Fact]
    public void ShouldReturnSortedList_WhenMethodInvoked()
    {
        List<Person> data =
        [
            new("John", 19),
            new("Ana", 17),
            new("Ana", 19),
            new("Bob", 31)
        ];
        
        var query = data.AsQueryable();

        query = query.ApplyOrderByColumn(new StringSort("Name", SortDirection.Ascending))
            .ApplyThenOrderByColumn(new StringSort("Age", SortDirection.Descending));
        
        var result = query.ToList();
        
        Assert.Equal("Ana", result[0].Name);
        Assert.Equal(19, result[0].Age);
        Assert.Equal("Ana", result[1].Name);
        Assert.Equal("Bob", result[2].Name);
        Assert.Equal("John", result[3].Name);
    }
}