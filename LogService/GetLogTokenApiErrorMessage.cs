using System;
using System.Collections.Generic;
using System.Text;

namespace Avalara.AvaTax.Adapter.LogService
{
    public static class GetLogTokenApiErrorMessage
    {
        public const string BadRequestValue = "Missing or bad log token request values.";
        public const string InternalServerError = "Internal server error.";
        public const string LogentriesAccountKeyNotFound = "Logentries account key not found for client.";
        public const string Unauthorized = "Unauthorized.";
        internal const string HttpStatusFormat = "HTTP status {0}.";
    }
}
