﻿namespace Prometheus;

/// <summary>
/// We occasionally need timestamps to attach to metrics metadata. In high-performance code, calling the standard get-timestamp functions can be a nontrivial cost.
/// This class does some caching to avoid calling the expensive timestamp functions too often, giving an accurate but slightly lower granularity clock as one might otherwise get.
/// </summary>
internal static class LowGranularityTimeSource
{
    [ThreadStatic]
    private static double LastUnixSeconds;

    [ThreadStatic]
    private static int LastTickCount;

    public static double GetSecondsFromUnixEpoch()
    {
        var currentTickCount = Environment.TickCount;

        if (LastTickCount != currentTickCount)
        {
            LastUnixSeconds = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1e3;
            LastTickCount = currentTickCount;
        }

        return LastUnixSeconds;
    }
}
