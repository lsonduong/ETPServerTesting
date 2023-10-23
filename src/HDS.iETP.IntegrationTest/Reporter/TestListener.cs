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
using System.Xml.Linq;
using System.Collections.Concurrent;

namespace PDS.WITSMLstudio.Desktop.Reporter
{
    public class TestListener
    {
        private static ThreadLocal<ConcurrentDictionary<long, TestListener>> testFactory = new ThreadLocal<ConcurrentDictionary<long, TestListener>>();
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
        // public static string projectBinPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;

        // This will get the current PROJECT directory
        public static string projectPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static string reportDirectory = Path.Combine(Constants.TEST_RESULTS_FOLDER, $"Report{DateTime.Now:_MMddyyyy_hhmmtt}");
        public static string reportFile = Path.Combine(reportDirectory, "index.html");
        /**
        * Returns an instance of {@link ExtentReports} object. If it doesn't exist creates a new instance and returns it
        */
        public TestListener GetListener()
        {
            CreateInstance();
            return testFactory.Value[Thread.CurrentThread.ManagedThreadId];
        }

        /**
         * Create ExtentReport and attaches htmlReporter to it
         */
        public void CreateInstance()
        {
            if (testFactory.Value == null)
            {
                testFactory.Value = new ConcurrentDictionary<long, TestListener>();
                InitExtentReport();
            }

            testFactory.Value.TryGetValue(Thread.CurrentThread.ManagedThreadId, out TestListener listener);
            if (listener != null)
            {
                testFactory.Value.TryAdd(Thread.CurrentThread.ManagedThreadId, listener);
            } else
            {
                testFactory.Value.TryAdd(Thread.CurrentThread.ManagedThreadId, new TestListener());

            }
        }

        private static void InitExtentReport()
        {
            _extent = new ExtentReports();
            var htmlReporter = new ExtentHtmlReporter(reportFile);
            _extent.AttachReporter(htmlReporter);
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
            _extent.Flush();
        }

        /**
        * This method flushes extent report
        */
        public void Remove()
        {
            testFactory.Value.TryRemove(Thread.CurrentThread.ManagedThreadId, out _);
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
