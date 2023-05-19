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

using System.Collections.Generic;
using Caliburn.Micro;
using Energistics.Etp.Common.Datatypes;
using PDS.WITSMLstudio.Connections;

namespace PDS.WITSMLstudio.Desktop.Plugins.EtpBrowser.ViewModels
{
    /// <summary>
    /// Defines methods that can be implemented to receive <see cref="Energistics.Etp.Common.IEtpClient"/> status notifications.
    /// </summary>
    public interface ISessionAware : IScreen
    {
        /// <summary>
        /// Gets a collection of supported ETP versions.
        /// </summary>
        string[] SupportedVersions { get; }

        /// <summary>
        /// Called when the selected connection has changed.
        /// </summary>
        /// <param name="connection">The connection.</param>
        void OnConnectionChanged(Connection connection);

        /// <summary>
        /// Called when the OpenSession message is recieved.
        /// </summary>
        /// <param name="supportedProtocols">The supported protocols.</param>
        void OnSessionOpened(IList<ISupportedProtocol> supportedProtocols);

        /// <summary>
        /// Called when the <see cref="Energistics.Etp.Common.IEtpClient"/> web socket is closed.
        /// </summary>
        void OnSocketClosed();
    }
}
