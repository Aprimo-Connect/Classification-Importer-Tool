
using System;

namespace Helpers
{
    public sealed class AccessHelper
    {        
        /// <summary>
        /// Instance for the NamePathConverter.
        /// </summary>
        public static AccessHelper Instance = new AccessHelper();

        private string clientToken;
        private string tokenEndpoint;
        private string clientId;
        private string accessToken;
        private string refreshToken;

        private object lockMe = new object();

        private AccessHelper()
        {
            clientToken = "";
            tokenEndpoint = "";
            clientId = "";
            accessToken = "";
            refreshToken = "";
        }
       
        public void Create(string clientToken, string tokenEndpoint, string clientId)
        {
            this.clientToken = clientToken;
            this.clientId = clientId;
            this.tokenEndpoint = tokenEndpoint;
        }
        public string GetToken()
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                return accessToken;
            }

            lock (lockMe)
            {
                accessToken = JsonHelper.GetAccessToken(clientToken, tokenEndpoint, clientId, ref refreshToken);
            }
            return accessToken;
        }

        public string GetRefreshedToken()
        {
            lock (lockMe)
            {
                try
                {
                    accessToken = JsonHelper.RefreshToken(clientToken, tokenEndpoint, clientId, refreshToken);
                }
                catch(Exception ex)
                {
                    //if exception happens when refresshing token, then the original token has expired, get a new one
                    accessToken = JsonHelper.GetAccessToken(clientToken, tokenEndpoint, clientId, ref refreshToken);
                }
            }
            return accessToken;
        }
    }
}
