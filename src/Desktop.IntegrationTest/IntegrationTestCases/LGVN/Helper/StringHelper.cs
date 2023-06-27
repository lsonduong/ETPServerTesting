using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PDS.WITSMLstudio.Desktop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDS.WITSMLstudio.Desktop.IntegrationTestCases.LGVN.Helper
{
    public static class StringHelper
    {
        public static void LogClientOutput(string message, bool logDetails)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            //if (logDetails)
            //    LogDetailMessage(message);

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
