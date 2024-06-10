using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace UnityUtils.Extensions
{
    public class HttpExtensions
    {
        public static HttpClient GetClient(bool includeSecretKey = true)
        {
            var handler = new MyHttpClientHandler();
            handler.AddSecretKey = includeSecretKey;
            var client = new HttpClient(handler);
            return client;
        }

        public static string GetStatus(HttpStatusCode code) => code.ToString();
        
    }

    public class MyHttpClientHandler : HttpClientHandler
    {
        public bool AddSecretKey { get; set; } = true;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (AddSecretKey)
            {
                //request.Headers.Add("UserSecret", PMemory.SecretKey);

            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
