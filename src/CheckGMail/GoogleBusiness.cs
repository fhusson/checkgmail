using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Threading;

namespace CheckGMail
{
    public enum Status
    {
        Init,
        ClientSecretsError,
        CredentialValid,
        TokenInvalid,
        Running
    }

    class GoogleBusiness : IDisposable
    {
        private readonly ClientSecrets clientSecrets;

        private GoogleBusiness()
        {
            RequestQ = "is:unread -in:trash -in:spam";
            Status = Status.Init;

            clientSecrets = GetClientSecrets();
        }

        private static GoogleBusiness instance;
        public static GoogleBusiness Instance
        {
            get
            {
                if (instance == null)
                    instance = new GoogleBusiness();

                return instance;
            }
        }

        public string RequestQ { get; set; }
        public Status Status { get; set; }
        private UserCredential _credential;
        private UserCredential Credential
        {
            get
            {
                InitCredential();
                return _credential;
            }
        }

        /// <summary>Retrieves the Client Configuration from the server path.</summary>
        /// <returns>Client secrets that can be used for API calls.</returns>
        private static ClientSecrets GetClientSecrets()
        {
            using var stream = MyResources.Instance.GetClientSecretsStream();
            return GoogleClientSecrets.Load(stream).Secrets;
        }

        private void InitCredential()
        {
            _credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                clientSecrets,
                new[] { GmailService.Scope.GmailReadonly },
                "user",
                CancellationToken.None,
                new FileDataStore("1f433.CheckGMail.Credential")
            ).Result;
        }

        private GmailService _gmail;
        private GmailService GMail
        {
            get
            {
                if (_gmail == null)
                {
                    var initializer = new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = Credential,
                        ApplicationName = "CheckGMail Client",
                    };
                    _gmail = new GmailService(initializer);
                }

                return _gmail;
            }
        }

        public void Revoke()
        {
            _credential.RevokeTokenAsync(CancellationToken.None).Wait();
            _credential = null;

            if (_gmail != null)
            {
                _gmail.Dispose();
                _gmail = null;
            }
        }

        public long? GMailCheckMessages()
        {
            // Request should fail now - invalid grant.
            try
            {
                return TryGMailCheckMessages();
            }
            catch (TokenResponseException)
            {
                GoogleWebAuthorizationBroker.ReauthorizeAsync(_credential, CancellationToken.None);
                return TryGMailCheckMessages();
            }
        }

        private long? TryGMailCheckMessages()
        {
            var msgRequest = GMail.Users.Messages.List("me");
            msgRequest.Q = RequestQ;

            var msgResponse = msgRequest.Execute();

            return msgResponse.ResultSizeEstimate;
        }

        internal void Authorize()
        {
            InitCredential();
        }

        public void Dispose()
        {
            if (_gmail != null)
            {
                _gmail.Dispose();
            }
        }
    }
}
