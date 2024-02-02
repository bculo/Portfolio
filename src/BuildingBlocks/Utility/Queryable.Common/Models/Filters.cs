namespace Queryable.Common.Models;

public abstract record Filter;

public record EqualFilter<TValue>(TValue? Value) : Filter
{
    public static implicit operator TValue(EqualFilter<TValue> filter) => filter.Value;
}

public record ContainFilter(string? Value) : Filter
{
    public static implicit operator string(ContainFilter filter) => filter.Value;
}


public record GreaterThanFilter<TValue>(TValue Value) : Filter where TValue : struct
{
    public static implicit operator TValue(GreaterThanFilter<TValue> filter) => filter.Value;
}

public record LessThenFilter<TValue>(TValue Value) : Filter where TValue : struct
{
    public static implicit operator TValue(LessThenFilter<TValue> filter) => filter.Value;
}