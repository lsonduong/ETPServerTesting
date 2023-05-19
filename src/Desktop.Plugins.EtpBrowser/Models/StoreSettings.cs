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

using System.Runtime.Serialization;
using Caliburn.Micro;

namespace PDS.WITSMLstudio.Desktop.Plugins.EtpBrowser.Models
{
    /// <summary>
    /// Encapsulates the ETP Browser settings for the Store protocol.
    /// </summary>
    /// <seealso cref="Caliburn.Micro.PropertyChangedBase" />
    [DataContract]
    public class StoreSettings : PropertyChangedBase
    {
        private string _uri;
        /// <summary>
        /// Gets or sets the uri.
        /// </summary>
        /// <value>The uri.</value>
        [DataMember]
        public string Uri
        {
            get { return _uri; }
            set
            {
                if (!ReferenceEquals(_uri, value))
                {
                    _uri = value;
                    NotifyOfPropertyChange(() => Uri);
                }
            }
        }

        private string _uuid;
        /// <summary>
        /// Gets or sets the uuid.
        /// </summary>
        /// <value>The uuid.</value>
        [DataMember]
        public string Uuid
        {
            get { return _uuid; }
            set
            {
                if (!ReferenceEquals(_uuid, value))
                {
                    _uuid = value;
                    NotifyOfPropertyChange(() => Uuid);
                }
            }
        }

        private string _name;
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataMember]
        public string Name
        {
            get { return _name; }
            set
            {
                if (!ReferenceEquals(_name, value))
                {
                    _name = value;
                    NotifyOfPropertyChange(() => Name);
                }
            }
        }

        private string _contentType;
        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        /// <value>The content type.</value>
        [DataMember]
        public string ContentType
        {
            get { return _contentType; }
            set
            {
                if (!ReferenceEquals(_contentType, value))
                {
                    _contentType = value;
                    NotifyOfPropertyChange(() => ContentType);
                }
            }
        }
    }
}
