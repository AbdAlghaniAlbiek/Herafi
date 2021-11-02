using Herafi.Core.Helpers;
using Herafi.Core.Models;
using Herafi.Core.Repositories.RemoteRepo;
using Herafi.Core.Security;
using Microsoft.Toolkit.Uwp.Connectivity;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Herafi.UWP.Views.DeviceFamily_Desktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashboardView : Page,INotifyPropertyChanged
    {
        #region Fields
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _system;
        private bool _dark;
        private bool _light;
        #endregion

        #region Properties
        public bool IsSystem
        {
            get { return _system; }
            set { _system = value; OnPropertyChanged(); }
        }
        public bool IsLight
        {
            get { return _light; }
            set { _light = value; OnPropertyChanged(); }
        }
        public bool IsDark
        {
            get { return _dark; }
            set { _dark = value; OnPropertyChanged(); }
        }
        #endregion


        private bool IsNetworkAvailable()
        {
            if (Common.App_Environment == Choices.ChooseAppEnvironment(AppEnvironment.Development)) //Development mode
            {
                return NetworkInterface.GetIsNetworkAvailable();
            }
            else //Production mode
            {
                return NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable;
            }
        }
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public DashboardView()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            //Initializing pointer pressed for all buttons of this view
            Initialize_PointerEvents();

            // Subscribe to the event that's raised when a qualifier value changes.
            ResourceContext.GetForCurrentView().QualifierValues.MapChanged += new MapChangedEventHandler<string, string>(QualifierValues_MapChanged);
        }

        private void Initialize_PointerEvents()
        {
            this.Loading += DashboardView_Loading;
            NetworkHelper.Instance.NetworkChanged += NetworkChanged;
            btnNoWifiDashboard.Click += new RoutedEventHandler(BtnNoWifiDashboard_Click);
            btnCaViErrCancel.Click += new RoutedEventHandler((s, e) => errorDetailsCardView.IsOpen = false);


            //System control events
            systemTheme.PointerEntered += new PointerEventHandler(SystemTheme_PointerEntered);
            systemTheme.PointerExited += new PointerEventHandler(SystemTheme_PointerExited);
            systemTheme.PointerPressed += new PointerEventHandler(SystemTheme_PointerPressed);

            //Light control events
            lightTheme.PointerEntered += new PointerEventHandler(LightTheme_PointerEntered);
            lightTheme.PointerExited += new PointerEventHandler(LightTheme_PointerExited);
            lightTheme.PointerPressed += new PointerEventHandler(LightTheme_PointerPressed);

            //Dark control events
            darkTheme.PointerEntered += new PointerEventHandler(DarkTheme_PointerEntered);
            darkTheme.PointerExited += new PointerEventHandler(DarkTheme_PointerExited);
            darkTheme.PointerPressed += new PointerEventHandler(DarkTheme_PointerPressed);


            //New users and craftmen events and For more info hyper button event
            gridNewUser.PointerPressed += new PointerEventHandler(GridNewUser_PointerPressed);
            gridNewCraftmen.PointerPressed += new PointerEventHandler(GridNewCraftmen_PointerPressed);
            hypBtnMoreInfo.Click += new RoutedEventHandler(HypBtnMoreInfo_Click);
        }
       

        #region Translating UI

        private async Task ActivateRefreshUIText()
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
            await ActivateRefreshUIText();
        }

        private void RefreshUIText()
        {
            txtDark.Text =
              TranslatingUI.DashboardResourceMap.GetValue("txtDark", TranslatingUI.ResourceContextObj).ValueAsString;
            txtLight.Text =
              TranslatingUI.DashboardResourceMap.GetValue("txtLight", TranslatingUI.ResourceContextObj).ValueAsString;
            txtSystem.Text =
              TranslatingUI.DashboardResourceMap.GetValue("txtSystem", TranslatingUI.ResourceContextObj).ValueAsString;
            txtProfitsStatusDesc.Text =
              TranslatingUI.DashboardResourceMap.GetValue("txtProfitsStatusDesc", TranslatingUI.ResourceContextObj).ValueAsString;
            txtHypBtnMoreInfo.Text =
              TranslatingUI.DashboardResourceMap.GetValue("txtHypBtnMoreInfo", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewCraftmenTitle.Text =
              TranslatingUI.DashboardResourceMap.GetValue("txtNewCraftmenTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNewUsersTitle.Text =
              TranslatingUI.DashboardResourceMap.GetValue("txtNewUsersTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            txtProfitsCalcPerDay.Text =
              TranslatingUI.DashboardResourceMap.GetValue("txtProfitsCalcPerDay", TranslatingUI.ResourceContextObj).ValueAsString;
            txtProfitsCalcPerHour.Text =
              TranslatingUI.DashboardResourceMap.GetValue("txtProfitsCalcPerHour", TranslatingUI.ResourceContextObj).ValueAsString;
            txtProfitsCalcTitle.Text =
              TranslatingUI.DashboardResourceMap.GetValue("txtProfitsCalcTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            txtProfitsTitle.Text =
              TranslatingUI.DashboardResourceMap.GetValue("txtProfitsTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            txtThemesTitle.Text =
              TranslatingUI.DashboardResourceMap.GetValue("txtThemesTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            txtProfitsCalcTotal.Text =
              TranslatingUI.DashboardResourceMap.GetValue("txtProfitsCalcTotal", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNoWifiDashboard.Text =
                TranslatingUI.DashboardResourceMap.GetValue("txtNoWifiDashboard", TranslatingUI.ResourceContextObj).ValueAsString;
            txtBtnNoWifiDashboard.Text =
                TranslatingUI.DashboardResourceMap.GetValue("txtBtnNoWifiDashboard", TranslatingUI.ResourceContextObj).ValueAsString;
            errorDetailsCardView.Header =
                TranslatingUI.DashboardResourceMap.GetValue("errorDetailsCardView", TranslatingUI.ResourceContextObj).ValueAsString;
            txtCaViErrCancel.Text =
                TranslatingUI.DashboardResourceMap.GetValue("txtCaViErrCancel", TranslatingUI.ResourceContextObj).ValueAsString;
        }

        #endregion


        #region Networking change and page Loading event 

        private async Task RefreshDashboardContent()
        {
            try
            {
                if (dashboardViewModel.NewMembers == null && dashboardViewModel.ProfitDetails == null)
                {
                    if (IsNetworkAvailable())
                    {
                        await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                        {
                            if (NoWifiDashboard.Visibility == Visibility.Visible)
                            {
                                NoWifiDashboard.Visibility = Visibility.Collapsed;

                                fixedToastNotificationMessage.Text =
                                    TranslatingUI.UsersResourceMap.GetValue("txtToastConnWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                                fixedToastNotification.Show(6000);
                            }

                            loadingData.IsLoading = true;

                            HttpReponse profDetHttpReponse =
                               await DashboardRepo.GetProfitsDetailsAsync();

                            HttpReponse newMemHttpReponse =
                               await DashboardRepo.GetNewMembersAsync();

                            loadingData.IsLoading = false;


                            if (newMemHttpReponse.Response.Result != null && profDetHttpReponse.Response.Result != null)
                            {
                                dashboardViewModel.ProfitDetails =
                                    (ProfitDetails)profDetHttpReponse.Response.Result;

                                dashboardViewModel.NewMembers =
                                    (NewMembers)newMemHttpReponse.Response.Result;
                            }
                            else
                            {
                                txtProfDetError.Text = !string.IsNullOrEmpty(profDetHttpReponse.ErrorMessage) ? profDetHttpReponse.ErrorMessage : "";
                                txtNewMemError.Text = !string.IsNullOrEmpty(newMemHttpReponse.ErrorMessage) ? newMemHttpReponse.ErrorMessage : "";

                                errorDetailsCardView.IsOpen = true;
                            }
                        });
                    }
                    else
                    {
                        NoWifiDashboard.Visibility = Visibility.Visible;
                    }
                }

            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
        }

        private async void NetworkChanged(object sender, EventArgs e)
        {
            await RefreshDashboardContent();
        }

        private async void BtnNoWifiDashboard_Click(object sender, RoutedEventArgs e)
        {
            await RefreshDashboardContent();
        }

        private async void DashboardView_Loading(FrameworkElement sender, object args)
        {
            //Translating this view
            await ActivateRefreshUIText();

            //Themes
            if (Settings.Theme == Choices.ChooseTheme(Themes.Light))
            {
                (lightTheme.Resources["LightThemePressed"] as Storyboard).Begin();
                IsLight = true;
            }
            else if (Settings.Theme == Choices.ChooseTheme(Themes.Dark))
            {
                (darkTheme.Resources["DarkThemePressed"] as Storyboard).Begin();
                IsDark = true;
            }
            else //System
            {
                (systemTheme.Resources["SystemThemePressed"] as Storyboard).Begin();
                IsSystem = true;
            }

            await RefreshDashboardContent();
        }

        #endregion

        #region Navigation from current page to another pages

        private void HypBtnMoreInfo_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.ContentFrame.Navigate(typeof(AnalyzesView), null, new EntranceNavigationTransitionInfo());
        }

        private void GridNewCraftmen_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            NavigationHelper.ContentFrame.Navigate(typeof(CraftmenView), "Craftmen", new EntranceNavigationTransitionInfo());
        }

        private void GridNewUser_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            NavigationHelper.ContentFrame.Navigate(typeof(UsersView), "Users", new EntranceNavigationTransitionInfo());
        }

        #endregion


        #region System theme

        private void SystemTheme_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!IsSystem)
            {
                (systemTheme.Resources["SystemThemeNormal"] as Storyboard).Begin();
            }
        }

        private void SystemTheme_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!IsSystem)
            {
                (systemTheme.Resources["SystemThemePointerOver"] as Storyboard).Begin();
            }
        }

        private void SystemTheme_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (IsLight)
            {
                (lightTheme.Resources["LightThemeNormal"] as Storyboard).Begin();
                IsLight = false;
            }
            else if (IsDark)
            {
                (darkTheme.Resources["DarkThemeNormal"] as Storyboard).Begin();
                IsDark = false;
            }

            (systemTheme.Resources["SystemThemePressed"] as Storyboard).Begin();
            IsSystem = true;


            if (Window.Current.Content is FrameworkElement frameworkElement)
            {
                frameworkElement.RequestedTheme = ElementTheme.Default;
            }
            Settings.Theme = Choices.ChooseTheme(Themes.System);

        }

        #endregion

        #region Light theme

        private void LightTheme_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!IsLight)
            {
                (lightTheme.Resources["LightThemeNormal"] as Storyboard).Begin();
            }
        }

        private void LightTheme_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!IsLight)
            {
                (lightTheme.Resources["LightThemePointerOver"] as Storyboard).Begin();
            }
        }

        private void LightTheme_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (IsSystem)
            {
                (systemTheme.Resources["SystemThemeNormal"] as Storyboard).Begin();
                IsSystem = false;
            }
            else if (IsDark)
            {
                (darkTheme.Resources["DarkThemeNormal"] as Storyboard).Begin();
                IsDark = false;
            }

            (lightTheme.Resources["LightThemePressed"] as Storyboard).Begin();
            IsLight = true;


            if (Window.Current.Content is FrameworkElement frameworkElement)
            {
                frameworkElement.RequestedTheme = ElementTheme.Light;
            }
            Settings.Theme = Choices.ChooseTheme(Themes.Light);

        }

        #endregion

        #region Dark theme

        private void DarkTheme_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!IsDark)
            {
                (darkTheme.Resources["DarkThemeNormal"] as Storyboard).Begin();
            }
        }

        private void DarkTheme_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!IsDark)
            {
                (darkTheme.Resources["DarkThemePointerOver"] as Storyboard).Begin();
            }
        }

        private void DarkTheme_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (IsSystem)
            {
                (systemTheme.Resources["SystemThemeNormal"] as Storyboard).Begin();
                IsSystem = false;
            }
            else if (IsLight)
            {
                (lightTheme.Resources["LightThemeNormal"] as Storyboard).Begin();
                IsLight = false;
            }

            (darkTheme.Resources["DarkThemePressed"] as Storyboard).Begin();
            IsDark = true;


            if (Window.Current.Content is FrameworkElement frameworkElement)
            {
                frameworkElement.RequestedTheme = ElementTheme.Dark;
            }
            Settings.Theme = Choices.ChooseTheme(Themes.Dark);
        }

        #endregion
    }
}
