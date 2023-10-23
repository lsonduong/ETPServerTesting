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
using Newtonsoft.Json;
using HAL.HDS.iETP.IntegrationTestCases.LGVN.Common;
using PDS.WITSMLstudio.Desktop.Core.Models;
using System.IO;
using HAL.HDS.iETP.IntegrationTestCases.LGVN.Core;
using System.Threading;

namespace HDS.iETP.IntegrationTestCases.LGVN.Common
{
    public static class ETPManagement
    {
        private static readonly ConcurrentDictionary<string, ETPSession> _etpSessions = new ConcurrentDictionary<string, ETPSession>();
        private static readonly ETPClientConfiguration2 clientConfig = JsonConvert.DeserializeObject<ETPClientConfiguration2>(JsonHelper.ReadFromJsonFile(Path.Combine(Constants.RESOURCE_FOLDER, "global.json")));

        //private static void Init()
        //{
        //    if (_etpSessions.Value == null)
        //    {
        //        _etpSessions.Value = new ConcurrentDictionary<string, ETPSession>();
        //    }
        //    _etpSessions.Value.TryAdd(etpSession.session, etpSession);
        //}

        private static void Init(string session)
        {
            _etpSessions.TryGetValue(session, out ETPSession _etpSession);

            if (_etpSession == null)
            {
                ETPSession etpSession = new ETPSession(session);
                _etpSessions.TryAdd(session, etpSession);
            }
        }

        public static void RemoveSession(string session)
        {
            _etpSessions.TryRemove(session, out _);
        }

        public static ETPTestRun GetTestRun(string testRun) => clientConfig.TestRuns.Find(i => i.name == testRun);
        public static ETPClientConfiguration2 GetTestConfig() => clientConfig;

        public static ETPSession GetEtpSession(string session)
        {
            _etpSessions.TryGetValue(session, out ETPSession etpSession);

            if (etpSession == null)
                throw new NullReferenceException("Etp Session is null. Please create new `ETP Session` firstly");

            return etpSession;
        }

        public static ETPSession CreateNewEtpSession(string session)
        {
            Init(session);
            _etpSessions.TryGetValue(session, out ETPSession _etpSession);
            return _etpSession;
        }

        //public static ETPSession CreateNewEtpSession()
        //{
        //    Init();
        //    return _etpSessions.Value[etpSession.session];
        //}
    }
}
