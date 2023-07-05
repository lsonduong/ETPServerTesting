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

namespace PDS.WITSMLstudio.Desktop.IntegrationTestCases.LGVN.Tests.Sample
{
    [TestClass]
    public class ConnectionTests : TestBase
    {
        [TestMethod]
        [Description("Connection")]
        public async Task Connection()
        {
            // Wait for Open connection
            var isOpen = await client.OpenAsync();
            Assert.IsTrue(isOpen);
        }
    }
}
