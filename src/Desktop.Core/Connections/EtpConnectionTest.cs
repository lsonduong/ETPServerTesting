//----------------------------------------------------------------------- 
// PDS WITSMLstudio Desktop, 2018.1
//
// Copyright 2018 PDS Americas LLC
// 
// Licensed under the PDS Open Source WITSML Product License Agreement (the
// "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.pds.group/WITSMLstudio/OpenSource/ProductLicenseAgreement
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using PDS.WITSMLstudio.Connections;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Desktop.Core.Runtime;
using PDS.WITSMLstudio.Desktop.Core.Models;
using System.Collections.Generic;

namespace PDS.WITSMLstudio.Desktop.Core.Connections
{
    /// <summary>
    /// Provides a connection test for an Ept Connection instance.
    /// </summary>
    /// <seealso cref="PDS.WITSMLstudio.Desktop.Core.Connections.IConnectionTest" />
    [Export("Etp", typeof(IConnectionTest))]
    public class EtpConnectionTest : IConnectionTest
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(EtpConnectionTest));

        /// <summary>
        /// Initializes a new instance of the <see cref="WitsmlConnectionTest"/> class.
        /// </summary>
        /// <param name="runtime">The runtime service.</param>
        [ImportingConstructor]
        public EtpConnectionTest(IRuntimeService runtime)
        {
            Runtime = runtime;
        }

        /// <summary>
        /// Gets the runtime service.
        /// </summary>
        /// <value>The runtime service.</value>
        public IRuntimeService Runtime { get; }

        /// <summary>
        /// Determines whether this Connection instance can connect to the specified connection Uri.
        /// </summary>
        /// <param name="connection">The connection instanace being tested.</param>
        /// <returns>The boolean result from the asynchronous operation.</returns>
        public async Task<bool> CanConnect(Connection connection)
        {
            //if (connection.AuthenticationType == AuthenticationTypes.OpenId)
            //    return await CanConnectUsingOpenId(connection);

            return await CanConnectUsingAuthorization(connection);
        }

        private async Task<bool> CanConnectUsingAuthorization(Connection connection)
        {
            try
            {
                _log.Debug($"ETP connection test for {connection}");

                var applicationName = GetType().Assembly.FullName;
                var applicationVersion = GetType().GetAssemblyVersion();

                using (var client = connection.CreateEtpClient(applicationName, applicationVersion))
                {
                    var protocols = connection.CreateEtpExtender().GetProtocolItems().ToList();
                    var extender = client.CreateEtpExtender(protocols);
                    extender.Register();

                    if (!await client.OpenAsync())
                    {
                        _log.Error("Error opening web socket connection");
                        return false;
                    }

                    var count = 0;
                    while (string.IsNullOrWhiteSpace(client.SessionId) && count < 10)
                    {
                        await Task.Delay(1000);
                        count++;
                    }

                    var result = !string.IsNullOrWhiteSpace(client.SessionId);
                    _log.DebugFormat("ETP connection test {0}", result ? "passed" : "failed");

                    return result;
                }
            }
            catch (Exception ex)
            {
                _log.Error("ETP connection test failed", ex);
                return false;
            }
        }

        /// <summary>
        /// Connection instance can connect to the specified connection Uri and return result string.
        /// </summary>
        /// <param name="connection">The connection instanace being tested.</param>
        /// <returns>The boolean result from the asynchronous operation.</returns>
        public async Task<string> ConnectSession(Connection connection)
        {
            //if (connection.AuthenticationType == AuthenticationTypes.OpenId)
            //    return await CanConnectUsingOpenId(connection);

            return await ConnectRequestSession(connection);
        }

        /// <summary>
        /// Enum for Requested Protocol list
        /// </summary>
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

        private async Task<string> ConnectRequestSession(Connection connection)
        {
            try
            {
                _log.Debug($"ETP connection test for {connection}");

                var applicationName = GetType().Assembly.FullName;
                var applicationVersion = GetType().GetAssemblyVersion();
                var requestedProtocol = GetProtocolItemsTest().ToList();

                using (var client = connection.CreateEtpClient(applicationName, applicationVersion))
                {
                    //var protocols = connection.CreateEtpExtender().GetProtocolItems().ToList();
                    var extender = client.CreateEtpExtender(requestedProtocol);
                    extender.Register();

                    if (!await client.OpenAsync())
                    {
                        _log.Error("Error opening web socket connection");
                        return null;
                    }

                    var count = 0;
                    while (string.IsNullOrWhiteSpace(client.SessionId) && count < 10)
                    {
                        await Task.Delay(1000);
                        count++;
                    }

                    string result = "";
                    string header = "";

                    foreach (KeyValuePair<string, string> headers in client.Headers)
                    {
                        header = String.Format("{0}: {1}", headers.Key, headers.Value);
                        result += string.Concat(
                            header.IsJsonString() ? string.Empty : "// ",
                            header,
                            Environment.NewLine);
                    }

                    _log.DebugFormat("ETP connection test {0}", result);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _log.Error("ETP connection test failed", ex);
                return null;
            }
        }

        private async Task<bool> CanConnectUsingOpenId(Connection connection)
        {
            try
            {
                using (var source = new CancellationTokenSource())
                {
                    var discoveryUrl = new Uri(new Uri(connection.Uri), "/" + OpenIdProviderMetadataNames.Discovery);
                    var config = await OpenIdConnectConfigurationRetriever.GetAsync(discoveryUrl.ToString(), source.Token);
                    var result = false;
                    var closed = false;

                    var authEndpoint = config.AuthorizationEndpoint +
                                       "?scope=email%20profile" +
                                       "&response_type=code" +
                                       "&redirect_uri=http://localhost:" + connection.RedirectPort +
                                       "&client_id=" + WebUtility.UrlEncode(connection.ClientId);

                    await Runtime.InvokeAsync(() =>
                    {
                        var window = new NavigationWindow()
                        {
                            WindowStartupLocation = WindowStartupLocation.CenterOwner,
                            Owner = Application.Current?.MainWindow,
                            Title = "Authenticate",
                            ShowsNavigationUI = false,
                            Source = new Uri(authEndpoint),
                            Width = 500,
                            Height = 400
                        };

                        var listener = new HttpListener();
                        listener.Prefixes.Add($"http://*:{ connection.RedirectPort }/");
                        listener.Start();

                        listener.BeginGetContext(x =>
                        {
                            if (!listener.IsListening || closed) return;

                            var context = listener.EndGetContext(x);
                            var code = context.Request.QueryString["code"];

                            Runtime.Invoke(() =>
                            {
                                result = !string.IsNullOrWhiteSpace(code);
                                window.Close();
                            });
                        }, null);

                        window.Closed += (s, e) =>
                        {
                            closed = true;
                            listener.Stop();
                        };

                        window.ShowDialog();
                    });

                    _log.DebugFormat("OpenID connection test {0}", result ? "passed" : "failed");
                    return await Task.FromResult(result);
                }
            }
            catch (Exception ex)
            {
                _log.Error("OpenID connection test failed", ex);
                return await Task.FromResult(false);
            }
        }
    }
}
