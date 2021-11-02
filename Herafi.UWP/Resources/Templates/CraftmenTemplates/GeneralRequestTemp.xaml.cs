using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Threading.Tasks;
using Herafi.Core.Helpers;
using Windows.ApplicationModel.Resources.Core;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Herafi.UWP.Resources.Templates.CraftmenTemplates
{
    public sealed partial class GeneralRequestTemp : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Request _request;

       
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
               TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralCardID", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCardName.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralCardName", TranslatingUI.ResourceContextObj).ValueAsString;
            txtRepCardShMoreInfo.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtRepCardShMoreInfo", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCardUserName.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralCardUserName", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCardProc.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralCardProc", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCardFrToReq.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralCardFrToReq", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCardCost.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralCardCost", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCardStat.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralCardStat", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCardHoursWork.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralCardHoursWork", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneraCardlRat.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtGeneraCardlRat", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCardComm.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralCardComm", TranslatingUI.ResourceContextObj).ValueAsString;
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

        private async void GeneralRequestTemp_Loading(FrameworkElement sender, object args)
        {
            await ActivateRefreshUITexts();
        }

        #endregion


        public Request Request
        {
            get { return this.DataContext as Request; }
            set { _request = this.DataContext as Request; OnPropertyChanged(); }
        }
    }
}
