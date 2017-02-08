using System;
using System.Collections.Generic;
using System.Text;

namespace Avalara.AvaTax.Adapter.LogService
{
    class LogToken
    {

        public void GenerateLogToken()
        {
            LogTokenRequest logTokenRequest = null;
            LogTokenResult logTokenResult = null;
            using (ILogTokenApi logTokenApi = new LogTokenApi())
            {
                try
                {
                    logTokenResult = logTokenApi.GetLogToken(logTokenRequest);
                }
                //catch (AggregateException ex)
                //{
                //    Console.WriteLine(ex.Flatten().InnerException.Message);
                //    Environment.Exit(1);
                //}
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Environment.Exit(1);
                }
            }
        }

    }
}
