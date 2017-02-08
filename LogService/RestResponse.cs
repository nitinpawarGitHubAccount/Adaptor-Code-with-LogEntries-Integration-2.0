using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Net;

namespace Avalara.AvaTax.Adapter.LogService
{
    public class RestResponse
    {
        public object Content { get; set; }

        public string ContentType { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
