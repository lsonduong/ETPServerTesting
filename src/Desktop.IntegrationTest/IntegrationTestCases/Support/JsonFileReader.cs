using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using PDS.WITSMLstudio.Connections;
using PDS.WITSMLstudio.Desktop.Core.Models;
using Energistics.Etp.v11.Datatypes.ChannelData;
using Energistics.Etp.v11.Protocol.ChannelStreaming;
using SharpCompress.Common;
using System.IO;
using Newtonsoft.Json.Linq;
using AventStack.ExtentReports.Utils;
using SuperSocket.Common;

namespace PDS.WITSMLstudio.Desktop.IntegrationTestCases.Support
{
    public static class JsonFileReader
    {
        /// <summary>
        /// Reads connection from an Json file from folder input.
        /// </summary>
        /// <param name="jsonPath">The folder path contains json data.</param>
        /// <returns>Returns a new instance of the connection read from the Json file.</returns>
        public static Connection ReadConnection (string jsonPath)
        {
            var connection = new Connection ().deserialize(jsonPath + "\\connection.json");
            return connection;
        }


        /// <summary>
        /// Reads Protocols from an Json file from folder input.
        /// </summary>
        /// <param name="jsonPath">The folder path contains json data.</param>
        /// <returns>Returns a new instance of the Protocols read from the Json file.</returns>
        public static IList<EtpProtocolItem> ReadProtocolList(string jsonPath)
        {
            List<EtpProtocolItem> protocols = JsonHelper.ReadFromJsonArray<EtpProtocolItem>(jsonPath + "\\protocols.json");
            return protocols;
        }

        /// <summary>
        /// Reads Uris from an Json file from folder input.
        /// </summary>
        /// <param name="jsonPath">The folder path contains json data.</param>
        /// <returns>Returns a new instance of the Uris read from the Json file.</returns>
        public static List<string> ReadUris(string jsonPath)
        {
            string[] uris = JsonHelper.ReadFromStringArray(jsonPath + "\\uris.json");
            List<string> uris_list = uris.ToList();
            return uris_list;
        }

        /// <summary>
        /// Reads Channels from an Json file from folder input.
        /// </summary>
        /// <param name="jsonPath">The folder path contains json data.</param>
        /// <returns>Returns a new instance of the Channels read from the Json file.</returns>
        public static List<ChannelStreamingInfo> ReadChannels(string jsonPath)
        {
            List<long> channels = JsonHelper.ReadFromJsonArray<long>(jsonPath + "\\channels.json");
            var listChannels = new List<ChannelStreamingInfo>();

            string startIndex = JsonHelper.ReadFromJsonFile(jsonPath + "\\startIndex.json").Trim();

            object startItem;

            if (startIndex.Equals("null") || startIndex.IsNullOrEmpty())
            {
                startItem = null;
            } else if (Convert.ToInt64(startIndex) <= 100000)
            {
                startItem = Convert.ToInt32(startIndex);
            } else
            {
                startItem = Convert.ToInt64(startIndex);
            }

            foreach (var id in channels)
            {
                var channelInfo = new ChannelStreamingInfo
                {
                    ChannelId = id,
                    StartIndex = new StreamingStartIndex { Item = startItem },
                    ReceiveChangeNotification = true
                };

                listChannels.Add(channelInfo);
            }

            return listChannels;

        }

        /// <summary>
        /// Compare json object to baseline
        /// </summary>
        public static bool CompareJsonObjectToFile(string jsonString, string jsonPath)
        {
            string jsonContent = JsonHelper.ReadFromJsonFile(jsonPath);
            JArray jsonExpected = JArray.Parse(jsonContent);
            JArray jsonActual = JArray.Parse(jsonString);

            return JToken.DeepEquals(jsonActual, jsonExpected);
        }
    }
}
