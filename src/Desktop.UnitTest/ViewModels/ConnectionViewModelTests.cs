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

using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.WITSMLstudio.Connections;

namespace PDS.WITSMLstudio.Desktop.ViewModels
{
    /// <summary>
    /// Unit tests for the ConnectionViewModel
    /// </summary>
    [TestClass]
    public class ConnectionViewModelTests : ConnectionViewModelTestBase
    {
        /// <summary>
        /// Tests the connection filename for each type of connection has the correct prefix.
        /// </summary>
        [TestMethod]
        public void ConnectionViewModel_Filename_Has_Correct_Prefix()
        {
            // Test Witsml filename
            var filename = Path.GetFileName(_witsmlConnectionVm.GetConnectionFilename());
            Assert.IsTrue(filename.StartsWith(ConnectionTypes.Witsml.ToString()));

            // Test Etp filename
            filename = Path.GetFileName(_etpConnectionVm.GetConnectionFilename());
            Assert.IsTrue(filename.StartsWith(ConnectionTypes.Etp.ToString()));
        }

        /// <summary>
        /// Tests the connection filename is correctly generated with the 
        /// ConnectionType prefix and the base file name.
        /// </summary>
        [TestMethod]
        public void ConnectionViewModel_Filename_Is_Correct()
        {
            var witsmlFilename = string.Format("{0}{1}", ConnectionTypes.Witsml, ConnectionBaseFileName);
            var etpFilename = string.Format("{0}{1}", ConnectionTypes.Etp, ConnectionBaseFileName);

            // Test Witsml filename
            var filename = Path.GetFileName(_witsmlConnectionVm.GetConnectionFilename());
            Assert.AreEqual(witsmlFilename, filename);

            // Test Etp filename
            filename = Path.GetFileName(_etpConnectionVm.GetConnectionFilename());
            Assert.AreEqual(etpFilename, filename);
        }

        /// <summary>
        /// Tests that a null connection is returned when no connection file is persisted.
        /// </summary>
        [TestMethod]
        public void ConnectionViewModel_OpenConnectionFile_Is_Null_When_No_Connection_File_Persisted()
        {
            Assert.IsNull(_witsmlConnectionVm.OpenConnectionFile());
            Assert.IsNull(_etpConnectionVm.OpenConnectionFile());
        }

        /// <summary>
        /// Tests the equivalence of a connection before and after file persistence.
        /// </summary>
        [TestMethod]
        public void ConnectionViewModel_Connection_File_Is_Persisted()
        {
            _witsmlConnectionVm.SaveConnectionFile(_witsmlConnection);
            _etpConnectionVm.SaveConnectionFile(_etpConnection);

            var witsmlRead = _witsmlConnectionVm.OpenConnectionFile();
            Assert.AreEqual(_witsmlConnection.Uri, witsmlRead.Uri);

            var etpRead = _etpConnectionVm.OpenConnectionFile();
            Assert.AreEqual(_etpConnection.Uri, etpRead.Uri);
        }

        [TestMethod]
        public void ConnectionViewModel_EditItem_Is_Defaulted_With_No_DataItem()
        {
            var emptyConnection = new Connection();
            _witsmlConnectionVm.InitializeEditItem();

            Assert.AreEqual(emptyConnection.Uri, _witsmlConnectionVm.EditItem.Uri);
        }

        [TestMethod]
        public void ConnectionViewModel_EditItem_Is_Initialized_With_DataItem()
        {
            _witsmlConnectionVm.DataItem = _witsmlConnection;
            _witsmlConnectionVm.InitializeEditItem();

            Assert.AreEqual(_witsmlConnection.Uri, _witsmlConnectionVm.EditItem.Uri);
        }

        [TestMethod]
        public void ConnectionViewModel_EditItem_Is_Initialized_With_Persisted_Connection()
        {
            _witsmlConnectionVm.SaveConnectionFile(_witsmlConnection);
            _witsmlConnectionVm.InitializeEditItem();

            Assert.AreEqual(_witsmlConnection.Uri, _witsmlConnectionVm.EditItem.Uri);
        }

        [TestMethod]
        public void ConnectionViewModel_DataItem_Not_Set_On_Cancel()
        {
            var newName = "xxx";

            // Initialze the Edit Item by setting the DataItem
            _witsmlConnectionVm.DataItem = _witsmlConnection;
            _witsmlConnectionVm.InitializeEditItem();

            // Make a change to the edit item
            _witsmlConnectionVm.EditItem.Name = newName;

            // Accept the changes
            _witsmlConnectionVm.Cancel();

            // Test that the newName was not assigned
            Assert.AreNotEqual(newName, _witsmlConnectionVm.DataItem.Name);

            // Test that the Name is unchanged
            Assert.AreEqual(_witsmlConnection.Name, _witsmlConnectionVm.DataItem.Name);
        }
    }
}
