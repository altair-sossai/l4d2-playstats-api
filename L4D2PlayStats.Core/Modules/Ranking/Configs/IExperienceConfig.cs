﻿namespace L4D2PlayStats.Core.Modules.Ranking.Configs;

public interface IExperienceConfig
{
    int Win { get; }
    int Loss { get; }
    int Mvps { get; }
    int MvpsCommon { get; }
}