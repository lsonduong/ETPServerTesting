using Avro.Specific;
using Energistics.Etp.Common;
using Energistics.Etp.v11.Datatypes.ChannelData;
using Energistics.Etp.v11.Protocol.ChannelStreaming;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.WITSMLstudio.Connections;
using PDS.WITSMLstudio.Data.Channels;
using PDS.WITSMLstudio.Desktop.Core;
using PDS.WITSMLstudio.Desktop.IntegrationTestCases.LGVN.Common;
using PDS.WITSMLstudio.Desktop.IntegrationTestCases.LGVN.Core;
using PDS.WITSMLstudio.Desktop.IntegrationTestCases.LGVN.Helper;
using PDS.WITSMLstudio.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.PeerToPeer;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PDS.WITSMLstudio.Desktop.IntegrationTestCases.LGVN.Tests
{

    [TestClass]
    public class TestBase
    {
        protected string session;
        protected IEtpClient client;
        protected ETPClientConfiguration clientConfiguration;
        private const string Uri = "wss://witsmlapptstetpx.halliburton.com/hal.witsml.host.iis/api/haletp";
        protected const string Username = "hai.vu3@halliburton.com";
        protected const string Password = "KhongCho@345";

        private List<DataItem> ChannelDataRecords;

        [TestInitialize]
        public void TestSetUp()
        {
            session = Guid.NewGuid().ToString();
            ChannelDataRecords = new List<DataItem>();

            var connection = new Connection()
            {
                Uri = Uri,
                AuthenticationType = AuthenticationTypes.Basic,
                Username = Username,
                Password = Password,
            };
            clientConfiguration = new ETPClientConfiguration(connection, GetType().Assembly.FullName, GetType().GetAssemblyVersion());
            client = ETPClientManagement.CreateEtpClient(session, clientConfiguration);
            client.Output = LogClientOutput;
        }

        internal void LogClientOutput(string message)
        {
            string outputLoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                + "\\ETPTesting\\outputs" + DateTime.Now.ToString("_MMddyyyy_hhmmtt") + "\\response.json";
            StringHelper.LogClientOutput(message, outputLoc);
        }

        protected void OnChannelData(object sender, ProtocolEventArgs<ChannelData> e)
        {
            //ChannelDataRecords = new List<DataItem>(e.Message.Data);
            ChannelDataRecords.AddRange(e.Message.Data);
            ChannelDataRecords = ChannelDataRecords.Distinct().ToList();
        }
        protected void OnChannelMetaData(object sender, ProtocolEventArgs<ChannelMetadata> e)
        {
            Console.WriteLine("Channel Meta Data");
        }

        protected async Task<List<DataItem>> StreamingChannel(List<ChannelStreamingInfo> listChannels, int count = 1, int timeOut = 30000, bool throwable = true)
        {
            var handler = client.Handler<IChannelStreamingConsumer>();
            handler.ChannelStreamingStart(listChannels);
            handler.OnChannelData += OnChannelData;

            var tokenSource = new CancellationTokenSource();

            var onGetChannelData = AsyncHelper.HandleAsync<ChannelData>(x => handler.OnChannelData += x);
            Task taskCount = new Task(() =>
            {
                while (ChannelDataRecords.Count != count) { }
            });
            taskCount.Start();

            var completedTask = await Task.WhenAny(taskCount, Task.Delay(timeOut, tokenSource.Token));
            if (completedTask == taskCount)
            {
                tokenSource.Cancel();
                handler.ChannelStreamingStop(listChannels.Select(x => x.ChannelId).ToList());
            }
            if (throwable)
                throw new TimeoutException($"[StreamingChannel] The operation has timed out.[{timeOut}]");
            var response = new List<DataItem>(ChannelDataRecords);
            ChannelDataRecords.Clear();
            return response;
        }

        protected async Task<List<DataItem>> RequestRangeChannel(ChannelMetadataRecord channel, DateTime startTime, DateTime endTime, int timeOut = 30000, bool throwable = true)
        {
            return await RequestRangeChannel(channel, new DateTimeOffset(startTime).ToUnixTimeMicroseconds(), new DateTimeOffset(endTime).ToUnixTimeMicroseconds(), timeOut, throwable);
        }

        protected async Task<List<DataItem>> RequestRangeChannel(ChannelMetadataRecord channel, long startIndex, long endIndex, int timeOut = 30000, bool throwable = true)
        {
            var handler = client.Handler<IChannelStreamingConsumer>();
            handler.OnChannelData += OnChannelData;
            var channelScale = channel.Indexes.FirstOrDefault()?.Scale ?? 0;
            var channelRangeInfo = new ChannelRangeInfo
            {
                ChannelId = new[] { channel.ChannelId },
                StartIndex = startIndex,
                EndIndex = endIndex
            };

            handler.ChannelRangeRequest(new[] { channelRangeInfo });
            
            var tokenSource = new CancellationTokenSource();

            var onGetChannelData = AsyncHelper.HandleAsync<ChannelData>(x => handler.OnChannelData += x);
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
            handler.ChannelStreamingStop(new[] { channel.ChannelId });
            if (throwable)
                throw new TimeoutException($"[RequestRangeChannel] The operation has timed out.[{timeOut}]");
            var response = new List<DataItem>(ChannelDataRecords);
            ChannelDataRecords.Clear();
            return response;
        }

        //private object GetStreamingStartValue(bool isRangeRequest = false, string type = "Lastet Value")
        //{
        //    if (isRangeRequest && !"Lastet Value".EqualsIgnoreCase(type))
        //        //if (isRangeRequest && !"TimeIndex".EqualsIgnoreCase(Model.Streaming.StreamingType) && !"DepthIndex".EqualsIgnoreCase(Model.Streaming.StreamingType))
        //        return default(long);
        //    if ("LatestValue".EqualsIgnoreCase(type))
        //        return null;
        //    else if ("IndexCount".EqualsIgnoreCase(type))
        //        return 1;

        //    var isTimeIndex = "TimeIndex".EqualsIgnoreCase(type);
        //    var scale = Convert.ToInt64(1 * Math.Pow(10, 3));
        //    var startIndex = isTimeIndex ? DateTimeOffset.Now.ToUnixTimeMicroseconds() : scale;
        //    //? new DateTimeOffset(Model.Streaming.StartTime).ToUnixTimeMicroseconds()

        //    return startIndex;
        //}
    }
}
