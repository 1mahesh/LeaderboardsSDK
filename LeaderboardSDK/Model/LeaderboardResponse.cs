using System.Runtime.Serialization;

namespace Anishoo.Services.Leaderboard.Model
{
    public class LeaderboardErrorResponse
    {
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
    }

    [DataContract]    
    public abstract class LeaderboardResponse
    {
        public LeaderboardErrorResponse ValidationError { get; set;}

        [DataMember(Name="result")]
        public string Result { get; set; }

        [DataMember(Name="warning")]
        public string[] Warning { get; set; }
    }
}
