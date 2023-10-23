using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDS.iETP.IntegrationTestCases.LGVN.Helper;
using HDS.iETP.IntegrationTestCases.Support;
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
using HAL.HDS.iETP.IntegrationTestCases.LGVN.Common;

namespace HDS.iETP.IntegrationTestCases.LGVN.Tests.TestCasesPOC
{
    [TestClass]
    public class TC005ValidateTheFunctionalityOfIndexCountAndRequestRangeWithoutClosingTheSession : TestBase2
    {

        protected string testFolder1 = Path.Combine(Constants.RESOURCE_TEST_DATA, "inputs_TC005_1");
        protected string testFolder2 = Path.Combine(Constants.RESOURCE_TEST_DATA, "inputs_TC005_2");


        [TestCategory("POC")]
        [TestMethod]
        [Description("ValidateTheFunctionalityOfIndexCountAndRequestRangeWithoutClosingTheSession")]
        public async Task POC5ValidateTheFunctionalityOfIndexCountAndRequestRangeWithoutClosingTheSession()
        {
            var isOpen = await etpSession.RequestSession();

            etpSession.StartStreamingChannel();

            // Describe 
            test.Info("Describing Uris");
            var uris = JsonFileReader.ReadUris(testFolder1);

            var argsMetadata = await etpSession.DescribeWell(uris.ToArray());

            var listChannelsIC = JsonFileReader.ReadChannels(testFolder2);

            var messageIC = await etpSession.StreamingChannel(listChannelsIC, count: -1, throwable: false);
            var messageJsonIC = EtpExtensions.Serialize(messageIC, true);

            var resultIC = JsonFileReader.CompareJsonObjectToFile(messageJsonIC, testFolder2 + "\\result.json");

            Assert.IsTrue(resultIC);


            test.Info("Reading parameters from json inputs");
            var listChannelIds = JsonFileReader.ReadChannelIds(testFolder1);
            var scale = JsonFileReader.ReadScale(testFolder1);
            var startIndex = JsonFileReader.ReadStartIndex(testFolder1);
            var endIndex = JsonFileReader.ReadEndIndex(testFolder1);


            test.Info("Call headless function to execute requests and receive message responses");
            var message = await etpSession.RequestRangeChannel(listChannelIds, startIndex, endIndex, scale, throwable: false);
            var messageJson = EtpExtensions.Serialize(message, true);

            test.Info("Comparing result json message with baseline result file");
            var result = JsonFileReader.CompareJsonObjectToFile(messageJson, testFolder1 + "\\result.json", test);

            test.AssertTrue(result);
        }
    }
}
