using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace Avalara.AvaTax.Adapter.GenericRestClient
{
    public class CookiedHttpWebRequestFactory
    {
        private static Object synchLock = new Object();
        // This dictionary keeps the cookie container for each domain.
        private static Dictionary<string, CookieContainer> containers
            = new Dictionary<string, CookieContainer>();

        public static HttpWebRequest Create(string url)
        {
            // Create a HttpWebRequest object
            var request = (HttpWebRequest)WebRequest.Create(url);

            // this get the dmain part of from the url
            string domain = (new Uri(url)).GetLeftPart(UriPartial.Authority);

            // try to get a container from the dictionary, if it is in the
            // dictionary, use it. Otherwise, create a new one and put it
            // into the dictionary and use it.
            CookieContainer container;
            lock (synchLock)
            {
                if (!containers.TryGetValue(domain, out container))
                {
                    container = new CookieContainer();
                    containers[domain] = container;
                }
            }

            // Assign the cookie container to the HttpWebRequest object
            request.CookieContainer = container;
            
            return request;
        }
    }

    // Defines an adapter interface for the serializers
    public interface ISerializerAdapter
    {
        string Serialize<T>(T obj);
        T Deserialize<T>(string input);
    }

    /// <summary>
    /// An implementation of ISerializerAdapter based on the JavaScriptSerializer
    /// </summary>
    public class XMLSerializerAdapter : ISerializerAdapter
    {
        //private XmlSerializer serializer;
        /// <summary>
        /// To Serialize object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Serialize<T>(T obj)
        {
            using (StringWriter stream = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, obj);
                stream.Flush();
                return stream.ToString();
            }
        }
        /// <summary>
        /// To deserialize object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public T Deserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentNullException("xml");
            }
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader stream = new StringReader(xml))
            {
                try
                {
                    return (T)serializer.Deserialize(stream);
                }
                catch (Exception ex)
                {
                    // The serialization error messages are cryptic at best.
                    // Give a hint at what happened
                    throw new InvalidOperationException("Failed to " +
                                     "create object from xml string", ex);
                }
            }
        }
    }

    /// <summary>
    /// The configuration class defines how the rest call is made.
    /// </summary>
    public class ClientConfiguration
    {
        public string ContentType { get; set; }
        public string Accept { get; set; }
        public bool RequireSession { get; set; }
        public ISerializerAdapter OutBoundSerializerAdapter { get; set; }
        public ISerializerAdapter InBoundSerializerAdapter { get; set; }
    }

    /// <summary>
    /// The delegates use for asychronous calls
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ex"></param>
    /// <param name="result"></param>
    public delegate void RestCallBack<T>(Exception ex, T result);
    /// <summary>
    /// The delegate use for asychronous calls
    /// </summary>
    /// <param name="ex"></param>
    public delegate void RestCallBackNonQuery(Exception ex);
}
