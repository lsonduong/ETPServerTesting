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

namespace PDS.WITSMLstudio.Desktop.IntegrationTestCases.Support
{
    public static class JsonFileReader
    {
        public static Connection ReadConnection (string jsonpath)
        {
            var connection = new Connection ().deserialize(jsonpath);
            return connection;
        }

        public static IList<EtpProtocolItem> ReadProtocolList(string jsonpath)
        {
            List<EtpProtocolItem> protocols = JsonHelper.ReadFromJsonArray<EtpProtocolItem>(jsonpath);
            return protocols;
        }

        public static List<string> ReadUris(string jsonpath)
        {
            string[] uris = JsonHelper.ReadFromStringArray(jsonpath);
            List<string> uris_list = uris.ToList();
            return uris_list;
        }

        public static List<ChannelStreamingInfo> ReadChannels(string jsonpath)
        {
            List<long> channels = JsonHelper.ReadFromJsonArray<long>(jsonpath);
            var listChannels = new List<ChannelStreamingInfo>();
            foreach (var id in channels)
            {
                var channelInfo = new ChannelStreamingInfo
                {
                    ChannelId = id,
                    StartIndex = new StreamingStartIndex { Item = null },
                    ReceiveChangeNotification = true
                };

                listChannels.Add(channelInfo);
            }
            return listChannels;

        }
    }
}
