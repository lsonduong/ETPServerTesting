using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.v11.Datatypes.ChannelData;
using Energistics.Etp.v11.Protocol.ChannelStreaming;
using Energistics.Etp.v11.Protocol.Discovery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.WITSMLstudio.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDS.iETP.IntegrationTestCases.LGVN.Tests.Sample
{
    [TestClass]
    public class ConnectionTests : TestBase
    {
        [TestCategory("Sample")]
        [TestMethod]
        [Description("Connection")]
        public async Task Connection()
        {
            RequestETPSession();
            // Wait for Open connection
            var isOpen = await client.OpenAsync();
            Assert.IsTrue(isOpen);
        }
    }
}
