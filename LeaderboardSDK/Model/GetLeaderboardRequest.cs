using System.Runtime.Serialization;

namespace Anishoo.Services.Leaderboard.Model
{
    [DataContract]
    public class GetLeaderboardRequest : LeaderboardRequest
    {
        [DataMember(Name = "page")]
        public int StartPage { get; set; }
    }
}
