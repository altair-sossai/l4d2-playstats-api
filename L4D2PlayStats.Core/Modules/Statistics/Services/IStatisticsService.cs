﻿using L4D2PlayStats.Core.Modules.Statistics.Commands;
using L4D2PlayStats.Core.Modules.Statistics.Results;

namespace L4D2PlayStats.Core.Modules.Statistics.Services;

public interface IStatisticsService
{
    Task<Statistics> AddOrUpdateAsync(string serverId, StatisticsCommand command);
    Task<List<StatisticsSimplifiedResult>> GetStatistics(string serverId);
}