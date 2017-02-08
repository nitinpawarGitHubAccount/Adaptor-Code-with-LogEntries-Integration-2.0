using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
namespace Avalara.AvaTax.Adapter.GenericRestClient
{
    internal partial class RESTClientPlainText:IDisposable
    {
        internal static ClientConfiguration DefaultConfiguration { get; set; }
        private Dictionary<string, string> _headers;
        
        internal RESTClientPlainText()
        {
            // Initiate the default configuration
            DefaultConfiguration = new ClientConfiguration();
            DefaultConfiguration.ContentType = "text/plain";
            DefaultConfiguration.Accept = "text/plain";
            DefaultConfiguration.RequireSession = false;
            //DefaultConfiguration.OutBoundSerializerAdapter = new XMLSerializerAdapter();
            //DefaultConfiguration.InBoundSerializerAdapter = new XMLSerializerAdapter();
        }
     
        internal RESTClientPlainText(Dictionary<string, string> headers)
            : this()
        {
            _headers = headers;
        }

        // Create a request object according to the configuration
        private HttpWebRequest CreateRequest(string url,
            ClientConfiguration clientConfig)
        {
            var request=(clientConfig.RequireSession) ? CookiedHttpWebRequestFactory.Create(url) :
                (HttpWebRequest)WebRequest.Create(url);
            if (_headers != null)
            {
                foreach (var header in _headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }  
            }
            return request;
        }

        // Post data to the service
        private static void PostData(HttpWebRequest request,
            ClientConfiguration clientConfig, string data)
        {
            //var xmlRequestString = clientConfig.OutBoundSerializerAdapter.Serialize(data).Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "").Remove(0, 1).Replace("xmlns=\"https://onboardingapi.avalara.com/DATA/V1\"", "").Replace(" >", ">").Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "xmlns=\"https://onboardingapi.avalara.com/DATA/V1\"").Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "xmlns=\"https://onboardingapi.avalara.com/DATA/V1\"").Replace("xsi:nil=\"true\" />", "/>");
            var xmlRequestString =data.ToString().Trim();
            Constants.RequestLog = data.ToString().Trim();
            var bytes = Encoding.UTF8.GetBytes(xmlRequestString);

            using (var postStream = request.GetRequestStream())
            {
                postStream.Write(bytes, 0, bytes.Length);
            }
        }

        // Receive data from the service
        private static string ReceiveData(HttpWebRequest request,
            ClientConfiguration clientConfig)
        {
            string xmlResponseString;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var stream = response.GetResponseStream();               
                using (var streamReader = new StreamReader(stream))
                {
                    xmlResponseString = streamReader.ReadToEnd();
                }
            }
            Constants.ResponseLog = xmlResponseString;
            return xmlResponseString;
        }

        public void Dispose()
        {
            
        }
    }
}
