using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDS.iETP.IntegrationTestCases.LGVN.Common;
using PDS.WITSMLstudio.Desktop.Reporter;
using System;
using HAL.HDS.iETP.IntegrationTestCases.LGVN.Core;
using System.Threading.Tasks;

namespace HDS.iETP.IntegrationTestCases.LGVN.Tests
{

    [TestClass]
    public class TestBase2
    {

        protected TestListener2 test;
        public TestContext TestContext { get; set; }
        protected ETPSession etpSession;
        protected string session;

        [TestCleanup]
        public async Task TearDown()
        {
            test.Flush();
            await etpSession.CloseSession();
        }

        [TestInitialize]
        public void TestSetUp()
        {
            session = Guid.NewGuid().ToString();
            test = TestListener2.CreateInstance(session);
            test.CreateTest(TestContext.TestName);
            test.Info("Namespace:" + TestContext.FullyQualifiedTestClassName + $" session[{session}]");
            etpSession = ETPManagement.CreateNewEtpSession(session);
        }
    }
}
