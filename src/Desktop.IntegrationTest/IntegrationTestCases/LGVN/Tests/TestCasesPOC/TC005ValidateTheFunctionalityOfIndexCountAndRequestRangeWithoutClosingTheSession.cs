using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.WITSMLstudio.Desktop.IntegrationTestCases.LGVN.Helper;
using PDS.WITSMLstudio.Desktop.IntegrationTestCases.Support;
using Energistics.Etp.Common;
using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.v11.Datatypes.ChannelData;
using Energistics.Etp.v11.Protocol.ChannelStreaming;
using Energistics.Etp.v11.Protocol.Discovery;
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
    public class TC005ValidateTheFunctionalityOfIndexCountAndRequestRangeWithoutClosingTheSession : TestBase
    {

        protected string testFolder1 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                + "\\ETPTesting\\inputs_TC005_1";
        protected string testFolder2 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                + "\\ETPTesting\\inputs_TC005_2";

        [TestMethod]
        [Description("TC005ValidateTheFunctionalityOfIndexCountAndRequestRangeWithoutClosingTheSession")]
        public async Task ValidateTheFunctionalityOfIndexCountAndRequestRangeWithoutClosingTheSession()
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
            var uris = JsonFileReader.ReadUris(testFolder1);

            handler.ChannelDescribe(uris);
            var argsMetadata = await onGetChannelMetaData.WaitAsync();

            var listChannelsIC = JsonFileReader.ReadChannels(testFolder2);

            var messageIC = await StreamingChannel(listChannelsIC, count: -1, throwable: false);
            var messageJsonIC = EtpExtensions.Serialize(messageIC, true);

            var resultIC = JsonFileReader.CompareJsonObjectToFile(messageJsonIC, testFolder2 + "\\result.json");

            Assert.IsTrue(resultIC);


            test.Info("Reading parameters from json inputs");
            var listChannelIds = JsonFileReader.ReadChannelIds(testFolder1);
            var scale = JsonFileReader.ReadScale(testFolder1);
            var startIndex = JsonFileReader.ReadStartIndex(testFolder1);
            var endIndex = JsonFileReader.ReadEndIndex(testFolder1);


            test.Info("Call headless function to execute requests and receive message responses");
            var message = await RequestRangeChannel(listChannelIds, startIndex, endIndex, scale, throwable: false);
            var messageJson = EtpExtensions.Serialize(message, true);

            test.Info("Comparing result json message with baseline result file");
            var result = JsonFileReader.CompareJsonObjectToFile(messageJson, testFolder1 + "\\result.json", test);

            test.AssertTrue(result);
        }
    }
}
