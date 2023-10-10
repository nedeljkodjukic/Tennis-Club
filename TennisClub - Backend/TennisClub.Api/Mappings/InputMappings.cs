using AutoMapper;
using TennisClub.Api.Models.Cosmos.Containers;
using TennisClub.Api.Models.Cosmos.SimpleObjects;
using TennisClub.Api.Models.Input;

namespace TennisClub.Api.Mappings;

public class InputMappings : Profile
{
    public InputMappings()
    {
        CreateMap<PlayerInputModel, Player>();

        CreateMap<TournamentInputModel, Tournament>();

        CreateMap<RankInputModel, WeeklyRank>();

        CreateMap<CourtInputOutputModel, Court>();

        CreateMap<MatchInputModel, Match>()
            .ForMember(dest => dest.DurationInMinutes, opt => opt.MapFrom(source => source.Sets.Select(set => set.Duration).Sum()))
            .ForMember(dest => dest.WinnerId,
                opt => opt
                    .MapFrom(source =>
                        source.Sets.Count(set => set.FirstPlayerScore > set.SecondPlayerScore) >
                        source.Sets.Count(set => set.SecondPlayerScore > set.FirstPlayerScore) ?
                        source.FirstPlayerId : source.SecondPlayerId
                        )
                    );

        CreateMap<MatchUpdateInputModel, Match>()
            .ForMember(dest => dest.DurationInMinutes, opt => opt.MapFrom(source => source.Sets.Select(set => set.Duration).Sum()));
    }
}
