using System;
using System.Collections.Generic;
using System.Text;

namespace Avalara.AvaTax.Adapter.LogService
{
    public class LogTokenResult
    {
        /// <summary>
        /// Gets or sets error message if log token request was unsuccessful.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets log token.
        /// </summary>
        public string LogToken { get; set; }

        /// <summary>
        /// Gets or sets whether log token request was successful.
        /// </summary>
        public bool Success { get; set; }
    }
}
