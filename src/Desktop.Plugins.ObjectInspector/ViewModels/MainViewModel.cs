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
using System.ComponentModel;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Desktop.Core.Runtime;
using PDS.WITSMLstudio.Desktop.Core.ViewModels;
using PDS.WITSMLstudio.Desktop.Plugins.ObjectInspector.Properties;

namespace PDS.WITSMLstudio.Desktop.Plugins.ObjectInspector.ViewModels
{
    /// <summary>
    /// Manages the behavior of the main user interface for the Object Inspector plug-in.
    /// </summary>
    /// <seealso cref="Conductor{IScreen}.Collection.AllActive" />
    /// <seealso cref="IPluginViewModel" />
    public sealed class MainViewModel : Conductor<IScreen>.Collection.AllActive, IPluginViewModel
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(MainViewModel));

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="runtime">The runtime service.</param>
        /// <exception cref="ArgumentNullException"><paramref name="runtime"/> is null.</exception>
        [ImportingConstructor]
        public MainViewModel(IRuntimeService runtime)
        {
            runtime.NotNull(nameof(runtime));

            Log.Debug("Creating view model instance");

            Runtime = runtime;
            DisplayName = Settings.Default.PluginDisplayName;

            // Create view models displayed within this view model.
            FamilyVersionControl = new FamilyVersionViewModel(Runtime);
            FamilyVersionObjectsControl = new FamilyVersionObjectsViewModel(Runtime);
            DataObjectControl = new DataObjectViewModel(Runtime);
            DataPropertiesControl = new DataPropertiesViewModel(Runtime);

            FamilyVersionObjectsControl.FamilyVersion = FamilyVersionControl.FamilyVersion;

            FamilyVersionControl.PropertyChanged += FamilyVersionControl_PropertyChanged;
            FamilyVersionObjectsControl.PropertyChanged += FamilyVersionObjects_PropertyChanged;
        }

        /// <summary>
        /// Gets the display order of the plug-in when loaded by the main application shell
        /// </summary>
        public int DisplayOrder => Settings.Default.PluginDisplayOrder;

        /// <summary>
        /// Gets the sub title to display in the main application title bar.
        /// </summary>
        public string SubTitle => string.Empty;

        /// <summary>
        /// Gets the runtime service.
        /// </summary>
        /// <value>The runtime.</value>
        public IRuntimeService Runtime { get; }

        /// <summary>
        /// Gets or sets the reference to the family version view model.
        /// </summary>
        /// <value>
        /// The family version view model.
        /// </value>
        public FamilyVersionViewModel FamilyVersionControl { get; }

        /// <summary>
        /// Gets or sets the reference to the family version objects view model.
        /// </summary>
        /// <value>
        /// The family version objects view model.
        /// </value>
        public FamilyVersionObjectsViewModel FamilyVersionObjectsControl { get; }

        /// <summary>
        /// Gets the reference to the Energistics Data Object view model.
        /// </summary>
        /// <value>
        /// The Energistics Data Object view model.
        /// </value>
        public DataObjectViewModel DataObjectControl { get; }

        /// <summary>
        /// Gets the reference to the Energistics Data Object properties view model.
        /// </summary>
        /// <value>
        /// The Energistics Data Object properties view model.
        /// </value>
        public DataPropertiesViewModel DataPropertiesControl { get; }

        /// <summary>
        /// Loads the screens hosted by the MainViewModel.
        /// </summary>
        internal void LoadScreens()
        {
            Log.Debug("Loading MainViewModel screens");
            Items.Add(FamilyVersionControl);
            Items.Add(FamilyVersionObjectsControl);
        }

        /// <summary>
        /// Called when initializing the MainViewModel.
        /// </summary>
        protected override void OnInitialize()
        {
            Log.Debug("Initializing screen");
            base.OnInitialize();
            LoadScreens();
        }

        /// <summary>
        /// Update status when activated
        /// </summary>
        protected override void OnActivate()
        {
            base.OnActivate();
            Runtime.Invoke(() =>
            {
                if (Runtime.Shell != null)
                    Runtime.Shell.StatusBarText = "Ready";
            });
        }

        /// <summary>
        /// Handles the PropertyChanged event of the FamilyVersionControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void FamilyVersionControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == string.Empty || e.PropertyName == nameof(FamilyVersionControl.FamilyVersion))
                FamilyVersionObjectsControl.FamilyVersion = FamilyVersionControl.FamilyVersion;
        }

        /// <summary>
        /// Handles the PropertyChanged event of the FamilyVersionObjects control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs" /> instance containing the event data.</param>
        private void FamilyVersionObjects_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == string.Empty || e.PropertyName == nameof(FamilyVersionObjectsControl.SelectedDataObject))
            {
                DataObjectControl.DataObject = FamilyVersionObjectsControl.SelectedDataObject;
                DataPropertiesControl.DataObject = FamilyVersionObjectsControl.SelectedDataObject;
            }
        }
    }
}
