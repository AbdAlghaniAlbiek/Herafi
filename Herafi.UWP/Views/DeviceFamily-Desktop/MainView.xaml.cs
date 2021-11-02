using Herafi.Core.Helpers;
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
using Windows.ApplicationModel.Background;
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
    public sealed partial class MainView : Page,INotifyPropertyChanged
    {
        #region Fields
        private string _navViewItemName;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public string NavViewItemName
        {
            get { return _navViewItemName; }
            set 
            {
                _navViewItemName = value;
                OnPropertyChanged();
            }
        }

        private bool IsNetworkAvailable()
        {
            if (Common.App_Environment == Choices.ChooseAppEnvironment(AppEnvironment.Development)) //Development
            {
                return NetworkInterface.GetIsNetworkAvailable();
            }
            else //Production
            {
                return NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable;
            }
        }
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void RegisterBackgroundService()
        {
            // Check for background access (optional)
            await BackgroundExecutionManager.RequestAccessAsync();

            if (BackgroundTaskHelper.IsBackgroundTaskRegistered("NewMembers"))
            {
                return;
            }
            else
            {
                BackgroundTaskHelper.Register(
                    nameof(Herafi.Background.Notifications.NewMembers),
                    "Herafi.Background.Notifications.NewMembers",
                    new TimeTrigger(15, false),
                    false,
                    true,
                    new SystemCondition(SystemConditionType.InternetAvailable));
            }
        }


        public MainView()
        {
            this.InitializeComponent();

            this.Loading += MainView_Loading;
            NetworkHelper.Instance.NetworkChanged += NetworkChanged;

            NavigationHelper.ContentFrame = contentFrame;

            btnCaViErrCancel.Click += new RoutedEventHandler( (s, e) => errorDetailsCardView.IsOpen = false);
            btnCaViAppCancel.Click += new RoutedEventHandler((s, e) => exitAppCardView.IsOpen = false);
            btnCaViAppOk.Click += new RoutedEventHandler( (s, e) => App.Current.Exit());

            // Subscribe to the event that's raised when a qualifier value changes.
            ResourceContext.GetForCurrentView().QualifierValues.MapChanged += new MapChangedEventHandler<string, string>(QualifierValues_MapChanged);

            InitializeNavigationViewItems_PointerHandlers();

            EnablingAllKindsOfGoBackStates();

            RegisterBackgroundService();
        }


        #region Translating UI

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

        private void RefreshUIText()
        {
            txtProjectName.Text = 
                TranslatingUI.MainResourceMap.GetValue("txtProjectName", TranslatingUI.ResourceContextObj).ValueAsString;
            dashboardItemText.Text =
                TranslatingUI.MainResourceMap.GetValue("dashboardItemText", TranslatingUI.ResourceContextObj).ValueAsString;
            craftmenItemText.Text =
                TranslatingUI.MainResourceMap.GetValue("craftmenItemText", TranslatingUI.ResourceContextObj).ValueAsString;
            usersItemText.Text =
                 TranslatingUI.MainResourceMap.GetValue("usersItemText", TranslatingUI.ResourceContextObj).ValueAsString;
            analyzesItemText.Text =
                 TranslatingUI.MainResourceMap.GetValue("analyzesItemText", TranslatingUI.ResourceContextObj).ValueAsString;
            settingsItemText.Text =
                TranslatingUI.MainResourceMap.GetValue("settingsItemText", TranslatingUI.ResourceContextObj).ValueAsString;
            txtAppVersion.Text =
                TranslatingUI.MainResourceMap.GetValue("txtAppVersion", TranslatingUI.ResourceContextObj).ValueAsString;


            errorDetailsCardView.Header =
                TranslatingUI.MainResourceMap.GetValue("errorDetailsCardViewHeader", TranslatingUI.ResourceContextObj).ValueAsString;
            txtCaViErroCancel.Text =
                 TranslatingUI.MainResourceMap.GetValue("txtCaViErroCancel", TranslatingUI.ResourceContextObj).ValueAsString;
            exitAppCardView.Header =
                TranslatingUI.MainResourceMap.GetValue("exitAppCardViewHeader", TranslatingUI.ResourceContextObj).ValueAsString;
            txtCaViAppCancel.Text =
                TranslatingUI.MainResourceMap.GetValue("txtCaViAppCancel", TranslatingUI.ResourceContextObj).ValueAsString;
            txtCaViAppOk.Text =
                TranslatingUI.MainResourceMap.GetValue("txtCaViAppOk", TranslatingUI.ResourceContextObj).ValueAsString;


            //For transalting the header of the view
            if (NavViewItemName == "Dashboard" || NavViewItemName == "لوحة القيادة")
            {
                NavViewItemName = dashboardItemText.Text;
            }
            else if (NavViewItemName == "Craftmen" || NavViewItemName == "حرفيين")
            {
                NavViewItemName = craftmenItemText.Text;
            }
            else if (NavViewItemName == "Users" || NavViewItemName == "مستخدمين")
            {
                NavViewItemName = usersItemText.Text;
            }
            else if (NavViewItemName == "Settings" || NavViewItemName == "إعدادات")
            {
                NavViewItemName = settingsItemText.Text;
            }
            else if (NavViewItemName == "Analyzes" || NavViewItemName == "تحليلات")
            {
                NavViewItemName = analyzesItemText.Text;
            }
        }

        #endregion


        #region Page Loading and Network change event 
        //Using loading event to direct sign in admin if he has an id stored in the settings
        private async Task SignInAdminDirectly()
        {
            try
            {
                //to make direct sign in if this admin have admin id previously
                await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
                    if (!string.IsNullOrEmpty(Settings.AdminId))
                    {
                        if (string.IsNullOrEmpty(Common.ADMIN_ID))
                        {
                            if (IsNetworkAvailable())
                            {
                                var httpResponse =
                                        await SignInRepo.DirectSignInAdminAsync(Settings.AdminId);

                                if (httpResponse.Response.Result != null)
                                {
                                    Common.ADMIN_ID = Settings.AdminId;

                                    contentFrame.Navigate(typeof(DashboardView));
                                }
                                else
                                {
                                    errorDetailsCardViewMessage.Text = httpResponse.ErrorMessage;
                                    errorDetailsCardView.IsOpen = true;
                                }
                            }
                            else
                            {
                                errorDetailsCardViewMessage.Text = TranslatingUI.MainResourceMap.GetValue("txtToastNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                                errorDetailsCardView.IsOpen = true;
                            }
                        }
                    }


                    if (contentFrame.BackStackDepth == 0)
                    {
                        contentFrame.Navigate(typeof(DashboardView));
                    }
                },
                Windows.UI.Core.CoreDispatcherPriority.High);
            }
            catch (Exception ex)
            {
                await new Windows.UI.Popups.MessageDialog(ex.Message).ShowAsync();
            }
            
            
        }

        private async void MainView_Loading(FrameworkElement sender, object args)
        {
            await ActivateRefreshUITexts();

            (navViewItemDashboard.Resources["navViewItemDashboardPressed"] as Storyboard).Begin();
            NavViewItemName = dashboardItemText.Text;

            await SignInAdminDirectly();
        }

        private async void NetworkChanged(object sender, EventArgs e)
        {
            await SignInAdminDirectly();
        }

        #endregion


        #region EnablingGoBackStates

        private void EnablingAllKindsOfGoBackStates()
        {
            var goBack = new KeyboardAccelerator();
            goBack.Key = Windows.System.VirtualKey.GoBack;
            goBack.Invoked += GoBack_Invoked;

            var altLeft = new KeyboardAccelerator();
            altLeft.Key = Windows.System.VirtualKey.Left;
            altLeft.Modifiers = Windows.System.VirtualKeyModifiers.Menu;
            altLeft.Invoked += AltLeft_Invoked;

            btnGoBack.Click += new RoutedEventHandler(BtnGoBack_Click);
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            GoBack_Invoked();
        }

        private void AltLeft_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            GoBack_Invoked();
        }

        private void GoBack_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            GoBack_Invoked();
        }

        private void GoBack_Invoked()
        {
            if (contentFrame.CanGoBack)
            {
                contentFrame.GoBack();

                if (!contentFrame.CanGoBack)
                {
                    btnGoBack.Visibility = Visibility.Collapsed;
                }
            }
        }

        #endregion


        #region InitializeNavigationViewItems Pointer events

        private void InitializeNavigationViewItems_PointerHandlers()
        {
            //Dashboard navigation view item pointer events handlers
            navViewItemDashboard.PointerEntered += new PointerEventHandler(NavViewItemDashboard_PointerEntered);
            navViewItemDashboard.PointerExited += new PointerEventHandler(NavViewItemDashboard_PointerExited);
            navViewItemDashboard.PointerPressed += new PointerEventHandler(NavViewItemDashboard_PointerPressed);

            //Craftmen navigation view item pointer events handlers
            navViewItemCraftmen.PointerEntered += new PointerEventHandler(NavViewItemCraftmen_PointerEntered);
            navViewItemCraftmen.PointerExited += new PointerEventHandler(NavViewItemCraftmen_PointerExited);
            navViewItemCraftmen.PointerPressed += new PointerEventHandler(NavViewItemCraftmen_PointerPressed);

            //Users navigation view item pointer events handlers
            navViewItemUsers.PointerEntered += new PointerEventHandler(NavViewItemUsers_PointerEntered);
            navViewItemUsers.PointerExited += new PointerEventHandler(NavViewItemUsers_PointerExited);
            navViewItemUsers.PointerPressed += new PointerEventHandler(NavViewItemUsers_PointerPressed);

            //Analyzes navigation view item pointer events handlers
            navViewItemAnalyzes.PointerEntered += new PointerEventHandler(NavViewItemAnalyzes_PointerEntered);
            navViewItemAnalyzes.PointerExited += new PointerEventHandler(NavViewItemAnalyzes_PointerExited);
            navViewItemAnalyzes.PointerPressed += new PointerEventHandler(NavViewItemAnalyzes_PointerPressed);

            //Settings navigation view item pointer events handlers
            navViewItemSettings.PointerEntered += new PointerEventHandler(NavViewItemSettings_PointerEntered);
            navViewItemSettings.PointerExited += new PointerEventHandler(NavViewItemSettings_PointerExited); 
            navViewItemSettings.PointerPressed += new PointerEventHandler(NavViewItemSettings_PointerPressed);
        }

        #region Settings

        private void NavViewItemSettings_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (NavViewItemName != TranslatingUI.MainResourceMap.GetValue("settingsItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (navViewItemSettings.Resources["navViewItemSettingsPointerOver"] as Storyboard).Begin();
            }
        }

        private void NavViewItemSettings_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (NavViewItemName != TranslatingUI.MainResourceMap.GetValue("settingsItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (navViewItemSettings.Resources["navViewItemSettingsNormal"] as Storyboard).Begin();
            }
        }

        private void NavViewItemSettings_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (NavViewItemName != TranslatingUI.MainResourceMap.GetValue("settingsItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (navViewItemDashboard.Resources["navViewItemDashboardNormal"] as Storyboard).Begin();
                (navViewItemCraftmen.Resources["navViewItemCraftmenNormal"] as Storyboard).Begin();
                (navViewItemUsers.Resources["navViewItemUsersNormal"] as Storyboard).Begin();
                (navViewItemAnalyzes.Resources["navViewItemAnalyzesNormal"] as Storyboard).Begin();

                (navViewItemSettings.Resources["navViewItemSettingsPressed"] as Storyboard).Begin();
                NavViewItemName = settingsItemText.Text;

                //Navigating to Settings
                contentFrame.Navigate(typeof(SettingsView));

                if (btnGoBack.Visibility == Visibility.Collapsed)
                {
                    btnGoBack.Visibility = Visibility.Visible;
                }
            }
        }

        #endregion

        #region Analyzes

        private void NavViewItemAnalyzes_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (NavViewItemName != TranslatingUI.MainResourceMap.GetValue("analyzesItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (navViewItemAnalyzes.Resources["navViewItemAnalyzesPointerOver"] as Storyboard).Begin();
            }
        }

        private void NavViewItemAnalyzes_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (NavViewItemName != TranslatingUI.MainResourceMap.GetValue("analyzesItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (navViewItemAnalyzes.Resources["navViewItemAnalyzesNormal"] as Storyboard).Begin();
            }
        }

        private void NavViewItemAnalyzes_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (NavViewItemName != TranslatingUI.MainResourceMap.GetValue("analyzesItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (navViewItemDashboard.Resources["navViewItemDashboardNormal"] as Storyboard).Begin();
                (navViewItemCraftmen.Resources["navViewItemCraftmenNormal"] as Storyboard).Begin();
                (navViewItemUsers.Resources["navViewItemUsersNormal"] as Storyboard).Begin();
                (navViewItemSettings.Resources["navViewItemSettingsNormal"] as Storyboard).Begin();

                (navViewItemAnalyzes.Resources["navViewItemAnalyzesPressed"] as Storyboard).Begin();
                NavViewItemName = analyzesItemText.Text;

                //Navigating to Analyzes
                contentFrame.Navigate(typeof(AnalyzesView));

                if (btnGoBack.Visibility == Visibility.Collapsed)
                {
                    btnGoBack.Visibility = Visibility.Visible;
                }
            }
        }

        #endregion

        #region Users

        private void NavViewItemUsers_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (NavViewItemName != TranslatingUI.MainResourceMap.GetValue("usersItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (navViewItemUsers.Resources["navViewItemUsersPointerOver"] as Storyboard).Begin();
            }
        }

        private void NavViewItemUsers_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (NavViewItemName != TranslatingUI.MainResourceMap.GetValue("usersItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (navViewItemUsers.Resources["navViewItemUsersNormal"] as Storyboard).Begin();
            }
        }

        private void NavViewItemUsers_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (NavViewItemName != TranslatingUI.MainResourceMap.GetValue("usersItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (navViewItemDashboard.Resources["navViewItemDashboardNormal"] as Storyboard).Begin();
                (navViewItemCraftmen.Resources["navViewItemCraftmenNormal"] as Storyboard).Begin();
                (navViewItemAnalyzes.Resources["navViewItemAnalyzesNormal"] as Storyboard).Begin();
                (navViewItemSettings.Resources["navViewItemSettingsNormal"] as Storyboard).Begin();

                (navViewItemUsers.Resources["navViewItemUsersPressed"] as Storyboard).Begin();
                NavViewItemName = usersItemText.Text;

                //Navigating to Users
                contentFrame.Navigate(typeof(UsersView));

                if (btnGoBack.Visibility == Visibility.Collapsed)
                {
                    btnGoBack.Visibility = Visibility.Visible;
                }
            }
        }

        #endregion

        #region Craftmen

        private void NavViewItemCraftmen_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (NavViewItemName != TranslatingUI.MainResourceMap.GetValue("craftmenItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (navViewItemCraftmen.Resources["navViewItemCraftmenPointerOver"] as Storyboard).Begin();
            }
        }

        private void NavViewItemCraftmen_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (NavViewItemName != TranslatingUI.MainResourceMap.GetValue("craftmenItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (navViewItemCraftmen.Resources["navViewItemCraftmenNormal"] as Storyboard).Begin();
            }
        }

        private void NavViewItemCraftmen_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (NavViewItemName != TranslatingUI.MainResourceMap.GetValue("craftmenItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (navViewItemDashboard.Resources["navViewItemDashboardNormal"] as Storyboard).Begin();
                (navViewItemUsers.Resources["navViewItemUsersNormal"] as Storyboard).Begin();
                (navViewItemAnalyzes.Resources["navViewItemAnalyzesNormal"] as Storyboard).Begin();
                (navViewItemSettings.Resources["navViewItemSettingsNormal"] as Storyboard).Begin();

                (navViewItemCraftmen.Resources["navViewItemCraftmenPressed"] as Storyboard).Begin();
                NavViewItemName = craftmenItemText.Text;

                //Navigating to Craftmen
                contentFrame.Navigate(typeof(CraftmenView));

                if (btnGoBack.Visibility == Visibility.Collapsed)
                {
                    btnGoBack.Visibility = Visibility.Visible;
                }
            }
        }

        #endregion

        #region Dashboard

        private void NavViewItemDashboard_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (NavViewItemName != TranslatingUI.MainResourceMap.GetValue("dashboardItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (navViewItemDashboard.Resources["navViewItemDashboardPointerOver"] as Storyboard).Begin();
            }
        }

        private void NavViewItemDashboard_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (NavViewItemName != TranslatingUI.MainResourceMap.GetValue("dashboardItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (navViewItemDashboard.Resources["navViewItemDashboardNormal"] as Storyboard).Begin();
            }
        }

        private void NavViewItemDashboard_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (NavViewItemName != TranslatingUI.MainResourceMap.GetValue("dashboardItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (navViewItemCraftmen.Resources["navViewItemCraftmenNormal"] as Storyboard).Begin();
                (navViewItemUsers.Resources["navViewItemUsersNormal"] as Storyboard).Begin();
                (navViewItemAnalyzes.Resources["navViewItemAnalyzesNormal"] as Storyboard).Begin();
                (navViewItemSettings.Resources["navViewItemSettingsNormal"] as Storyboard).Begin();

                (navViewItemDashboard.Resources["navViewItemDashboardPressed"] as Storyboard).Begin();
                NavViewItemName = dashboardItemText.Text;

                //Navigating to Dashboard
                contentFrame.Navigate(typeof(DashboardView));

                if (btnGoBack.Visibility == Visibility.Collapsed)
                {
                    btnGoBack.Visibility = Visibility.Visible;
                }
            }
        }

        #endregion

        #endregion

    }
}
