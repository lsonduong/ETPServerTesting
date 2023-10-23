using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Caliburn.Micro;
using Energistics.DataAccess.PRODML200.ComponentSchemas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;
using Avro;
using HAL.HDS.iETP.IntegrationTestCases.LGVN.Common;
using System.Collections.Concurrent;
using System.Xml.Linq;
using AventStack.ExtentReports.Model;

namespace PDS.WITSMLstudio.Desktop.Reporter
{
    public class TestListener2
    {
        public string Session { get; set; } = "";

        private static readonly ConcurrentDictionary<string, TestListener2> testFactory = new ConcurrentDictionary<string, TestListener2>();

        /* Create an object of the ExtenReports class */
        public static ExtentReports Extent { get; set; }
        /* Create an object of the ExtentTest class */
        /* This is used for adding detailed information about the tests being executed using the framework */
        public ExtentTest _test;
        /* Unique Test Case to distinguish between tests */
        public String TC_Name;
        // This will get the current WORKING directory (i.e. \bin\Debug)
        public static string workingDir = Environment.CurrentDirectory;
        // or: Directory.GetCurrentDirectory() gives the same result

        // This will get the current PROJECT bin directory (ie ../bin/)
        // public static string projectBinPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;

        // This will get the current PROJECT directory
        public static string projectPath = Constants.TEST_RESULTS_FOLDER;

        public static string reportDirectory = Path.Combine(Constants.TEST_RESULTS_FOLDER, $"Report{DateTime.Now:_MMddyyyy_hhmmtt}");
        public static string reportFile = Path.Combine(reportDirectory, "index.html");

        /**
        * Returns an instance of {@link ExtentReports} object. If it doesn't exist creates a new instance and returns it
        */

        public static string ReportPath() => reportDirectory;

        public TestListener2 GetListener(string session)
        {
            testFactory.TryGetValue(session, out TestListener2 listener);

            if (listener == null)
                throw new NullReferenceException("TestListener is null. Please create new `TestListener` firstly");

            return listener;
        }

        /**
         * Create ExtentReport and attaches htmlReporter to it
         */
        public static TestListener2 CreateInstance(string session)
        {
            if (testFactory.Count == 0)
                initExtentReport();

            testFactory.TryGetValue(session, out TestListener2 listener);
            if (listener != null)
                return listener;

            TestListener2 test = new TestListener2 { Session = session };
            testFactory.TryAdd(session, test);
            return test;
        }

        private static void initExtentReport()
        {
            Console.WriteLine("Initiating new Extend Report....");
            Console.WriteLine($"Report file will be at: [{reportFile}]");
            Extent = new ExtentReports();
            Directory.CreateDirectory(projectPath);
            Console.WriteLine(projectPath);
            var htmlReporter = new ExtentHtmlReporter(reportFile);

            Extent.AttachReporter(htmlReporter);
            Extent.AddSystemInfo("Host Name", "Etp Server Testing MSTest Framework");
            Extent.AddSystemInfo("Environment", "Test Environment");
            Extent.AddSystemInfo("UserName", "Duong Luong");
        }

        /**
         * This method return test instance
         */
        public void CreateTest(string test)
        {
            Console.WriteLine($"Creating new TestCase: [{test}]");
            _test = Extent.CreateTest(test);
        }

        /**
         * This method logs a message with the INFO level for both instances of TestNG Logger and ExtentTest
         */
        public void Info(String message)
        {
            Console.WriteLine($"{message}");
            _test.Log(Status.Info, message);
        }

        /**
        * This method logs a message with the FAIL level for both instances of TestNG Logger and ExtentTest
        */
        public void LogFail(String message)
        {
            Console.WriteLine($"{message}");
            _test.Log(Status.Fail, message);
        }

        /**
* This method logs a message with the PASS level for both instances of TestNG Logger and ExtentTest
*/
        public void LogPass(String message)
        {
            Console.WriteLine($"{message}");
            _test.Log(Status.Pass, message);
        }

        /**
        * This method flushes extent report
        */
        public void Flush()
        {
            Console.WriteLine($"Flushing Report...");
            Extent.Flush();
        }

        /**
        * This method put assert to report
        */
        public void AssertTrue(bool condition)
        {
            try
            {
                Assert.IsTrue(condition);
                //Assertion to be placed here
                LogPass("Check Passed");
            }
            catch (Exception e)
            {
                LogFail("Check Failed:" + e);
                Assert.IsTrue(condition);
            }
        }

        /**
        * This method put assert to report
        */
        public void AssertEquals(object objA, object objB)
        {
            try
            {
                Assert.Equals(objA, objB);
                //Assertion to be placed here
                LogPass("Check Passed");
            }
            catch (AssertFailedException e)
            {
                Assert.Equals(objA, objB);
                LogFail("Check Failed:" + e);
            }
        }

        /**
        * This method put assert to report
        */
        public void AssertNotEquals(object objA, object objB)
        {
            try
            {
                Assert.AreNotEqual(objA, objB);
                //Assertion to be placed here
                LogPass("Check Passed");
            }
            catch (AssertFailedException e)
            {
                Assert.AreNotEqual(objA, objB);
                LogFail("Check Failed:" + e);
            }
        }

        /**
        * This method put assert to report
        */
        public void AssertContains(string objA, string objB)
        {
            try
            {
                StringAssert.Contains(objA, objB);
                //Assertion to be placed here
                LogPass("Check Passed");
            }
            catch (AssertFailedException e)
            {
                StringAssert.Contains(objA, objB);
                LogFail("Check Failed:" + e);
            }
        }
    }
}
