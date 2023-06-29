using AventStack.ExtentReports.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium.DevTools.V111.CSS;
using PDS.WITSMLstudio.Desktop.Core;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDS.WITSMLstudio.Desktop.IntegrationTestCases.LGVN.Helper
{
    public static class StringHelper
    {
        public static void LogClientOutput(string message, string filePath = "")
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            try
            {
                if (message.IsJsonString())
                    message = FormatMessage(message);
                else
                {
                    const string receivedText = "Message received at ";
                    var index = message.IndexOf(receivedText, StringComparison.InvariantCultureIgnoreCase);
                    if (index != -1)
                        if (DateTimeOffset.TryParse(message.Substring(index + receivedText.Length).Trim(), out DateTimeOffset _dateReceived))
                            _dateReceived = _dateReceived.ToUniversalTime();
                }

                if (filePath.IsNullOrEmpty())
                    Console.WriteLine(message);
                else
                {
                    TextWriter writer = null;
                    writer = new StreamWriter(filePath, false);
                    writer.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                //_log.Warn($"Error formatting ETP message:{Environment.NewLine}{message}", ex);
            }

            //_ETPMessage = string.Concat(
            //    message.IsJsonString() ? string.Empty : "// ",
            //    message,
            //    Environment.NewLine);
        }

        public static string FormatMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return string.Empty;

            var jObject = JObject.Parse(message);

            //FormatResource(jObject["resource"] as JObject);
            //FormatDataObject(jObject["dataObject"] as JObject);
            //FormatChannelMetadataRecords(jObject["channels"] as JArray);
            //FormatChannelRangeRequests(jObject["channelRanges"] as JArray);
            //FormatChannelData(jObject["data"] as JArray);

            return jObject["protocol"] != null
                ? jObject.ToString(Formatting.None)
                : jObject.ToString();
        }
    }
}
