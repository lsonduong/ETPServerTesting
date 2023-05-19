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
using System.Collections.Generic;
using Energistics.Etp.Common;
using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.v11.Datatypes.Object;
using Energistics.Etp.v11.Protocol.Discovery;
using PDS.WITSMLstudio.Framework;

namespace PDS.WITSMLstudio.Desktop.Plugins.DataReplay.Providers
{
    public class SimulationDiscovery11Provider : DiscoveryStoreHandler
    {
        public SimulationDiscovery11Provider(IEtpSimulator simulator)
        {
            Simulator = simulator;
        }

        public IEtpSimulator Simulator { get; }

        public Models.Simulation Simulation => Simulator.Model;

        protected override void HandleGetResources(ProtocolEventArgs<GetResources, IList<Resource>> args)
        {
            if (EtpUri.IsRoot(args.Message.Uri))
            {
                args.Context.Add(New(
                    Guid.NewGuid().ToString(),
                    EtpUris.Witsml141,
                    contentType: EtpContentTypes.Witsml141,
                    resourceType: ResourceTypes.UriProtocol,
                    name: "WITSML 1.4.1.1 Store"));
            }
            else if (args.Message.Uri == EtpUris.Witsml141)
            {
                args.Context.Add(New(
                    Simulation.WellUid,
                    string.Format("{0}/well({1})", EtpUris.Witsml141, Simulation.WellUid),
                    contentType: EtpContentTypes.Witsml141.For(ObjectTypes.Well),
                    resourceType: ResourceTypes.DataObject,
                    name: Simulation.WellName));
            }
            else if (string.Format("{0}/well({1})", EtpUris.Witsml141, Simulation.WellUid).EqualsIgnoreCase(args.Message.Uri))
            {
                args.Context.Add(New(
                    Simulation.WellboreUid,
                    string.Format("{0}/well({1})/wellbore({2})", EtpUris.Witsml141, Simulation.WellUid, Simulation.WellboreUid),
                    contentType: EtpContentTypes.Witsml141.For(ObjectTypes.Wellbore),
                    resourceType: ResourceTypes.DataObject,
                    name: Simulation.WellboreName));
            }
            else if (string.Format("{0}/well({1})/wellbore({2})", EtpUris.Witsml141, Simulation.WellUid, Simulation.WellboreUid).EqualsIgnoreCase(args.Message.Uri))
            {
                args.Context.Add(New(
                    Simulation.LogUid,
                    string.Format("{0}/well({1})/wellbore({2})/log({3})", EtpUris.Witsml141, Simulation.WellUid, Simulation.WellboreUid, Simulation.LogUid),
                    contentType: EtpContentTypes.Witsml141.For(ObjectTypes.Log),
                    resourceType: ResourceTypes.DataObject,
                    name: Simulation.LogName));
            }
            else if (string.Format("{0}/well({1})/wellbore({2})/log({3})", EtpUris.Witsml141, Simulation.WellUid, Simulation.WellboreUid, Simulation.LogUid).EqualsIgnoreCase(args.Message.Uri))
            {
                foreach (var channel in Simulation.Channels)
                {
                    channel.ChannelUri = string.Format("{0}/well({1})/wellbore({2})/log({3})/curve({4})", EtpUris.Witsml141, Simulation.WellUid, Simulation.WellboreUid, Simulation.LogUid, channel.Uuid);

                    args.Context.Add(New(
                        channel.Uuid,
                        channel.ChannelUri,
                        contentType: EtpContentTypes.Witsml141.For("logCurveInfo"),
                        resourceType: ResourceTypes.DataObject,
                        name: channel.ChannelName,
                        count: 0));
                }
            }
        }

        private Resource New(string uuid, string uri, ResourceTypes resourceType, string contentType, string name, int count = -1)
        {
            return new Resource()
            {
                Uuid = uuid,
                Uri = uri,
                Name = name,
                HasChildren = count,
                ContentType = contentType,
                ResourceType = resourceType.ToString(),
                CustomData = new Dictionary<string, string>()
            };
        }
    }
}
