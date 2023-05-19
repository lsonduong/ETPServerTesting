//----------------------------------------------------------------------- 
// PDS WITSMLstudio Desktop, 2018.1
//
// Copyright 2018 PDS Americas LLC
// 
// Licensed under the PDS Open Source WITSML Product License Agreement (the
// "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.pds.group/WITSMLstudio/OpenSource/ProductLicenseAgreement
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using System.Threading.Tasks;
using PDS.WITSMLstudio.Connections;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Desktop.Core.Connections;
using PDS.WITSMLstudio.Desktop.Core.Runtime;
using NUnit.Framework;
using NUnit.Allure.Core;
using NUnit.Allure.Attributes;

namespace PDS.WITSMLstudio.Desktop.Connections
{
    [TestFixture]
    [AllureNUnit]

    [AllureSuite("BasicConnectionTests")]

    [AllureDisplayIgnored]
    public class ConnectionUnitTests
    {
        private static string _validWitsmlUri = "http://localhost/Witsml.Web/WitsmlStore.svc";
        private static string _validEtpUri = "ws://localhost/witsml.web/api/etp";
        private IRuntimeService _runtime;

        public TestContext TestContext { get; set; }

        public void TestSetup()
        {
            _runtime = new TestRuntimeService(ContainerFactory.Create());
        }

        [Test(Description = "WITSML Connection Can Connect Valid Endpoint")]
        public async Task EtpServer_CanConnect_Valid_Endpoint()
        {

            _validEtpUri = "wss://witsmlapptstetpx.halliburton.com/hal.witsml.host.iis/api/haletp";

            var etpConnectionTest = new EtpConnectionTest(_runtime);
            var connection = new Connection()
            {
                Uri = _validEtpUri,
                AuthenticationType = AuthenticationTypes.Basic,
                Username = "hai.vu3@halliburton.com",
                Password = "KhongCho!234"
            };

            var result = await etpConnectionTest.CanConnect(connection);

            Assert.IsTrue(result);
        }

        [Test(Description = "EtpServer CanConnect Session")]
        public async Task EtpServer_CanConnect_Session()
        {

            _validEtpUri = "wss://witsmlapptstetpx.halliburton.com/hal.witsml.host.iis/api/haletp";

            var etpConnectionTest = new EtpConnectionTest(_runtime);
            var connection = new Connection()
            {
                Uri = _validEtpUri,
                AuthenticationType = AuthenticationTypes.Basic,
                Username = "hai.vu3@halliburton.com",
                Password = "KhongCho!234"
            };

            var result = await etpConnectionTest.ConnectSession(connection);

            Assert.AreNotEqual(result, "");
        }
    }
}
