using AspectInjector.Broker;
using AventStack.ExtentReports.Utils;
using Energistics.Etp.Common;
using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.v11.Datatypes.ChannelData;
using Energistics.Etp.v11.Protocol.ChannelStreaming;
using Energistics.Etp.v11.Protocol.Discovery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PDS.WITSMLstudio.Data.Channels;
using PDS.WITSMLstudio.Desktop.Core.Models;
using PDS.WITSMLstudio.Desktop.IntegrationTestCases.LGVN.Helper;
using PDS.WITSMLstudio.Desktop.IntegrationTestCases.Support;
using PDS.WITSMLstudio.Desktop.Reporter;
using PDS.WITSMLstudio.Framework;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDS.WITSMLstudio.Desktop.IntegrationTestCases.LGVN.Tests.TestCasesPOC
{
    [TestClass]
    public class TC004ValidateTheFunctionalityOfDepthIndexWhenStartStreamingIsSelectedForRTLog : TestBase
    {

        protected string testFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                + "\\ETPTesting\\inputs_TC004";

        [TestMethod]
        [Description("ValidateTheFunctionalityOfDepthIndexWhenStartStreamingIsSelectedForRTLog")]
        public async Task ValidateTheFunctionalityOfDepthIndexWhenStartStreamingIsSelectedForRTLog()
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
            var uris = JsonFileReader.ReadUris(testFolder);

            handler.ChannelDescribe(uris);
            var argsMetadata = await onGetChannelMetaData.WaitAsync();

            var listChannels = JsonFileReader.ReadChannelsDepthIndex(testFolder);

            var message = await StreamingChannel(listChannels, count: -1, throwable: false);

            var lastItems = MiscExtentions.TakeLast(message, 100);

            var messageJson = EtpExtensions.Serialize(lastItems, true);

            var result = JsonFileReader.CompareJsonObjectToFile(messageJson, testFolder + "\\result.json");

            Assert.IsTrue(result);

            Console.WriteLine("End........");
        }
    }
}
