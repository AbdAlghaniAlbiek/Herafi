using System;
using System.Collections.Generic;
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
using Herafi.Core.Helpers;
using Windows.ApplicationModel.Resources.Core;
using Windows.UI.Xaml.Documents;
using Herafi.Core.Repositories.RemoteRepo;
using Microsoft.Toolkit.Uwp.Connectivity;
using Herafi.Core.Security;
using System.Net.NetworkInformation;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.Helpers;
using System.Threading.Tasks;
using Herafi.Core.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Media.Animation;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using winsdkfb;
using winsdkfb.Graph;
using CommunityToolkit.Authentication;
using Microsoft.Toolkit;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Herafi.UWP.Views.DeviceFamily_Desktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignInView : Page, INotifyPropertyChanged
    {
        #region Fields
        public event PropertyChangedEventHandler PropertyChanged;
        private string _microsoftId;
        private string _facebookId;
        private StorageFile _pickProfileImage;
        private StorageFile _pickPersIdenImage;
        private bool _isPicturesUploaded = false;

        #endregion

        #region Properties
        public StorageFile PickProfileImage
        {
            get { return _pickProfileImage; }
            set { _pickProfileImage = value; OnPropertyChanged(); }
        }
        public StorageFile PickPersIdenImage
        {
            get { return _pickPersIdenImage; }
            set { _pickPersIdenImage = value; OnPropertyChanged(); }
        }
        public string FacebookId
        {
            get { return _facebookId; }
            set { _facebookId = value; OnPropertyChanged(); }
        }
        public string MicrosoftId
        {
            get { return _microsoftId; }
            set { _microsoftId = value; OnPropertyChanged(); }
        }
        public bool IsPicturesUploaded
        {
            get { return _isPicturesUploaded; }
            set { _isPicturesUploaded = value; OnPropertyChanged(); }
        }

        #endregion


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
        public SignInView()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            //Extending sign in view to entire title bar
            ExtedingTitleBar();

            //Initializing pointer pressed for all buttons of this view
            InitializeButtons_PointerPressed();

            // Subscribe to the event that's raised when a qualifier value changes.
            ResourceContext.GetForCurrentView().QualifierValues.MapChanged += new MapChangedEventHandler<string, string>(QualifierValues_MapChanged);
        }


        #region Exteding views to titlebar

        /// <summary>
        /// This method will change the background color of min,max  button in all of their states
        /// </summary>
        private void ExtedingTitleBar()
        {
            // Hide default title bar.
            var coreTitleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            UpdateTitleBarLayout(coreTitleBar);

            // Register a handler for when the size of the overlaid caption control changes.
            // For example, when the app moves to a screen with a different DPI.
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;

            // Register a handler for when the title bar visibility changes.
            // For example, when the title bar is invoked in full screen mode.
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
        }

        private void UpdateTitleBarLayout(Windows.ApplicationModel.Core.CoreApplicationViewTitleBar coreTitleBar)
        {
            // Get the size of the caption controls area and back button 
            // (returned in logical pixels), and move your content around as necessary.
            //TitleBarButton.Margin = new Thickness(0, 0, coreTitleBar.SystemOverlayRightInset, 0);
            LeftPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayLeftInset);
            RightPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayRightInset);

            // Update title bar control size as needed to account for system size changes.
            AppTitleBar.Height = coreTitleBar.Height;
        }

        private void CoreTitleBar_IsVisibleChanged(Windows.ApplicationModel.Core.CoreApplicationViewTitleBar sender, object args)
        {
            if (sender.IsVisible)
            {
                AppTitleBar.Visibility = Visibility.Visible;
            }
            else
            {
                AppTitleBar.Visibility = Visibility.Collapsed;
            }
        }

        private void CoreTitleBar_LayoutMetricsChanged(Windows.ApplicationModel.Core.CoreApplicationViewTitleBar sender, object args)
        {
            UpdateTitleBarLayout(sender);
        }

        #endregion


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
            TranslatingSignUpPivotTexts();
            TranslatingFacebookPivotTexts();
            TranslatingSignInPivotTexts();
            TranslatingVerifyPivotTexts();
            TranslatingChoosePicturesPivotTexts();
            TranslatingCardViews();
        }

        private void TranslatingCardViews()
        {
            flyoutContinueHeader.Text = 
                TranslatingUI.SignInResourceMap.GetValue("compProcCardViewHeader", TranslatingUI.ResourceContextObj).ValueAsString;
            flyoutContinueContent.Text = 
                TranslatingUI.SignInResourceMap.GetValue("compProcCardViewMessage", TranslatingUI.ResourceContextObj).ValueAsString;
            txtChPicNoContinueCont.Text =
                TranslatingUI.SignInResourceMap.GetValue("txtCaViCompProcNo", TranslatingUI.ResourceContextObj).ValueAsString;
            txtChPicYesContinueCont.Text =
                 TranslatingUI.SignInResourceMap.GetValue("txtCaViCompProcOk", TranslatingUI.ResourceContextObj).ValueAsString;
            exitAppCardView.Header =
                 TranslatingUI.SignInResourceMap.GetValue("exitAppCardViewHeader", TranslatingUI.ResourceContextObj).ValueAsString;
            exitAppCardViewMessage.Text =
                TranslatingUI.SignInResourceMap.GetValue("exitAppCardViewMessage", TranslatingUI.ResourceContextObj).ValueAsString;
            txtCaViExiAppCancel.Text =
                 TranslatingUI.SignInResourceMap.GetValue("txtCaViExiAppCancel", TranslatingUI.ResourceContextObj).ValueAsString;
            txtCaViExiAppOk.Text =
                TranslatingUI.SignInResourceMap.GetValue("txtCaViExiAppCancel", TranslatingUI.ResourceContextObj).ValueAsString;
        }

        private void TranslatingSignInPivotTexts()
        {
            txtSignInTitle.Text =
                TranslatingUI.SignInResourceMap.GetValue("txtSignInTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            txtSignInDesc.Text =
                TranslatingUI.SignInResourceMap.GetValue("txtSignInDesc", TranslatingUI.ResourceContextObj).ValueAsString;
            txtSignInDescForgPass.Text =
               TranslatingUI.SignInResourceMap.GetValue("txtSignInDescForgPass", TranslatingUI.ResourceContextObj).ValueAsString;
            txtSignInEmailErr.Text =
                 TranslatingUI.SignInResourceMap.GetValue("txtSignInEmailErr", TranslatingUI.ResourceContextObj).ValueAsString;
            txtBoxSignInEmail.PlaceholderText =
                 TranslatingUI.SignInResourceMap.GetValue("txtBoxSignInEmail", TranslatingUI.ResourceContextObj).ValueAsString;
            txtBoxSignInPassword.PlaceholderText =
                TranslatingUI.SignInResourceMap.GetValue("txtBoxSignInPassword", TranslatingUI.ResourceContextObj).ValueAsString;
            chBoxSignInKeepMeSig.Content =
                TranslatingUI.SignInResourceMap.GetValue("chBoxSignInKeepMeSig", TranslatingUI.ResourceContextObj).ValueAsString;
            txtSignInSubmCont.Text =
                TranslatingUI.SignInResourceMap.GetValue("txtSignInSubmCont", TranslatingUI.ResourceContextObj).ValueAsString;
            txtSignInChargeToSignUpDesc.Text =
                 TranslatingUI.SignInResourceMap.GetValue("txtSignInChargeToSignUpDesc", TranslatingUI.ResourceContextObj).ValueAsString;
            txtSignInChargeToSignUpDescrLink.Text =
                 TranslatingUI.SignInResourceMap.GetValue("txtSignInChargeToSignUpDescrLink", TranslatingUI.ResourceContextObj).ValueAsString;
            txtSignInChargeToMicFbDesc2.Text =
                TranslatingUI.SignInResourceMap.GetValue("txtSignInChargeToMicFbDesc2", TranslatingUI.ResourceContextObj).ValueAsString;
            txtSignInFbCont.Text =
                TranslatingUI.SignInResourceMap.GetValue("txtSignInFbCont", TranslatingUI.ResourceContextObj).ValueAsString;
            txtSignInMicCont.Text =
                TranslatingUI.SignInResourceMap.GetValue("txtSignInMicCont", TranslatingUI.ResourceContextObj).ValueAsString;
        }

        private void TranslatingSignUpPivotTexts()
        {
            txtSignUpTitle.Text =
                TranslatingUI.SignInResourceMap.GetValue("txtSignUpTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            txtSignUpDesc.Text =
                 TranslatingUI.SignInResourceMap.GetValue("txtSignUpDesc", TranslatingUI.ResourceContextObj).ValueAsString;
            txtSignUpNameErr.Text =
                 TranslatingUI.SignInResourceMap.GetValue("txtSignUpNameErr", TranslatingUI.ResourceContextObj).ValueAsString;
            txtBoxSignUpName.PlaceholderText =
                TranslatingUI.SignInResourceMap.GetValue("txtBoxSignUpName", TranslatingUI.ResourceContextObj).ValueAsString;
            txtSignUpEmailErr.Text =
                TranslatingUI.SignInResourceMap.GetValue("txtSignUpEmailErr", TranslatingUI.ResourceContextObj).ValueAsString;
            txtBoxSignUpEmail.PlaceholderText =
                TranslatingUI.SignInResourceMap.GetValue("txtBoxSignUpEmail", TranslatingUI.ResourceContextObj).ValueAsString;
            txtBoxSignUpPassword.PlaceholderText =
                TranslatingUI.SignInResourceMap.GetValue("txtBoxSignUpPassword", TranslatingUI.ResourceContextObj).ValueAsString;
            txtSignUpPhonNumErr.Text =
                TranslatingUI.SignInResourceMap.GetValue("txtSignUpPhonNumErr", TranslatingUI.ResourceContextObj).ValueAsString;
            txtBoxSignUpPhonNum.PlaceholderText =
                TranslatingUI.SignInResourceMap.GetValue("txtBoxSignUpPhonNum", TranslatingUI.ResourceContextObj).ValueAsString;
            txtSignUpNatNumErr.Text =
                 TranslatingUI.SignInResourceMap.GetValue("txtSignUpNatNumErr", TranslatingUI.ResourceContextObj).ValueAsString;
            txtBoxSignUpNatNum.PlaceholderText =
                TranslatingUI.SignInResourceMap.GetValue("txtBoxSignUpNatNum", TranslatingUI.ResourceContextObj).ValueAsString;
            txtSignUpSubmCont.Text =
                TranslatingUI.SignInResourceMap.GetValue("txtSignUpSubmCont", TranslatingUI.ResourceContextObj).ValueAsString;
        }

        private void TranslatingFacebookPivotTexts()
        {
            txtFbDescription.Text =
                 TranslatingUI.SignInResourceMap.GetValue("txtFbDescription", TranslatingUI.ResourceContextObj).ValueAsString;
            txtFbNameErr.Text =
                TranslatingUI.SignInResourceMap.GetValue("txtFbEmailErr", TranslatingUI.ResourceContextObj).ValueAsString;
            txtBoxFbName.PlaceholderText =
                TranslatingUI.SignInResourceMap.GetValue("txtBoxFbName", TranslatingUI.ResourceContextObj).ValueAsString;
            txtFbEmailErr.Text =
                TranslatingUI.SignInResourceMap.GetValue("txtFbEmailErr", TranslatingUI.ResourceContextObj).ValueAsString;
            txtBoxFbEmail.PlaceholderText =
                 TranslatingUI.SignInResourceMap.GetValue("txtBoxFbEmail", TranslatingUI.ResourceContextObj).ValueAsString;
            txtBoxFbPassword.PlaceholderText =
                 TranslatingUI.SignInResourceMap.GetValue("txtBoxFbPassword", TranslatingUI.ResourceContextObj).ValueAsString;
            txtFbPhonNumErr.Text =
                TranslatingUI.SignInResourceMap.GetValue("txtFbPhonNumErr", TranslatingUI.ResourceContextObj).ValueAsString;
            txtBoxFbPhonNum.PlaceholderText =
                TranslatingUI.SignInResourceMap.GetValue("txtBoxFbPhonNum", TranslatingUI.ResourceContextObj).ValueAsString;
            txtFbNatNumErr.Text =
                 TranslatingUI.SignInResourceMap.GetValue("txtFbNatNumErr", TranslatingUI.ResourceContextObj).ValueAsString;
            txtBoxFbNatNum.PlaceholderText =
                 TranslatingUI.SignInResourceMap.GetValue("txtBoxFbNatNum", TranslatingUI.ResourceContextObj).ValueAsString;
            txtFbSubmCont.Text =
                 TranslatingUI.SignInResourceMap.GetValue("txtFbSubmCont", TranslatingUI.ResourceContextObj).ValueAsString;
        }

        private void TranslatingVerifyPivotTexts()
        {
            txtVerifyTitle.Text =
                TranslatingUI.SignInResourceMap.GetValue("txtVerifyTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            txtVerifyDesc.Text =
                 TranslatingUI.SignInResourceMap.GetValue("txtVerifyDesc", TranslatingUI.ResourceContextObj).ValueAsString;
            txtBoxVerifyCode.PlaceholderText =
                TranslatingUI.SignInResourceMap.GetValue("txtBoxVerifyCode", TranslatingUI.ResourceContextObj).ValueAsString;
            txtVerifySendCont.Text =
                TranslatingUI.SignInResourceMap.GetValue("txtVerifySendCont", TranslatingUI.ResourceContextObj).ValueAsString;
        }

        private void TranslatingChoosePicturesPivotTexts()
        {
            txtChoPicTitle.Text =
                TranslatingUI.SignInResourceMap.GetValue("txtChoPicTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            txtChoPicDesc.Text =
                TranslatingUI.SignInResourceMap.GetValue("txtChoPicDesc", TranslatingUI.ResourceContextObj).ValueAsString;
            txtChPicContinueCont.Text =
                 TranslatingUI.SignInResourceMap.GetValue("txtChPicContinueCont", TranslatingUI.ResourceContextObj).ValueAsString;
        }

        #endregion


        #region Initialize Buttons PointerPressed Handlers

        private void InitializeButtons_PointerPressed()
        {
            this.Loading += SignInView_Loading;
            NetworkHelper.Instance.NetworkChanged += NetworkChanged;

            btnCaViAppCancel.Click += new RoutedEventHandler((s, e) => exitAppCardView.IsOpen = false);
            btnCaViAppOk.PointerPressed += new PointerEventHandler((s, e) => App.Current.Exit());
            
            //Sign Up pivot
            btnGoBack.Click += new RoutedEventHandler((s, e) => mainPivot.SelectedIndex = 0);
            btnSignUpSubm.Click += new RoutedEventHandler(BtnSignUpSubmit_PointerPressed);
            txtBoxSignUpName.TextChanged += (s, e) => txtSignUpNameErr.Visibility = Visibility.Collapsed;
            txtBoxSignUpEmail.TextChanged += (s, e) => txtSignUpEmailErr.Visibility = Visibility.Collapsed;
            txtBoxSignUpPhonNum.TextChanged += (s, e) => txtSignUpPhonNumErr.Visibility = Visibility.Collapsed;
            txtBoxSignUpNatNum.TextChanged += (s, e) => txtSignUpNatNumErr.Visibility = Visibility.Collapsed;


            //Facebook, Microsoft, Google... etc pivot
            btnFbSubm.Click += new RoutedEventHandler(BtnFbSubmit_PointerPressed);
            txtBoxFbName.TextChanged += (s, e) => txtFbNameErr.Visibility = Visibility.Collapsed;
            txtBoxFbEmail.TextChanged += (s, e) => txtFbEmailErr.Visibility = Visibility.Collapsed;
            txtBoxFbPhonNum.TextChanged += (s, e) => txtFbPhonNumErr.Visibility = Visibility.Collapsed;
            txtBoxFbNatNum.TextChanged += (s, e) => txtFbNatNumErr.Visibility = Visibility.Collapsed;


            //Verify Account pivot
            btnVerifySend.Click += new RoutedEventHandler(BtnVerifySend_PointerPressed);


            //Choose Your Photos pivot
            btnChoPicPersIdentity.PointerPressed += new PointerEventHandler(BtnChoPicPersIdentity_PointerPressed);
            btnChoPicProfile.PointerPressed += new PointerEventHandler(BtnChoPicProfile_PointerPressed);
            btnChPicContinue.Tapped += new TappedEventHandler(BtnChPicContinue_Tapped);
            btnChPicNoContinue.Click += new RoutedEventHandler(BtnChPicNoContinue_Click);
            btnChPicYesContinue.Click += new RoutedEventHandler(BtnChPicYesContinue_Click);


            //Sign in pivot
            btnSignInSubm.Click += new RoutedEventHandler(BtnSignInSubm_PointerPressed);
            btnSignInFb.Click += new RoutedEventHandler(BtnSignInFb_PointerPressed);
            btnSignInMic.Click += new RoutedEventHandler(BtnSignInMic_PointerPressed);
            HypLinSignInForgPass.Click += new TypedEventHandler<Hyperlink, HyperlinkClickEventArgs>((s, e) => mainPivot.SelectedIndex = 1) ;
            HypLinSignInChargeToSignUp.Click += new TypedEventHandler<Hyperlink, HyperlinkClickEventArgs>((s, e) => mainPivot.SelectedIndex = 4);
            txtBoxSignInEmail.TextChanged += (s, e) => txtSignInEmailErr.Visibility = Visibility.Collapsed;


            //Check your identity pivot
            txtBoxCheYourIdText.TextChanged += (s, e) => txtCheYourIdTextErr.Visibility = Visibility.Collapsed;
            btnCheYourIdSend.Click += new RoutedEventHandler(BtnCheckYourIdentitySend_Click);
            btnCheYourIdGoBack.Click += new RoutedEventHandler((s, e) => mainPivot.SelectedIndex = 0);


            //Verification Identity pivot
            btnVeriIdSend.Click += new RoutedEventHandler(BtnVeriIdSend_Click);

            //Reset password pivot
            btnResetPasswordContinue.Click += new RoutedEventHandler(BtnResetPasswordContinue_Click);

        }



        #region page loading and network change

        private ObservableCollection<City> ReTranslatingCities(bool toEnglish, ObservableCollection<City> cities)
        {
            if (toEnglish)
            {
                foreach (var city in cities)
                {
                    switch (city.Name)
                    {
                        case "حلب": 
                            city.Name = "Aleppo";
                            break;
                        case "اللاذقية":
                            city.Name = "Latakia";
                            break;
                        case "حمص":
                            city.Name = "Homs";
                            break;
                        case "دمشق":
                            city.Name = "Damascus";
                            break;
                        case "حماة":
                            city.Name = "Hama";
                            break;
                        case "طرطوس":
                            city.Name = "Tartus";
                            break;
                        case "درعا":
                            city.Name = "Daraa";
                            break;
                        case "الرقة":
                            city.Name = "Al Raqqah";
                            break;
                        case "دير الزور":
                            city.Name = "Deir ez-zur";
                            break;
                        case "الحسكة":
                            city.Name = "Al Hasakah";
                            break;
                        case "القنيطرة":
                            city.Name = "Al Qunaitra";
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                foreach (var city in cities)
                {
                    switch (city.Name)
                    {
                        case "Aleppo":
                            city.Name = "حلب"; 
                            break;
                        case "Latakia":
                            city.Name = "اللاذقية"; 
                            break;
                        case "Homs":
                            city.Name = "حمص"; 
                            break;
                        case "Damascus":
                            city.Name = "دمشق"; 
                            break;
                        case "Hama":
                            city.Name = "حماة"; 
                            break;
                        case "Tartus":
                            city.Name = "طرطوس"; 
                            break;
                        case "Daraa":
                            city.Name = "درعا"; 
                            break;
                        case "Al Raqqah":
                            city.Name = "الرقة"; 
                            break;
                        case "Deir ez-zur":
                            city.Name = " دير الزور";
                            break;
                        case "Al Hasakah":
                            city.Name = "الحسكة"; 
                            break;
                        case "Al Qunaitra":
                            city.Name = "القنيطرة"; 
                            break;
                        default:
                            break;
                    }
                }
            }

            return cities;
        }

        private async Task RefreshCitiesData(bool isNetworkChangeCall)
        {
            try
            {
                if (signInViewModel.Cities == null)
                {
                    if (IsNetworkAvailable())
                    {
                        await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                        {
                            if (isNetworkChangeCall)
                            {
                                fixedToastNotificationMessage.Text =
                                    TranslatingUI.SignInResourceMap.GetValue("txtToastConnWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                                fixedToastNotification.Show(6000);
                            }

                            progBarProcess.IsIndeterminate = true;

                            var httpResponse = await SignInRepo.GetCitiesAsync();

                            if (httpResponse.Response.Result != null)
                            {
                                if (Settings.Language == Choices.ChooseLanguage(Languages.Arabic))
                                {
                                    signInViewModel.Cities =
                                        ReTranslatingCities(false, httpResponse.Response.Result as ObservableCollection<City>);
                                }
                                else
                                {
                                    signInViewModel.Cities =
                                        (ObservableCollection<City>)httpResponse.Response.Result;
                                }
                            }
                            else
                            {
                                errorToastNotificationMessage.Text = httpResponse.ErrorMessage;
                                errorToastNotification.Show(6000);
                            }

                            progBarProcess.IsIndeterminate = false;
                        });
                    }
                    else
                    {
                        errorToastNotificationMessage.Text =
                            TranslatingUI.SignInResourceMap.GetValue("txtToastNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
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

        private async void SignInView_Loading(FrameworkElement sender, object args)
        {
            await ActivateRefreshUITexts();

            await RefreshCitiesData(false);
        }

        private async void NetworkChanged(object sender, EventArgs e)
        {
            await RefreshCitiesData(true);
        }

        #endregion


        #region Choose Your Photos pivot

        private async void BtnChoPicProfile_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var pickSingleFile = new FileOpenPicker();

            pickSingleFile.CommitButtonText = "Open";
            pickSingleFile.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            pickSingleFile.ViewMode = PickerViewMode.Thumbnail;

            pickSingleFile.FileTypeFilter.Add(".png");
            pickSingleFile.FileTypeFilter.Add(".jpg");
            pickSingleFile.FileTypeFilter.Add(".jpeg");
            pickSingleFile.FileTypeFilter.Add("*");

            StorageFile pickedFile = await pickSingleFile.PickSingleFileAsync();
            if (pickedFile != null)
            {
                var stream = await pickedFile.OpenAsync(FileAccessMode.Read);
                var tem = new BitmapImage();
                tem.SetSource(stream);
                imgExProfile.Source = tem;
                PickProfileImage = pickedFile;
            }
        }

        private async void BtnChoPicPersIdentity_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var pickSingleFile = new FileOpenPicker();

            pickSingleFile.CommitButtonText = "Open";
            pickSingleFile.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            pickSingleFile.ViewMode = PickerViewMode.Thumbnail;

            pickSingleFile.FileTypeFilter.Add(".png");
            pickSingleFile.FileTypeFilter.Add(".jpg");
            pickSingleFile.FileTypeFilter.Add(".jpeg");
            pickSingleFile.FileTypeFilter.Add("*");

            StorageFile pickedFile = await pickSingleFile.PickSingleFileAsync();
            if (pickedFile != null)
            {
                var stream = await pickedFile.OpenAsync(FileAccessMode.Read);
                var tem = new BitmapImage();
                tem.SetSource(stream);
                imgExPersIdentity.Source = tem;
                PickPersIdenImage = pickedFile;
            }
        }

        private async void BtnChPicContinue_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (IsPicturesUploaded)
                {
                    flyoutContinue.ShowAt(btnChPicContinue);
                }
                else
                {
                    if (PickProfileImage != null && PickPersIdenImage != null)
                    {
                        if (IsNetworkAvailable())
                        {
                            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                            {
                                progBarProcess.IsIndeterminate = true;

                                //Uploading profile image
                                var streamProfileImage = await PickProfileImage.OpenStreamForReadAsync();
                                var profImgHttpResponse = await SignInRepo.UploadProfileImageAsync(Common.ADMIN_ID,
                                    new Refit.StreamPart(
                                            streamProfileImage, PickProfileImage.Name,
                                            PickProfileImage.ContentType));


                                //Uploading personal identity image
                                var streamPersIdenImage = await PickPersIdenImage.OpenStreamForReadAsync();
                                var persIdentHttpResponse = await SignInRepo.UploadPersonalIdentityImageAsync(Common.ADMIN_ID,
                                    new Refit.StreamPart(
                                            streamPersIdenImage, PickPersIdenImage.Name,
                                            PickPersIdenImage.ContentType));

                                if (profImgHttpResponse.Response.Result != null && persIdentHttpResponse.Response.Result != null)
                                {
                                    IsPicturesUploaded = true;
                                    flyoutContinue.ShowAt(btnChPicContinue);
                                }
                                else
                                {
                                    errorToastNotificationMessage.Text = profImgHttpResponse.ErrorMessage; //Or we can use persIdentHttpResponse.ErrorMessage
                                    errorToastNotification.Show(6000);
                                }


                                progBarProcess.IsIndeterminate = false;
                            });
                        }
                        else
                        {
                            errorToastNotificationMessage.Text =
                                TranslatingUI.SignInResourceMap.GetValue("txtToastNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                            errorToastNotification.Show(6000);
                        }
                    }
                    else
                    {
                        errorToastNotificationMessage.Text =
                            TranslatingUI.SignInResourceMap.GetValue("txtToastPickPicErr", TranslatingUI.ResourceContextObj).ValueAsString;
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
        
        private  void BtnChPicYesContinue_Click(object sender, RoutedEventArgs e)
        {
            //Implemented in the end
            //Settings.AdminId = Common.ADMIN_ID;
            Settings.FacebookId = !string.IsNullOrEmpty(FacebookId) ? FacebookId : "";
            Settings.MicrosoftId = !string.IsNullOrEmpty(MicrosoftId) ? MicrosoftId : "";

            this.Frame.Navigate(typeof(MainView), null, new DrillInNavigationTransitionInfo());
        }

        private void BtnChPicNoContinue_Click(object sender, RoutedEventArgs e)
        {
            Settings.FacebookId = !string.IsNullOrEmpty(FacebookId) ? FacebookId : "";
            Settings.MicrosoftId = !string.IsNullOrEmpty(MicrosoftId) ? MicrosoftId : "";

            this.Frame.Navigate(typeof(MainView), null, new DrillInNavigationTransitionInfo());
        }

        #endregion


        #region Sign in pivot

        private bool SignInCheckConstrains()
        {
            //Check if the user connect to internet
            if (IsNetworkAvailable())
            {
                bool canPass = true;

                //Check if any textbox is empty and tell user to fill all of them
                if (string.IsNullOrEmpty(txtBoxSignInEmail.Text) || string.IsNullOrEmpty(txtBoxSignInPassword.Password))
                {
                    errorToastNotificationMessage.Text =
                        TranslatingUI.SignInResourceMap.GetValue("txtToastFillTextBoxesErr", TranslatingUI.ResourceContextObj).ValueAsString;
                    errorToastNotification.Show(6000);
                    canPass = false;
                }

                //Check if there is Unvalid Email format
                if (!txtBoxSignInEmail.Text.IsEmail())
                {
                    txtSignInEmailErr.Visibility = Visibility.Visible;
                    canPass = false;
                }

                return canPass;
            }
            else
            {
                errorToastNotificationMessage.Text =
                    TranslatingUI.SignInResourceMap.GetValue("txtToastNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                errorToastNotification.Show(6000);
                return false;
            }
        }

        private async void BtnSignInSubm_PointerPressed(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SignInCheckConstrains())
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                    {
                        progBarProcess.IsIndeterminate = true;

                        var httpResoponse = await SignInRepo.SignInAdminAsync(
                        new Admin()
                        {
                            Email = txtBoxSignInEmail.Text,
                            Password = txtBoxSignInPassword.Password
                        });

                        if (httpResoponse.Response.Result != null)
                        {
                            Common.ADMIN_ID = httpResoponse.Response.Result as string;

                            if (chBoxSignInKeepMeSig.IsChecked == true)
                            {
                                Settings.AdminId = Common.ADMIN_ID;
                            }

                            this.Frame.Navigate(typeof(MainView), null, new DrillInNavigationTransitionInfo());
                        }
                        else
                        {
                            errorToastNotificationMessage.Text = httpResoponse.ErrorMessage;
                            errorToastNotification.Show(6000);
                        }

                        progBarProcess.IsIndeterminate = false;
                    });
                }

            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
        }

        private async void BtnSignInMic_PointerPressed(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Settings.MicrosoftId))
                {
                    if (IsNetworkAvailable())
                    {
                        await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                        {
                            progBarProcess.IsIndeterminate = true;

                            var httpResponse =
                                    await SignInRepo.DirectSignInWithMicrosoftIdAsync(Settings.MicrosoftId);

                            if (httpResponse.Response.Result != null)
                            {
                                Common.ADMIN_ID = httpResponse.Response.Result as string;

                                this.Frame.Navigate(typeof(MainView), null, new DrillInNavigationTransitionInfo());
                            }
                            else
                            {
                                errorToastNotificationMessage.Text = httpResponse.ErrorMessage;
                                errorToastNotification.Show(6000);
                            }

                            progBarProcess.IsIndeterminate = false;
                        });
                    }
                    else
                    {
                        errorToastNotificationMessage.Text =
                             TranslatingUI.SignInResourceMap.GetValue("txtToastNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                        errorToastNotification.Show(6000);
                    }
                }
                else
                {
                    if (IsNetworkAvailable())
                    {
                        await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                        {
                            progBarProcess.IsIndeterminate = true;

                            string microsoftCliendId = Common.MICROSOFT_APP_ID;
                            string[] scopes = new string[] { "User.Read" };

                            ProviderManager.Instance.GlobalProvider = new MsalProvider(microsoftCliendId, scopes);
                            await ProviderManager.Instance.GlobalProvider.SignInAsync();


                            if (!string.IsNullOrEmpty(ProviderManager.Instance.GlobalProvider.CurrentAccountId))
                            {
                                MicrosoftId = ProviderManager.Instance.GlobalProvider.CurrentAccountId;

                                BitmapImage bitMapImgAuthProvider = new BitmapImage();
                                bitMapImgAuthProvider.UriSource = new Uri("ms-appx:///Assets/Images/microsoft_66px.png");

                                imgAuthProvider.Source = bitMapImgAuthProvider;
                                txtFbDescription.Text = TranslatingUI.SignInResourceMap.GetValue("txtMicDescription", TranslatingUI.ResourceContextObj).ValueAsString;

                                mainPivot.SelectedIndex = 5;
                            }


                            progBarProcess.IsIndeterminate = false;
                        });
                    }
                    else
                    {
                        errorToastNotificationMessage.Text =
                             TranslatingUI.SignInResourceMap.GetValue("txtToastNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
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

        private async void BtnSignInFb_PointerPressed(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Settings.FacebookId))
                {
                    if (IsNetworkAvailable())
                    {
                        await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                        {
                            progBarProcess.IsIndeterminate = true;

                            var httpResponse =
                                    await SignInRepo.DirectSignInWithFacebookIdAsync(Settings.FacebookId);

                            if (httpResponse.Response.Result != null)
                            {
                                Common.ADMIN_ID = httpResponse.Response.Result as string;

                                this.Frame.Navigate(typeof(MainView), null, new DrillInNavigationTransitionInfo());
                            }
                            else
                            {
                                errorToastNotificationMessage.Text = httpResponse.ErrorMessage;
                                errorToastNotification.Show(6000);
                            }

                            progBarProcess.IsIndeterminate = false;
                        });
                    }
                    else
                    {
                        errorToastNotificationMessage.Text =
                            TranslatingUI.SignInResourceMap.GetValue("txtToastNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                        errorToastNotification.Show(6000);
                    }
                }
                else
                {
                    if (IsNetworkAvailable())
                    {
                        await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                        {
                            progBarProcess.IsIndeterminate = true;

                            FBSession sess = FBSession.ActiveSession;
                            sess.FBAppId = Common.FACEBOOK_APP_ID;
                            sess.WinAppId = Common.WINDOWS_APP_ID;

                            // Add permissions required by the app
                            List<String> permissionList = new List<String>();
                            permissionList.Add("public_profile");
                            FBPermissions permissions = new FBPermissions(permissionList);

                            // Login to Facebook
                            FBResult result = await sess.LoginAsync(permissions);


                            if (result.Succeeded)
                            {
                                // Get current user
                                FBUser user = sess.User;
                                FacebookId = user.Id;

                                BitmapImage bitMapImgAuthProvider = new BitmapImage();
                                bitMapImgAuthProvider.UriSource = new Uri("ms-appx:///Assets/Images/facebook_66px.png");

                                imgAuthProvider.Source = bitMapImgAuthProvider;
                                txtFbDescription.Text = TranslatingUI.SignInResourceMap.GetValue("txtFbDescription", TranslatingUI.ResourceContextObj).ValueAsString;

                                mainPivot.SelectedIndex = 5;
                            }
                            else
                            {
                                errorToastNotificationMessage.Text = result.ErrorInfo.ErrorUserMessage;
                                errorToastNotification.Show(6000);
                            }

                            progBarProcess.IsIndeterminate = false;

                        });
                    }
                    else
                    {
                        errorToastNotificationMessage.Text =
                             TranslatingUI.SignInResourceMap.GetValue("txtToastNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
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


        #region Check Identity

        private bool CheckYourIdentityCheckConstrains()
        {
            //Check if the user connect to internet
            if (IsNetworkAvailable())
            {
                bool canPass = true;

                //Check if any textbox is empty and tell user to fill all of them
                if (string.IsNullOrEmpty(txtBoxCheYourIdText.Text))
                {
                    errorToastNotificationMessage.Text =
                        TranslatingUI.SignInResourceMap.GetValue("txtToastFillTextBoxesErr", TranslatingUI.ResourceContextObj).ValueAsString;
                    errorToastNotification.Show(6000);

                    canPass = false;
                }

                //Check if there is Unvalid Email format
                //**Is email
                if (txtBoxCheYourIdText.Text.ToCharArray().Contains('@'))
                {
                    if (!txtBoxCheYourIdText.Text.IsEmail())
                    {
                        txtCheYourIdTextErr.Visibility = Visibility.Visible;
                        canPass = false;
                    }
                }
                //**Is phone number
                else
                {
                    if (!txtBoxCheYourIdText.Text.IsPhoneNumber())
                    {
                        txtCheYourIdTextErr.Visibility = Visibility.Visible;
                        canPass = false;
                    }
                }
               
                return canPass;
            }
            else
            {
                errorToastNotificationMessage.Text =
                    TranslatingUI.SignInResourceMap.GetValue("txtToastNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                errorToastNotification.Show(6000);

                return false;
            }
        }

        private async void BtnCheckYourIdentitySend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CheckYourIdentityCheckConstrains())
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                    {
                        progBarProcess.IsIndeterminate = true;

                        var httpResponse =
                           await SignInRepo.CheckIdentityAsync(txtBoxCheYourIdText.Text);

                        if (httpResponse.Response.Result != null)
                        {
                            mainPivot.SelectedIndex = 2;
                        }
                        else
                        {
                            errorToastNotificationMessage.Text = httpResponse.ErrorMessage;
                            errorToastNotification.Show(6000);
                        }

                        progBarProcess.IsIndeterminate = false;
                    });
                }

            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
        }

        #endregion


        #region Verification Identity

        private async void BtnVeriIdSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsNetworkAvailable())
                {
                    if (!string.IsNullOrEmpty(txtBoxVeriIdCode.Text))
                    {
                        await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                        {
                            var httpResponse =
                                    await SignInRepo.VerificationIdentityAsync(txtBoxVeriIdCode.Text);

                            if (httpResponse.Response.Result != null)
                            {
                                Common.ADMIN_ID = httpResponse.Response.Result as string;
                                mainPivot.SelectedIndex = 3;
                            }
                            else
                            {
                                errorToastNotificationMessage.Text = httpResponse.ErrorMessage;
                                errorToastNotification.Show(6000);
                            }
                        });
                    }
                    else
                    {
                        errorToastNotificationMessage.Text =
                           TranslatingUI.SignInResourceMap.GetValue("txtToastFillTextBoxesErr", TranslatingUI.ResourceContextObj).ValueAsString;
                        errorToastNotification.Show(6000);
                    }
                }
                else
                {
                    errorToastNotificationMessage.Text =
                       TranslatingUI.SignInResourceMap.GetValue("txtToastNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                    errorToastNotification.Show(6000);
                }
            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
           
        }

        #endregion


        #region Reset password

        private async void BtnResetPasswordContinue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsNetworkAvailable())
                {
                    if (!string.IsNullOrEmpty(txtBoxResetPassword.Text))
                    {
                        await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                        {
                            var httpResponse =
                                    await SignInRepo.ResetPasswordAsync(Common.ADMIN_ID, txtBoxResetPassword.Text);

                            if (httpResponse.Response.Result != null)
                            {
                                this.Frame.Navigate(typeof(MainView), null, new EntranceNavigationTransitionInfo());
                            }
                            else
                            {
                                errorToastNotificationMessage.Text = httpResponse.ErrorMessage;
                                errorToastNotification.Show(6000);
                            }
                        });
                    }
                    else
                    {
                        errorToastNotificationMessage.Text =
                          TranslatingUI.SignInResourceMap.GetValue("txtToastFillTextBoxesErr", TranslatingUI.ResourceContextObj).ValueAsString;
                        errorToastNotification.Show(6000);
                    }
                }
                else
                {
                    errorToastNotificationMessage.Text =
                          TranslatingUI.SignInResourceMap.GetValue("txtToastNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                    errorToastNotification.Show(6000);
                }
            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
           
        }

        #endregion


        #region Verify Account pivot
        private async void BtnVerifySend_PointerPressed(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsNetworkAvailable())
                {
                    progBarProcess.IsIndeterminate = true;

                    if (string.IsNullOrEmpty(txtBoxVerifyCode.Text))
                    {
                        errorToastNotificationMessage.Text =
                             TranslatingUI.SignInResourceMap.GetValue("txtToastFillTextBoxesErr", TranslatingUI.ResourceContextObj).ValueAsString;
                        errorToastNotification.Show(6000);
                    }
                    else
                    {
                        var httpResponse =
                                await SignInRepo.VerifyAdminAsync(txtBoxVerifyCode.Text.Trim());

                        if (httpResponse.Response.Result != null)
                        {
                            Common.ADMIN_ID = httpResponse.Response.Result as string;

                            mainPivot.SelectedIndex = 7;
                        }
                        else
                        {
                            errorToastNotificationMessage.Text = httpResponse.ErrorMessage;
                            errorToastNotification.Show(6000);
                        }
                    }

                    progBarProcess.IsIndeterminate = false;
                }
                else
                {
                    errorToastNotificationMessage.Text =
                         TranslatingUI.SignInResourceMap.GetValue("txtToastNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                    errorToastNotification.Show(6000);
                }
            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
            
        }

        #endregion


        #region Facebook, Microsoft, Google auth pivot

        private bool FacebookSignUpCheckConstrains()
        {
            //Check if the user connect to internet
            if (IsNetworkAvailable())
            {
                bool canPass = true;

                //Check if any textbox is empty and tell user to fill all of them
                if (string.IsNullOrEmpty(txtBoxFbName.Text) || string.IsNullOrEmpty(txtBoxFbEmail.Text) ||
                    string.IsNullOrEmpty(txtBoxFbPhonNum.Text) || string.IsNullOrEmpty(txtBoxFbPassword.Password) ||
                    string.IsNullOrEmpty(txtBoxFbNatNum.Text))
                {
                    errorToastNotificationMessage.Text =
                         TranslatingUI.SignInResourceMap.GetValue("txtToastFillTextBoxesErr", TranslatingUI.ResourceContextObj).ValueAsString;
                    errorToastNotification.Show(6000);
                    canPass = false;
                }

                //I make this way beacause IsCharacterString() method if there is spaces ot tabs between words in 
                // txtName.Text then it return false state to me
                //to solve that I made this 'for' to split the text of txtName to subStrings and check each of them once by once
                string[] subTxtName = txtBoxFbName.Text.Split(" ");

                for (int i = 0; i < subTxtName.Length; i++)
                {
                    if (subTxtName[i] != "")
                    {
                        //Check if there is Unvalid Name format
                        if (!subTxtName[i].IsCharacterString())
                        {
                            txtFbNameErr.Visibility = Visibility.Visible;
                            canPass = false;
                            break;
                        }
                    }
                }

                //Check if there is Unvalid Email format
                if (!txtBoxFbEmail.Text.IsEmail())
                {
                    txtFbEmailErr.Visibility = Visibility.Visible;
                    canPass = false;
                }

                //Check if there is Unvalid Phone number format
                if (!txtBoxFbPhonNum.Text.IsPhoneNumber())
                {
                    txtFbPhonNumErr.Visibility = Visibility.Visible;
                    canPass = false;
                }

                //Check if there is Unvalid National number format
                if (!txtBoxFbNatNum.Text.IsNumeric())
                {
                    txtFbNatNumErr.Visibility = Visibility.Visible;
                    canPass = false;
                }

                return canPass;
            }
            else
            {
                errorToastNotificationMessage.Text =
                    TranslatingUI.SignInResourceMap.GetValue("txtToastNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                errorToastNotification.Show(6000);
                return false;
            }
        }

        private async void BtnFbSubmit_PointerPressed(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FacebookSignUpCheckConstrains())
                {
                    ///Authentication Facebook
                    if (!string.IsNullOrEmpty(FacebookId))
                    {
                        await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                        {
                            progBarProcess.IsIndeterminate = true;

                            var httpResponse = await SignInRepo.SignUpWidthFacebookIdAdminAsync(
                                   new Admin()
                                   {
                                       Name = txtBoxFbName.Text,
                                       Email = txtBoxFbEmail.Text,
                                       Password = txtBoxFbPassword.Password,
                                       PhoneNumber = txtBoxFbPhonNum.Text,
                                       NationalNumber = txtBoxFbNatNum.Text,
                                       City = ((City)comBoxFBCities.SelectedItem).Id,
                                       FacebookId = FacebookId
                                   });

                            if (httpResponse.Response.Result != null)
                            {
                                Common.ADMIN_ID = httpResponse.Response.Result as string;

                                mainPivot.SelectedIndex = 7;
                            }
                            else
                            {
                                errorToastNotificationMessage.Text = httpResponse.ErrorMessage;
                                errorToastNotification.Show(6000);
                            }

                            progBarProcess.IsIndeterminate = false;
                        });
                    }

                    ///Authentication Microsoft
                    else if (!string.IsNullOrEmpty(MicrosoftId))
                    {
                        await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                        {
                            progBarProcess.IsIndeterminate = true;

                            var httpResponse = await SignInRepo.SignUpWidthMicrosoftIdAdminAsync(
                                    new Admin()
                                    {
                                        Name = txtBoxFbName.Text,
                                        Email = txtBoxFbEmail.Text,
                                        Password = txtBoxFbPassword.Password,
                                        PhoneNumber = txtBoxFbPhonNum.Text,
                                        NationalNumber = txtBoxFbNatNum.Text,
                                        City = ((City)comBoxFBCities.SelectedItem).Id,
                                        MicrosoftId = MicrosoftId
                                    });

                            if (httpResponse.Response.Result != null)
                            {
                                Common.ADMIN_ID = httpResponse.Response.Result as string;

                                mainPivot.SelectedIndex = 7;
                            }
                            else
                            {
                                errorToastNotificationMessage.Text = httpResponse.ErrorMessage;
                                errorToastNotification.Show(6000);
                            }

                            progBarProcess.IsIndeterminate = false;
                        });
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


        #region Sign up pivot

        private bool SignUpCheckConstrains()
        {
            //Check if the user connect to internet
            if (IsNetworkAvailable())
            {
                bool canPass = true;

                //Check if any textbox is empty and tell user to fill all of them
                if (string.IsNullOrEmpty(txtBoxSignUpName.Text) || string.IsNullOrEmpty(txtBoxSignUpEmail.Text) ||
                    string.IsNullOrEmpty(txtBoxSignUpPhonNum.Text) || string.IsNullOrEmpty(txtBoxSignUpPassword.Password) ||
                    string.IsNullOrEmpty(txtBoxSignUpNatNum.Text))
                {
                    errorToastNotificationMessage.Text =
                        TranslatingUI.SignInResourceMap.GetValue("txtToastFillTextBoxesErr", TranslatingUI.ResourceContextObj).ValueAsString;
                    errorToastNotification.Show(6000);
                    canPass = false;
                }

                //I make this way beacause IsCharacterString() method if there is spaces ot tabs between words in 
                // txtName.Text then it return false state to me
                //to solve that I made this 'for' to split the text of txtName to subStrings and check each of them once by once
                string[] subTxtName = txtBoxSignUpName.Text.Split(" ");

                for (int i = 0; i < subTxtName.Length; i++)
                {
                    if (subTxtName[i] != "")
                    {
                        //Check if there is Unvalid Name format
                        if (!subTxtName[i].IsCharacterString())
                        {
                            txtSignUpNameErr.Visibility = Visibility.Visible;
                            canPass = false;
                            break;
                        }
                    }
                }

                //Check if there is Unvalid Email format
                if (!txtBoxSignUpEmail.Text.IsEmail())
                {
                    txtSignUpEmailErr.Visibility = Visibility.Visible;
                    canPass = false;
                }

                //Check if there is Unvalid Phone number format
                if (!txtBoxSignUpPhonNum.Text.IsPhoneNumber())
                {
                    txtSignUpPhonNumErr.Visibility = Visibility.Visible;
                    canPass = false;
                }

                //Check if there is Unvalid National number format
                if (!txtBoxSignUpNatNum.Text.IsNumeric())
                {
                    txtSignUpNatNumErr.Visibility = Visibility.Visible;
                    canPass = false;
                }

                return canPass;
            }
            else
            {
                errorToastNotificationMessage.Text =
                    TranslatingUI.SignInResourceMap.GetValue("txtToastNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                errorToastNotification.Show(6000);
                return false;
            }
        }

        private async void BtnSignUpSubmit_PointerPressed(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SignUpCheckConstrains())
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                    {
                        progBarProcess.IsIndeterminate = true;

                        var httpResponse = await SignInRepo.SignUpAdminAsync(
                                new Admin()
                                {
                                    Name = txtBoxSignUpName.Text,
                                    Email = txtBoxSignUpEmail.Text,
                                    Password = txtBoxSignUpPassword.Password,
                                    PhoneNumber = txtBoxSignUpPhonNum.Text,
                                    NationalNumber = txtBoxSignUpNatNum.Text,
                                    City = ((City)comBoxSignUpCities.SelectedItem).Id
                                });

                        if (httpResponse.Response.Result != null)
                        {
                            mainPivot.SelectedIndex = 6;
                        }
                        else
                        {
                            errorToastNotificationMessage.Text = httpResponse.ErrorMessage;
                            errorToastNotification.Show(6000);
                        }

                        progBarProcess.IsIndeterminate = false;
                    });
                }

            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
        }

        #endregion


        #endregion
    }
}
