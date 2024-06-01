using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

namespace Trend.Application;

public static class Telemetry
{
    public static readonly ActivitySource Trend = new("TrendActivity");
    
    public const string SyncSrv = "Warmup search engines";
    public const string SyncSrvNumTag = "sync.engine.count";
    
    public const string SyncEngine = "Fire up search engine";
    public const string SyncEngineNameTag = "search.engine.name";
}