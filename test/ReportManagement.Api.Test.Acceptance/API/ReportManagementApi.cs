using RestSharp;
using System.Net;

namespace ReportManagement.Api.Test.Acceptance.API
{
    internal class ReportManagementApi
    {
        private readonly RestClient _client;

        public ReportManagementApi()
        {
            _client = new RestClient("https://localhost:5001");

            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;
        }
    }
}
