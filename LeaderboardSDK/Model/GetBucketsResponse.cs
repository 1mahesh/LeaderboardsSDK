using System.Runtime.Serialization;

namespace Anishoo.Services.Leaderboard.Model
{
    [DataContract]
    public class GetBucketsResponse : LeaderboardResponse
    {
        [DataMember(Name="buckets")]
        public string[] Buckets { get; set; }
    }
}
