using HAL.HDS.iETP.IntegrationTestCases.LGVN.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAL.HDS.iETP.IntegrationTestCases.LGVN.Common
{
    public static class Constants
    {
        public static readonly string RESOURCE_FOLDER = Path.Combine(PathHelper.GetRootPath(), "Resources");
        public static readonly string RESOURCE_TEST_DATA = Path.Combine(RESOURCE_FOLDER, "test-data");
        public static readonly string TEST_RESULTS_FOLDER = Path.Combine(PathHelper.GetRootPath(), "test-results");
        public static readonly string TEST_DEBUG_FOLDER = 
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
                "ETPTesting", "Debug", $"outputs{DateTime.Now:_MMddyyyy_hhmmtt}");
        
        public static readonly string APP_NAME = Path.Combine(PathHelper.GetRootPath(), "test-results");
        public static readonly string APP_VERSION = Path.Combine(PathHelper.GetRootPath(), "test-results");
    }
}
