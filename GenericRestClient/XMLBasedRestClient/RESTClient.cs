using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
namespace Avalara.AvaTax.Adapter.GenericRestClient
{
    internal partial class RESTClientXML:IDisposable
    {
        internal static ClientConfiguration DefaultConfiguration { get; set; }
        private Dictionary<string, string> _headers;
        
        internal RESTClientXML()
        {
            // Initiate the default configuration
            DefaultConfiguration = new ClientConfiguration();
            DefaultConfiguration.ContentType = "application/xml";
            DefaultConfiguration.Accept = "application/xml";
            DefaultConfiguration.RequireSession = false;
            DefaultConfiguration.OutBoundSerializerAdapter = new XMLSerializerAdapter();
            DefaultConfiguration.InBoundSerializerAdapter = new XMLSerializerAdapter();
        }
        internal RESTClientXML(Dictionary<string,string> headers):this()
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
        private static void PostData<T>(HttpWebRequest request,
            ClientConfiguration clientConfig, T data)
        {
            var xmlRequestString = clientConfig.OutBoundSerializerAdapter.Serialize(data).Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "").Remove(0, 1).Replace("xmlns=\"https://onboardingapi.avalara.com/DATA/V1\"", "").Replace(" >", ">").Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "xmlns=\"https://onboardingapi.avalara.com/DATA/V1\"").Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "xmlns=\"https://onboardingapi.avalara.com/DATA/V1\"").Replace("xsi:nil=\"true\" />", "/>").Replace("xsi:nil=\"true\" ","") ;
            Constants.RequestLog = xmlRequestString;
            var bytes = Encoding.UTF8.GetBytes(xmlRequestString);

            using (var postStream = request.GetRequestStream())
            {
                postStream.Write(bytes, 0, bytes.Length);
            }
        }

        // Receive data from the service
        private static T ReceiveData<T>(HttpWebRequest request,
            ClientConfiguration clientConfig)
        {
            string xmlResponseString;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var stream = response.GetResponseStream();
                if (stream == null) { return default(T); }
                using (var streamReader = new StreamReader(stream))
                {
                    xmlResponseString = streamReader.ReadToEnd();
                }
            }
            Constants.ResponseLog = xmlResponseString;
            try
            {
                return clientConfig.InBoundSerializerAdapter.Deserialize<T>(xmlResponseString);
            }
            catch(InvalidOperationException ex)
            {                
                //if something weired happened while deserilization, response returned by service can be 
                var e = new NotSupportedException(xmlResponseString, ex);
                throw e;
            }
            catch (Exception e)
            {
                var ex = new NotSupportedException(xmlResponseString, e);
                throw ex;
            }
            
        }

        public void Dispose()
        {
            
        }
    }
}
