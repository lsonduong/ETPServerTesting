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
    public class TC006VerifyNewlyAddedCurveGettingDisplayedWhilePerformingDescribeOperation : TestBase2
    {
        protected string testFolder = Path.Combine(Constants.RESOURCE_TEST_DATA, "inputs_TC001");


        [TestCategory("POC")]
        [TestMethod]
        [Description("TC006VerifyNewlyAddedCurveGettingDisplayedWhilePerformingDescribeOperation")]
        public async Task POC6VerifyNewlyAddedCurveGettingDisplayedWhilePerformingDescribeOperation()
        {
            var isOpen = await etpSession.RequestSession();

            etpSession.StartStreamingChannel();

            // Describe 
            test.Info("Describing Uris");
            var uris = JsonFileReader.ReadUris(testFolder);
            var argsMetadata = await etpSession.DescribeWell(uris.ToArray());


            test.Info("Get newly added curve channel data");
            var channels = argsMetadata.Channels;
            int depthType = (int)ChannelIndexTypes.Depth;

            var channelStreaming = channels
                .Where(c => c.ChannelName == "FRPI" && c.Indexes.First().IndexKind == depthType).First();

            var channelInfo = new ChannelStreamingInfo
            {
                ChannelId = channelStreaming.ChannelId,
                StartIndex = new StreamingStartIndex { Item = null },
                ReceiveChangeNotification = true
            };
            var listChannels = new List<ChannelStreamingInfo> { channelInfo };

            test.Info("Call headless app to get latest value streaming data from curve channel data");
            var message = await etpSession.StreamingChannel(listChannels, count: -1, throwable: false);

            test.Info("Comparing result json message with baseline result file");
            var messageJson = EtpExtensions.Serialize(message, true);
            var result = JsonFileReader.CompareJsonObjectToFile(messageJson, testFolder + "\\result.json");

            test.AssertTrue(result);

            test.Info("End of Test Case.");
        }
    }
}
