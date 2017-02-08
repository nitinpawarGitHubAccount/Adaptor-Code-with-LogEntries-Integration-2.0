using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Avalara.AvaTax.Adapter.LogService
{
    public interface ILogTokenApi : IDisposable
    {
        /// <summary>
        /// Gets or creates log token.
        /// </summary>
        LogTokenResult GetLogToken(LogTokenRequest request);
    }

    /// <summary>
    /// LogToken API.
    /// </summary>
    public class LogTokenApi : ILogTokenApi
    {
        private string _getLogTokenApiQueryStringFormat;
        private Uri _logTokenApiUrl;
        private IRestClient _restClient;

        public LogTokenApi()
            : this(null, null)
        {
        }

        /// <summary>
        /// For internal and testing use only.
        /// </summary>
        internal LogTokenApi(Uri logTokenApiUri, IRestClient restClient)
        {
            _getLogTokenApiQueryStringFormat = Properties.Settings.Default.GetLogTokenApiQueryStringFormat;
            _logTokenApiUrl = logTokenApiUri ?? new Uri(Properties.Settings.Default.LogTokenApiUrl);
            _restClient = restClient ?? new RestClient();
        }

        /// <summary>
        /// Creates basic authentication header
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> CreateAuthHeader(LogTokenRequest request)
        {
            var auth = new Dictionary<string, string>();
            auth.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", request.Account, request.Password))));
            return auth;
        }

        /// <summary>
        /// Gets or creates log token.
        /// </summary>
        public LogTokenResult GetLogToken(LogTokenRequest request)
        {
            ParameterValidator.MustNotBeNull(request, request.ToString());
            using (var svcClient = new GenericRestClient.RESTClientXML(CreateAuthHeader(request)))
            {
                try
                {
                    Uri getLogTokenApiUrl = new Uri(_logTokenApiUrl, string.Format("LogToken/?" + _getLogTokenApiQueryStringFormat, request.Authorization, request.Client, request.LogName, request.LogSet, request.Source));
                                    
                    var res = svcClient.RestGet<string>(getLogTokenApiUrl.ToString());
                }
                catch (NotSupportedException nse)
                {
                    return new LogTokenResult()
                    {
                        LogToken = nse.Message,
                        Success = true,
                    };
                }
                catch (Exception ex)
                {
                    return new LogTokenResult()
                    {
                        Error = ex.Message,
                        Success = false,
                    };
                }

                return new LogTokenResult();
            }

        }

        static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes= System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue= System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
        #region IDisposable

        ~LogTokenApi()
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
            if (disposing)
            {
                if (_restClient != null)
                {
                    try { _restClient.Dispose(); }
                    catch { }
                    finally { _restClient = null; }
                }
            }
        }

        #endregion
    }
}
