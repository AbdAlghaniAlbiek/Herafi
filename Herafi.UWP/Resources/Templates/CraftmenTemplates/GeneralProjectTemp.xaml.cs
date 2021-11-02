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
using Windows.ApplicationModel.Resources.Core;
using System.Threading.Tasks;
using Herafi.Core.Helpers;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Herafi.UWP.Resources.Templates.CraftmenTemplates
{
    public sealed partial class GeneralProjectTemp : UserControl,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Project _project;

       
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GeneralProjectTemp()
        {
            this.InitializeComponent();

            // Subscribe to the event that's raised when a qualifier value changes.
            ResourceContext.GetForCurrentView().QualifierValues.MapChanged += new MapChangedEventHandler<string, string>(QualifierValues_MapChanged);
            this.Loading += GeneralProjectTemp_Loading;

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

        private async void GeneralProjectTemp_Loading(FrameworkElement sender, object args)
        {
            await ActivateRefreshUITexts();
        }

        #endregion


        public Project Project
        {
            get { return this.DataContext as Project; }
            set { _project = this.DataContext as Project; OnPropertyChanged(); }
        }
    }
}
