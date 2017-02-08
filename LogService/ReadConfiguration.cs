using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Avalara.AvaTax.Adapter.LogService
{
    public class ReadWriteConfig
    {
        static void ShowConfig()
        {
            // For read access you do not need to call OpenExeConfiguraton
            foreach (string key in ConfigurationManager.AppSettings)
            {
                string value = ConfigurationManager.AppSettings[key];
                Console.WriteLine("Key: {0}, Value: {1}", key, value);
            }
        }
    }
}
