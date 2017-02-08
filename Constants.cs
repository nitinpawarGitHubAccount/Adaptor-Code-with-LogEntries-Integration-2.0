using System;
using System.Collections.Generic;
using System.Text;

namespace Avalara.AvaTax.Adapter
{
    internal static class Constants
    {
        private static string _createAccountDetailsUrl_Sandbox;
        private static string _createAccountDetailsUrl_Production;
        static Constants()
        {
            //fetch these urls from config
            _createAccountDetailsUrl_Sandbox =  "https://sandbox.onboarding.api.avalara.com/v1/Accounts";//"https://onboardingapi.connectorsqa.avatax.com/v1/Accounts";
            _createAccountDetailsUrl_Production = "https://onboarding.api.avalara.com/v1/Accounts";
        }
        internal static string CreateAccountDetailsUrl_Sandbox
        {
            get
            {
                return _createAccountDetailsUrl_Sandbox;
            }
        }

        internal static string CreateAccountDetailsUrl_Production
        {
            get
            {
                return _createAccountDetailsUrl_Production;
            }
        }

        internal static string RequestLog;
        internal static string ResponseLog;        
    }    
}
