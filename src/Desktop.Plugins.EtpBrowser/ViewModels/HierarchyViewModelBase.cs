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
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Energistics.Etp.Common.Datatypes;
using PDS.WITSMLstudio.Connections;
using PDS.WITSMLstudio.Desktop.Core.Commands;
using PDS.WITSMLstudio.Desktop.Core.Runtime;
using PDS.WITSMLstudio.Desktop.Core.ViewModels;
using PDS.WITSMLstudio.Framework;

namespace PDS.WITSMLstudio.Desktop.Plugins.EtpBrowser.ViewModels
{
    /// <summary>
    /// Manages the behavior of the tree view user interface elements.
    /// </summary>
    /// <seealso cref="Caliburn.Micro.Screen" />
    public abstract class HierarchyViewModelBase : Screen, ISessionAware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchyViewModelBase"/> class.
        /// </summary>
        protected HierarchyViewModelBase(IRuntimeService runtime)
        {
            Runtime = runtime;
            DisplayName = "Discovery";
            GetBaseUriCommand = new DelegateCommand(x => GetBaseUri(), x => CanExecute);
        }

        /// <summary>
        /// Gets or Sets the Parent <see cref="T:Caliburn.Micro.IConductor" />
        /// </summary>
        public new MainViewModel Parent => (MainViewModel) base.Parent;

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>The model.</value>
        public Models.EtpSettings Model => Parent?.Model;

        /// <summary>
        /// Gets a collection of supported ETP versions.
        /// </summary>
        public string[] SupportedVersions { get; protected set; }

        /// <summary>
        /// Gets the runtime service.
        /// </summary>
        /// <value>The runtime service.</value>
        public IRuntimeService Runtime { get; }

        /// <summary>
        /// Gets the GetBaseUri command.
        /// </summary>
        public ICommand GetBaseUriCommand { get; }

        private bool _canExecute;
        /// <summary>
        /// Gets or sets a value indicating whether the Discovery protocol messages can be executed.
        /// </summary>
        /// <value><c>true</c> if Discovery protocol messages can be executed; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool CanExecute
        {
            get { return _canExecute; }
            set
            {
                if (_canExecute != value)
                {
                    _canExecute = value;
                    NotifyOfPropertyChange(() => CanExecute);
                }
            }
        }

        /// <summary>
        /// Gets resources using the current Base URI
        /// </summary>
        public void GetBaseUri()
        {
            //Parent.OnConnectionChanged(true, false);
            Parent.Resources.Clear();

            if (Parent.EtpExtender == null) return;

            Parent.GetResources(Model?.BaseUri);
        }

        /// <summary>
        /// Determines whether the GetObject message can be sent for the selected resource.
        /// </summary>
        /// <returns><c>true</c> if the selected resource's level is greater than 1; otherwise, <c>false</c>.</returns>
        public bool CanGetObject
        {
            get
            {
                var resource = Parent.SelectedResource;
                return CanExecute && !string.IsNullOrWhiteSpace(resource?.Resource?.Uri);
            }
        }

        /// <summary>
        /// Gets the selected resource's details using the Store protocol.
        /// </summary>
        public void GetObject()
        {
            Parent.GetObject();
        }

        /// <summary>
        /// Determines whether the DeleteObject message can be sent for the selected resource.
        /// </summary>
        /// <returns><c>true</c> if the selected resource's level is greater than 1; otherwise, <c>false</c>.</returns>
        public bool CanDeleteObject => CanGetObject;

        /// <summary>
        /// Gets a value indicating whether this instance can export object.
        /// </summary>
        public bool CanExportObject => CanGetObject;
        
        /// <summary>
        /// Deletes the selected resource using the Store protocol.
        /// </summary>
        public void DeleteObject()
        {
            if (Runtime.ShowConfirm("Are you sure you want to delete the selected resource?", MessageBoxButton.YesNo))
            {
                Parent.DeleteObject();
            }
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        public void ExportObject()
        {
            Parent.ExportFile = true;
            Parent.GetObject();
        }

        /// <summary>
        /// Refreshes the hierarchy.
        /// </summary>
        public void RefreshHierarchy()
        {
            //Parent.OnConnectionChanged(true, false);
            GetBaseUri();
        }

        /// <summary>
        /// Gets a value indicating whether this selected node can be refreshed.
        /// </summary>
        /// <value><c>true</c> if this instance can be refreshed; otherwise, <c>false</c>.</value>
        public bool CanRefreshSelected
        {
            get
            {
                var resource = Parent.SelectedResource;

                if (CanExecute && !string.IsNullOrWhiteSpace(resource?.Resource?.Uri))
                {
                    return resource.Resource.TargetCount != 0 ||
                           ResourceTypes.Folder.ToString().EqualsIgnoreCase(resource.Resource.ResourceType);
                }

                return false;
            }
        }

        /// <summary>
        /// Refreshes the selected node.
        /// </summary>
        public void RefreshSelected()
        {
            var resource = Parent.Resources.FindSelected();
            // Return if there is nothing currently selected
            if (resource == null) return;

            resource.ClearAndLoadChildren();
            // Expand the node if it wasn't previously
            resource.IsExpanded = true;
        }

        /// <summary>
        /// Refreshes the context menu.
        /// </summary>
        public void RefreshContextMenu()
        {
            NotifyOfPropertyChange(() => CanGetObject);
            NotifyOfPropertyChange(() => CanDeleteObject);
            NotifyOfPropertyChange(() => CanExportObject);
            NotifyOfPropertyChange(() => CanCopyUriToStreaming);
            NotifyOfPropertyChange(() => CanRefreshSelected);
            NotifyOfPropertyChange(() => CanCopyUuidToClipboard);
            NotifyOfPropertyChange(() => CanCopyUriToClipboard);
            NotifyOfPropertyChange(() => CanCopyUriToStore);
        }

        /// <summary>
        /// Gets a value indicating whether this instance can copy URI to clipboard.
        /// </summary>
        /// <value><c>true</c> if this instance can copy URI to clipboard; otherwise, <c>false</c>.</value>
        public bool CanCopyUuidToClipboard => CanGetObject;

        /// <summary>
        /// Copies the UUID to clipboard.
        /// </summary>
        public void CopyUuidToClipboard()
        {
            var resource = Parent.SelectedResource;

            if (resource?.Resource == null)
                return;

            var uri = new EtpUri(resource.Resource.Uri);
            Clipboard.SetText(uri.ObjectId);
        }

        /// <summary>
        /// Gets a value indicating whether this instance can copy URI to clipboard.
        /// </summary>
        /// <value><c>true</c> if this instance can copy URI to clipboard; otherwise, <c>false</c>.</value>
        public bool CanCopyUriToClipboard => CanGetObject;

        /// <summary>
        /// Copies the URI to clipboard.
        /// </summary>
        public void CopyUriToClipboard()
        {
            var resource = Parent.SelectedResource;

            if (resource?.Resource == null)
                return;

            Clipboard.SetText(resource.Resource.Uri);
        }

        /// <summary>
        /// Gets a value indicating whether this instance can copy URI to store.
        /// </summary>
        /// <value><c>true</c> if this instance can copy URI to store; otherwise, <c>false</c>.</value>
        public bool CanCopyUriToStore => CanGetObject;

        /// <summary>
        /// Copies the URI to store.
        /// </summary>
        public void CopyUriToStore()
        {
            var resource = Parent.SelectedResource;
            var storeViewModel = Parent.Items.OfType<StoreViewModel>().FirstOrDefault();

            if (resource?.Resource == null || storeViewModel == null)
                return;

            storeViewModel.ClearInputSettings();
            Parent.ActivateItem(storeViewModel);
            storeViewModel.Model.Store.Uri = resource.Resource.Uri;
            NotifyOfPropertyChange(() => storeViewModel.Model.Store.Uri);
        }

        /// <summary>
        /// Determines whether the ChannelDescribe message can be sent for the selected resource.
        /// </summary>
        /// <value><c>true</c> if the channels can be described; otherwise, <c>false</c>.</value>
        public bool CanCopyUriToStreaming
        {
            get
            {
                if (CanExecute)
                {
                    var isChecked = Parent.CheckedResources.Any(x => !string.IsNullOrWhiteSpace(x.Resource?.Uri) && x.Resource.ChannelSubscribable);
                    if (isChecked) return true;

                    var resource = Parent.SelectedResource;

                    if (!string.IsNullOrWhiteSpace(resource?.Resource?.Uri))
                    {
                        return resource.Resource.ChannelSubscribable;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Copies the URI to streaming.
        /// </summary>
        public abstract void CopyUriToStreaming();

        /// <summary>
        /// Determines whether the NotificationRequest message can be sent for the selected resource.
        /// </summary>
        /// <value><c>true</c> if the object is notifiable; otherwise, <c>false</c>.</value>
        public bool CanCopyUriToNotification
        {
            get
            {
                var resource = Parent.SelectedResource;

                if (CanExecute && !string.IsNullOrWhiteSpace(resource?.Resource?.Uri))
                {
                    return resource.Resource.ObjectNotifiable;
                }

                return false;
            }
        }

        /// <summary>
        /// Copies the URI to the Notification tab.
        /// </summary>
        public void CopyUriToNotification()
        {
            var viewModel = Parent.Items.OfType<StoreNotificationViewModel>().FirstOrDefault();
            var resource = Parent.SelectedResource;

            if (viewModel != null && resource != null)
            {
                Model.StoreNotification.Uri = resource.Resource.Uri;
                Parent.ActivateItem(viewModel);
            }
        }

        /// <summary>
        /// Clears the checked items.
        /// </summary>
        public void ClearCheckedItems()
        {
            foreach (var resource in Parent.CheckedResources)
            {
                resource.IsChecked = false;
            }
        }

        /// <summary>
        /// Called when the selected connection has changed.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public virtual void OnConnectionChanged(Connection connection)
        {
            Runtime.InvokeAsync(() =>
            {
                RefreshFunctionList();
                Refresh();
            });
        }

        /// <summary>
        /// Called when the OpenSession message is recieved.
        /// </summary>
        /// <param name="supportedProtocols">The supported protocols.</param>
        public abstract void OnSessionOpened(IList<ISupportedProtocol> supportedProtocols);

        /// <summary>
        /// Called when the <see cref="Energistics.Etp.Common.IEtpClient" /> web socket is closed.
        /// </summary>
        public void OnSocketClosed()
        {
            CanExecute = false;
            RefreshContextMenu();
        }

        /// <summary>
        /// Called when an attached view's Loaded event fires.
        /// </summary>
        /// <param name="view"></param>
        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            RefreshFunctionList();
        }

        /// <summary>
        /// Refreshes the function list.
        /// </summary>
        protected abstract void RefreshFunctionList();

        /// <summary>
        /// Copies the URI to streaming.
        /// </summary>
        protected void CopyUriToStreaming<T>(Action<T> addUriCallback) where T : IScreen
        {
            var viewModel = Parent.Items.OfType<T>().FirstOrDefault();
            if (viewModel == null) return;

            // Get list of checked resources
            var checkedResources = Parent.CheckedResources.ToList();
            if (checkedResources.Count < 1)
            {
                // Use selected resource of none are checked
                var selectedResource = Parent.SelectedResource;
                if (selectedResource != null)
                {
                    checkedResources.Add(selectedResource);
                }
            }

            foreach (var resource in checkedResources)
            {
                Model.Streaming.Uri = resource.Resource.Uri;
                addUriCallback?.Invoke(viewModel);
                Parent.ActivateItem(viewModel);
            }
        }
    }
}
