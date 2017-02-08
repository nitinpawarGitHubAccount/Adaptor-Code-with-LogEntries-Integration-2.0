using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace Avalara.AvaTax.Adapter.GenericRestClient
{
    internal partial class RESTClientPlainText
    {
        private delegate TR GetDelegate<TR>(string url,
            ClientConfiguration configuration);
        private delegate void GetNonQueryDelegate(string url,
            ClientConfiguration configuration);
        private delegate TR PostDelegate<TR, TI>(string url, TI data,
            ClientConfiguration configuration);
        private delegate void PostNonQueryDelegate<TI>(string url, TI data,
            ClientConfiguration configuration);

        // *************** Asynchronous Get ***************************
        internal void RestGetAsync<TR>(string url, RestCallBack<TR> callback,
            ClientConfiguration configuration)
        {
            var get = new GetDelegate<string>(RestGet);
            get.BeginInvoke(url, configuration,
            ar =>
            {
                var result = (AsyncResult)ar;
                var del = (GetDelegate<TR>)result.AsyncDelegate;
                var value = default(TR);
                Exception e = null;

                try { value = del.EndInvoke(result); }
                catch (Exception ex) { e = ex; }

                if (callback != null) { callback(e, value); }

            }, null);
        }

        // Overload method
        internal void RestGetAsync<TR>(string url, RestCallBack<TR> callback)
        {
            RestGetAsync<TR>(url, callback, DefaultConfiguration);
        }

        // *********** Asynchronous Get, no response expected *************
        internal void RestGetNonQueryAsync(string url,
            RestCallBackNonQuery callback, ClientConfiguration configuration)
        {
            var get = new GetNonQueryDelegate(RestGetNonQuery);
            get.BeginInvoke(url, configuration,
            ar =>
            {
                var result = (AsyncResult)ar;
                var del = (GetNonQueryDelegate)result.AsyncDelegate;
                Exception e = null;

                try { del.EndInvoke(result); }
                catch (Exception ex) { e = ex; }

                if (callback != null) { callback(e); }

            }, null);

        }

        // Overload method
        internal void RestGetNonQueryAsync(string url,
            RestCallBackNonQuery callback)
        {
            RestGetNonQueryAsync(url, callback, DefaultConfiguration);
        }

        // *************** Asynchronous Post *********************
        internal void RestPostAsync(string url, string data,
            RestCallBack<string> callback, ClientConfiguration configuration)
        {
            var post = new PostDelegate<string, string>(RestPost);
            post.BeginInvoke(url, data, configuration,
            ar =>
            {
                var result = (AsyncResult)ar;
                var del = (PostDelegate<string, string>)result.AsyncDelegate;
                var value = string.Empty;
                Exception e = null;

                try { value = del.EndInvoke(result); }
                catch (Exception ex) { e = ex; }

                if (callback != null) { callback(e, value); }

            }, null);
        }

        // Overload method
        internal void RestPostAsync(string url, string data,
            RestCallBack<string> callback)
        {
            RestPostAsync(url, data, callback, DefaultConfiguration);
        }


        // ********* Asynchronous Post, not response expected *********
        internal void RestPostNonQueryAsync(string url, string data,
            RestCallBackNonQuery callback, ClientConfiguration configuration)
        {
            var post = new PostNonQueryDelegate<string>(RestPostNonQuery);
            post.BeginInvoke(url, data, configuration,
            ar =>
            {
                var result = (AsyncResult)ar;
                var del = (PostNonQueryDelegate<string>)result.AsyncDelegate;
                Exception e = null;

                try { del.EndInvoke(result); }
                catch (Exception ex) { e = ex; }

                if (callback != null) { callback(e); }

            }, null);
        }

        // Overload method
        internal void RestPostNonQueryAsync(string url, string data,
            RestCallBackNonQuery callback)
        {
            RestPostNonQueryAsync(url, data, callback, DefaultConfiguration);
        }
    }
}
