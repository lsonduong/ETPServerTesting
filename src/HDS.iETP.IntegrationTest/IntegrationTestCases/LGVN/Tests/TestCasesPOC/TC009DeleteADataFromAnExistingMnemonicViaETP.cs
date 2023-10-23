using AventStack.ExtentReports.Utils;
using Energistics.Etp.Common;
using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.v11.Datatypes.ChannelData;
using Energistics.Etp.v11.Protocol.ChannelStreaming;
using Energistics.Etp.v11.Protocol.Discovery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PDS.WITSMLstudio.Desktop.Core;
using PDS.WITSMLstudio.Desktop.Core.Adapters;
using PDS.WITSMLstudio.Desktop.Core.Models;
using HDS.iETP.IntegrationTestCases.LGVN.Helper;
using HDS.iETP.IntegrationTestCases.Support;
using PDS.WITSMLstudio.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HAL.HDS.iETP.IntegrationTestCases.LGVN.Common;
using HAL.HDS.iETP.IntegrationTestCases.LGVN.Core;

namespace HDS.iETP.IntegrationTestCases.LGVN.Tests.TestCasesPOC
{
    [TestClass]
    public class TC009DeleteADataFromAnExistingMnemonicViaETP : TestBase2
    {

        protected string testFolder = Path.Combine(Constants.RESOURCE_TEST_DATA, "inputs_TC001");

        [TestCategory("POC")]
        [TestMethod]
        [Description("DeleteADataFromAnExistingMnemonicViaETP")]
        public async Task POC9DeleteADataFromAnExistingMnemonicViaETP()
        {
            test.Info("Test data folder: " + testFolder);

            test.Info("Wait for Open connection");
            var isOpen = await etpSession.RequestSessionFromFileInputs(testFolder);

            etpSession.StartStreamingChannel();
            etpSession.StartStreamingChannel();

            test.Info("Describing Uris");
            var uris = JsonFileReader.ReadUris(testFolder);
            var argsMetadata = await etpSession.DescribeWell(uris.ToArray());

            test.Info("Reading parameters from json inputs");
            var listChannels = JsonFileReader.ReadChannels(testFolder);

            test.Info("Call headless function to execute streaming (latest value) and receive message responses");
            var message = await etpSession.StreamingChannel(listChannels, count: -1, throwable: false);
            var message2 = await etpSession.StreamingChannel(listChannels, count: -1, throwable: false);

            var messageJson = EtpExtensions.Serialize(message, true);
            test.Info($"Message Response: {messageJson}");
            var message2Json = EtpExtensions.Serialize(message2, true);
            test.Info($"Message Response: {message2Json}");

            test.Info("Comparing result json message with baseline result file");
            var result = MessageCompare.CompareJsonObjectToFile(messageJson, testFolder + "\\result.json", test);
            test.AssertTrue(result);

            test.Info("Comparing result from 2 message reponses");
            var resultCompare = MessageCompare.CompareJsonObjects(messageJson, message2Json, test);
            test.AssertTrue(resultCompare);

            Console.WriteLine("End........");
        }
    }
}
