using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDS.WITSMLstudio.Desktop.IntegrationTestCases.LGVN.Helper
{
    public static class DateTimeHelper
    {
        public static long GetCurrentDateTimeOffset() 
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public static string GetCurrentDate()
        {
            return DateTime.Now.ToString();
        }
    }
}
