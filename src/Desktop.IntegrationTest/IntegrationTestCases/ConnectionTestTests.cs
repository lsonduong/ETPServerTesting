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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.WITSMLstudio.Connections;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Desktop.Core.Connections;
using PDS.WITSMLstudio.Desktop.Core.Runtime;
using System.Security.RightsManagement;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System.IO;
using System;
using PDS.WITSMLstudio.Desktop.Reporter;

namespace PDS.WITSMLstudio.Desktop.IntegrationTestCases
{
    [TestClass]
    public class ConnectionTestTests
    {
        // private string _validWitsmlUri = "http://localhost/Witsml.Web/WitsmlStore.svc";
        private string _validEtpUri = "wss://witsmlapptstetpx.halliburton.com/hal.witsml.host.iis/api/haletp";
        //"ws://localhost/witsml.web/api/etp";
        private IRuntimeService _runtime;
        private static readonly TestListener test = new TestListener().GetListener();

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            _runtime = new TestRuntimeService(ContainerFactory.Create());
        }

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
        }

        [ClassCleanup]
        public static void TearDown()
        {
            test.Flush();
        }

        [TestMethod]
        public async Task EtpServer_CanConnect_Valid_Endpoint()
        {
            TestContext.Properties["EtpServerUrl"] = "wss://witsmlapptstetpx.halliburton.com/hal.witsml.host.iis/api/haletp";

            test.CreateTest(TestContext.TestName);
            test.Info("Namespace:" + TestContext.FullyQualifiedTestClassName);

            var etpConnectionTest = new EtpConnectionTest(_runtime);
            var connection = new Connection().deserialize(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                + "\\ETPTesting\\inputs_06202023_0305PM\\connection.json");
            //var connection = new Connection() { Uri = _validEtpUri, AuthenticationType = AuthenticationTypes.Basic, 
            //Username = "hai.vu3@halliburton.com", Password = "KhongCho@345"};

            test.Info("Check Connection Test");

            var result = await etpConnectionTest.CanConnect(connection);

            test.AssertTrue(result);
        }

        [TestMethod]
        public async Task EtpServer_CanConnect_Session()
        {

            TestContext.Properties["EtpServerUrl"] = "wss://witsmlapptstetpx.halliburton.com/hal.witsml.host.iis/api/haletp";

            test.CreateTest(TestContext.TestName);
            test.Info("Namespace:" + TestContext.FullyQualifiedTestClassName);

            var etpConnectionTest = new EtpConnectionTest(_runtime);
            var connection = new Connection().deserialize(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                + "\\ETPTesting\\inputs_06202023_0305PM\\connection.json");
            //{
            //    Uri = _validEtpUri,
            //    AuthenticationType = AuthenticationTypes.Basic,
            //    Username = "hai.vu3@halliburton.com",
            //    Password = "KhongCho@345"
            //};

            test.Info("Check Connect Session Test");

            var result = await etpConnectionTest.ConnectSession(connection);

            test.AssertNotEquals(result, "");
        }
    }
}
