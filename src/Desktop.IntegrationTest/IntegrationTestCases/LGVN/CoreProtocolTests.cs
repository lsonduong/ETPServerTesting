using Avro.Specific;
using Caliburn.Micro;
using Energistics.Etp;
using Energistics.Etp.Common;
using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.Security;
using Energistics.Etp.v11.Protocol.ChannelStreaming;
using Energistics.Etp.v11.Protocol.Core;
using Energistics.Etp.v11.Protocol.Discovery;
using Energistics.Etp.v11.Protocol.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PDS.WITSMLstudio.Connections;
using PDS.WITSMLstudio.Desktop.Core;
using PDS.WITSMLstudio.Desktop.Core.Adapters;
using PDS.WITSMLstudio.Desktop.Core.Models;
using PDS.WITSMLstudio.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PDS.WITSMLstudio.Desktop.IntegrationTestCases.LGVN
{
    [TestClass]
    public class CoreProtocolTests
    {
        private const string Uri = "wss://witsmlapptstetpx.halliburton.com/hal.witsml.host.iis/api/haletp";
        private const string AppName = "EtpClientTests";
        private const string AppVersion = "1.0.0.0";
        protected const string Username = "hai.vu3@halliburton.com";
        protected const string Password = "KhongCho@345";
        protected IEtpClient client;
        protected IEtpExtender extender;
        protected DateTimeOffset _dateReceived;
        protected string _ETPMessage;
            protected async Task<ProtocolEventArgs<T, TContext>> HandleAsync<T, TContext>(
            Action<ProtocolEventHandler<T, TContext>> action, int milliseconds = 5000)
            where T : ISpecificRecord
        {
            ProtocolEventArgs<T, TContext> args = null;
            var task = new Task<ProtocolEventArgs<T, TContext>>(() => args);

            action((s, e) =>
            {
                args = e;

                if (task.Status == TaskStatus.Created)
                    task.Start();
            });

            return await task.WaitAsync(milliseconds);
        }

        protected async Task<ProtocolEventArgs<T>> HandleAsync<T>(Action<ProtocolEventHandler<T>> action, int milliseconds = 5000)
            where T : ISpecificRecord
        {
            ProtocolEventArgs<T> args = null;
            var task = new Task<ProtocolEventArgs<T>>(() => args);

            action((s, e) =>
            {
                args = e;

                if (task.Status == TaskStatus.Created)
                    task.Start();
            });
            return await task.WaitAsync(milliseconds);
        }

        public IEnumerable<EtpProtocolItem> GetProtocolItemsTest()
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

        [TestInitialize]
        public void TestSetUp()
        {
            //var version = GetType().Assembly.GetName().Version.ToString();
            //var auth = Authorization.Basic("hai.vu3@halliburton.com", "KhongCho@345");
            //client = EtpFactory.CreateClient(Uri, AppName, AppVersion, EtpSettings.EtpSubProtocolName, auth);

            var applicationName = GetType().Assembly.FullName;
            var applicationVersion = GetType().GetAssemblyVersion();
            var requestedProtocol = GetProtocolItemsTest().ToList();
            var connection = new Connection()
            {
                Uri = Uri,
                AuthenticationType = AuthenticationTypes.Basic,
                Username = Username,
                Password = Password,
            };
            client = connection.CreateEtpClient(applicationName, applicationVersion);
            client.Register<IChannelStreamingConsumer, ChannelStreamingConsumerHandler>();
            client.Register<IDiscoveryCustomer, DiscoveryCustomerHandler>();
            client.Register<IStoreCustomer, StoreCustomerHandler>();
            extender = client.CreateEtpExtender(requestedProtocol);
            client.Output = LogClientOutput;
        }
        internal void LogClientOutput(string message)
        {
            LogClientOutput(message, false);
        }

        /// <summary>
        /// Logs the client output.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logDetails">if set to <c>true</c> logs the detail message.</param>
        internal void LogClientOutput(string message, bool logDetails)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            //if (logDetails)
            //    LogDetailMessage(message);

            try
            {
                if (message.IsJsonString())
                {
                    message = FormatMessage(message);
                }
                else
                {
                    const string receivedText = "Message received at ";
                    var index = message.IndexOf(receivedText, StringComparison.InvariantCultureIgnoreCase);
                    if (index != -1)
                    {
                        if (DateTimeOffset.TryParse(message.Substring(index + receivedText.Length).Trim(), out _dateReceived))
                        {
                            _dateReceived = _dateReceived.ToUniversalTime();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //_log.Warn($"Error formatting ETP message:{Environment.NewLine}{message}", ex);
            }

            _ETPMessage = string.Concat(
                message.IsJsonString() ? string.Empty : "// ",
                message,
                Environment.NewLine);
        }
        private string FormatMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return string.Empty;

            var jObject = JObject.Parse(message);

            //FormatResource(jObject["resource"] as JObject);
            //FormatDataObject(jObject["dataObject"] as JObject);
            //FormatChannelMetadataRecords(jObject["channels"] as JArray);
            //FormatChannelRangeRequests(jObject["channelRanges"] as JArray);
            //FormatChannelData(jObject["data"] as JArray);

            return jObject["protocol"] != null
                ? jObject.ToString(Formatting.None)
                : jObject.ToString();
        }
        [TestMethod]
        [Description("EtpClient connects to web socket server")]
        public async Task EtpClient_v11_Open_Connects_To_WebSocket_Server()
        {
            var handler = client.Handler<ICoreClient>();
            var onChannelData = HandleAsync<OpenSession>(x =>
            {
                handler.OnOpenSession += x;
            });
            var result = await client.OpenAsync();
            var args = await onChannelData.WaitAsync();
            var message = args.Message;
            Assert.IsTrue(result, "EtpClient connection not opened");
        }

        [TestMethod]
        [Description("Discovery")]
        public async Task Discovery()
        {
            var handler = client.Handler<ICoreClient>();
            var handlerD = client.Handler<IDiscoveryCustomer>();

            var onChannelData = HandleAsync<OpenSession>(x => handler.OnOpenSession += x);
            var onGetRootResourcesResponse = HandleAsync<GetResourcesResponse, string>(x => handlerD.OnGetResourcesResponse += x);
            
            var result = await client.OpenAsync();
            var args = await onChannelData.WaitAsync();
            var message = args.Message;


            handlerD.GetResources(EtpUri.RootUri);
            var argsRoot = await onGetRootResourcesResponse.WaitAsync();

            var onGetChildResourcesResponse = HandleAsync<GetResourcesResponse, string>(x => handlerD.OnGetResourcesResponse += x);
            handlerD.GetResources(argsRoot.Message.Resource.Uri);
            var argsChild = await onGetChildResourcesResponse.WaitAsync();
            Assert.IsTrue(result, "EtpClient connection not opened");
        }

        [TestMethod]
        [Description("Streaming")]
        public async Task Streaming()
        {
            var handler = client.Handler<ICoreClient>();
            var handlerD = client.Handler<IDiscoveryCustomer>();
            var handlerS1 = client.Handler<IChannelStreamingConsumer>();

            var onChannelData = HandleAsync<OpenSession>(x => handler.OnOpenSession += x);
            var onGetRootResourcesResponse = HandleAsync<GetResourcesResponse, string>(x => handlerD.OnGetResourcesResponse += x, 30000);

            var result = await client.OpenAsync();
            var args = await onChannelData.WaitAsync();
            var message = args.Message;


            handlerD.GetResources(EtpUri.RootUri);
            var argsRoot = await onGetRootResourcesResponse.WaitAsync();

            var onGetChildResourcesResponse = HandleAsync<GetResourcesResponse, string>(x => handlerD.OnGetResourcesResponse += x);
            handlerD.GetResources(argsRoot.Message.Resource.Uri);
            var argsChild = await onGetChildResourcesResponse.WaitAsync();

            
            var handlerS = client.Handler<IChannelStreamingConsumer>();
            var onGetChannelMetaData = HandleAsync<ChannelMetadata>(x => handlerS.OnChannelMetadata += x, 50000);
            //var onGetChannelData = HandleAsync<ChannelData>(x => handlerS.OnChannelData += x);
            //var onChannelStatus = HandleAsync<ChannelStatusChange>(x => handlerS.OnChannelStatusChange += x);
            //var onGetChannelDataChange = HandleAsync<ChannelDataChange>(x => handlerS.OnChannelDataChange += x);
            handlerS.Start();
            var uris = new List<string>();
            uris.Add("eml://witsml14/well(b6e41495-8b5f-4a87-927b-eed03cc60e46)");
            //uris.Add(argsChild.Message.Resource.Uri);
            
            extender.ChannelDescribe(uris);
            
            //var argsMetadata1 = await onChannelStatus.WaitAsync(60000);
            var argsMetadata = await onGetChannelMetaData.WaitAsync(milliseconds:70000);

            var a = argsMetadata.Message.Channels;
            //var argsData = await onGetChannelData.WaitAsync();

        }
    }
}
