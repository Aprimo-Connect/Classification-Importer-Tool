﻿using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using RestSharp;
using Models;

namespace Helpers
{
    public class JsonHelper
    {
        public static T Deserialize<T>(string json)
        {
            var result = Activator.CreateInstance<T>();
            var settings = new DataContractJsonSerializerSettings
            {
                DateTimeFormat = new System.Runtime.Serialization.DateTimeFormat("o")
            };
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var ser = new DataContractJsonSerializer(result.GetType(), settings);
                return (T)ser.ReadObject(ms);
            }
        }

        public static string Serialize<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

        public static string GetAccessToken(string clientToken, string tokenEndpoint, string clientId, ref string refreshToken)
        {
            // Get the access and refresh tokens
            string accessToken = "";
            var client = new RestClient(tokenEndpoint);
            var request = new RestRequest("api/oauth/create-native-token", Method.POST);
            request.AddHeader("Authorization", string.Format("Basic " + clientToken));
            request.AddHeader("ContentType", "application/json");
            request.AddHeader("client-id", clientId);
            
            IRestResponse response = client.Execute(request);
            if (response.StatusCode.ToString().Equals("OK"))
            {
                var tokens = JsonHelper.Deserialize<Tokens>(response.Content);

                accessToken = tokens.accessToken;
                refreshToken = tokens.refreshToken;
            }
            else throw new Exception(string.Format("Access token was not created, responese status is {0}, response message: {1}", response.StatusCode, response.Content));
            return accessToken;
        }

        public static string RefreshToken(string clientToken, string tokenEndpoint, string clientId, string refreshToken)
        {
            // Get the access and refresh tokens
            string accessToken = "";
            var client = new RestClient(tokenEndpoint);
            var request = new RestRequest("api/token", Method.POST);
            request.AddHeader("Authorization", string.Format("Basic " + clientToken));
            request.AddHeader("ContentType", "application/json");
            request.AddHeader("client-id", clientId);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new { refreshToken = refreshToken });

            IRestResponse response = client.Execute(request);
            if (response.StatusCode.ToString().Equals("OK"))
            {               
                accessToken = response.Content.Replace("\"", "");
            }
            else throw new Exception(string.Format("Access token could not be refreshed, responese status is {0}, response message: {1}", response.StatusCode, response.Content));
            return accessToken;
        }

        protected string GetLinkData(string Token, string linkURL, string Registration, string UserAgent)
        {
            //TODO: refactor to use restsharp
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            client.DefaultRequestHeaders.Add("registration", Registration);
            client.DefaultRequestHeaders.Add("API-VERSION", "1");
            client.DefaultRequestHeaders.Add("Accept", "application/hal+json");
            client.DefaultRequestHeaders.Add("User-Agent", UserAgent);

            HttpResponseMessage response = client.GetAsync(linkURL).Result;

            string result = response.Content.ReadAsStringAsync().Result;

            return result;
        }
    }
}
