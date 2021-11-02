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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Herafi.Core.Models;
using Herafi.Core.Repositories.RemoteRepo;
using Herafi.Core.Helpers;
using Herafi.Core.Security;
using System.Net.NetworkInformation;
using Microsoft.Toolkit.Uwp.Connectivity;
using Microsoft.Toolkit.Uwp.Helpers;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Helpers;
using Windows.UI.Xaml.Markup;
using Windows.ApplicationModel.Resources.Core;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Background;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Herafi.UWP.Views.DeviceFamily_Desktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsView : Page,INotifyPropertyChanged
    {
        #region Fields
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _system;
        private bool _dark;
        private bool _light;
        private bool _isArabic;
        private bool _isEnglish;
        #endregion

        #region Properties

        public bool IsArabic
        {
            get { return _isArabic; }
            set { _isArabic = value; OnPropertyChanged(); }
        }
        public bool IsEnglish
        {
            get { return _isEnglish; }
            set { _isEnglish = value; OnPropertyChanged(); }
        }
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
        Geometry PathMarkupToGeometry(string pathMarkup)
        {
            string xaml =
            "<Path " +
            "xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>" +
            "<Path.Data>" + pathMarkup + "</Path.Data></Path>";
            var path = XamlReader.Load(xaml) as Windows.UI.Xaml.Shapes.Path;
            // Detach the PathGeometry from the Path
            Geometry geometry = path.Data;
            path.Data = null;
            return geometry;
        }
        
        public SettingsView()
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
            this.Loading += SettingsView_Loading;
            NetworkHelper.Instance.NetworkChanged += NetworkChanged;


            //System control events
            systemTheme.PointerEntered += new PointerEventHandler(SystemTheme_PointerEntered);
            systemTheme.PointerExited += new PointerEventHandler(SystemTheme_PointerExited);
            systemTheme.PointerPressed += new PointerEventHandler(SystemTheme_PointerPressed);

            //Light control events
            lightTheme.PointerEntered += new PointerEventHandler(LightTheme_PointerEntered);
            lightTheme.PointerExited += new PointerEventHandler(LightTheme_PointerExited);
            lightTheme.PointerPressed += new PointerEventHandler(LightTheme_PointerPressed);

            //Light control events
            darkTheme.PointerEntered += new PointerEventHandler(DarkTheme_PointerEntered);
            darkTheme.PointerExited += new PointerEventHandler(DarkTheme_PointerExited);
            darkTheme.PointerPressed += new PointerEventHandler(DarkTheme_PointerPressed);

            //English control events
            englishLanguage.PointerEntered += new PointerEventHandler(EnglishLanguage_PointerEntered);
            englishLanguage.PointerExited += new PointerEventHandler(EnglishLanguage_PointerExited);
            englishLanguage.PointerPressed += new PointerEventHandler(EnglishLanguage_PointerPressed);

            //Arabic control events
            arabicLanguage.PointerEntered += new PointerEventHandler(ArabicLanguage_PointerEntered);
            arabicLanguage.PointerExited += new PointerEventHandler(ArabicLanguage_PointerExited);
            arabicLanguage.PointerPressed += new PointerEventHandler(ArabicLanguage_PointerPressed);

            //No wifi, modify events
            btnNoWifiSettings.Click += new RoutedEventHandler(BtnNoWifiSettings_Click);
            btnModify.Click += new RoutedEventHandler(BtnModify_Click);

            //sign out process events
            btnSignOut.Click += new RoutedEventHandler(BtnSignOut_Click);
            btnCaViSignOutCancel.Click += new RoutedEventHandler((s, e) => sigOutCardView.IsOpen = false);
            btnCaViSignOutOk.Click += new RoutedEventHandler(BtnCaViSignOutOk_Click);

            //Adding accounts (Facebook, Microsoft) account
            patIcoFacebook.PointerPressed += new PointerEventHandler(PatIcoFacebook_PointerPressed);
            patIcoMicrosoft.PointerPressed += new PointerEventHandler(PatIcoMicrosoft_PointerPressed);
            patIcoYourEmail.PointerPressed += new PointerEventHandler(PatIcoYourEmail_PointerPressed);

            //Sound enable toggle and Spatial audio checkbox events
            togSwiSound.Toggled += new RoutedEventHandler(TogSwiSound_Toggled);
            cheBoxSpatialAudio.Click += new RoutedEventHandler(CheBoxSpatialAudio_Click);
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
            this.txtProfileTitle.Text = 
                TranslatingUI.SettingsResourceMap.GetValue("txtProfileTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtName.Text =
                TranslatingUI.SettingsResourceMap.GetValue("txtName", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtEmail.Text =
                TranslatingUI.SettingsResourceMap.GetValue("txtEmail", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtPhoneNumber.Text =
                TranslatingUI.SettingsResourceMap.GetValue("txtPhoneNumber", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtNationalNumber.Text =
                 TranslatingUI.SettingsResourceMap.GetValue("txtNationalNumber", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtDateJoin.Text =
                TranslatingUI.SettingsResourceMap.GetValue("txtDateJoin", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtCity.Text =
                  TranslatingUI.SettingsResourceMap.GetValue("txtCity", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtNoWifiSettings.Text =
                 TranslatingUI.SettingsResourceMap.GetValue("txtNoWifiSettings", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtBtnNoWifiSettings.Text =
                TranslatingUI.SettingsResourceMap.GetValue("txtBtnNoWifiSettings", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtAboutTitle.Text =
                TranslatingUI.SettingsResourceMap.GetValue("txtAboutTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtAboutDesc.Text =
                TranslatingUI.SettingsResourceMap.GetValue("txtAboutDesc", TranslatingUI.ResourceContextObj).ValueAsString;
            this.hypLinkPrivPolic.Content =
                TranslatingUI.SettingsResourceMap.GetValue("hypLinkPrivPolic", TranslatingUI.ResourceContextObj).ValueAsString;
            this.hypLinkCondTerm.Content =
                 TranslatingUI.SettingsResourceMap.GetValue("hypLinkCondTerm", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtThemesTitle.Text =
                TranslatingUI.SettingsResourceMap.GetValue("txtThemesTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtSystem.Text =
                TranslatingUI.SettingsResourceMap.GetValue("txtSystem", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtLight.Text =
                TranslatingUI.SettingsResourceMap.GetValue("txtLight", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtDark.Text =
                TranslatingUI.SettingsResourceMap.GetValue("txtDark", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtSoundTitle.Text =
                TranslatingUI.SettingsResourceMap.GetValue("txtSoundTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            this.cheBoxSpatialAudio.Content =
                TranslatingUI.SettingsResourceMap.GetValue("cheBoxSpatialAudio", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtSignedWithTitle.Text =
                TranslatingUI.SettingsResourceMap.GetValue("txtSignedWithTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtYourEmail.Text =
                TranslatingUI.SettingsResourceMap.GetValue("txtYourEmail", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtFacebook.Text =
                 TranslatingUI.SettingsResourceMap.GetValue("txtFacebook", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtMicrosoft.Text =
                TranslatingUI.SettingsResourceMap.GetValue("txtMicrosoft", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtSignOut.Text =
                TranslatingUI.SettingsResourceMap.GetValue("txtSignOut", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtEnglish.Text =
                TranslatingUI.SettingsResourceMap.GetValue("txtEnglish", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtArabic.Text =
                  TranslatingUI.SettingsResourceMap.GetValue("txtArabic", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtSigOutDesc.Text =
                TranslatingUI.SettingsResourceMap.GetValue("txtSigOutDesc", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtCaViSignOutCancel.Text =
                 TranslatingUI.SettingsResourceMap.GetValue("txtCaViSignOutCancel", TranslatingUI.ResourceContextObj).ValueAsString;
            this.txtCaViSignOutOk.Text =
                TranslatingUI.SettingsResourceMap.GetValue("txtCaViSignOutOk", TranslatingUI.ResourceContextObj).ValueAsString;
            this.sigOutCardView.Header =
                TranslatingUI.SettingsResourceMap.GetValue("sigOutCardViewHeader", TranslatingUI.ResourceContextObj).ValueAsString;
        }

        #endregion


        #region Loading and Network changing and No wifi button 

        private async Task RefreshAdminProfileContent()
        {
            try
            {
                if (settingsViewModel.Admin == null)
                {
                    if (IsNetworkAvailable())
                    {
                        await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                        {
                            if (NoWifiSettings.Visibility == Visibility.Visible)
                            {
                                NoWifiSettings.Visibility = Visibility.Collapsed;

                                fixedToastNotificationMessage.Text =
                                    TranslatingUI.SettingsResourceMap.GetValue("txtToastConnWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                                fixedToastNotification.Show(6000);
                            }

                            loadingData.IsLoading = true;

                            var httpReponse = await SettingsRepo.GetAdminProfileAsync(Common.ADMIN_ID);

                            settingsViewModel.Admin = new Admin();

                            if (httpReponse.Response.Result != null)
                            {
                                settingsViewModel.Admin.Name = ((Admin)httpReponse.Response.Result).Name;
                                settingsViewModel.Admin.Email = ((Admin)httpReponse.Response.Result).Email;
                                settingsViewModel.Admin.PhoneNumber = ((Admin)httpReponse.Response.Result).PhoneNumber;
                                settingsViewModel.Admin.NationalNumber = ((Admin)httpReponse.Response.Result).NationalNumber;
                                settingsViewModel.Admin.DateJoin = ((Admin)httpReponse.Response.Result).DateJoin;
                                settingsViewModel.Admin.City = ((Admin)httpReponse.Response.Result).City;
                                settingsViewModel.Admin.ProfileImage = ((Admin)httpReponse.Response.Result).ProfileImage;
                                settingsViewModel.Admin.PersonalIdentityImage = ((Admin)httpReponse.Response.Result).PersonalIdentityImage;
                            }
                            else
                            {
                                errorToastNotificationMessage.Text = httpReponse.ErrorMessage;
                                errorToastNotification.Show(6000);
                            }

                            loadingData.IsLoading = false;
                        });
                    }
                    else
                    {
                        NoWifiSettings.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
           
        }

        private async void SettingsView_Loading(FrameworkElement sender, object args)
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


            //Sounds
            if (Settings.SoundEnabled)
            {
                ElementSoundPlayer.State = ElementSoundPlayerState.On;
                togSwiSound.IsOn = true;
                cheBoxSpatialAudio.IsEnabled = true;

                if (Settings.SpatialAudio)
                {
                    ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.On;
                    cheBoxSpatialAudio.IsChecked = true;
                }
                else
                {
                    ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.Off;
                    cheBoxSpatialAudio.IsChecked = false;
                }
            }
            else
            {
                ElementSoundPlayer.State = ElementSoundPlayerState.Off;
                ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.Off;
                cheBoxSpatialAudio.IsChecked = false;
                cheBoxSpatialAudio.IsEnabled = false;
            }


            //Languages
            if (Settings.Language == Choices.ChooseLanguage(Languages.English))
            {
                (englishLanguage.Resources["EnglishThemePressed"] as Storyboard).Begin();
                IsEnglish = true;
            }
            else //Arabic
            {
                (arabicLanguage.Resources["ArabicThemePressed"] as Storyboard).Begin();
                IsArabic = true;
            }


            // Facebook and Microsoft and Admin id 
            if (!string.IsNullOrEmpty(Settings.AdminId))
            {
                patIcoYourEmail.Data = PathMarkupToGeometry(this.Resources["CheckMark"] as string);
                patIcoYourEmail.Foreground = App.Current.Resources["FixedPathIconForegroundBrush"] as SolidColorBrush;
            }
            if (!string.IsNullOrEmpty(Settings.FacebookId))
            {
                patIcoFacebook.Data = PathMarkupToGeometry(this.Resources["CheckMark"] as string);
                patIcoFacebook.Foreground = App.Current.Resources["FixedPathIconForegroundBrush"] as SolidColorBrush;
            }
            if (!string.IsNullOrEmpty(Settings.MicrosoftId))
            {
                patIcoMicrosoft.Data = PathMarkupToGeometry(this.Resources["CheckMark"] as string);
                patIcoMicrosoft.Foreground = App.Current.Resources["FixedPathIconForegroundBrush"] as SolidColorBrush;
            }

            await RefreshAdminProfileContent();
        }

        private async void NetworkChanged(object sender, EventArgs e)
        {
            await RefreshAdminProfileContent();
        }

        private async void BtnNoWifiSettings_Click(object sender, RoutedEventArgs e)
        {
            await RefreshAdminProfileContent();
        }

        #endregion


        #region Sign out process

        private void BtnSignOut_Click(object sender, RoutedEventArgs e)
        {
            sigOutCardView.IsOpen = true;
        }

        private async void BtnCaViSignOutOk_Click(object sender, RoutedEventArgs e)
        {
            sigOutCardView.IsOpen = false;

            Common.ADMIN_ID = "";
            Common.SUB_TOKEN = "";
            Settings.AdminId = "";
            Settings.MicrosoftId = "";
            Settings.FacebookId = "";


            // Check for background access (optional)
            await BackgroundExecutionManager.RequestAccessAsync();

            if (BackgroundTaskHelper.IsBackgroundTaskRegistered("NewMembers"))
            {
                BackgroundTaskHelper.Unregister("NewMembers");
            }


            (Window.Current.Content as Frame).Navigate(typeof(SignInView));
        }

        #endregion


        #region modify profile process

        private void BtnModify_Click(object sender, RoutedEventArgs e)
        {
            errorToastNotificationMessage.Text = "Not supported yet";
            errorToastNotification.Show(6000);
        }

        #endregion


        #region Your email, microsoft, facebook
        private void PatIcoYourEmail_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty( Settings.AdminId))
            {
                Settings.AdminId = Common.ADMIN_ID;
            }
        }

        private async void PatIcoFacebook_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Settings.FacebookId))
                {
                    if (IsNetworkAvailable())
                    {
                        var getFacebookId = await SettingsRepo.RequestFacebookIdAsync();

                        if (getFacebookId.Response.Result != null)
                        {
                            var sendFbIdReponse =
                                    await SettingsRepo.AddFacebookAccountAsync(
                                        Common.ADMIN_ID, getFacebookId.Response.Result as string);

                            if (sendFbIdReponse.Response.Result != null)
                            {
                                patIcoFacebook.Data = PathMarkupToGeometry(this.Resources["CheckMark"] as string);
                                patIcoFacebook.Foreground = App.Current.Resources["FixedPathIconForegroundBrush"] as SolidColorBrush;

                                Settings.FacebookId =
                                    AESCryptography.Encrypt(getFacebookId.Response.Result as string);

                                fixedToastNotificationMessage.Text =
                                    TranslatingUI.SettingsResourceMap.GetValue("txtToastAddFbAccount", TranslatingUI.ResourceContextObj).ValueAsString;
                                fixedToastNotification.Show(6000);
                            }
                            else
                            {
                                errorToastNotificationMessage.Text = sendFbIdReponse.ErrorMessage;
                                errorToastNotification.Show(6000);
                            }
                        }
                        else
                        {
                            errorToastNotificationMessage.Text = getFacebookId.ErrorMessage;
                            errorToastNotification.Show(6000);
                        }
                    }
                    else
                    {
                        errorToastNotificationMessage.Text =
                            TranslatingUI.SettingsResourceMap.GetValue("txtToastNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                        errorToastNotification.Show(6000);
                    }
                }
            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
        }

        private async void PatIcoMicrosoft_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Settings.MicrosoftId))
                {
                    if (IsNetworkAvailable())
                    {
                        var getMicrosoftId = await SettingsRepo.RequestMicrosoftIdAsync();

                        if (getMicrosoftId.Response.Result != null)
                        {
                            var sendMicIdReponse =
                                    await SettingsRepo.AddMicrosoftAccountAsync(
                                        Common.ADMIN_ID, getMicrosoftId.Response.Result as string);

                            if (sendMicIdReponse.Response.Result != null)
                            {
                                patIcoMicrosoft.Data = PathMarkupToGeometry(this.Resources["CheckMark"] as string);
                                patIcoMicrosoft.Foreground = App.Current.Resources["FixedPathIconForegroundBrush"] as SolidColorBrush;

                                Settings.MicrosoftId =
                                    AESCryptography.Encrypt(getMicrosoftId.Response.Result as string);

                                fixedToastNotificationMessage.Text =
                                    TranslatingUI.SettingsResourceMap.GetValue("txtToastAddMicAccount", TranslatingUI.ResourceContextObj).ValueAsString;
                                fixedToastNotification.Show(6000);
                            }
                            else
                            {
                                errorToastNotificationMessage.Text = sendMicIdReponse.ErrorMessage;
                                errorToastNotification.Show(6000);
                            }
                        }
                        else
                        {
                            errorToastNotificationMessage.Text = getMicrosoftId.ErrorMessage;
                            errorToastNotification.Show(6000);
                        }
                    }
                    else
                    {
                        errorToastNotificationMessage.Text =
                            TranslatingUI.SettingsResourceMap.GetValue("txtToastNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                        errorToastNotification.Show(6000);
                    }
                }

            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
        }

        #endregion

        #region Sound

        private void TogSwiSound_Toggled(object sender, RoutedEventArgs e)
        {
            if (togSwiSound.IsOn)
            {
                ElementSoundPlayer.State = ElementSoundPlayerState.On;
                cheBoxSpatialAudio.IsEnabled = true;
                Settings.SoundEnabled = true;
            }
            else
            {
                ElementSoundPlayer.State = ElementSoundPlayerState.Off;
                ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.Off;
                Settings.SoundEnabled = false;
                Settings.SpatialAudio = false;
                cheBoxSpatialAudio.IsEnabled = false;
            }
        }


        private void CheBoxSpatialAudio_Click(object sender, RoutedEventArgs e)
        {
            if (cheBoxSpatialAudio.IsChecked == true)
            {
                ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.On;
                Settings.SpatialAudio = true;
            }
            else
            {
                ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.Off;
                Settings.SpatialAudio = false;
            }
        }

        #endregion


        #region Arabic Language

        private void ArabicLanguage_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!IsArabic)
            {
                (arabicLanguage.Resources["ArabicThemeNormal"] as Storyboard).Begin();
            }
        }

        private void ArabicLanguage_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!IsArabic)
            {
                (arabicLanguage.Resources["ArabicThemePointerOver"] as Storyboard).Begin();
            }
        }

        private void ArabicLanguage_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (IsEnglish)
            {
                (englishLanguage.Resources["EnglishThemeNormal"] as Storyboard).Begin();
                IsEnglish = false;
            }

            (arabicLanguage.Resources["ArabicThemePressed"] as Storyboard).Begin();
            IsArabic = true;

            Settings.Language = 
                Choices.ChooseLanguage(Languages.Arabic);

            this.Resources["FontRegular"] = this.Resources["TajawalRegular"] as string;
            this.Resources["FontLight"] = this.Resources["TajawalLight"] as string;
            this.Resources["FontBold"] = this.Resources["TajawalBold"] as string;

            //Converting this page and all pages to arabic language
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "ar-SY";
            ResourceContext.SetGlobalQualifierValue("Language", "ar-SY");


            (Window.Current.Content as Frame).FlowDirection = FlowDirection.RightToLeft;
            Window.Current.CoreWindow.FlowDirection = Windows.UI.Core.CoreWindowFlowDirection.RightToLeft;
        }

        #endregion

        #region English Language

        private void EnglishLanguage_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!IsEnglish)
            {
                (englishLanguage.Resources["EnglishThemeNormal"] as Storyboard).Begin();
            }
        }

        private void EnglishLanguage_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!IsEnglish)
            {
                (englishLanguage.Resources["EnglishThemePointerOver"] as Storyboard).Begin();
            }
        }

        private void EnglishLanguage_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (IsArabic)
            {
                (arabicLanguage.Resources["ArabicThemeNormal"] as Storyboard).Begin();
                IsArabic = false;
            }

            (englishLanguage.Resources["EnglishThemePressed"] as Storyboard).Begin();
            IsEnglish = true;

            Settings.Language = 
                Choices.ChooseLanguage(Languages.English);

            this.Resources["FontRegular"] = this.Resources["RobotoRegular"] as string;
            this.Resources["FontLight"] = this.Resources["RobotoLight"] as string;
            this.Resources["FontBold"] = this.Resources["RobotoBold"] as string;

            //Converting this page and all pages to englsih language
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US";
            ResourceContext.SetGlobalQualifierValue("Language", "en-US");

            (Window.Current.Content as Frame).FlowDirection = FlowDirection.LeftToRight;
            Window.Current.CoreWindow.FlowDirection = Windows.UI.Core.CoreWindowFlowDirection.LeftToRight;
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
