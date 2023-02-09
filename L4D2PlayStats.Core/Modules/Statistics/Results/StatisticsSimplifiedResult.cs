﻿namespace L4D2PlayStats.Core.Modules.Statistics.Results;

public class StatisticsSimplifiedResult
{
    public string? Server { get; set; }
    public string? FileName { get; set; }
    public GameRound? GameRound { get; set; }
    public Scoring? Scoring { get; set; }
    public List<PlayerName>? PlayerNames { get; set; }
}