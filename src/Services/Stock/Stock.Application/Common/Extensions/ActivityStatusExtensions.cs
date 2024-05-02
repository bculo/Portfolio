using Queryable.Common.Models;
using Stock.Core.Enums;

namespace Stock.Application.Common.Extensions;

public static class ActivityStatusExtensions
{
    public static bool? ToBoolean(this Status status)
    {
        if (status == Status.None)
        {
            return null;
        }

        return (status == Status.Active) ? true : false;
    }

    public static EqualFilter<bool>? ToEqualFilter(this Status status)
    {
        var boolValue = status.ToBoolean();
        if (boolValue.HasValue)
        {
            return new EqualFilter<bool>(boolValue.Value);
        }
        return null;
    }
}