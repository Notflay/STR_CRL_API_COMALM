using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.SL
{
    public class B1SLInteract
    {

        private string baseURL;

        public B1SLInteract(string baseURL)
        {
            this.baseURL = baseURL;
        }

        public IRestResponse httpPOST<T>(string uri, string sessionId, T t)     // LOGIN
           where T : class
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                var client = new RestClient(baseURL);
                var request = new RestRequest(uri, Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddCookie("B1SESSION", sessionId);
                request.AddCookie("ROUTEID", ".node0");
                request.AddJsonBody(t);
                return client.Execute(request);
            }
            catch { throw; }
        }

        public IRestResponse httpPOST(string uri, string sessionId, string json)
        {

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var client = new RestClient(baseURL);
            var request = new RestRequest(uri, Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddCookie("B1SESSION", sessionId);
            //request.AddCookie("ROUTEID", ".node2");
            request.AddCookie("ROUTEID", ".node3");
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            return client.Execute(request);
        }

        public IRestResponse httpGET(string uri, string sessionId)          // Traer ITEM
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                var client = new RestClient(baseURL);
                var request = new RestRequest(uri, Method.GET);
                //request.AddHeader("content-type", "application/json");
                request.AddCookie("B1SESSION", sessionId);
                request.AddCookie("ROUTEID", ".node0");
                // request.AddParameter("application/json", ParameterType.RequestBody);
                return client.Execute(request);
            }
            catch { throw; }
        }

    }
}
