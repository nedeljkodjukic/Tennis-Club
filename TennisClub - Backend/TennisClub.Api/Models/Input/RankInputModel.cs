using System.ComponentModel.DataAnnotations;

namespace TennisClub.Api.Models.Input;

public class RankInputModel
{

    [Required]
    public string PlayerId { get; init; }


    [Required]
    public int RankNumber { get; init; }


    [Required]
    public int PreviousRankNumber { get; init; }


    [Required]
    public int Points { get; init; }
}
