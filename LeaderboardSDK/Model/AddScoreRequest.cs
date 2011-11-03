using System.Runtime.Serialization;

namespace Anishoo.Services.Leaderboard.Model
{
    [DataContract]
    public class AddScoreRequest : LeaderboardRequest
    {
        [DataMember]
        public string uname { get; set; }

        [DataMember]
        public decimal score { get; set; }

        [DataMember]
        public string category { get; set; }
    }
}
