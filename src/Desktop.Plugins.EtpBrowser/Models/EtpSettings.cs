﻿//----------------------------------------------------------------------- 
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
using System.Runtime.Serialization;
using Caliburn.Micro;
using Energistics.Etp.Common.Datatypes;
using PDS.WITSMLstudio.Connections;
using PDS.WITSMLstudio.Desktop.Core.Adapters;
using PDS.WITSMLstudio.Desktop.Core.Models;
using PDS.WITSMLstudio.Desktop.Plugins.EtpBrowser.Properties;
using PDS.WITSMLstudio.Framework;

namespace PDS.WITSMLstudio.Desktop.Plugins.EtpBrowser.Models
{
    /// <summary>
    /// Defines all of the properties needed to comunicate via ETP.
    /// </summary>
    /// <seealso cref="Caliburn.Micro.PropertyChangedBase" />
    [DataContract]
    public class EtpSettings : PropertyChangedBase
    {
        private static readonly int _defaultMaxDataItems = Settings.Default.ChannelStreamingDefaultMaxDataItems;
        private static readonly int _defaultMaxMessageRate = Settings.Default.ChannelStreamingDefaultMaxMessageRate;

        /// <summary>
        /// Initializes a new instance of the <see cref="EtpSettings"/> class.
        /// </summary>
        public EtpSettings()
        {
            Connection = new Connection();
            Streaming = new StreamingSettings
            {
                MaxDataItems = _defaultMaxDataItems,
                MaxMessageRate = _defaultMaxMessageRate,
                StreamingType = "LatestValue",
                StartTime = DateTimeOffset.Now.TruncateToSeconds().UtcDateTime,
                StartIndex = 0,
                IndexCount = 10
            };
            DiscoveryFunction = Functions.GetResources;
            Store = new StoreSettings();
            StoreFunction = Functions.GetObject;
            StoreNotification = new StoreNotificationSettings();
            GrowingObject = new GrowingObjectSettings();
            GrowingObjectFunction = Functions.GetPart;
            DataLoad = new DataLoadSettings();
            RequestedProtocols = new BindableCollection<EtpProtocolItem>();
            BaseUri = EtpUri.RootUri;
            IsEtpClient = true;
            DecodeByteArrays = true;
            PortNumber = 9000;
            DiscoveryDepth = 1;
        }

        private Connection _connection;
        /// <summary>
        /// Gets or sets the connection.
        /// </summary>
        /// <value>The connection.</value>
        [DataMember]
        public Connection Connection
        {
            get { return _connection; }
            set
            {
                if (!ReferenceEquals(_connection, value))
                {
                    _connection = value;
                    NotifyOfPropertyChange(() => Connection);
                }
            }
        }

        private StreamingSettings _streaming;
        /// <summary>
        /// Gets or sets the Channel Streaming settings.
        /// </summary>
        /// <value>The Channel Streaming settings.</value>
        [DataMember]
        public StreamingSettings Streaming
        {
            get { return _streaming; }
            set
            {
                if (!ReferenceEquals(_streaming, value))
                {
                    _streaming = value;
                    NotifyOfPropertyChange(() => Streaming);
                }
            }
        }

        private StoreSettings _store;
        /// <summary>
        /// Gets or sets the Store settings.
        /// </summary>
        /// <value>The Store settings.</value>
        [DataMember]
        public StoreSettings Store
        {
            get { return _store; }
            set
            {
                if (!ReferenceEquals(_store, value))
                {
                    _store = value;
                    NotifyOfPropertyChange(() => Store);
                }
            }
        }

        private StoreNotificationSettings _storeNotification;
        /// <summary>
        /// Gets or sets the Store Notification settings.
        /// </summary>
        /// <value>The Store Notification settings.</value>
        [DataMember]
        public StoreNotificationSettings StoreNotification
        {
            get { return _storeNotification; }
            set
            {
                if (!ReferenceEquals(_storeNotification, value))
                {
                    _storeNotification = value;
                    NotifyOfPropertyChange(() => StoreNotification);
                }
            }
        }

        private GrowingObjectSettings _growingObject;
        /// <summary>
        /// Gets or sets the Growing Object settings.
        /// </summary>
        /// <value>The Growing Object settings.</value>
        [DataMember]
        public GrowingObjectSettings GrowingObject
        {
            get { return _growingObject; }
            set
            {
                if (!ReferenceEquals(_growingObject, value))
                {
                    _growingObject = value;
                    NotifyOfPropertyChange(() => GrowingObject);
                }
            }
        }

        private DataLoadSettings _dataLoad;
        /// <summary>
        /// Gets or sets the Data Load settings.
        /// </summary>
        /// <value>The Data Load settings.</value>
        [DataMember]
        public DataLoadSettings DataLoad
        {
            get { return _dataLoad; }
            set
            {
                if (!ReferenceEquals(_dataLoad, value))
                {
                    _dataLoad = value;
                    NotifyOfPropertyChange(() => DataLoad);
                }
            }
        }

        private bool _isEtpClient;
        /// <summary>
        /// Gets or sets a value indicating whether the ETP client role is selected.
        /// </summary>
        [DataMember]
        public bool IsEtpClient
        {
            get { return _isEtpClient; }
            set
            {
                if (_isEtpClient == value) return;
                _isEtpClient = value;
                NotifyOfPropertyChange(() => IsEtpClient);
            }
        }

        private bool _decodeByteArrays;
        /// <summary>
        /// Gets or sets a value indicating whether to decode byte arrays.
        /// </summary>
        [DataMember]
        public bool DecodeByteArrays
        {
            get { return _decodeByteArrays; }
            set
            {
                if (_decodeByteArrays == value) return;
                _decodeByteArrays = value;
                NotifyOfPropertyChange(() => DecodeByteArrays);
            }
        }

        private bool _displayByteArrays;
        /// <summary>
        /// Gets or sets a value indicating whether to display byte arrays.
        /// </summary>
        [DataMember]
        public bool DisplayByteArrays
        {
            get { return _displayByteArrays; }
            set
            {
                if (_displayByteArrays == value) return;
                _displayByteArrays = value;
                NotifyOfPropertyChange(() => DisplayByteArrays);
            }
        }

        private int _portNumber;
        /// <summary>
        /// Gets or sets the port number.
        /// </summary>
        [DataMember]
        public int PortNumber
        {
            get { return _portNumber; }
            set
            {
                if (_portNumber == value) return;
                _portNumber = value;
                NotifyOfPropertyChange(() => PortNumber);
            }
        }

        private string _applicationName;
        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        [DataMember]
        public string ApplicationName
        {
            get { return _applicationName; }
            set
            {
                if (!string.Equals(_applicationName, value))
                {
                    _applicationName = value;
                    NotifyOfPropertyChange(() => ApplicationName);
                }
            }
        }

        private string _applicationVersion;
        /// <summary>
        /// Gets or sets the version of the application.
        /// </summary>
        /// <value>The version of the application.</value>
        [DataMember]
        public string ApplicationVersion
        {
            get { return _applicationVersion; }
            set
            {
                if (!string.Equals(_applicationVersion, value))
                {
                    _applicationVersion = value;
                    NotifyOfPropertyChange(() => ApplicationVersion);
                }
            }
        }

        private string _baseUri;
        /// <summary>
        /// Gets or sets the base URI for discovery.
        /// </summary>
        /// <value>The base URI for discovery.</value>
        [DataMember]
        public string BaseUri
        {
            get { return _baseUri; }
            set
            {
                if (!string.Equals(_baseUri, value))
                {
                    _baseUri = value;
                    NotifyOfPropertyChange(() => BaseUri);
                }
            }
        }

        private bool _groupByType;
        /// <summary>
        /// Gets or sets a value indicating whether resources should be grouped by type.
        /// </summary>
        [DataMember]
        public bool GroupByType
        {
            get { return _groupByType; }
            set
            {
                if (_groupByType == value) return;
                _groupByType = value;
                NotifyOfPropertyChange(() => GroupByType);
            }
        }

        private int _discoveryDepth;
        /// <summary>
        /// Gets or sets the discovery depth.
        /// </summary>
        /// <value>The discovery depth.</value>
        [DataMember]
        public int DiscoveryDepth
        {
            get { return _discoveryDepth; }
            set
            {
                if (!Equals(_discoveryDepth, value))
                {
                    _discoveryDepth = value;
                    NotifyOfPropertyChange(() => DiscoveryDepth);
                }
            }
        }

        private GraphScopes _discoveryScope;
        /// <summary>
        /// Gets or sets the ETP discovery scope.
        /// </summary>
        /// <value>The ETP discovery scope.</value>
        [DataMember]
        public GraphScopes DiscoveryScope
        {
            get { return _discoveryScope; }
            set
            {
                if (!Equals(_discoveryScope, value))
                {
                    _discoveryScope = value;
                    NotifyOfPropertyChange(() => DiscoveryScope);
                }
            }
        }

        private Functions _discoveryFunction;
        /// <summary>
        /// Gets or sets the ETP discovery function.
        /// </summary>
        /// <value>The ETP discovery function.</value>
        [DataMember]
        public Functions DiscoveryFunction
        {
            get { return _discoveryFunction; }
            set
            {
                if (!Equals(_discoveryFunction, value))
                {
                    _discoveryFunction = value;
                    NotifyOfPropertyChange(() => DiscoveryFunction);
                }
            }
        }

        private Functions _storeFunction;
        /// <summary>
        /// Gets or sets the ETP store function.
        /// </summary>
        /// <value>The ETP store function.</value>
        [DataMember]
        public Functions StoreFunction
        {
            get { return _storeFunction; }
            set
            {
                if (!Equals(_storeFunction, value))
                {
                    _storeFunction = value;
                    NotifyOfPropertyChange(() => StoreFunction);
                }
            }
        }

        private Functions _growingObjectFunction;
        /// <summary>
        /// Gets or sets the ETP Growing Object function.
        /// </summary>
        /// <value>The ETP Growing Object function.</value>
        [DataMember]
        public Functions GrowingObjectFunction
        {
            get { return _growingObjectFunction; }
            set
            {
                if (!Equals(_growingObjectFunction, value))
                {
                    _growingObjectFunction = value;
                    NotifyOfPropertyChange(() => GrowingObjectFunction);
                }
            }
        }

        /// <summary>
        /// Gets the collection of requested protocols.
        /// </summary>
        /// <value>The collection of requested protocols.</value>
        public BindableCollection<EtpProtocolItem> RequestedProtocols { get; }
    }
}
