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
using System.IO;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Energistics.Etp.Common;
using Microsoft.Win32;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Desktop.Core.Runtime;

namespace PDS.WITSMLstudio.Desktop.Plugins.DataReplay.ViewModels.Simulation
{
    public class SimulationViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private static readonly string _pluginVersion = typeof(SimulationViewModel).GetAssemblyVersion();

        public SimulationViewModel(IRuntimeService runtime)
        {
            Runtime = runtime;
            Model = new Models.Simulation()
            {
                Version = _pluginVersion
            };
        }

        public IRuntimeService Runtime { get; }

        private Models.Simulation _model;

        public Models.Simulation Model
        {
            get { return _model; }
            set
            {
                if (!ReferenceEquals(_model, value))
                {
                    _model = value;
                    NotifyOfPropertyChange(() => Model);
                }
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            ActivateItem(new GeneralViewModel());
            Items.Add(new ChannelsViewModel(Runtime));
        }

        protected override void OnDeactivate(bool close)
        {
            if (close)
            {
                foreach (var child in Items.ToArray())
                {
                    this.CloseItem(child);
                }
            }

            base.OnDeactivate(close);
        }

        public void Save()
        {
            var dialog = new SaveFileDialog()
            {
                Title = "Save Simulation Configuration Settings...",
                Filter = "JSON Files|*.json;*.js|All Files|*.*",
                DefaultExt = ".json",
                AddExtension = true,
                FileName = DisplayName
            };

            if (dialog.ShowDialog(Application.Current.MainWindow).GetValueOrDefault())
            {
                try
                {
                    Model.Name = DisplayName;
                    var json = EtpExtensions.Serialize(Model, true);
                    File.WriteAllText(dialog.FileName, json);
                }
                catch (Exception ex)
                {
                    Runtime.ShowError("Error saving configuration settings.", ex);
                }
            }
        }
    }
}
