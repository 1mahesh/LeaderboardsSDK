using System.Runtime.Serialization;

namespace Anishoo.Services.Leaderboard.Model
{
    [DataContract]
    public abstract class LeaderboardRequest
    {
        [DataMember(Name="page")]
        public int StartPage { get; set; }
    }
}
