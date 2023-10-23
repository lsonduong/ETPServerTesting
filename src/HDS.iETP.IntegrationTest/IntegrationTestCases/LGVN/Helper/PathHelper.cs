using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAL.HDS.iETP.IntegrationTestCases.LGVN.Helper
{
    public static class PathHelper
    {
        public static string GetRootPath()
        {
            var path = GetBasePath();
            path = path.Substring(0, path.IndexOf("\\bin"));
            if(!path.EndsWith("\\"))
                path += "\\";
            return path;
        }
        public static string GetBasePath()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            if(!path.EndsWith("\\"))
                path += "\\";
            return path;
        }
    }
}
