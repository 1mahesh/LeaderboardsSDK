
namespace Anishoo.Services.Leaderboard
{
    static class Constants
    {
        public const string LeaderboardBaseUrl = "http://api.anishoo.com";
        public const string Version = "v1";

        public struct UrlHashFormats
        {
            public const string AddScoreFormat = "{0}category{1}score{2}uname{3}";
        }

        public struct UrlFormats
        {
            public static string AddScoreFormat = string.Format("{0}/{1}/addscore/{{0}}/{{1}}", Constants.LeaderboardBaseUrl, Constants.Version);
            public static string GetBucketsFormat = string.Format("{0}/{1}/getbuckets/{{0}}", Constants.LeaderboardBaseUrl, Constants.Version);
            public static string GetRankFormat = string.Format("{0}/{1}/{{0}}rank/{{1}}/{{2}}/{{3}}", Constants.LeaderboardBaseUrl, Constants.Version);
            public static string GetLeaderboardFormat = string.Format("{0}/{1}/{{0}}lb/{{1}}/{{2}}", Constants.LeaderboardBaseUrl, Constants.Version);
        }
    }
}
