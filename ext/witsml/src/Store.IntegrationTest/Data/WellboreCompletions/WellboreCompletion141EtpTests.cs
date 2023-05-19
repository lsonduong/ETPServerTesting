//----------------------------------------------------------------------- 
// PDS WITSMLstudio Store, 2018.3
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

using Energistics.DataAccess.WITSML141;

namespace PDS.WITSMLstudio.Store.Data.WellboreCompletions
{
    /// <summary>
    /// WellboreCompletion141EtpTests
    /// </summary>
    public partial class WellboreCompletion141EtpTests
    {
        protected override void AddParents()
        {
            base.AddParents();
            DevKit.AddAndAssert<WellCompletionList, WellCompletion>(WellCompletion);
        }
    }
}