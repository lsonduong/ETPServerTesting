using Energistics.Etp.Common;
using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.v11.Datatypes.ChannelData;
using Energistics.Etp.v11.Protocol.ChannelStreaming;
using Energistics.Etp.v11.Protocol.Discovery;
using HAL.HDS.iETP.IntegrationTestCases.LGVN.Common;
using HAL.HDS.iETP.IntegrationTestCases.LGVN.Helper;
using HDS.iETP.IntegrationTestCases.LGVN.Common;
using HDS.iETP.IntegrationTestCases.LGVN.Helper;
using HDS.iETP.IntegrationTestCases.Support;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal;
using PDS.WITSMLstudio;
using PDS.WITSMLstudio.Connections;
using PDS.WITSMLstudio.Data.Channels;
using PDS.WITSMLstudio.Desktop.Core;
using PDS.WITSMLstudio.Desktop.Core.Adapters;
using PDS.WITSMLstudio.Desktop.Core.Models;
using PDS.WITSMLstudio.Desktop.Reporter;
using PDS.WITSMLstudio.Framework;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.PeerToPeer;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HAL.HDS.iETP.IntegrationTestCases.LGVN.Core
{
    public class ETPSession
    {
        public string session;
        public string clientOutputFolder;
        public IEtpClient client;
        public IEtpExtender extender;
        public List<DataItem> ChannelDataRecords;
        public List<DataItem> ChannelMetaDataRecords;
        public List<GetResourcesResponse> DiscoveryDataRecords;
        public List<string> _logs;

        private string outputLog;
        private IChannelStreamingConsumer _handlerS;
        private IDiscoveryCustomer _handlerD;

        public ETPSession() => _init(Guid.NewGuid().ToString());
        public ETPSession(string session) => _init(session);


        private void _init(string session)
        {
            Console.WriteLine($"Initiating new ETPSEssion... [{session}]");
            this.session = session;
            ChannelDataRecords = new List<DataItem>();
            ChannelMetaDataRecords = new List<DataItem>();
            DiscoveryDataRecords = new List<GetResourcesResponse>();
            _logs = new List<string>();
            clientOutputFolder = Path.Combine(Constants.TEST_RESULTS_FOLDER, $"{session}{DateTime.Now:_MMddyyyy_hhmmtt}", "response.json");
        }

        public async Task CloseSession()
        {
            Console.WriteLine($"Closing session... [{session}]");
            await client.CloseAsync("");
            ETPManagement.RemoveSession(session);
            //Write logs;
            var directoryPath = Path.GetDirectoryName(outputLog);
            if (!System.IO.Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Creating directory [{directoryPath}]");
                System.IO.Directory.CreateDirectory(directoryPath);
            }
            Console.WriteLine($"Writing logs... [{outputLog}] [{client.IsOpen}]");
            client.Output = null;
            File.WriteAllLines(outputLog, _logs);
        }

        public async Task<bool> RequestSession(string testRun = "RTS-X-41-A", Connection conn = null, IList<EtpProtocolItem> protocols = null)
        {
            Console.WriteLine($"Request new ETP session testRun[{testRun}]");
            var testConfig = ETPManagement.GetTestConfig();
            Console.WriteLine($"testConfig [{testConfig.ToString()}]");
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

            client = connection.CreateEtpClient(applicationName, applicationVersion);
            outputLog = Path.Combine(TestListener2.reportDirectory, session, "response.json");
            client.Output = LogClientOutput3;
            extender = client.CreateEtpExtender(requestedProtocol);

            client.Register<IChannelStreamingConsumer, ChannelStreamingConsumerHandler>();
            _handlerS = client.Handler<IChannelStreamingConsumer>();
            _handlerS.OnChannelData += OnChannelData;
            _handlerS.OnChannelMetadata += OnChannelMetaData;

            client.Register<IDiscoveryCustomer, DiscoveryCustomerHandler>();
            _handlerD = client.Handler<IDiscoveryCustomer>();
            _handlerD.OnGetResourcesResponse += OnGetResourcesResponse;

            return await client.OpenAsync();
        }

        public async Task<bool> RequestSessionFromFileInputs(string filePath = "")
        {
            var connection = JsonFileReader.ReadConnection(filePath);
            var protocols = JsonFileReader.ReadProtocolList(filePath);
            return await RequestSession("", connection, protocols);
        }

        #region Discovery

        public async Task<List<GetResourcesResponse>> Discovery(string uri, string wellName = "", int timeOut = 10000, bool throwable = true)
        {
            //var handlerD = client.Handler<IDiscoveryCustomer>();
            //handlerD.OnGetResourcesResponse += OnGetResourcesResponse;
            var tokenSource = new CancellationTokenSource();

            //var onGetChannelData = AsyncHelper.HandleAsync<GetResourcesResponse, string>(x => _handlerD.OnGetResourcesResponse += x, timeOut);
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

        public async Task<GetResourcesResponse> DiscoveryWell(string wellPath, string separator = "->", bool throwable = true)
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

        #endregion

        #region Streaming
        public void StartStreamingChannel(int maxDataitems = 10000, int maxMsgRate = 1000) => _handlerS.Start(maxDataitems, maxMsgRate);

        public async Task<ChannelMetadata> DescribeWell(params string[] uris)
        {

            var onGetChannelMetaData = AsyncHelper.HandleAsync<ChannelMetadata>(x => _handlerS.OnChannelMetadata += x);
            _handlerS.ChannelDescribe(uris);
            var argsMetadata = await onGetChannelMetaData.WaitAsync();
            return argsMetadata.Message;
        }

        public async Task<List<DataItem>> StreamingChannel(List<ChannelStreamingInfo> listChannels, int count = 1, int timeOut = 30000, bool throwable = true)
        {
            //var handler = client.Handler<IChannelStreamingConsumer>();
            _handlerS.ChannelStreamingStart(listChannels);
            //handler.OnChannelData += OnChannelData;

            var tokenSource = new CancellationTokenSource();

            //var onGetChannelData = AsyncHelper.HandleAsync<ChannelData>(x => _handlerS.OnChannelData += x);
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
        #endregion

        #region RequestRangeChannel
        public async Task<List<DataItem>> RequestRangeChannel(ChannelMetadataRecord channel, DateTime startTime, DateTime endTime, int timeOut = 30000, bool throwable = true)
        {
            return await _requestRangeChannel(channel, new DateTimeOffset(startTime).ToUnixTimeMicroseconds(), new DateTimeOffset(endTime).ToUnixTimeMicroseconds(), timeOut, throwable);
        }

        public async Task<List<DataItem>> RequestRangeChannel(ChannelMetadataRecord channel, long startIndex, long endIndex, int timeOut = 30000, bool throwable = true)
        {
            var channelScale = channel.Indexes.FirstOrDefault()?.Scale ?? 0;
            var startIndexScaled = Convert.ToInt64((startIndex * Math.Pow(10, channelScale)));
            var endIndexScaled = Convert.ToInt64((endIndex * Math.Pow(10, channelScale)));

            return await _requestRangeChannel(channel, startIndexScaled, endIndexScaled, timeOut, throwable);
        }

        public async Task<List<DataItem>> RequestRangeChannel(IList<long> channelIds, long startIndex, long endIndex, int scale = 0, int timeOut = 30000, bool throwable = true)
        {
            //var handler = client.Handler<IChannelStreamingConsumer>();
            //handler.OnChannelData += OnChannelData;

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

        private async Task<List<DataItem>> _requestRangeChannel(ChannelMetadataRecord channel, long startIndex, long endIndex, int timeOut = 30000, bool throwable = true)
        {
            //var handler = client.Handler<IChannelStreamingConsumer>();
            //handler.OnChannelData += OnChannelData;

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
        #endregion

        public async Task FileWriteAsync(string filePath, string messaage, bool append = true)
        {
            Console.WriteLine($"Writing to file [{filePath}]");
            using (FileStream stream = new FileStream(filePath, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            using (StreamWriter sw = new StreamWriter(stream))
            {
                await sw.WriteLineAsync(messaage);
            }
        }

        internal void LogClientOutput(string message)
        {
            string outputLoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            + "\\ETPTesting\\Debug\\outputs" + DateTime.Now.ToString("_MMddyyyy_hhmmtt") + "\\response.json";
            StringHelper.LogClientOutput(message, outputLoc);
            //lock (fileWriteLock)
            //{
            //    //StringHelper.LogClientOutput(message, outputLog);
            //}
        }

        internal async void LogClientOutput3(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            try
            {
                if (message.IsJsonString())
                    message = StringHelper.FormatMessage(message);
                else
                {
                    const string receivedText = "Message received at ";
                    var index = message.IndexOf(receivedText, StringComparison.InvariantCultureIgnoreCase);
                    if (index != -1)
                        if (DateTimeOffset.TryParse(message.Substring(index + receivedText.Length).Trim(), out DateTimeOffset _dateReceived))
                            _dateReceived = _dateReceived.ToUniversalTime();
                }

                if (string.IsNullOrEmpty(outputLog))
                    Console.WriteLine(message);
                _logs.Add(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        internal async void LogClientOutput2(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            try
            {
                if (message.IsJsonString())
                    message = StringHelper.FormatMessage(message);
                else
                {
                    const string receivedText = "Message received at ";
                    var index = message.IndexOf(receivedText, StringComparison.InvariantCultureIgnoreCase);
                    if (index != -1)
                        if (DateTimeOffset.TryParse(message.Substring(index + receivedText.Length).Trim(), out DateTimeOffset _dateReceived))
                            _dateReceived = _dateReceived.ToUniversalTime();
                }

                if (string.IsNullOrEmpty(outputLog))
                    Console.WriteLine(message);
                else
                {
                    var directoryPath = Path.GetDirectoryName(outputLog);
                    if (!System.IO.Directory.Exists(directoryPath))
                    {
                        Console.WriteLine($"Creating directory [{directoryPath}]");
                        System.IO.Directory.CreateDirectory(directoryPath);
                    }

                    //File.AppendAllText(filePath, message + Environment.NewLine);
                    await FileWriteAsync(outputLog, message + Environment.NewLine);
                    //using (var writer = File.AppendText(outputLog))
                    //{
                    //    writer.Write(message + Environment.NewLine);
                    //    writer.Flush();
                    //    ((FileStream)writer.BaseStream).Flush(true);
                    //}

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        protected void OnChannelData(object sender, ProtocolEventArgs<ChannelData> e)
        {
            ChannelDataRecords.AddRange(e.Message.Data);
            ChannelDataRecords = ChannelDataRecords.Distinct().ToList();
        }

        protected void OnGetResourcesResponse(object sender, ProtocolEventArgs<GetResourcesResponse, string> e)
        {
            DiscoveryDataRecords.Add(e.Message);
        }
        protected void OnChannelMetaData(object sender, ProtocolEventArgs<ChannelMetadata> e)
        {
            Console.WriteLine("Channel Meta Data");
        }
    }
}
