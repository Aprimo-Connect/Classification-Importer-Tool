namespace Helpers
{
    public sealed class AccessHelper
    {        
        /// <summary>
        /// Instance for the NamePathConverter.
        /// </summary>
        public static AccessHelper Instance = new AccessHelper();

        private string tokenEndpoint;
        private string clientId;
        private string clientSecret;
        private string accessToken;

        private object lockMe = new object();

        private AccessHelper()
        {
            tokenEndpoint = "";
            clientId = "";
            accessToken = "";
            clientSecret = "";
        }
       
        public void Create(string tokenEndpoint, string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
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
                accessToken = JsonHelper.GetAccessToken(tokenEndpoint, clientId, clientSecret);
            }
            return accessToken;
        }
    }
}
