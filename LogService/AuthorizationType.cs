using System;
using System.Collections.Generic;
using System.Text;

namespace Avalara.AvaTax.Adapter.LogService
{
    public enum AuthorizationType
    {
        /// <summary>
        /// Not set.
        /// </summary>
        NotSet = 0,

        /// <summary>
        /// AvaTax 15 Production.
        /// </summary>
        AvaTax15Production = 1,

        /// <summary>
        /// AvaTax 15 SDK.
        /// </summary>
        AvaTax15Sdk = 2,
    }
}
