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
using System.Linq;
using Caliburn.Micro;
using Energistics.DataAccess.Reflection;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Desktop.Core.Runtime;
using PDS.WITSMLstudio.Desktop.Plugins.ObjectInspector.Models;

namespace PDS.WITSMLstudio.Desktop.Plugins.ObjectInspector.ViewModels
{
    /// <summary>
    /// Manages the behavior for the family version selection UI elements.
    /// </summary>
    /// <seealso cref="Screen" />
    public sealed class FamilyVersionViewModel : Screen
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(FamilyVersionViewModel));

        private FamilyVersion _familyVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilyVersionViewModel"/> class.
        /// </summary>
        /// <param name="runtime">The runtime.</param>
        /// <exception cref="ArgumentNullException"><paramref name="runtime"/> is null.</exception>
        public FamilyVersionViewModel(IRuntimeService runtime)
        {
            runtime.NotNull(nameof(runtime));

            Log.Debug("Creating view model instance");
            Runtime = runtime;

            SetModelToDefault(Energistics.DataAccess.Reflection.StandardFamily.WITSML);
        }

        /// <summary>
        /// Gets or sets the data model.
        /// </summary>
        public FamilyVersion FamilyVersion
        {
            get { return _familyVersion; }
            set
            {
                if (_familyVersion == value) return;

                _familyVersion = value;

                Refresh();
            }
        }

        /// <summary>
        /// Gets the runtime service.
        /// </summary>
        /// <value>The runtime.</value>
        public IRuntimeService Runtime { get; }

        /// <summary>
        /// Gets the current standard family from the model.
        /// </summary>
        /// <exception cref="ArgumentNullException">StandardFamily is set to a null value.</exception>
        /// <exception cref="ArgumentException">StandardFamily is set to a standard family that is not available.</exception>
        public StandardFamily? StandardFamily
        {
            get
            {
                return FamilyVersion?.StandardFamily;
            }
            set
            {
                if (value == null) throw new ArgumentNullException();
                if (value != null && !FamilyVersion.IsAvailableStandardFamily(value.Value))
                    throw new ArgumentException($"Standard family '{value.Value.ToString()}' not available");

                if (FamilyVersion != null && FamilyVersion.StandardFamily == value) return;

                SetModelToDefault(value.Value);
            }
        }

        /// <summary>
        /// Gets the current data schema version from the model.
        /// </summary>
        /// <exception cref="ArgumentNullException">DataSchemaVersion is set to a null value.</exception>
        /// <exception cref="InvalidOperationException">DataSchemaVersion is set to a non-null value when FamilyVersion is null.</exception>
        /// <exception cref="ArgumentException">DataSchemaVersion is set to a data schema version that is not available for the current standard family.</exception>
        public Version DataSchemaVersion
        {
            get
            {
                return FamilyVersion?.DataSchemaVersion;
            }
            set
            {
                if (value == null) throw new ArgumentNullException();
                if (FamilyVersion == null)
                    throw new InvalidOperationException("FamilyVersion must not be null when setting a non-null DataSchemaVersion.");
                if (value != null && !FamilyVersion.IsAvailableDataSchemaVersion(FamilyVersion.StandardFamily, value))
                    throw new ArgumentException($"Data schema version not available for {FamilyVersion.StandardFamily.ToString()}");

                if (FamilyVersion.DataSchemaVersion == value) return;

                FamilyVersion = new FamilyVersion(FamilyVersion.StandardFamily, value);
            }
        }

        /// <summary>
        /// Available standard families for the model.
        /// </summary>
        public IEnumerable<StandardFamily> StandardFamilies
        {
            get
            {
                if (FamilyVersion == null) return new List<StandardFamily>();

                return FamilyVersion.StandardFamilies;
            }
        }

        /// <summary>
        /// Available data schema versions for the selected standard family in the model.
        /// </summary>
        public IEnumerable<Version> DataSchemaVersions
        {
            get
            {
                if (FamilyVersion == null) return new List<Version>();

                return FamilyVersion.GetDataSchemaVersions(FamilyVersion.StandardFamily);
            }
        }

        /// <summary>
        /// Gets the default model for the specified standard family.
        /// </summary>
        /// <param name="standardFamily">The standard family to get the default model for.</param>
        private void SetModelToDefault(StandardFamily standardFamily)
        {
            Version dataSchemaVersion = FamilyVersion.GetDataSchemaVersions(standardFamily).First();

            FamilyVersion = new FamilyVersion(standardFamily, dataSchemaVersion);
        }
    }
}
