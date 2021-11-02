using Herafi.Core.Helpers;
using Herafi.Core.Models;
using Herafi.Core.Security;
using Microsoft.Toolkit.Uwp.Connectivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
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
using Herafi.Core.Repositories.RemoteRepo;
using Herafi.UWP.Views.DeviceFamily_Desktop;
using Windows.ApplicationModel.Resources.Core;
using System.Threading.Tasks;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Herafi.UWP.Resources.Templates.CraftmenTemplates
{
    public sealed partial class ReportCraftmanTemp : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ReportCraftman _reportCraftman;
        private string _buttonText;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ReportCraftmanTemp()
        {
            this.InitializeComponent();

            this.DataContextChanged += (s, e) => Bindings.Update();

            // Subscribe to the event that's raised when a qualifier value changes.
            ResourceContext.GetForCurrentView().QualifierValues.MapChanged += new MapChangedEventHandler<string, string>(QualifierValues_MapChanged);
            this.Loading += ReportCraftmanTemp_Loading; ;


            btnBloRefCraftman.Click += new RoutedEventHandler(BtnBloRefCraftman_Click);
        }
      

        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when user clicks button")]
        public event EventHandler ReportCraftmanEvent;


        private void BtnBloRefCraftman_Click(object sender, RoutedEventArgs e)
        {
            //bubble the event up to the parent
            if (this.ReportCraftmanEvent != null)
                this.ReportCraftmanEvent(this, null);
        }

        public ReportCraftman ReportCraftman
        {
            get { return this.DataContext as ReportCraftman; }
            set { _reportCraftman = this.DataContext as ReportCraftman; OnPropertyChanged(); }
        }

        public string ButtonText
        {
            get { return _buttonText; }
            set { _buttonText = value; OnPropertyChanged(); }
        }

        #region Translating UI

        private void RefreshUIText()
        {
            txtRepCardID.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtRepCardID", TranslatingUI.ResourceContextObj).ValueAsString;
            txtRepCardShMoreInfo.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtRepCardShMoreInfo", TranslatingUI.ResourceContextObj).ValueAsString;

            if (ButtonText == "Block" || ButtonText == "إيقاف")
            {
                ButtonText = TranslatingUI.CraftmenResourceMap.GetValue("txtBtnRepBlockText", TranslatingUI.ResourceContextObj).ValueAsString;
            }
            else
            {
                ButtonText = TranslatingUI.CraftmenResourceMap.GetValue("txtBtnRepFiringText", TranslatingUI.ResourceContextObj).ValueAsString;
            }
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

        private async void ReportCraftmanTemp_Loading(FrameworkElement sender, object args)
        {
            await ActivateRefreshUITexts();
        }

        #endregion
    }
}
