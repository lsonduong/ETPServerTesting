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
    public class TC002ValidateTheFunctionalityOfIndexCountForRTLog : TestBase2
    {

        protected string testFolder = Path.Combine(Constants.RESOURCE_TEST_DATA, "inputs_TC002");

        [TestCategory("POC")]
        [TestMethod]
        [Description("ValidateTheFunctionalityOfIndexCountForRTLog")]
        public async Task POC2ValidateTheFunctionalityOfIndexCountForRTLog()
        {
            var isOpen = await etpSession.RequestSession();

            etpSession.StartStreamingChannel();

            // Describe 
            test.Info("Describing Uris");
            var uris = JsonFileReader.ReadUris(testFolder);

            var argsMetadata = await etpSession.DescribeWell(uris.ToArray());

            test.Info("Reading parameters from json inputs");
            var listChannels = JsonFileReader.ReadChannels(testFolder);

            test.Info("Call headless function to execute requests and receive message responses");
            var message = await etpSession.StreamingChannel(listChannels, count: -1, throwable: false);

            test.Info("Comparing result json message with baseline result file");
            var messageJson = EtpExtensions.Serialize(message, true);
            var result = JsonFileReader.CompareJsonObjectToFile(messageJson, testFolder + "\\result.json");

            Assert.IsTrue(result);

            Console.WriteLine("End........");
        }
    }
}
