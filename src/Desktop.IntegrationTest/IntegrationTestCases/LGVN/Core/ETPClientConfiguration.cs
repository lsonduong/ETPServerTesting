using Energistics.Etp.Common;
using PDS.WITSMLstudio.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.PeerToPeer;
using System.Text;
using System.Threading.Tasks;

namespace PDS.WITSMLstudio.Desktop.IntegrationTestCases.LGVN.Core
{
    public class ETPClientConfiguration
    {
        public string ApplicationName { get; set; }
        public string ApplicationVersion { get; set; }
        public string Uri { get; set; }
        public Connection EtpConnection { get; set; }

        public ETPClientConfiguration(Connection connection, string applicationName, string applicationVersion) { 
        
            ApplicationName = applicationName; 
            ApplicationVersion = applicationVersion;
            EtpConnection = connection;
        }

        public IEtpClient CreateEtpClient() {
            return EtpConnection.CreateEtpClient(ApplicationName, ApplicationVersion);
        }
    }
}
