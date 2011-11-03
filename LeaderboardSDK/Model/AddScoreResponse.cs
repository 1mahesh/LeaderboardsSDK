using System.Runtime.Serialization;

namespace Anishoo.Services.Leaderboard.Model
{
    [DataContract]
    public class AddScoreResponse : LeaderboardResponse
    {
        [DataMember(Name="rank")]
        public int Rank { get; set; }

        [DataMember(Name = "totalcount")]
        public int TotalUsers { get; set; }
    }
}
