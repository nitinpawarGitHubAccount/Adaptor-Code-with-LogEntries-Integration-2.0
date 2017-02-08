using System;
using System.Collections.Generic;
using System.Text;
  
namespace Avalara.AvaTax.Adapter.LogService
{
    public interface IRestClient : IDisposable
    {
        /// <summary>
        /// Adds HTTP request header.
        /// </summary>
        void AddHttpRequestHeader(string name, string value);

        /// <summary>
        /// Gets.
        /// </summary>
        RestResponse Get(Uri url);

        /// <summary>
        /// Posts JSON.
        /// </summary>
        RestResponse PostJson(Uri url, string json);

        /// <summary>
        /// Posts text.
        /// </summary>
        RestResponse PostText(Uri url, string text);

        /// <summary>
        /// Posts data as specified content type.
        /// </summary>
        RestResponse Post(Uri url, object data, string contentType);

        /// <summary>
        /// Replaces HTTP request header.
        /// </summary>
        void ReplaceHttpRequestHeader(string name, string value);
    }

    /// <summary>
    /// Very simple REST client.
    /// </summary>
    public class RestClient : IRestClient
    {
       //private HttpClient _httpClient;

        public RestClient()
        {
         //   _httpClient = new HttpClient();
        }

        /// <summary>
        /// Adds HTTP request header.
        /// </summary>
        public void AddHttpRequestHeader(string name, string value)
        {
           // _httpClient.DefaultRequestHeaders.Add(name, value);
        }

        /// <summary>
        /// Gets.
        /// </summary>
        public RestResponse Get(Uri url)
		{
            //HttpResponseMessage httpResponseMessage; //= _httpClient.GetAsync(url).Result;

            //return new RestResponse()
            //{
            //    Content = httpResponseMessage.Content.ReadAsStringAsync().Result,
            //    ContentType = httpResponseMessage.Content.Headers.ContentType.MediaType,
            //    StatusCode = httpResponseMessage.StatusCode,
            //};
            return null;
		}

        /// <summary>
        /// Posts JSON.
        /// </summary>
        public RestResponse PostJson(Uri url, string json)
        {
            return Post(url, json, "application/json");
        }

        /// <summary>
        /// Posts text.
        /// </summary>
        public RestResponse PostText(Uri url, string text)
        {
            return Post(url, text, "text/plain");
        }

        /// <summary>
        /// Posts data as specified content type.
        /// </summary>
        public RestResponse Post(Uri url, object data, string contentType)
		{
            //using (StringContent content = new StringContent(data.ToString(), Encoding.UTF8, contentType))
            //{

            //   HttpResponseMessage httpResponseMessage;// = _httpClient.PostAsync(url, content).Result;

            //return new RestResponse()
            //{
            //    Content = httpResponseMessage.Content.ReadAsStringAsync().Result,
            //    ContentType = httpResponseMessage.Content.Headers.ContentType.MediaType,
            //    StatusCode = httpResponseMessage.StatusCode,
            //};
            //}

            return null;
		}

        /// <summary>
        /// Replaces HTTP request header.
        /// </summary>
        public void ReplaceHttpRequestHeader(string name, string value)
        {
            //_httpClient.DefaultRequestHeaders.Remove(name);
            //AddHttpRequestHeader(name, value);
        }

        #region IDisposable

        ~RestClient()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    if (_httpClient != null)
            //    {
            //        try { _httpClient.Dispose(); }
            //        catch { }
            //        finally { _httpClient = null; }
            //    }
            //}
        }

        #endregion
    }
}
