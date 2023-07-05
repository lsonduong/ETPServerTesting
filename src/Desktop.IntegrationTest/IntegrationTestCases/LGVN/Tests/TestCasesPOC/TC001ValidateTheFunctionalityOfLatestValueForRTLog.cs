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
    public class TC001ValidateTheFunctionalityOfLatestValueForRTLog : TestBase
    {
        [TestMethod]
        [Description("ValidateTheFunctionalityOfLatestValueForRTLog")]
        public async Task ValidateTheFunctionalityOfLatestValueForRTLog()
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
            var uris = JsonFileReader.ReadUris(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                + "\\ETPTesting\\inputs_TC001");

            handler.ChannelDescribe(uris);
            var argsMetadata = await onGetChannelMetaData.WaitAsync();

            var listChannels = JsonFileReader.ReadChannels(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                + "\\ETPTesting\\inputs_TC001");

            var a33 = await StreamingChannel(listChannels, count: -1, throwable: false);

            var a33Json = EtpExtensions.Serialize(a33, true);
            var result = JsonFileReader.CompareJsonObjectToFile(a33Json, Environment.GetFolderPath
                (Environment.SpecialFolder.MyDocuments) + "\\ETPTesting\\inputs_TC001\\result.json");

            Assert.IsTrue(result);

            Console.WriteLine("End........");
        }
    }
}
