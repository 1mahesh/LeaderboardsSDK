using System;
using Anishoo.Services.Leaderboard.Model;
using System.Runtime.Serialization.Json;
using System.Net;
using System.ComponentModel;

namespace Anishoo.Services.Leaderboard
{
    public class LeaderboardContext
    {
        string devKey;
        string secret;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="devKey">your leaderboards dev key</param>
        /// <param name="secret">Your accounts secret key</param>
        public LeaderboardContext(string devKey, string secret)
        {
            Utilities.CheckPropertyForNullOrEmpty(devKey, "devKey");
            Utilities.CheckPropertyForNullOrEmpty(secret, "secret");

            this.devKey = devKey;
            this.secret = secret;
        }

        /// <summary>
        /// Add a score to the leaderboard
        /// </summary>
        /// <typeparam name="T">Whether its an int or decimal type</typeparam>
        /// <param name="user">Username - Cannot be empty</param>
        /// <param name="score">Score</param>
        /// <param name="callback">Callback to call with response</param>
        public void AddScore<T>(string user, T score, Action<AddScoreResponse> callback) where T : struct
        {
            AddScore<T>(user, score, "default", callback);
        }

        /// <summary>
        /// Add a score to the leaderboard
        /// </summary>
        /// <typeparam name="T">Whether its an int or decimal type</typeparam>
        /// <param name="user">Username - Cannot be empty</param>
        /// <param name="score">Score</param>
        /// <param name="category">Specify a category</param>
        /// <param name="callback">Callback to call with response</param>
        public void AddScore<T>(string user, T score, string category, Action<AddScoreResponse> callback) where T : struct
        {
            Utilities.CheckPropertyForNullOrEmpty(user, "uname");
            Utilities.CheckPropertyForNullOrEmpty(category, "category");
            Utilities.CheckPropertyForNullOrEmpty(callback, "callback");


            if (typeof(T) != typeof(int) && typeof(T) != typeof(decimal))
            {
                throw new ArgumentException("Score must be either int or decimal to be comparable");
            }

            string formatStr = String.Format(Constants.UrlHashFormats.AddScoreFormat, this.secret, category, score, user);
            string hash = MD5Core.GetHashString(formatStr).ToLower();

            string url = string.Format(Constants.UrlFormats.AddScoreFormat, this.devKey, hash);

            AddScoreRequest request = new AddScoreRequest()
            {
                uname = user,
                category = category,
                score = decimal.Parse(score.ToString())
            };
            ExecuteRequest<AddScoreRequest, AddScoreResponse>(url, request, callback, true);

        }

        /// <summary>
        /// Get rank for the specified username and specified category
        /// </summary>
        /// <param name="user">Username</param>
        /// <param name="kind">Day, Week or Alltime rank specification</param>
        /// <param name="category">Category</param>
        /// <param name="callback">Callback to call with response</param>
        public void GetRank(string user, LeaderboardRequestKind kind, string category, Action<GetRankResponse> callback)
        {
            Utilities.CheckPropertyForNullOrEmpty(user, "uname");
            Utilities.CheckPropertyForNullOrEmpty(category, "category");
            Utilities.CheckPropertyForNullOrEmpty(callback, "callback");

            string url = string.Format(Constants.UrlFormats.GetRankFormat, kind.ToString().ToLower(), this.devKey, user, category);

            GetRankRequest request = new GetRankRequest();
            ExecuteRequest<GetRankRequest, GetRankResponse>(url, request, callback);

        }

        /// <summary>
        /// Get leaderboard for the specified category
        /// </summary>
        /// <param name="user">Username</param>
        /// <param name="kind">Day, Week or Alltime leaderboard specification</param>
        /// <param name="category">Category</param>
        /// <param name="callback">Callback to call with response</param>
        public void GetLeaderboard(LeaderboardRequestKind kind, string category, int page, Action<GetLeaderboardResponse> callback)
        {
            if (page < 0)
            {
                throw new ArgumentOutOfRangeException("page");
            }
            Utilities.CheckPropertyForNullOrEmpty(category, "category");
            Utilities.CheckPropertyForNullOrEmpty(callback, "callback");

            string url = string.Format(Constants.UrlFormats.GetLeaderboardFormat, kind.ToString().ToLower(), this.devKey, category);

            GetLeaderboardRequest request = new GetLeaderboardRequest() { StartPage = page };
            ExecuteRequest<GetLeaderboardRequest, GetLeaderboardResponse>(url, request, callback);

        }

        /// <summary>
        /// Get list of buckets for specified leaderboard
        /// </summary>
        /// <param name="callback">Callback to call with response</param>
        public void GetBuckets(Action<GetBucketsResponse> callback)
        {
            Utilities.CheckPropertyForNullOrEmpty(callback, "callback");

            string url = string.Format(Constants.UrlFormats.GetBucketsFormat, this.devKey);

            GetBucketsRequest request = new GetBucketsRequest();

            ExecuteRequest<GetBucketsRequest, GetBucketsResponse>(url, request, callback);
        }

        void ExecuteRequest<T, V>(string url, T request, Action<V> callback, bool doPost = false)
            where T : LeaderboardRequest
            where V : LeaderboardResponse, new()
        {
            WebClient wc = this.GetWebClient();
            if (doPost)
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.UploadStringCompleted += (object sender, UploadStringCompletedEventArgs args) =>
                {
                    callback(HandleWebClientResponse<V>(doPost, args));
                };
                string body = Utilities.Serialize<T>(request);
                wc.UploadStringAsync(new Uri(url), body);
            }
            else
            {
                wc.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs args) =>
                {
                    callback(HandleWebClientResponse<V>(doPost, args));
                };
                wc.DownloadStringAsync(new Uri(url));
            }
        }

        T HandleWebClientResponse<T>(bool doPost, AsyncCompletedEventArgs args) where T : LeaderboardResponse, new()
        {
            try
            {
                if (args.Error != null)
                {
                    return new T()
                    {
                        ValidationError = new LeaderboardErrorResponse()
                        {
                            ErrorCode = "Network Error",
                            ErrorDescription = args.Error.Message
                        }
                    };
                }
                else if (args.Cancelled)
                {
                    return new T()
                    {
                        ValidationError = new LeaderboardErrorResponse()
                        {
                            ErrorCode = "User Cancel",
                            ErrorDescription = "Cancelled"
                        }
                    };
                }
                else
                {
                    return Utilities.Deserialize<T>(doPost
                        ? ((UploadStringCompletedEventArgs)args).Result
                        : ((DownloadStringCompletedEventArgs)args).Result);
                }
            }
            catch (Exception exp)
            {
                return new T()
                {
                    ValidationError = new LeaderboardErrorResponse()
                    {
                        ErrorCode = "Deserialization Error",
                        ErrorDescription = exp.Message
                    }
                };
            }
        }

        WebClient GetWebClient()
        {
            WebClient wc = new WebClient();
            return wc;
        }
    }
}
