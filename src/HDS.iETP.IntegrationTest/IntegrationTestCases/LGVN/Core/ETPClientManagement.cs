using Energistics.Etp.Common;
using PDS.WITSMLstudio.Connections;
using PDS.WITSMLstudio.Desktop.Core;
using HDS.iETP.IntegrationTestCases.LGVN.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDS.iETP.IntegrationTestCases.LGVN.Common
{
    public static class ETPClientManagement
    {
        private static readonly ConcurrentDictionary<string, IEtpClient> _clients = new ConcurrentDictionary<string, IEtpClient>();

        internal static void RemoveSession(string session)
        {
            _clients.TryRemove(session, out _);
        }

        public static IEtpClient GetEtpClient(string session)
        {
            _clients.TryGetValue(session, out IEtpClient client);
            
            if (client == null)
                throw new NullReferenceException("Etp Client is null. Please call `CreateEtpClient` function first");
            
            return client;
        }

        public static IEtpClient CreateEtpClient(string session, ETPClientConfiguration config)
        {
            var client = config.CreateEtpClient();
            _clients.TryAdd(session, client);
            return client;
        }


    }
}
