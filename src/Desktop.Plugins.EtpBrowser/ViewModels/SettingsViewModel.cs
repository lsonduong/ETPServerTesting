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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using Energistics.Etp.Common;
using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.Common.Datatypes.ChannelData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PDS.WITSMLstudio.Connections;
using PDS.WITSMLstudio.Desktop.Core;
using PDS.WITSMLstudio.Desktop.Core.Commands;
using PDS.WITSMLstudio.Desktop.Core.Models;
using PDS.WITSMLstudio.Desktop.Core.Runtime;
using PDS.WITSMLstudio.Desktop.Core.ViewModels;
using PDS.WITSMLstudio.Desktop.Plugins.EtpBrowser.Models;
using PDS.WITSMLstudio.Framework;

namespace PDS.WITSMLstudio.Desktop.Plugins.EtpBrowser.ViewModels
{
    /// <summary>
    /// Manages the behavior of the settings view.
    /// </summary>
    /// <seealso cref="Caliburn.Micro.Screen" />
    public sealed class SettingsViewModel : Screen, ISessionAware
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(SettingsViewModel));

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel" /> class.
        /// </summary>
        /// <param name="runtime">The runtime service.</param>
        public SettingsViewModel(IRuntimeService runtime)
        {
            Runtime = runtime;
            DisplayName =  "Save Inputs";

            ConnectionPicker = new ConnectionPickerViewModel(runtime, ConnectionTypes.Etp)
            {
                AutoConnectEnabled = true,
                OnConnectionChanged = OnConnectionChanged
            };

            EtpProtocols = new BindableCollection<EtpProtocolItem>();
            ParamList = new BindableCollection<Parameters>();
            Channels = new BindableCollection<ChannelMetadataViewModel>();
            ToggleChannelCommand = new DelegateCommand(x => ToggleSelectedChannel());
        }

        /// <summary>
        /// Gets the collection of channel metadata.
        /// </summary>
        /// <value>The channel metadata.</value>
        public BindableCollection<ChannelMetadataViewModel> Channels { get; }

        /// <summary>
        /// Gets the toggle channel command.
        /// </summary>
        public ICommand ToggleChannelCommand { get; }

        private ChannelMetadataViewModel _selectedChannel;

        /// <summary>
        /// Gets or sets the selected channel.
        /// </summary>
        public ChannelMetadataViewModel SelectedChannel
        {
            get { return _selectedChannel; }
            set
            {
                if (ReferenceEquals(_selectedChannel, value)) return;
                _selectedChannel = value;
                NotifyOfPropertyChange(() => SelectedChannel);
            }
        }

        /// <summary>
        /// Toggles the selected channel.
        /// </summary>
        public void ToggleSelectedChannel()
        {
            if (SelectedChannel == null) return;
            SelectedChannel.IsChecked = !SelectedChannel.IsChecked;
        }

        /// <summary>
        /// Called when checkbox in ID column of channels datagrid is checked or unchecked.
        /// </summary>
        /// <param name="isSelected">if set to <c>true</c> if all channels should be selected, <c>false</c> if channels should be unselected.</param>
        public void OnChannelSelection(bool isSelected)
        {
            foreach (var channelMetadataViewModel in Channels)
            {
                channelMetadataViewModel.IsChecked = isSelected;
            }
        }

        /// <summary>
        /// Gets or Sets the Parent <see cref="T:Caliburn.Micro.IConductor" />
        /// </summary>
        public new MainViewModel Parent => (MainViewModel) base.Parent;

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>The model.</value>
        public Models.EtpSettings Model => Parent.Model;

        /// <summary>
        /// Gets the runtime service.
        /// </summary>
        /// <value>The runtime.</value>
        public IRuntimeService Runtime { get; }

        /// <summary>
        /// Gets a collection of supported ETP versions.
        /// </summary>
        public string[] SupportedVersions { get; }

        /// <summary>
        /// Gets the connection picker view model.
        /// </summary>
        /// <value>The connection picker view model.</value>
        public ConnectionPickerViewModel ConnectionPicker { get; }

        /// <summary>
        /// Gets the collection of all ETP protocols.
        /// </summary>
        /// <value>The collection of ETP protocols.</value>
        public BindableCollection<EtpProtocolItem> EtpProtocols { get; }

        public BindableCollection<Parameters> ParamList { get; }

        public bool IsRangeRequest { get; set; }

        public bool IsRandomChannel { get; set; }

        /// <summary>
        /// Adds the URI to the collection of URIs.
        /// </summary>
        public void AddUri()
        {
            var uri = Model.Streaming.Uri;

            if (string.IsNullOrWhiteSpace(uri) || Model.Streaming.Uris.Contains(uri))
                return;

            Model.Streaming.Uris.Add(uri);
            Model.Streaming.Uri = string.Empty;
        }

        /// <summary>
        /// Handles the KeyUp event for the ListBox control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        public void OnKeyUp(ListBox control, KeyEventArgs e)
        {
            var index = control.SelectedIndex;

            if (e.Key == Key.Delete && index > -1)
            {
                Model.Streaming.Uris.RemoveAt(index);
            }
        }

        private bool _canRequestSession;

        /// <summary>
        /// Gets or sets a value indicating whether the Request Session button is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if Request Session is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool CanRequestSession
        {
            get { return _canRequestSession; }
            set
            {
                if (_canRequestSession == value)
                    return;

                _canRequestSession = value;
                NotifyOfPropertyChange(() => CanRequestSession);
            }
        }

        private bool _canCloseSession;

        /// <summary>
        /// Gets or sets a value indicating whether the Close Session button is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if Close Session is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool CanCloseSession
        {
            get { return _canCloseSession; }
            set
            {
                if (_canCloseSession == value)
                    return;

                _canCloseSession = value;
                NotifyOfPropertyChange(() => CanCloseSession);
            }
        }

        private bool _canStartServer;

        /// <summary>
        /// Gets or sets a value indicating whether the Start Server button is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if Start Server is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool CanStartServer
        {
            get { return _canStartServer; }
            set
            {
                if (_canStartServer == value)
                    return;

                _canStartServer = value;
                NotifyOfPropertyChange(() => CanStartServer);
            }
        }

        private bool _canStopServer;

        /// <summary>
        /// Gets or sets a value indicating whether the Stop Server button is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if Stop Server is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool CanStopServer
        {
            get { return _canStopServer; }
            set
            {
                if (_canStopServer == value)
                    return;

                _canStopServer = value;
                NotifyOfPropertyChange(() => CanStopServer);
            }
        }

        /// <summary>
        /// Requests a new ETP session.
        /// </summary>
        public void StartServer()
        {
            Model.RequestedProtocols.Clear();
            Model.RequestedProtocols.AddRange(EtpProtocols.Where(x => x.IsSelected));
            Parent.InitEtpServer();
            CanRequestSession = false;
            CanStartServer = !Parent.SelfHostedWebServer?.IsRunning ?? true;
            CanStopServer = !CanStartServer;
        }

        /// <summary>
        /// Closes the current ETP session.
        /// </summary>
        public void StopServer()
        {
            Parent.SelfHostedWebServer?.Stop();
            CanStartServer = true;
            CanStopServer = false;
            CanCloseSession = false;
            CanRequestSession = true;
        }

        /// <summary>
        /// Requests a new ETP session.
        /// </summary>
        public async Task RequestSession()
        {
            Model.RequestedProtocols.Clear();
            Model.RequestedProtocols.AddRange(EtpProtocols.Where(x => x.IsSelected));
            await Parent.OnConnectionChanged();
            CanRequestSession = false;
        }

        public async Task RequestSessionTest(Connection connection)
        {
            await OnConnectionChanged(connection);
            CanRequestSession = false;
        }

        /// <summary>
        /// Closes the current ETP session.
        /// </summary>
        public void CloseSession()
        {
            Parent.EtpExtender?.CloseSession();
        }

        /// <summary>
        /// Retrieves the ETP versions.
        /// </summary>
        public void EtpVersions()
        {
            Task.Run(GetEtpVersions);
        }

        /// <summary>
        /// Retrieves the ETP Server's capabilities.
        /// </summary>
        public void ServerCapabilities()
        {
            Task.Run(GetServerCapabilities);
        }

        /// <summary>
        /// Requests channel metadata for the collection of URIs.
        /// </summary>
        public void Describe()
        {
            if (Parent.Session == null)
            {
                return;
            }

            //Channels.Clear();
            //ChannelStreamingInfos.Clear();

            // Verify streaming start value is not scaled


            Parent.EtpExtender.Register(
                onChannelMetadata: OnChannelMetadata,
                onChannelData: OnChannelData);

            Channels.Clear();

            Parent.EtpExtender.ChannelDescribe(Model.Streaming.Uris);
        }

        public void OnChannelMetadata(IMessageHeader header, IList<IChannelMetadataRecord> channels)
        {
            if (!channels.Any())
            {
                Parent.Details.Append(Environment.NewLine + "// No channels were described");
                return;
            }

            // add to channel metadata collection
            channels.ForEach(x =>
            {
                if (Channels.Any(c => c.Record.ChannelUri.EqualsIgnoreCase(x.ChannelUri)))
                    return;

                Channels.Add(new ChannelMetadataViewModel(x));
            });
        }

        public void OnChannelData(IMessageHeader header, IList<IDataItem> channelData)
        {
        }

        /// <summary>
        /// Add Data Parameter Row
        /// </summary>
        public void AddRow()
        {
            ParamList.Add(new Parameters());
        }

        /// <summary>
        /// Delete Data Parameter Row
        /// </summary>
        public void DeleteRow()
        {
            ParamList.RemoveAt(ParamList.Count - 1);
        }

        /// <summary>
        /// Sets the type of channel streaming.
        /// </summary>
        /// <param name="type">The type.</param>
        public void SetStreamingType(string type)
        {
            Model.Streaming.StreamingType = type;
        }
        private object GetStreamingStartValue(bool isRangeRequest = false, int scale = 4)
        {
            if (isRangeRequest && !"TimeIndex".EqualsIgnoreCase(Model.Streaming.StreamingType) && !"DepthIndex".EqualsIgnoreCase(Model.Streaming.StreamingType))
                return default(long);
            if ("LatestValue".EqualsIgnoreCase(Model.Streaming.StreamingType))
                return null;
            else if ("IndexCount".EqualsIgnoreCase(Model.Streaming.StreamingType))
                return Model.Streaming.IndexCount;

            var isTimeIndex = "TimeIndex".EqualsIgnoreCase(Model.Streaming.StreamingType);

            var startIndex = isTimeIndex
                ? new DateTimeOffset(Model.Streaming.StartTime).ToUnixTimeMicroseconds()
                : Model.Streaming.StartIndex.IndexToScale(scale);

            return startIndex;
        }

        private object GetStreamingEndValue(int scale = 4)
        {
            var isTimeIndex = "TimeIndex".EqualsIgnoreCase(Model.Streaming.StreamingType);

            if ("LatestValue".EqualsIgnoreCase(Model.Streaming.StreamingType) ||
                "IndexCount".EqualsIgnoreCase(Model.Streaming.StreamingType) ||
                (isTimeIndex && !Model.Streaming.EndTime.HasValue) ||
                (!isTimeIndex && !Model.Streaming.EndIndex.HasValue))
                return default(long);

            var endIndex = isTimeIndex
                ? new DateTimeOffset(Model.Streaming.EndTime.Value).ToUnixTimeMicroseconds()
                : ((double)Model.Streaming.EndIndex).IndexToScale(scale);

            return endIndex;
        }

        /// <summary>
        /// Save Inputs for Connection to File
        /// </summary>
        public void SaveInputs()
        {
            var outputPath = Parent.GetOutputFilePath();
            List<Parameters> paramList = ParamList
                .GroupBy(p => p.SelectedName)
                .Select(g => g.Last())
                .ToList();
            var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "Select a folder or just click OK to Save Inputs to default folder",
                SelectedPath = outputPath,
                ShowNewFolderButton = true
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                outputPath = dialog.SelectedPath;
            }
            else
            {
                return;
            }

            Model.RequestedProtocols.Clear();
            Model.RequestedProtocols.AddRange(EtpProtocols.Where(x => x.IsSelected));

            var channels = Channels
                .Where(c => c.IsChecked)
                .Select(x => x.Record.ChannelId)
                .ToArray();

            JsonHelper.WriteToJsonFile(outputPath + "\\protocols.json",Model.RequestedProtocols);
            Parent.WriteConnectionInfo(outputPath);
            JsonHelper.WriteToJsonFile(outputPath + "\\testcaseParams.json", paramList);
            JsonHelper.WriteToJsonFile(outputPath + "\\uris.json", Model.Streaming.Uris);
            JsonHelper.WriteToJsonFile(outputPath + "\\isRangeRequest.json", IsRangeRequest);
            JsonHelper.WriteToJsonFile(outputPath + "\\startIndex.json", GetStreamingStartValue(false));

            if (IsRangeRequest)
            {
                JsonHelper.WriteToJsonFile(outputPath + "\\endIndex.json", GetStreamingEndValue());
            }

            if (IsRandomChannel)
            {
                JsonHelper.WriteToJsonFile(outputPath + "\\isRandomChannel.json", IsRandomChannel);
            } else
            {
                JsonHelper.WriteToJsonFile(outputPath + "\\channels.json", channels);
            }

            MessageBox.Show("Save info successfully to " + outputPath);
        }

        /// <summary>
        /// Clears the selected protocols.
        /// </summary>
        public void ClearSelectedProtocols()
        {
            foreach (var protocol in EtpProtocols)
            {
                protocol.IsSelected = false;
            }
        }

        /// <summary>
        /// Called when the selected connection has changed.
        /// </summary>
        /// <param name="connection">The connection.</param>
        void ISessionAware.OnConnectionChanged(Connection connection)
        {
            // Nothing to do here as the connection change was initiated on this tab.
        }

        public void OnSessionOpened(IList<ISupportedProtocol> supportedProtocols)
        {
            CanRequestSession = false;
            CanCloseSession = true;
            CanStartServer = false;
            CanStopServer = Parent.SelfHostedWebServer?.IsRunning ?? false;
        }

        public void OnSocketClosed()
        {
            CanCloseSession = false;
            CanStartServer = !Parent.SelfHostedWebServer?.IsRunning ?? true;
            CanStopServer = !CanStartServer;
            CanRequestSession = CanStartServer;
        }

        public async Task OnConnectionChanged(Connection connection)
        {
            Model.Connection = connection;
            Model.Connection.SetServerCertificateValidation();
            await Parent.OnConnectionChanged(false);
            CanStartServer = !Parent.SelfHostedWebServer?.IsRunning ?? true;
            CanStopServer = !CanStartServer;
            CanRequestSession = !CanStopServer;
            CanCloseSession = false;

            var protocols = Parent.EtpExtender ??
                            connection.CreateEtpExtender();

            await Runtime.InvokeAsync(() =>
            {
                EtpProtocols.Clear();
                EtpProtocols.AddRange(protocols.GetProtocolItems());
            });
        }

        private Task<bool> GetEtpVersions()
        {
            if (!Model.Connection.Uri.ToLowerInvariant().StartsWith("ws"))
                return Task.FromResult(false);

            try
            {
                Runtime.ShowBusy();

                var versions = Model.Connection.GetEtpVersions();

                Parent.LogDetailMessage(
                    "ETP Versions:",
                    EtpExtensions.Serialize(versions, true));

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _log.Warn("Error getting ETP versions", ex);
                Parent.LogClientError("Error getting ETP versions:", ex);
                return Task.FromResult(false);
            }
            finally
            {
                Runtime.ShowBusy(false);
            }
        }

        private Task<bool> GetServerCapabilities()
        {
            if (!Model.Connection.Uri.ToLowerInvariant().StartsWith("ws"))
                return Task.FromResult(false);

            try
            {
                Runtime.ShowBusy();
                
                var capabilities = Model.Connection.GetEtpServerCapabilities();

                Parent.LogDetailMessage(
                    "Server Capabilites:",
                    EtpExtensions.Serialize(capabilities, true));

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _log.Warn("Error getting server capabilities", ex);
                Parent.LogClientError("Error getting server capabilities:", ex);
                return Task.FromResult(false);
            }
            finally
            {
                Runtime.ShowBusy(false);
            }
        }
    }
}
