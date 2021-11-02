using Herafi.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Herafi.Core.Helpers;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Herafi.UWP.Resources.Templates.UsersTemplates
{
    public sealed partial class GeneralRequestTemp : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private RequestUser _requestUser;


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public GeneralRequestTemp()
        {
            this.InitializeComponent();

            // Subscribe to the event that's raised when a qualifier value changes.
            ResourceContext.GetForCurrentView().QualifierValues.MapChanged += new MapChangedEventHandler<string, string>(QualifierValues_MapChanged);

            this.Loading += GeneralRequestTemp_Loading;

            this.DataContextChanged += (s, e) => Bindings.Update();
        }
        

        #region Translating UI

        private void RefreshUIText()
        {
            txtGeneralCardID.Text = 
                TranslatingUI.UsersResourceMap.GetValue("txtGeneralCardID", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCardName.Text =
               TranslatingUI.UsersResourceMap.GetValue("txtGeneralCardName", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCardShMoreInfo.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtGeneralCardShMoreInfo", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCardCraftmanName.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtGeneralCardCraftmanName", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCardProc.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtGeneralCardProc", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCardFrToReq.Text =
               TranslatingUI.UsersResourceMap.GetValue("txtGeneralCardFrToReq", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCardCost.Text =
               TranslatingUI.UsersResourceMap.GetValue("txtGeneralCardCost", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCardStat.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtGeneralCardStat", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCardHouWor.Text =
               TranslatingUI.UsersResourceMap.GetValue("txtGeneralCardHouWor", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCardRat.Text =
              TranslatingUI.UsersResourceMap.GetValue("txtGeneralCardRat", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCardComm.Text =
              TranslatingUI.UsersResourceMap.GetValue("txtGeneralCardComm", TranslatingUI.ResourceContextObj).ValueAsString;
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

        private async void GeneralRequestTemp_Loading(FrameworkElement sender, object args)
        {
            await ActivateRefreshUITexts();
        }

        private async void QualifierValues_MapChanged(IObservableMap<string, string> sender, IMapChangedEventArgs<string> @event)
        {
            await ActivateRefreshUITexts();
        }

        #endregion


        public RequestUser RequestUser
        {
            get { return this.DataContext as RequestUser; }
            set { _requestUser = this.DataContext as RequestUser; OnPropertyChanged(); }
        }
    }
}
