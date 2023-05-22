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

namespace PDS.WITSMLstudio.Desktop.Reporter
{
    internal class TestListener
    {
        /* Create an object of the ExtenReports class */
        public static ExtentReports _extent;
        /* Create an object of the ExtentTest class */
        /* This is used for adding detailed information about the tests being executed using the framework */
        public static ExtentTest _test;
        /* Unique Test Case to distinguish between tests */
        public String TC_Name;
        // This will get the current WORKING directory (i.e. \bin\Debug)
        public static string workingDir = Environment.CurrentDirectory;
        // or: Directory.GetCurrentDirectory() gives the same result

        // This will get the current PROJECT bin directory (ie ../bin/)
        public static string projectBinPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;

        // This will get the current PROJECT directory
        public static string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        /**
        * Returns an instance of {@link ExtentReports} object. If it doesn't exist creates a new instance and returns it
        */
        public TestListener GetListener()
        {
            if (_extent == null)
            {
                CreateInstance();
            }
            return this;
        }

        /**
         * Create ExtentReport and attaches htmlReporter to it
         */
        public void CreateInstance()
        {
            string timeNow = DateTime.Now.ToString("_MMddyyyy_hhmmtt");
            Directory.CreateDirectory(projectPath + "\\Reports");
            Directory.CreateDirectory(projectPath + "\\Reports\\" + timeNow);

            Console.WriteLine(projectPath);
            var reportPath = projectPath + "\\Reports\\" + timeNow + "\\index.html";
            var htmlReporter = new ExtentHtmlReporter(reportPath);
            _extent = new ExtentReports();
            _extent.AttachReporter(htmlReporter); 
            _extent.AddSystemInfo("Host Name", "Etp Server Testint MSTest Framework");
            _extent.AddSystemInfo("Environment", "Test Environment");
            _extent.AddSystemInfo("UserName", "Duong Luong");
        }

        /**
         * This method return test instance
         */
        public void CreateTest(string test)
        {
            _test = _extent.CreateTest(test);
        }

        /**
         * This method logs a message with the INFO level for both instances of TestNG Logger and ExtentTest
         */
        public void Info(String message)
        {
            _test.Log(Status.Info, message);
        }

         /**
         * This method logs a message with the FAIL level for both instances of TestNG Logger and ExtentTest
         */
        public void LogFail(String message)
        {
            _test.Log(Status.Fail, message);
        }

        /**
* This method logs a message with the PASS level for both instances of TestNG Logger and ExtentTest
*/
        public void LogPass(String message)
        {
            _test.Log(Status.Pass, message);
        }

        /**
        * This method flushes extent report
        */
        public void Flush()
        {
            _extent.Flush();
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
            catch (AssertFailedException e)
            {
                Assert.IsTrue(condition);
                LogFail("Check Failed:" + e);
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
