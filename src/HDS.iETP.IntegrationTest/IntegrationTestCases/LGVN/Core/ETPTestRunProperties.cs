using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAL.HDS.iETP.IntegrationTestCases.LGVN.Core
{
    public class ETPTestRunProperties
    {
        public string witsmlVersionPath { get; set; }
        public bool isHistoricalWell { get; set; }
        public string uidWell { get; set; }
        public string uidWellbore { get; set; }
        public string logUID { get; set; }
    }
}
