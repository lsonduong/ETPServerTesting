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

using Energistics.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.WITSMLstudio.Connections;
using PDS.WITSMLstudio.Desktop.Plugins.EtpBrowser.ViewModels;
using PDS.WITSMLstudio.Desktop.Core.Runtime;
using PDS.WITSMLstudio.Desktop.Core.ViewModels;
using System.Threading.Tasks;

namespace PDS.WITSMLstudio.Desktop.IntegrationTestCases.EtpBrowser
{
    [TestClass]
    public class SettingsViewModelTests
    {
        private static string _validEtpUri = "wss://witsmlapptstetpx.halliburton.com/hal.witsml.host.iis/api/haletp";

        private BootstrapperHarness _bootstrapper;
        private TestRuntimeService _runtime;
        private SettingsViewModel _settingsViewModel;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestSetUp()
        {
            _bootstrapper = new BootstrapperHarness();
            _runtime = new TestRuntimeService(_bootstrapper.Container);
            _runtime.Shell = new ShellViewModel(_runtime);
            _settingsViewModel = new SettingsViewModel(_runtime);
        }

        [TestMethod]
        public async Task SettingsViewModel_GetVersions()
        {
            var connection = new Connection()
            {
                Uri = _validEtpUri,
                AuthenticationType = AuthenticationTypes.Basic,
                Username = "hai.vu3@halliburton.com",
                Password = "KhongCho!234"
            };

            await _settingsViewModel.RequestSessionTest(connection);

        }
    }
}
