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

namespace PDS.WITSMLstudio.Desktop.IntegrationTestCases.LGVN.Tests.TestCasesPOC
{
    [TestClass]
    public class TC003ValidateTheFunctionalityOfRequestRangeWhenDepthIndexIsSelectedForRTLog : TestBase
    {

        protected string testFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                + "\\ETPTesting\\inputs_TC003";

        [TestMethod]
        [Description("ValidateTheFunctionalityOfRequestRangeWhenDepthIndexIsSelectedForRTLog")]
        public async Task ValidateTheFunctionalityOfRequestRangeWhenDepthIndexIsSelectedForRTLog()
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

            var listChannelIds = JsonFileReader.ReadChannelIds(testFolder);
            var scale = JsonFileReader.ReadScale(testFolder);
            var startIndex = JsonFileReader.ReadStartIndex(testFolder);
            var endIndex = JsonFileReader.ReadEndIndex(testFolder);

            var message = await RequestRangeChannel(listChannelIds, scale, startIndex, endIndex, throwable: false);

            var messageJson = EtpExtensions.Serialize(message, true);
            var result = JsonFileReader.CompareJsonObjectToFile(messageJson, testFolder + "\\result.json");

            Assert.IsTrue(result);

            Console.WriteLine("End........");
        }
    }
}
