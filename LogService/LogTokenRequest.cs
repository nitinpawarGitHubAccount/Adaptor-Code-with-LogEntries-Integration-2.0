using System;
using System.Collections.Generic;
using System.Text;

namespace Avalara.AvaTax.Adapter.LogService
{
    public class LogTokenRequest
    {
        /// <summary>
        /// Gets or sets account.
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// Gets or sets authorization type.
        /// </summary>
        public AuthorizationType Authorization { get; set; }

        /// <summary>
        /// Gets or sets client.
        /// </summary>
        public string Client { get; set; }

        /// <summary>
        /// Gets or sets log name.
        /// </summary>
        public string LogName { get; set; }

        /// <summary>
        /// Gets or sets log set.
        /// </summary>
        public string LogSet { get; set; }

        /// <summary>
        /// Gets or sets account password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets log token source.
        /// </summary>
        public LogTokenSourceType Source { get; set; }
    }

    public static class LogTokenRequestExtensions
    {
        /// <summary>
        /// Verifies whether a log token request is valid.
        /// </summary>
        //public static bool IsValid(this LogTokenRequest request)
        //{
        //    return request != null &&
        //        request.Authorization > AuthorizationType.NotSet && Enum.GetName(typeof(AuthorizationType), request.Authorization) != null &&
        //        !string.IsNullOrEmpty(request.Client) &&
        //        !string.IsNullOrEmpty(request.LogName) &&
        //        !string.IsNullOrEmpty(request.LogSet) &&
        //        request.Source > LogTokenSourceType.NotSet && Enum.GetName(typeof(LogTokenSourceType), request.Source) != null;
        //}

        ///// <summary>
        ///// Verifies whether a log token request is valid.
        ///// </summary>
        //public static bool IsValid(this LogTokenRequest request, bool expectCredentials)
        //{
        //    return request != null &&
        //        request.IsValid() &&
        //        (!expectCredentials || (
        //            !string.IsNullOrEmpty(request.Account) &&
        //            !string.IsNullOrEmpty(request.Password)
        //        ));
        //}
    }
}
