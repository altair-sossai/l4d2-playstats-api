using FluentValidation;
using L4D2PlayStats.Core.Modules.Mix.Commands;
using L4D2PlayStats.Core.Modules.Mix.Results;
using L4D2PlayStats.Core.Modules.Ranking.Services;

namespace L4D2PlayStats.Core.Modules.Mix.Services;

public class MixService(IRankingService rankingService, IValidator<MixCommand> validator) : IMixService
{
    public async Task<MixResult> MixAsync(string serverId, int count, MixCommand command)
    {
        await validator.ValidateAndThrowAsync(command);

        var ranking = await rankingService.RankingAsync(serverId, count);
        var players = ranking.ToDictionary(k => k.CommunityId.ToString(), v => v);
        var availables = command.All
            .Where(players.ContainsKey)
            .OrderBy(communityId => players[communityId].Position)
            .ToList();

        if (availables.Count != 8)
            throw new ValidationException("One or more players are not present in the ranking");

        var result = new MixResult(availables, players);

        return result;
    }
}