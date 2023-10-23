using AventStack.ExtentReports.Utils;
using Energistics.Etp.Common;
using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.v11.Datatypes.ChannelData;
using Energistics.Etp.v11.Protocol.ChannelStreaming;
using Energistics.Etp.v11.Protocol.Discovery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PDS.WITSMLstudio.Desktop.Core.Models;
using HDS.iETP.IntegrationTestCases.LGVN.Helper;
using HDS.iETP.IntegrationTestCases.Support;
using PDS.WITSMLstudio.Desktop.Reporter;
using PDS.WITSMLstudio.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HAL.HDS.iETP.IntegrationTestCases.LGVN.Common;
using System.IO;

namespace HDS.iETP.IntegrationTestCases.LGVN.Tests.TestCasesPOC
{
    [TestClass]
    public class TC003ValidateTheFunctionalityOfRequestRangeWhenDepthIndexIsSelectedForRTLog : TestBase2
    {
        protected string testFolder = Path.Combine(Constants.RESOURCE_TEST_DATA, "inputs_TC003");

        [TestCategory("POC")]
        [TestMethod]
        [Description("ValidateTheFunctionalityOfRequestRangeWhenDepthIndexIsSelectedForRTLog")]
        public async Task POC3ValidateTheFunctionalityOfRequestRangeWhenDepthIndexIsSelectedForRTLog()
        {
            var isOpen = await etpSession.RequestSession();

            etpSession.StartStreamingChannel();

            // Describe 
            test.Info("Describing Uris");
            var uris = JsonFileReader.ReadUris(testFolder);

            var argsMetadata = await etpSession.DescribeWell(uris.ToArray());


            test.Info("Reading parameters from json inputs");
            var listChannelIds = JsonFileReader.ReadChannelIds(testFolder);
            var scale = JsonFileReader.ReadScale(testFolder);
            var startIndex = JsonFileReader.ReadStartIndex(testFolder);
            var endIndex = JsonFileReader.ReadEndIndex(testFolder);


            test.Info("Call headless function to execute requests and receive message responses");
            var message = await etpSession.RequestRangeChannel(listChannelIds, startIndex, endIndex, scale, throwable: false);
            var messageJson = EtpExtensions.Serialize(message, true);

            test.Info("Comparing result json message with baseline result file");
            var result = JsonFileReader.CompareJsonObjectToFile(messageJson, testFolder + "\\result.json", test);

            test.AssertTrue(result);

            test.Info("End of Test Case.");
        }
    }
}
