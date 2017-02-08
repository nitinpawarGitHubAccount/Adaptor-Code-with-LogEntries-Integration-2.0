using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Avalara.AvaTax.Adapter.GenericRestClient
{
    internal partial class RESTClientPlainText
    {
        internal string RestGet(string url, ClientConfiguration configuration) 
        {
            var clientConfig = configuration ?? DefaultConfiguration;
            var request = CreateRequest(url, clientConfig);
            request.Accept = clientConfig.Accept;
            request.Method = "GET";

            return ReceiveData(request, clientConfig);
        }

        // Overload method
        internal string RestGet(string url)
        {
            return RestGet(url, DefaultConfiguration);
        }


        // ******** Synchronous GET, no response expected *******
        internal void RestGetNonQuery(string url,
            ClientConfiguration configuration)
        {
            var clientConfig = configuration ?? DefaultConfiguration;
            var request = CreateRequest(url, clientConfig);
            request.Method = "GET";

            request.GetResponse().Close();
        }

        // Overload method
        internal void RestGetNonQuery(string url)
        {
            RestGetNonQuery(url, DefaultConfiguration);
        }

        // ***************** Synchronous POST ********************
        internal string RestPost(string url,
            string data, ClientConfiguration configuration)
        {
            var clientConfig = configuration ?? DefaultConfiguration;
            var request = CreateRequest(url, clientConfig);
            request.ContentType = clientConfig.ContentType;
            request.Accept = clientConfig.Accept;
            request.Method = "POST";            
            request.PreAuthenticate = true;
            request.Timeout = int.MaxValue;

            PostData(request, clientConfig, data);
            return ReceiveData(request, clientConfig);
        }

        // Overload method
        internal string RestPost(string url, string data)
        {
            return RestPost(url, data, DefaultConfiguration);
        }

        // ****** Synchronous GET, no respons expected ******
        internal void RestPostNonQuery(string url,
            string data, ClientConfiguration configuration)
        {
            var clientConfig = configuration ?? DefaultConfiguration;
            var request = CreateRequest(url, clientConfig);
            request.ContentType = clientConfig.ContentType;
            request.Accept = clientConfig.Accept;
            request.Method = "POST";           

            PostData(request, clientConfig, data);
            request.GetResponse().Close();
        }

        // Overload method
        internal void RestPostNonQuery(string url, string data)
        {
            RestPostNonQuery(url, data, DefaultConfiguration);
        }
    }
}
