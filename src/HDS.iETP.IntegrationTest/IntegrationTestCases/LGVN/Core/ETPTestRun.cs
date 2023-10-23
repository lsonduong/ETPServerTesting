using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAL.HDS.iETP.IntegrationTestCases.LGVN.Core
{
    public class ETPTestRun
    {
        public string name { get; set; }
        public string tag { get; set; }
        public bool enabled { get; set; }
        public string url { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string secretKey { get; set; }
        public ETPTestRunProperties properties { get; set; }

    }
}
