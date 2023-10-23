using JsonDiffPatchDotNet;
using Newtonsoft.Json.Linq;
using PDS.WITSMLstudio.Desktop.Core.Models;
using PDS.WITSMLstudio.Desktop.Reporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDS.iETP.IntegrationTestCases.Support
{
    public static class MessageCompare
    {
        /// <summary>
        /// Compare json object to baseline
        /// </summary>
        public static bool CompareJsonObjectToFile(string jsonString, string jsonPath, TestListener2 test = null)
        {
            var diffObj = new JsonDiffPatch();

            string jsonContent = JsonHelper.ReadFromJsonFile(jsonPath);
            JArray jsonExpected = JArray.Parse(jsonContent);
            JArray jsonActual = JArray.Parse(jsonString);

            bool comparison = JToken.DeepEquals(jsonActual, jsonExpected);

            if (!comparison)
            {
                var result = diffObj.Diff(jsonActual, jsonExpected);
                if (test != null)
                {
                    test.Info("Json Comparing Differences:");
                    test.Info(result.ToString());
                }
                else
                {
                    Console.WriteLine("Json Comparing Differences:");
                    Console.WriteLine(result.ToString());
                }
            }

            return comparison;
        }

        /// <summary>
        /// Compare 2 json object
        /// </summary>
        public static bool CompareJsonObjects(string jsonString, string jsonContent, TestListener2 test = null)
        {
            var diffObj = new JsonDiffPatch();

            JArray jsonExpected = JArray.Parse(jsonContent);
            JArray jsonActual = JArray.Parse(jsonString);

            bool comparison = JToken.DeepEquals(jsonActual, jsonExpected);

            if (!comparison)
            {
                var result = diffObj.Diff(jsonActual, jsonExpected);
                if (test != null)
                {
                    test.Info("Json Comparing Differences:");
                    test.Info(result.ToString());
                }
                else
                {
                    Console.WriteLine("Json Comparing Differences:");
                    Console.WriteLine(result.ToString());
                }
            }

            return comparison;
        }
    }
}
