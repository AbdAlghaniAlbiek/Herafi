using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Herafi.Core.Models;
using System.Runtime.CompilerServices;
using Herafi.Core.Repositories.RemoteRepo;
using Herafi.Core.Security;
using Herafi.Core.Helpers;
using System.Net.NetworkInformation;
using Microsoft.Toolkit.Uwp.Connectivity;
using Herafi.UWP.Views.DeviceFamily_Desktop;
using Windows.ApplicationModel.Resources.Core;
using System.Threading.Tasks;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Herafi.UWP.Resources.Templates.CraftmenTemplates
{
    public sealed partial class NewMemCraftmanTemp : UserControl,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NewMemberCraftman _newMemberCraftman;
        
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public NewMemCraftmanTemp()
        {
            this.InitializeComponent();

            this.DataContextChanged += (s, e) => Bindings.Update();

            // Subscribe to the event that's raised when a qualifier value changes.
            ResourceContext.GetForCurrentView().QualifierValues.MapChanged += new MapChangedEventHandler<string, string>(QualifierValues_MapChanged);
            this.Loading += NewMemCraftmanTemp_Loading;

            radBtnNormal.Click += new RoutedEventHandler(RadBtnNormal_Click);
            radBtnMedium.Click += new RoutedEventHandler(RadBtnMedium_Click);

            btnAcceptCraftman.Click += new RoutedEventHandler(BtnAcceptCraftman_Click);
            btnRefuseCraftman.Click += new RoutedEventHandler(BtnRefuseCraftman_Click);
        }

        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when user clicks accept button")]
        public event EventHandler AcceptButtonEvent;

        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when user clicks refuse button")]
        public event EventHandler RefuseButtonEvent;


        private void BtnAcceptCraftman_Click(object sender, RoutedEventArgs e)
        {
            //bubble the event up to the parent
            if (this.AcceptButtonEvent != null)
                this.AcceptButtonEvent(this, null);
        }
        private void BtnRefuseCraftman_Click(object sender, RoutedEventArgs e)
        {
            //bubble the event up to the parent
            if (this.RefuseButtonEvent != null)
                this.RefuseButtonEvent(this, null);
        }
        

        private void RadBtnNormal_Click(object sender, RoutedEventArgs e)
        {
            NewMemeberCraftman.Level = "Normal";
        }
        private void RadBtnMedium_Click(object sender, RoutedEventArgs e)
        {
            NewMemeberCraftman.Level = "Medium";
        }
        

        #region Translating UI

        private void RefreshUIText()
        {
            txtNewMemName.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtNewMemName", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemID.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtNewMemID", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemCardShMoreInfo.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtNewMemCardShMoreInfo", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemProfileTitle.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtNewMemProfileTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemName.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtNewMemName", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemEmail.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtNewMemEmail", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemPhonNum.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtNewMemPhonNum", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemNatNum.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtNewMemNatNum", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemDatJoin.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtNewMemDatJoin", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemCity.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtNewMemCity", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemTaxi.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtNewMemTaxi", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemCraftsTitle.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtNewMemCraftsTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemCert.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtNewMemCert", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemChoLev.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtNewMemChoLev", TranslatingUI.ResourceContextObj).ValueAsString;
            radBtnNormal.Content =
               TranslatingUI.CraftmenResourceMap.GetValue("radBtnNormal", TranslatingUI.ResourceContextObj).ValueAsString;
            radBtnMedium.Content =
               TranslatingUI.CraftmenResourceMap.GetValue("radBtnMedium", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemAccRefCraf.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtNewMemAccRefCraf", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemAccRefCraf2.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtNewMemAccRefCraf2", TranslatingUI.ResourceContextObj).ValueAsString;
            txtBtnNewMemAcc.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtBtnNewMemAcc", TranslatingUI.ResourceContextObj).ValueAsString;
            txtBtnNewMemRef.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtBtnNewMemRef", TranslatingUI.ResourceContextObj).ValueAsString;
        }

        private async Task ActivateRefreshUITexts()
        {
            if (this.Dispatcher.HasThreadAccess)
            {
                RefreshUIText();
            }
            else
            {
                await this.Dispatcher.RunAsync(
                    Windows.UI.Core.CoreDispatcherPriority.Normal,
                    () => this.RefreshUIText());
            }
        }

        private async void QualifierValues_MapChanged(IObservableMap<string, string> sender, IMapChangedEventArgs<string> @event)
        {
            await ActivateRefreshUITexts();
        }

        private async void NewMemCraftmanTemp_Loading(FrameworkElement sender, object args)
        {
            await ActivateRefreshUITexts();
        }

        #endregion

        public NewMemberCraftman NewMemeberCraftman
        {
            get { return this.DataContext as NewMemberCraftman; }
            set { _newMemberCraftman = this.DataContext as NewMemberCraftman; OnPropertyChanged(); }
        }
    }
}
