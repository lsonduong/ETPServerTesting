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
    public class TC001ValidateTheFunctionalityOfLatestValueForRTLog : TestBase
    {
        [TestMethod]
        [Description("ValidateTheFunctionalityOfLatestValueForRTLog")]
        public async Task ValidateTheFunctionalityOfLatestValueForRTLog()
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

            uris.Add("eml://witsml14/well(b6e41495-8b5f-4a87-927b-eed03cc60e46)/wellbore(5cb47631-dc38-4370-ae04-f60b100abbd8)/log(9e41d8b8-9113-3df7-aa98-e4e6b068d06e)/logCurveInfo(EWR%20Medium%20Resis)");
            //uris.Add("eml://witsml14/well(b6e41495-8b5f-4a87-927b-eed03cc60e46)");
            handler.ChannelDescribe(uris);
            var argsMetadata = await onGetChannelMetaData.WaitAsync();

            var channels = argsMetadata.Message.Channels;
            var channelStreaming = channels.Where(c => c.ChannelName == "EWR Medium Resis").First();

            // Stream Time Index
            int startInDex = 20223;
            DateTime startDate = DateTime.ParseExact("2022/27/06", "yyyy/dd/MM", System.Globalization.CultureInfo.InvariantCulture);
            var startTime = new DateTimeOffset(startDate).ToUnixTimeMicroseconds();
            int channelScale = channelStreaming.Indexes.FirstOrDefault()?.Scale ?? 0;
            var scale = Convert.ToInt64((startInDex * Math.Pow(10, channelScale)));
            var channelInfo = new ChannelStreamingInfo
            {
                ChannelId = channelStreaming.ChannelId,
                StartIndex = new StreamingStartIndex { Item = null },
                ReceiveChangeNotification = true
            };

            var listChannels = new List<ChannelStreamingInfo>();
            listChannels.Add(channelInfo);

            var a33 = await StreamingChannel(listChannels, count: -1, throwable: false);

            var valueOut = a33[0].Value.Item;

            Assert.AreEqual(valueOut.ToString(), "4.76966857910156");

            Console.WriteLine("End........");
        }
    }
}
