using Energistics.Etp.Common;
using HAL.HDS.iETP.IntegrationTestCases.LGVN.Common;
using HAL.HDS.iETP.IntegrationTestCases.LGVN.Core;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal;
using Newtonsoft.Json;
using PDS.WITSMLstudio.Connections;
using PDS.WITSMLstudio.Desktop.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.PeerToPeer;
using System.Text;
using System.Threading.Tasks;

namespace HDS.iETP.IntegrationTestCases.LGVN.Core
{
    public class ETPClientConfiguration2
    {
        [JsonProperty("writeToDB")] public bool writeToDB {get; set;} //: false,							//option to write to DB flag
        [JsonProperty("writeResultsToDisk")] public bool writeResultsToDisk {get; set;} //: true,					//option to write test results to file flag
        [JsonProperty("writeResponseToDisk")] public bool writeResponseToDisk {get; set;} //: true,					//option to write ETP server responses to file flag
        [JsonProperty("diskResultsPath")] public string diskResultsPath {get; set;} //: "",						//path to use if write to file  
        [JsonProperty("iterations")] public string iterations {get; set;} //: 1,								//how many times each test case should be run
        [JsonProperty("testTimeout")] public int testTimeout {get; set;} //: 60000,							//Time out for tests
        [JsonProperty("runTestsInParallel")] public bool runTestsInParallel {get; set;} //: true,					//run tests parallel flag
        [JsonProperty("DBUri")] public string DBUri {get; set;} //: "",									//DB connection if use DB to store results
        [JsonProperty("streamingTimeOutindexCountFor10InSec")] public int streamingTimeOutindexCountFor10InSec {get; set;} //: 800,  //timeout in seconds if no data is recieved for 10 index count, this increases with index count
        [JsonProperty("streamingTimerIndexCountNoDataInSec")] public int streamingTimerIndexCountNoDataInSec {get; set;} //: 800,   //timeout timer to exit index count after streaming is started and data data recieved
        [JsonProperty("streamingTimeOutIndexValueInSec")] public int streamingTimeOutIndexValueInSec {get; set;} //: 800,       //timeout in seconds if no data is recieved for index value
        [JsonProperty("streamingTimeOutRangeInSec")] public int streamingTimeOutRangeInSec {get; set;} //: 800,            //timeout in seconds if no data is recieved for range value
        [JsonProperty("streamingTimerCheckIntervalInSec")] public int streamingTimerCheckIntervalInSec {get; set;} //: 800,      //timeout timer check interval
        [JsonProperty("generateDetailedLog")] public bool generateDetailedLog {get; set;} //: true,
        [JsonProperty("maxMessageRate")] public int maxMessageRate {get; set;} //: 1000,
        [JsonProperty("maxDataItems")] public int maxDataItems { get; set;} //: 10000,
        [JsonProperty("testRuns")] public List<ETPTestRun> TestRuns { get; set; }

        public IList<EtpProtocolItem> requestedProtocol { get; set; } = DefaultProtocolItems().ToList();

        private static IEnumerable<EtpProtocolItem> DefaultProtocolItems()
        {
            yield return new EtpProtocolItem(Energistics.Etp.v11.Protocols.ChannelStreaming, "consumer");
            yield return new EtpProtocolItem(Energistics.Etp.v11.Protocols.ChannelStreaming, "producer", true);
            yield return new EtpProtocolItem(Energistics.Etp.v11.Protocols.ChannelDataFrame, "consumer");
            yield return new EtpProtocolItem(Energistics.Etp.v11.Protocols.ChannelDataFrame, "producer");
            yield return new EtpProtocolItem(Energistics.Etp.v11.Protocols.Discovery, "store", true);
            yield return new EtpProtocolItem(Energistics.Etp.v11.Protocols.Discovery, "customer");
            yield return new EtpProtocolItem(Energistics.Etp.v11.Protocols.Store, "store", true);
            yield return new EtpProtocolItem(Energistics.Etp.v11.Protocols.Store, "customer");
            yield return new EtpProtocolItem(Energistics.Etp.v11.Protocols.StoreNotification, "store", true);
            yield return new EtpProtocolItem(Energistics.Etp.v11.Protocols.StoreNotification, "customer");
            yield return new EtpProtocolItem(Energistics.Etp.v11.Protocols.GrowingObject, "store", true);
            yield return new EtpProtocolItem(Energistics.Etp.v11.Protocols.GrowingObject, "customer");
            yield return new EtpProtocolItem(Energistics.Etp.v11.Protocols.DataArray, "store");
            yield return new EtpProtocolItem(Energistics.Etp.v11.Protocols.DataArray, "customer");
            yield return new EtpProtocolItem(Energistics.Etp.v11.Protocols.WitsmlSoap, "store", isEnabled: false);
            yield return new EtpProtocolItem(Energistics.Etp.v11.Protocols.WitsmlSoap, "customer", isEnabled: false);
        }
    }
}
