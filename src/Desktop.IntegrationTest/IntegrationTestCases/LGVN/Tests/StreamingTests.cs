using Energistics.Etp.Common;
using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.v11.Datatypes.ChannelData;
using Energistics.Etp.v11.Protocol.ChannelStreaming;
using Energistics.Etp.v11.Protocol.Discovery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PDS.WITSMLstudio.Desktop.IntegrationTestCases.LGVN.Helper;
using PDS.WITSMLstudio.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDS.WITSMLstudio.Desktop.IntegrationTestCases.LGVN.Tests
{
    [TestClass]
    public class StreamingTests : TestBase
    {
        [TestMethod]
        [Description("Streaming")]
        public async Task Test_Streaming()
        {
            client.Register<IChannelStreamingConsumer, ChannelStreamingConsumerHandler>();
            client.Register<IDiscoveryCustomer, DiscoveryCustomerHandler>();

            var handlerD = client.Handler<IDiscoveryCustomer>();

            var handler = client.Handler<IChannelStreamingConsumer>();
            handler.OnChannelData += OnChannelData;
            handler.OnChannelMetadata += OnChannelMetaData;

            // Wait for Open connection
            var isOpen = await client.OpenAsync();

            var onGetChannelMetaData = AsyncHelper.HandleAsync<ChannelMetadata>(x => handler.OnChannelMetadata += x);
            handler.Start();

            // Describe 
            var uris = new List<string>();
            //uris.Add("eml://witsml14/well(b6e41495-8b5f-4a87-927b-eed03cc60e46)/wellbore(5cb47631-dc38-4370-ae04-f60b100abbd8)/log(9e41d8b8-9113-3df7-aa98-e4e6b068d06e)/logCurveInfo(EWR%20Medium%20Resis)");
            uris.Add("eml://witsml14/well(b6e41495-8b5f-4a87-927b-eed03cc60e46)");
            handler.ChannelDescribe(uris);
            var argsMetadata = await onGetChannelMetaData.WaitAsync();

            var channels = argsMetadata.Message.Channels;
            //var channelStreaming = channels.Where(c => c.ChannelName == "Hole Depth").First();
            //var channelStreaming = channels.Where(c => c.ChannelName == "EWR Medium Resis").First();

            //======================================================================================================
            // Stream Latest Value
            //var channelInfo = new ChannelStreamingInfo
            //{
            //    ChannelId = channelStreaming.ChannelId,
            //    StartIndex = new StreamingStartIndex { Item = null },
            //    ReceiveChangeNotification = true
            //};
            //======================================================================================================

            //======================================================================================================
            // Stream Index Count
            var channelStreaming = channels.Where(c => c.ChannelName == "Hole Depth").First();
            var channelInfo = new ChannelStreamingInfo
            {
                ChannelId = channelStreaming.ChannelId,
                StartIndex = new StreamingStartIndex { Item = 3 },
                ReceiveChangeNotification = true
            };

            var listChannels = new List<ChannelStreamingInfo>();
            listChannels.Add(channelInfo);
            var streamResponse = await StreamingChannel(listChannels, count: 3, throwable: false);

            var jsonObjectRespone = EtpExtensions.Serialize(streamResponse[0], true);
            var jsonObjectRespone2 = EtpExtensions.Serialize(streamResponse[0], false);

            var jsonArrayRespone = EtpExtensions.Serialize(streamResponse, true);
            var jsonArrayRespone2 = EtpExtensions.Serialize(streamResponse, false);

            Console.WriteLine("End........");
            //======================================================================================================


            //======================================================================================================
            // Stream Depth Index
            //int startInDex = 20223;
            //int channelScale = channelStreaming.Indexes.FirstOrDefault()?.Scale ?? 0;
            //var scale = Convert.ToInt64((startInDex * Math.Pow(10, channelScale)));
            //var channelInfo = new ChannelStreamingInfo
            //{
            //    ChannelId = channelStreaming.ChannelId,
            //    StartIndex = new StreamingStartIndex { Item = scale },
            //    ReceiveChangeNotification = true
            //};
            //======================================================================================================

            //======================================================================================================
            // Stream Time Index
            //int startInDex = 20223;
            //DateTime startDate = DateTime.ParseExact("2022/27/06", "yyyy/dd/MM", System.Globalization.CultureInfo.InvariantCulture);
            //var startTime = new DateTimeOffset(startDate).ToUnixTimeMicroseconds();
            //int channelScale = channelStreaming.Indexes.FirstOrDefault()?.Scale ?? 0;
            //var scale = Convert.ToInt64((startInDex * Math.Pow(10, channelScale)));
            //var channelInfo = new ChannelStreamingInfo
            //{
            //    ChannelId = channelStreaming.ChannelId,
            //    StartIndex = new StreamingStartIndex { Item = startTime },
            //    ReceiveChangeNotification = true
            //};
            //======================================================================================================

            //var streamResponse = await StreamingChannel(listChannels, count: -1, throwable: false);
            //var streamResponse = await StreamingChannel(listChannels, count: -1, throwable: false);

            Console.WriteLine("End........");
        }
    }
}
