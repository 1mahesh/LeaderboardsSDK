using System.Runtime.Serialization;

namespace Anishoo.Services.Leaderboard.Model
{
    [DataContract]
    public class GetLeaderboardResponse : LeaderboardResponse
    {
        [DataMember(Name = "totalpages")]
        public int TotalPages { get; set; }

        [DataMember(Name = "totalcount")]
        public int TotalUsers { get; set; }

        [DataMember(Name = "page")]
        public int CurrentPage { get; set; }

        [DataMember(Name = "users")]
        public string[] Users { get; set; }

        [DataMember(Name = "scores")]
        public string[] Scores { get; set; }
    }
}
