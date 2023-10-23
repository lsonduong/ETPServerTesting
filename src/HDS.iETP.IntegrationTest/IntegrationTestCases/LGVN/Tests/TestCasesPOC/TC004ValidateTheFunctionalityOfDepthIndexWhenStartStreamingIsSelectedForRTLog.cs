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
using HDS.iETP.IntegrationTestCases.LGVN.Helper;
using HDS.iETP.IntegrationTestCases.Support;
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
    public class TC004ValidateTheFunctionalityOfDepthIndexWhenStartStreamingIsSelectedForRTLog : TestBase2
    {

        protected string testFolder = Path.Combine(Constants.RESOURCE_TEST_DATA, "inputs_TC004");


        [TestCategory("POC")]
        [TestMethod]
        [Description("ValidateTheFunctionalityOfDepthIndexWhenStartStreamingIsSelectedForRTLog")]
        public async Task POC4ValidateTheFunctionalityOfDepthIndexWhenStartStreamingIsSelectedForRTLog()
        {

            var isOpen = await etpSession.RequestSession();

            etpSession.StartStreamingChannel();

            // Describe 
            test.Info("Describing Uris");
            var uris = JsonFileReader.ReadUris(testFolder);

            var argsMetadata = await etpSession.DescribeWell(uris.ToArray());

            var listChannels = JsonFileReader.ReadChannelsDepthIndex(testFolder);

            var message = await etpSession.StreamingChannel(listChannels, count: -1, 60000, throwable: false);

            var lastItems = MiscExtentions.TakeLast(message, 100);

            var messageJson = EtpExtensions.Serialize(lastItems, true);

            var result = JsonFileReader.CompareJsonObjectToFile(messageJson, testFolder + "\\result.json");

            Assert.IsTrue(result);

            Console.WriteLine("End........");
        }
    }
}
