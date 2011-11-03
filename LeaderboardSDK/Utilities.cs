using System;
using Anishoo.Services.Leaderboard.Model;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Anishoo.Services.Leaderboard
{
    static class Utilities
    {
        public static void CheckPropertyForNullOrEmpty(object property, string propertyName)
        {
            if (property == null)
            {
                throw new ArgumentNullException(propertyName);
            }

            if (property is string && string.IsNullOrEmpty((string)property))
            {
                throw new ArgumentException(propertyName);
            }
        }

        public static string Serialize<T>(T input) where T : LeaderboardRequest
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T), 
                new List<Type>() 
                {
                    typeof(LeaderboardRequest)
                });

            MemoryStream ms = new MemoryStream();

            json.WriteObject(ms, input);

            ms.Seek(0, SeekOrigin.Begin);
            return new StreamReader(ms).ReadToEnd();
        }


        public static T Deserialize<T>(string input) where T : LeaderboardResponse
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T),
                new List<Type>() 
                {
                    typeof(LeaderboardResponse)
                });

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(input));
            ms.Seek(0, SeekOrigin.Begin);
            return (T)json.ReadObject(ms);
        }
    }
}
