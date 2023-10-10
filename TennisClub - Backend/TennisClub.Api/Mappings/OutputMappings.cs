using AutoMapper;
using TennisClub.Api.Models.Cosmos.Containers;
using TennisClub.Api.Models.Cosmos.SimpleObjects;
using TennisClub.Api.Models.Input;
using TennisClub.Api.Models.Output;

namespace TennisClub.Api.Mappings;

public class OutputMappings : Profile
{
    public OutputMappings()
    {
        CreateMap<Player, PlayerOutputModel>()
            .ForMember(dest => dest.Instagram, opt =>
                opt.MapFrom(source => source.Accounts.SingleOrDefault(x => x.SocialNetwork == "Instagram").Link))
            .ForMember(dest => dest.Twitter,
                opt => opt.MapFrom(source => source.Accounts.SingleOrDefault(x => x.SocialNetwork == "Twitter").Link));

        /*
        CreateMap<WeeklyRank, RankOutputModel>()
            .ForMember(dest => dest.PreviousRank, opt => opt.MapFrom(source => source.PreviousRankNumber))
            .ForMember(dest => dest.Rank, opt => opt.MapFrom(source => source.RankNumber));
        */

        CreateMap<Court, CourtInputOutputModel>();

        CreateMap<Tournament, TournamentOutputModel>();

        CreateMap<Match, MatchOutputModel>()
            .ForMember(dest => dest.FirstPlayerFullName, opt =>
                opt.MapFrom(source => $"{source.FirstPlayer.FirstName} {source.FirstPlayer.LastName}"))
            .ForMember(dest => dest.SecondPlayerFullName, opt =>
                opt.MapFrom(source => $"{source.SecondPlayer.FirstName} {source.SecondPlayer.LastName}"));

    }
}
