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
    public class TC001ValidateTheFunctionalityOfLatestValueForRTLog : TestBase
    {

        protected string testFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                + "\\ETPTesting\\inputs_TC001";

        [TestMethod]
        [Description("ValidateTheFunctionalityOfLatestValueForRTLog")]
        public async Task ValidateTheFunctionalityOfLatestValueForRTLog()
        {
            var requestedProtocol = JsonFileReader.ReadProtocolList(testFolder);
            IEtpExtender extender = client.CreateEtpExtender(requestedProtocol);

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

            extender.ChannelDescribe(uris);
            var argsMetadata = await onGetChannelMetaData.WaitAsync();

            var listChannels = JsonFileReader.ReadChannels(testFolder);

            var message = await StreamingChannel(listChannels, count: -1, throwable: false);

            var messageJson = EtpExtensions.Serialize(message, true);
            var result = JsonFileReader.CompareJsonObjectToFile(messageJson, testFolder + "\\result.json");

            Assert.IsTrue(result);

            Console.WriteLine("End........");
        }
    }
}
