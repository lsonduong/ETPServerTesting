using Energistics.Etp.Common;
using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.v11.Datatypes.ChannelData;
using Energistics.Etp.v11.Protocol.ChannelStreaming;
using Energistics.Etp.v11.Protocol.Discovery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PDS.WITSMLstudio.Data.Channels;
using HDS.iETP.IntegrationTestCases.LGVN.Helper;
using PDS.WITSMLstudio.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDS.iETP.IntegrationTestCases.LGVN.Tests.Sample
{
    [TestClass]
    public class RequestRangeTests : TestBase
    {
        [TestCategory("Sample")]
        [TestMethod]
        [Description("Streaming")]
        public async Task Test_Request_Range()
        {
            RequestETPSession();
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

            //======================================================================================================
            // Request Range - Depth Index
            var channelStreamingDepthIndex = channels.Where(c => c.ChannelName == "EWR Deep Resis").First();
            var resultRangeRequest1 = await RequestRangeChannel(channelStreamingDepthIndex, 0, 100, throwable: false);
            //======================================================================================================

            //======================================================================================================
            // Request Range - Time Index
            var channelStreamingTimeIndex = channels.Where(c => c.ChannelName == "ROP Avg").Last();
            DateTime startTime = DateTime.ParseExact("2010/15/06", "yyyy/dd/MM", System.Globalization.CultureInfo.InvariantCulture);
            DateTime endTime = DateTime.ParseExact("2023/05/07", "yyyy/dd/MM", System.Globalization.CultureInfo.InvariantCulture);
            var resultRangeRequest2 = await RequestRangeChannel(channelStreamingTimeIndex, startTime, endTime, throwable: false);
            //======================================================================================================


            Console.WriteLine("End........");
        }
    }
}
