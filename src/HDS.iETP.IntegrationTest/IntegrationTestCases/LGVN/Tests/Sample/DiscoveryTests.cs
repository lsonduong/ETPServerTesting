using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.v11.Datatypes.ChannelData;
using Energistics.Etp.v11.Protocol.ChannelStreaming;
using Energistics.Etp.v11.Protocol.Discovery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDS.iETP.IntegrationTestCases.LGVN.Helper;
using PDS.WITSMLstudio.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDS.iETP.IntegrationTestCases.LGVN.Tests.Sample
{
    [TestClass]
    public class DiscoveryTests : TestBase
    {
        [TestCategory("Sample")]
        [TestMethod]
        [Description("Discovery")]
        public async Task Test_Discovery()
        {
            RequestETPSession();
            client.Register<IDiscoveryCustomer, DiscoveryCustomerHandler>();

            var handlerD = client.Handler<IDiscoveryCustomer>();

            // Wait for Open connection
            var isOpen = await client.OpenAsync();

            var wellPath = "WITSML Store (1.4.1.1)->Demo well_test (1)->Demo well_test (2)->log (2)->SSC surface_Depth (12)->TFHA";
            var wellPath2 = "WITSML Store (1.4.1.1)->Demo well_test (1)->Demo well_test (2)->log (2)->SSC surface_Time (13)->TFHA";

            //var tmp = await Discovery(EtpUri.RootUri, -1);
            var tmp = await DiscoveryWell(wellPath);
            var tmp2 = await DiscoveryWell(wellPath2);

            var log_uri = tmp.Resource.Uri.ToString();
            var log_uri2 = tmp.Resource.Uri.ToString();

            var onGetRootResourcesResponse = AsyncHelper.HandleAsync<GetResourcesResponse, string>(
                x => handlerD.OnGetResourcesResponse += x, 10000);
            handlerD.GetResources(EtpUri.RootUri);

            var argsRoot = await onGetRootResourcesResponse.WaitAsync();
            Assert.IsNotNull(argsRoot);
            Assert.IsNotNull(argsRoot.Message.Resource);
            Assert.IsNotNull(argsRoot.Message.Resource.Uri);

            // Register event handler for child resources
            var onGetChildResourcesResponse = AsyncHelper.HandleAsync<GetResourcesResponse, string>(
                x => handlerD.OnGetResourcesResponse += x, 10000);
            var resource = argsRoot.Message.Resource;
            handlerD.GetResources(resource.Uri);
            var argsChild = await onGetChildResourcesResponse.WaitAsync();
            Assert.IsNotNull(argsChild);
            var test = argsChild.Message.Resource;
            if (argsChild.Header.MessageFlags == (int)MessageFlags.NoData)
                Assert.IsNull(argsChild.Message.Resource);
            else
            {
                Assert.IsNotNull(argsChild.Message.Resource);
                Assert.AreNotEqual(resource.Uri, argsChild.Message.Resource.Uri);
            }
        }
    }
}
