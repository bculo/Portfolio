using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

namespace Trend.Application;

public static class Telemetry
{
    public static readonly ActivitySource Trend = new("TrendActivity");
    
    public const string SYNC_SRV = "Warmup search engines";
    public const string SYNC_SRV_NUM_TAG = "sync.engine.count";
    
    public const string SYNC_ENGINE = "Fire up search engine";
    public const string SYNC_ENGINE_NAME_TAG = "search.engine.name";
}