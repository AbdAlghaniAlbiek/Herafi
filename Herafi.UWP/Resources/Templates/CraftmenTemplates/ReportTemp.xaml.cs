using Herafi.Core.Helpers;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Herafi.UWP.Resources.Templates.CraftmenTemplates
{
    public sealed partial class ReportTemp : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Report _report;

      
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ReportTemp()
        {
            this.InitializeComponent();

            // Subscribe to the event that's raised when a qualifier value changes.
            ResourceContext.GetForCurrentView().QualifierValues.MapChanged += new MapChangedEventHandler<string, string>(QualifierValues_MapChanged);
            this.Loading += ReportTemp_Loading;


            this.DataContextChanged += (s, e) => Bindings.Update();
        }


        #region Translating UI

        private void RefreshUIText()
        {
            txtRepCardUserName.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtRepCardUserName", TranslatingUI.ResourceContextObj).ValueAsString;
            txtRepCardReqID.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtRepCardReqID", TranslatingUI.ResourceContextObj).ValueAsString;
            txtRepCardProbRel.Text =
              TranslatingUI.CraftmenResourceMap.GetValue("txtRepCardProbRel", TranslatingUI.ResourceContextObj).ValueAsString;
            txtRepCardReport.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtRepCardReport", TranslatingUI.ResourceContextObj).ValueAsString;
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

        private async void ReportTemp_Loading(FrameworkElement sender, object args)
        {
            await ActivateRefreshUITexts();
        }

        #endregion


        public Report Report
        {
            get { return this.DataContext as Report; }
            set { _report = this.DataContext as Report; OnPropertyChanged(); }
        }
    }
}
