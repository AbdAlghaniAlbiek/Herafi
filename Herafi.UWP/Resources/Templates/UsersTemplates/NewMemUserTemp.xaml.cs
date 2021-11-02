using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using Herafi.Core.Security;
using Herafi.Core.Helpers;
using System.Net.NetworkInformation;
using Microsoft.Toolkit.Uwp.Connectivity;
using Herafi.Core.Repositories.RemoteRepo;
using Herafi.UWP.Views.DeviceFamily_Desktop;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Herafi.UWP.Resources.Templates.UsersTemplates
{
    public sealed partial class NewMemUserTemp : UserControl,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ProfileUser _profileUser;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when user clicks accept button")]
        public event EventHandler AcceptUserEvent;

        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when user clicks refuse button")]
        public event EventHandler RefuseUserEvent;

        public NewMemUserTemp()
        {
            this.InitializeComponent();

            this.DataContextChanged += (s, e) => Bindings.Update();

            // Subscribe to the event that's raised when a qualifier value changes.
            ResourceContext.GetForCurrentView().QualifierValues.MapChanged += new MapChangedEventHandler<string, string>(QualifierValues_MapChanged);
            this.Loading += NewMemUserTemp_Loading;

            btnAcceptUser.Click += new RoutedEventHandler(BtnAcceptUser_Click); 
            btnRefuseUser.Click += new RoutedEventHandler (BtnRefuseUser_Click); 
        }


        #region Translating UI

        private void RefreshUIText()
        {
            txtNewMemCardID.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtNewMemCardID", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemCardShMoreInfo.Text =
               TranslatingUI.UsersResourceMap.GetValue("txtNewMemCardShMoreInfo", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemCardName.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtNewMemCardName", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemCardEmail.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtNewMemCardEmail", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemCardPhonNum.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtNewMemCardPhonNum", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemCardNatNum.Text =
               TranslatingUI.UsersResourceMap.GetValue("txtNewMemCardNatNum", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemCardDatJoin.Text =
               TranslatingUI.UsersResourceMap.GetValue("txtNewMemCardDatJoin", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemCardCity.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtNewMemCardCity", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemCardAccRefUser.Text =
               TranslatingUI.UsersResourceMap.GetValue("txtNewMemCardAccRefUser", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemCardAccRefUser2.Text =
              TranslatingUI.UsersResourceMap.GetValue("txtNewMemCardAccRefUser2", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemAcc.Text =
              TranslatingUI.UsersResourceMap.GetValue("txtNewMemAcc", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewMemRef.Text =
               TranslatingUI.UsersResourceMap.GetValue("txtNewMemRef", TranslatingUI.ResourceContextObj).ValueAsString;
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

        private async void NewMemUserTemp_Loading(FrameworkElement sender, object args)
        {
            await ActivateRefreshUITexts();
        }

        #endregion

        private void BtnAcceptUser_Click(object sender, RoutedEventArgs e)
        {
            //bubble the event up to the parent
            if (this.AcceptUserEvent != null)
                this.AcceptUserEvent(this, null);
        }

        private void BtnRefuseUser_Click(object sender, RoutedEventArgs e)
        {
            //bubble the event up to the parent
            if (this.RefuseUserEvent != null)
                this.RefuseUserEvent(this, null);
        }

        public ProfileUser ProfileUser
        {
            get { return this.DataContext as ProfileUser; }
            set { _profileUser = this.DataContext as ProfileUser; OnPropertyChanged(); }
        }

    }
}
