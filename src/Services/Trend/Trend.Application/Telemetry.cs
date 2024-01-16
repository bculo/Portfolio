using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Trend.Application;

public static class Telemetry
{
    public static readonly ActivitySource TrendActivity = new("TrendActivity");

    public static readonly string MassTransit = "MassTransit";
}