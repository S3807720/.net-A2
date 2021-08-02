using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MCBA_Web.Helper
{
    public static class McbaApi
    {
        private const string ApiBaseUri = "http://localhost:5000";

        public static HttpClient InitializeClient()
        {
            var client = new HttpClient { BaseAddress = new Uri(ApiBaseUri) };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
