using Avro.Specific;
using Energistics.Etp.Common;
using Energistics.Etp.v11.Datatypes.ChannelData;
using Energistics.Etp.v11.Protocol.ChannelStreaming;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.WITSMLstudio.Connections;
using PDS.WITSMLstudio.Data.Channels;
using PDS.WITSMLstudio.Desktop.Core;
using HDS.iETP.IntegrationTestCases.LGVN.Common;
using HDS.iETP.IntegrationTestCases.LGVN.Core;
using HDS.iETP.IntegrationTestCases.LGVN.Helper;
using HDS.iETP.IntegrationTestCases.Support;
using PDS.WITSMLstudio.Desktop.Reporter;
using PDS.WITSMLstudio.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.PeerToPeer;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PDS.WITSMLstudio;
using PDS.WITSMLstudio.Desktop.Core.Adapters;
using PDS.WITSMLstudio.Desktop.Core.Models;
using Energistics.Etp.v11.Protocol.Discovery;
using Energistics.Etp.Common.Datatypes;
using System.IO;
using HAL.HDS.iETP.IntegrationTestCases.LGVN.Common;

namespace HDS.iETP.IntegrationTestCases.LGVN.Tests
{

    [TestClass]
    public class TestBase
    {
        protected TestListener2 test = new TestListener2();
        public TestContext TestContext { get; set; }

        protected string session;
        protected IEtpClient client;
        protected IEtpExtender extender;
        protected ETPClientConfiguration clientConfiguration;
        protected Connection connection;
        protected IList<EtpProtocolItem> requestedProtocol;

        protected const string secretKey = "ncUEXCuCq7TZdFjdMGtieghZPUWS8R2c";
        protected const string Uri = "wss://witsmlapptstetpx.halliburton.com/hal.witsml.host.iis/api/haletp";
        protected const string Username = "NDslSRNcW2Vkca5LCCeB5+22OmRcm6U62zzwOXyR60Y=";
        protected const string Password = "FEuZmL/puqlICBHgO+PYXA==";

        private List<DataItem> ChannelDataRecords;
        private List<GetResourcesResponse> DiscoveryDataRecords;

        private IChannelStreamingConsumer _handlerS;
        private IDiscoveryCustomer _handlerD;

        public IEnumerable<EtpProtocolItem> DefaultProtocolItems()
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

        [TestCleanup]
        public void TearDown()
        {
            test.Flush();
            //test.Remove(session);
        }

        [TestInitialize]
        public void TestSetUp()
        {
            session = Guid.NewGuid().ToString();
            test = TestListener2.CreateInstance(session);
            test.CreateTest(TestContext.TestName);
            test.Info("Namespace:" + TestContext.FullyQualifiedTestClassName + $" session[{session}]");
        }

        public void RequestETPSessionx(string testRun = "RTS-X-41-A", Connection conn = null, IList<EtpProtocolItem> protocols = null)
        {
            ChannelDataRecords = new List<DataItem>();
            DiscoveryDataRecords = new List<GetResourcesResponse>();

            var testConfig = ETPManagement.GetTestConfig();
            var testRunConfig = ETPManagement.GetTestRun(testRun);
            var applicationName = GetType().Assembly.FullName;
            var applicationVersion = GetType().GetAssemblyVersion();
            var connection = conn;
            if (connection == null)
            {
                var decryptedUsername = Encryption.DecryptString(testRunConfig.secretKey, testRunConfig.username);
                var decryptedPassword = Encryption.DecryptString(testRunConfig.secretKey, testRunConfig.password);
                connection = new Connection()
                {
                    Uri = testRunConfig.url,
                    AuthenticationType = AuthenticationTypes.Basic,
                    Username = decryptedUsername,
                    Password = decryptedPassword,
                };
            }

            var requestedProtocol = protocols;

            if (requestedProtocol == null)
            {
                requestedProtocol = testConfig.requestedProtocol;
            }

            clientConfiguration = new ETPClientConfiguration(connection, applicationName, applicationVersion);
            client = ETPClientManagement.CreateEtpClient(session, clientConfiguration);
            client.Output = LogClientOutput;
            extender = client.CreateEtpExtender(requestedProtocol);

            client.Register<IChannelStreamingConsumer, ChannelStreamingConsumerHandler>();
            _handlerS = client.Handler<IChannelStreamingConsumer>();
            _handlerS.OnChannelData += OnChannelData;
            _handlerS.OnChannelMetadata += OnChannelMetaData;

            client.Register<IDiscoveryCustomer, DiscoveryCustomerHandler>();
            _handlerD = client.Handler<IDiscoveryCustomer>();
        }

        public void RequestETPSession(string filepath = "")
        {
            ChannelDataRecords = new List<DataItem>();
            DiscoveryDataRecords = new List<GetResourcesResponse>();

            var applicationName = GetType().Assembly.FullName;
            var applicationVersion = GetType().GetAssemblyVersion();

            if (string.IsNullOrEmpty(filepath))
            {
                var decryptedUsername = Encryption.DecryptString(secretKey, Username);
                var decryptedPassword = Encryption.DecryptString(secretKey, Password);

                connection = new Connection()
                {
                    Uri = Uri,
                    AuthenticationType = AuthenticationTypes.Basic,
                    Username = decryptedUsername,
                    Password = decryptedPassword,
                };
                requestedProtocol = DefaultProtocolItems().ToList();
            }
            else
            {
                connection = JsonFileReader.ReadConnection(filepath);
                requestedProtocol = JsonFileReader.ReadProtocolList(filepath);
            }

            clientConfiguration = new ETPClientConfiguration(connection, applicationName, applicationVersion);
            client = ETPClientManagement.CreateEtpClient(session, clientConfiguration);
            client.Output = LogClientOutput;
            extender = client.CreateEtpExtender(requestedProtocol);

            client.Register<IChannelStreamingConsumer, ChannelStreamingConsumerHandler>();
            _handlerS = client.Handler<IChannelStreamingConsumer>();
            _handlerS.OnChannelData += OnChannelData;
            _handlerS.OnChannelMetadata += OnChannelMetaData;

            client.Register<IDiscoveryCustomer, DiscoveryCustomerHandler>();
            _handlerD = client.Handler<IDiscoveryCustomer>();
        }

        internal void LogClientOutput(string message)
        {
            string outputLog = Path.Combine(Constants.TEST_DEBUG_FOLDER, "response.json");
            StringHelper.LogClientOutput(message, outputLog);
        }

        protected void OnChannelData(object sender, ProtocolEventArgs<ChannelData> e)
        {
            //ChannelDataRecords = new List<DataItem>(e.Message.Data);
            ChannelDataRecords.AddRange(e.Message.Data);
            ChannelDataRecords = ChannelDataRecords.Distinct().ToList();
        }

        protected void OnGetResourcesResponse(object sender, ProtocolEventArgs<GetResourcesResponse, string> e)
        {
            DiscoveryDataRecords.Add(e.Message);

            //throw new NotImplementedException();
        }

        protected void OnChannelMetaData(object sender, ProtocolEventArgs<ChannelMetadata> e)
        {
            Console.WriteLine("Channel Meta Data");
        }

        protected async Task<List<GetResourcesResponse>> Discovery(string uri, string wellName = "", int timeOut = 10000, bool throwable = true)
        {
            _handlerD.OnGetResourcesResponse += OnGetResourcesResponse;
            var tokenSource = new CancellationTokenSource();

            var onGetChannelData = AsyncHelper.HandleAsync<GetResourcesResponse, string>(x => _handlerD.OnGetResourcesResponse += x, timeOut);
            _handlerD.GetResources(uri);
            Task taskCount = new Task(() =>
            {
                while (DiscoveryDataRecords?.Find(s => {
                    string resourceName = s?.Resource.Name;
                    if (s?.Resource.HasChildren > 0)
                        resourceName = $"{resourceName} ({s?.Resource.HasChildren})";
                    return resourceName == wellName;
                }) == null) { }
            });
            taskCount.Start();

            var completedTask = await Task.WhenAny(taskCount, Task.Delay(timeOut, tokenSource.Token));
            if (completedTask == taskCount)
            {
                tokenSource.Cancel();
                var response1 = new List<GetResourcesResponse>(DiscoveryDataRecords);
                DiscoveryDataRecords.Clear();
                return response1;
            }
            if (throwable)
                throw new TimeoutException($"[Discovery] The operation has timed out.[{timeOut}]");
            //var response = new List<DataItem>(ChannelDataRecords);
            //ChannelDataRecords.Clear();
            var response = new List<GetResourcesResponse>(DiscoveryDataRecords);
            DiscoveryDataRecords.Clear();
            return response;

        }

        protected async Task<GetResourcesResponse> DiscoveryWell(string wellPath, string separator = "->", bool throwable = true)
        {
            var paths = wellPath.Split(new string[] { separator }, StringSplitOptions.None);
            string uri = EtpUri.RootUri;
            GetResourcesResponse responseMapping = null;
            for (int i = 0; i < paths.Length; i++)
            {
                var wellName = paths[i];
                var responseD = await Discovery(uri, wellName, throwable: false);

                responseMapping = responseD.Find(s =>
                {
                    string resourceName = s.Resource.Name;
                    if (s.Resource.HasChildren > 0)
                        resourceName = $"{resourceName} ({s.Resource.HasChildren})";
                    return resourceName == wellName;
                });

                if (responseMapping == null)
                    if (throwable)
                        throw new Exception($"[DiscoveryWell] Don't see well [{wellName}] in the Resources.");
                    else
                        return null;

                uri = responseMapping?.Resource.Uri;
            }

            return responseMapping;
        }

        protected async Task<List<DataItem>> StreamingChannel(List<ChannelStreamingInfo> listChannels, int count = 1, int timeOut = 30000, bool throwable = true)
        {
            _handlerS.ChannelStreamingStart(listChannels);
            _handlerS.OnChannelData += OnChannelData;

            var tokenSource = new CancellationTokenSource();

            var onGetChannelData = AsyncHelper.HandleAsync<ChannelData>(x => _handlerS.OnChannelData += x);
            Task taskCount = new Task(() =>
            {
                while (ChannelDataRecords.Count != count) { }
            });
            taskCount.Start();

            var completedTask = await Task.WhenAny(taskCount, Task.Delay(timeOut, tokenSource.Token));
            if (completedTask == taskCount)
            {
                tokenSource.Cancel();
            }
            _handlerS.ChannelStreamingStop(listChannels.Select(x => x.ChannelId).ToList());
            if (throwable)
                throw new TimeoutException($"[StreamingChannel] The operation has timed out.[{timeOut}]");
            var response = new List<DataItem>(ChannelDataRecords);
            ChannelDataRecords.Clear();
            return response;
        }

        protected async Task<List<DataItem>> RequestRangeChannel(ChannelMetadataRecord channel, DateTime startTime, DateTime endTime, int timeOut = 30000, bool throwable = true)
        {
            return await _requestRangeChannel(channel, new DateTimeOffset(startTime).ToUnixTimeMicroseconds(), new DateTimeOffset(endTime).ToUnixTimeMicroseconds(), timeOut, throwable);
        }

        protected async Task<List<DataItem>> RequestRangeChannel(ChannelMetadataRecord channel, long startIndex, long endIndex, int timeOut = 30000, bool throwable = true)
        {
            var channelScale = channel.Indexes.FirstOrDefault()?.Scale ?? 0;
            var startIndexScaled = Convert.ToInt64((startIndex * Math.Pow(10, channelScale)));
            var endIndexScaled = Convert.ToInt64((endIndex * Math.Pow(10, channelScale)));

            return await _requestRangeChannel(channel, startIndexScaled, endIndexScaled, timeOut, throwable);
        }

        private async Task<List<DataItem>> _requestRangeChannel(ChannelMetadataRecord channel, long startIndex, long endIndex, int timeOut = 30000, bool throwable = true)
        {
            _handlerS.OnChannelData += OnChannelData;
            var channelScale = channel.Indexes.FirstOrDefault()?.Scale ?? 0;
            var channelRangeInfo = new ChannelRangeInfo
            {
                ChannelId = new[] { channel.ChannelId },
                StartIndex = startIndex,
                EndIndex = endIndex
            };

            _handlerS.ChannelRangeRequest(new[] { channelRangeInfo });

            var tokenSource = new CancellationTokenSource();

            var onGetChannelData = AsyncHelper.HandleAsync<ChannelData>(x => _handlerS.OnChannelData += x);
            Task taskCount = new Task(() =>
            {
                var lastIndex = ChannelDataRecords.Count != 0 ? ChannelDataRecords.Last().Indexes.First().IndexFromScale(channelScale) : 0;

                while (lastIndex < endIndex)
                {
                    lastIndex = ChannelDataRecords.Count != 0 ? ChannelDataRecords.Last().Indexes.First().IndexFromScale(channelScale) : 0;
                }
            });
            taskCount.Start();

            var completedTask = await Task.WhenAny(taskCount, Task.Delay(timeOut, tokenSource.Token));
            if (completedTask == taskCount)
            {
                tokenSource.Cancel();
            }
            _handlerS.ChannelStreamingStop(new[] { channel.ChannelId });
            if (throwable)
                throw new TimeoutException($"[RequestRangeChannel] The operation has timed out.[{timeOut}]");
            var response = new List<DataItem>(ChannelDataRecords);
            ChannelDataRecords.Clear();
            return response;
        }

        protected async Task<List<DataItem>> RequestRangeChannel(IList<long> channelIds, long startIndex, long endIndex, int scale = 0, int timeOut = 30000, bool throwable = true)
        {
            _handlerS.OnChannelData += OnChannelData;
            long startIndexScaled = Convert.ToDouble(startIndex).IndexToScale(scale);
            long endIndexScaled = Convert.ToDouble(endIndex).IndexToScale(scale);
            var channelRangeInfo = new ChannelRangeInfo
            {
                ChannelId = channelIds,
                StartIndex = startIndexScaled,
                EndIndex = endIndexScaled
            };

            _handlerS.ChannelRangeRequest(new[] { channelRangeInfo });

            var tokenSource = new CancellationTokenSource();

            var onGetChannelData = AsyncHelper.HandleAsync<ChannelData>(x => _handlerS.OnChannelData += x);
            Task taskCount = new Task(() =>
            {
                var lastIndex = ChannelDataRecords.Count != 0 ? ChannelDataRecords.Last().Indexes.FirstOrDefault().IndexFromScale(scale) : 0;

                while (lastIndex < endIndex)
                {
                    lastIndex = ChannelDataRecords.Count != 0 ? ChannelDataRecords.Last().Indexes.FirstOrDefault().IndexFromScale(scale) : 0;
                }
            });
            taskCount.Start();

            var completedTask = await Task.WhenAny(taskCount, Task.Delay(timeOut, tokenSource.Token));
            if (completedTask == taskCount)
            {
                tokenSource.Cancel();
                _handlerS.ChannelStreamingStop(channelIds);
            }
            if (throwable)
                throw new TimeoutException($"[RequestRangeChannel] The operation has timed out111.[{timeOut}]");
            return ChannelDataRecords;
        }

        public void StartStreamingChannel(int maxDataitems = 10000, int maxMsgRate = 1000) => _handlerS.Start(maxDataitems, maxMsgRate);

        public async Task<ChannelMetadata> DescribeWell(List<string> uris)
        {

            var onGetChannelMetaData = AsyncHelper.HandleAsync<ChannelMetadata>(x => _handlerS.OnChannelMetadata += x);
            _handlerS.ChannelDescribe(uris);
            var argsMetadata = await onGetChannelMetaData.WaitAsync();
            return argsMetadata.Message;
        }
    }
}
