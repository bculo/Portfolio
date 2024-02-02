namespace Queryable.Common.Models;

public abstract record Filter;
public record EqualFilter<TValue>(TValue? Value) : Filter;
public record ContainFilter(string? Value) : Filter;
public record GreaterThanFilter<TValue>(TValue? Value) : Filter where TValue : struct;
public record LessThenFilter<TValue>(TValue? Value) : Filter where TValue : struct;