using System;
using System.Collections.Generic;
using System.Text;
using Avalara.AvaTax.Adapter.Proxies.TaxSvcProxy;

namespace Avalara.AvaTax.Adapter
{
    public static class JsonParser
    {
        // This is the template of json message. The entire logic of json parsing is based on this template.
        /*  
            Name: InvoiceMessageInfo
            Severity: Success
            Summary: Invoice  Messages for the transaction
            Details: {"InvoiceMessageMasterList":[{"MessageCode":0,"Message":"No applicable messaging for this line."},{"MessageCode":1,"Message":"Intra-EU Supply of Goods as per Art. 138 EU VAT Directive 2006/112"}],"InvoiceMessageList":[{"TaxLineNo":"1","MessageCode":1},{"TaxLineNo":"2","MessageCode":1},{"TaxLineNo":"3","MessageCode":1}]}
            Source: Avalara.AvaTax.Services.Tax
            RefersTo: 
            HelpLink: http://www.avalara.com
        */

        // strings for the tags in json
        private static string MasterMessageCodeTag = "MessageCode";
        private static string MasterMessageTag = "Message";
        private static string LineTaxLineNoTag = "TaxLineNo";
        private static string LineMessageCodeTag = "MessageCode";

        public static void ReadInvoiceMessages(string JsonText, ref ProxyGetTaxResult svcResult)
        {
            int LineCount = GetLineCount(JsonText);
            InitializeInvoiceMessages(LineCount, ref svcResult);
            ReadLines(LineCount, JsonText, ref svcResult);
            ReadMessages(LineCount, JsonText, ref svcResult);
        }

        private static int GetLineCount(string JsonText)
        {
            // split by tax lines to get the line count
            string[] separator = new string[] { LineTaxLineNoTag };
            return JsonText.Split(separator, StringSplitOptions.RemoveEmptyEntries).Length - 1;
        }

        private static void InitializeInvoiceMessages(int LineCount, ref ProxyGetTaxResult svcResult)
        {
            // initialize the variable as per the line count
            svcResult.InvoiceInfoMessages = new Proxies.InvoiceMessageInfo[LineCount];
            for (int i = 0; i < LineCount; i++)
            {
                svcResult.InvoiceInfoMessages[i] = new Proxies.InvoiceMessageInfo();
            }
        }

        private static void ReadLines(int LineCount, string JsonText, ref ProxyGetTaxResult svcResult)
        {
            string[] separator = new string[] { LineTaxLineNoTag };
            string[] chunks = JsonText.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < LineCount; i++)
            {
                string lineNo = "";
                int index = 3; // index to skip ":"
                do
                {
                    lineNo = lineNo + chunks[i + 1][index++]; // read line number
                } while (chunks[i + 1][index] != '"');
                svcResult.InvoiceInfoMessages[i].LineNo = Convert.ToInt32(lineNo);

                separator = new string[] { LineMessageCodeTag };
                string chunk = chunks[i + 1].Split(separator, StringSplitOptions.RemoveEmptyEntries)[1];
                string messageCode = ""; // index to skip ":
                index = 2;
                do
                {
                    messageCode = messageCode + chunk[index++]; // read the message code number
                } while (chunk[index] != '}');
                svcResult.InvoiceInfoMessages[i].Message = messageCode;
            }
        }

        private static void ReadMessages(int LineCount, string JsonText, ref ProxyGetTaxResult svcResult)
        {
            for (int i = 0; i < LineCount; i++)
            {
                string message = "";
                // index for the tag plus its length plus 2 for ": plus code length plus 2 for ," plus message tag length plus 3 for ":"
                int index = JsonText.IndexOf(MasterMessageCodeTag + "\":" + svcResult.InvoiceInfoMessages[i].Message) + MasterMessageCodeTag.Length + 2 + svcResult.InvoiceInfoMessages[i].Message.Length + 2 + MasterMessageTag.Length + 3;
                do
                {
                    message = message + JsonText[index++]; // read the message
                } while (JsonText[index] != '"');
                svcResult.InvoiceInfoMessages[i].Message = message; // replace the message code by actual message
            }
        }
    }
}
