using FluentValidation;
using L4D2PlayStats.Core.Modules.Mix.Commands;
using L4D2PlayStats.Core.Modules.Mix.Results;
using L4D2PlayStats.Core.Modules.Ranking.Services;

namespace L4D2PlayStats.Core.Modules.Mix.Services;

public interface IMixService
{
    Task<MixResult> MixAsync(string server, MixCommand command);
}

public class MixService : IMixService
{
    private readonly IRankingService _rankingService;
    private readonly IValidator<MixCommand> _validator;

    public MixService(IRankingService rankingService,
        IValidator<MixCommand> validator)
    {
        _rankingService = rankingService;
        _validator = validator;
    }

    public async Task<MixResult> MixAsync(string server, MixCommand command)
    {
        await _validator.ValidateAndThrowAsync(command);

        var ranking = await _rankingService.RankingAsync(server);
        var positions = ranking.ToDictionary(k => k.CommunityId.ToString(), v => v.Position);
        var players = command.All
            .Where(communityId => positions.ContainsKey(communityId))
            .OrderBy(communityId => positions[communityId])
            .ToList();

        if (players.Count != 8)
            throw new Exception("It was not possible to generate the mix based on the data entered");

        var result = new MixResult(players);

        return result;
    }
}