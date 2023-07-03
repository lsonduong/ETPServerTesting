using AventStack.ExtentReports.Utils;
using Energistics.Etp.Common;
using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.v11.Datatypes.ChannelData;
using Energistics.Etp.v11.Protocol.ChannelStreaming;
using Energistics.Etp.v11.Protocol.Discovery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PDS.WITSMLstudio.Desktop.Core.Models;
using PDS.WITSMLstudio.Desktop.IntegrationTestCases.LGVN.Helper;
using PDS.WITSMLstudio.Desktop.IntegrationTestCases.Support;
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

            //uris.Add("eml://witsml14/well(b6e41495-8b5f-4a87-927b-eed03cc60e46)/wellbore(5cb47631-dc38-4370-ae04-f60b100abbd8)/log(0a4342fa-02f0-3470-b685-e9d86ac83294)/logCurveInfo(Hole%20Depth)");
            uris.Add("eml://witsml14/well(b6e41495-8b5f-4a87-927b-eed03cc60e46)");
            handler.ChannelDescribe(uris);
            var argsMetadata = await onGetChannelMetaData.WaitAsync();

            var channels = argsMetadata.Message.Channels;
            var channelStreaming = channels.Where(c => c.ChannelName == "Hole Depth").First();

            //Stream Time Index
            //int startInDex = 20223;
            //DateTime startDate = DateTime.ParseExact("2022/27/06", "yyyy/dd/MM", System.Globalization.CultureInfo.InvariantCulture);
            //var startTime = new DateTimeOffset(startDate).ToUnixTimeMicroseconds();
            //int channelScale = channelStreaming.Indexes.FirstOrDefault()?.Scale ?? 0;
            //var scale = Convert.ToInt64((startInDex * Math.Pow(10, channelScale)));
            //var channelInfo = new ChannelStreamingInfo
            //{
            //    ChannelId = channelStreaming.ChannelId,
            //    StartIndex = new StreamingStartIndex { Item = null },
            //    ReceiveChangeNotification = true
            //};

            //var listChannels = new List<ChannelStreamingInfo>();
            //listChannels.Add(channelInfo);
            //inputs_07032023_0706PM
            //inputs_07032023_0658PM

            var listChannels = JsonFileReader.ReadChannels(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                + "\\ETPTesting\\inputs_07032023_0706PM");

            var a33 = await StreamingChannel(listChannels, count: -1, throwable: false);

            var a33Json = EtpExtensions.Serialize(a33, true);
            var result = JsonFileReader.CompareJsonObjectToFile(a33Json, Environment.GetFolderPath
                (Environment.SpecialFolder.MyDocuments) + "\\testCompare.json");

            Assert.IsTrue(result);

            Console.WriteLine("End........");
        }
    }
}
