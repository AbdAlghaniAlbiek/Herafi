using Herafi.Core.Helpers;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using System.Collections.ObjectModel;
using Herafi.Core.Models;
using Herafi.Core.Repositories.RemoteRepo;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Herafi.Core.ViewModels;
using Windows.ApplicationModel.Resources.Core;
using Herafi.UWP.Resources.Templates.UsersTemplates;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Herafi.UWP.Views.DeviceFamily_Desktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UsersView : Page, INotifyPropertyChanged
    {
        #region Fields
        public event PropertyChangedEventHandler PropertyChanged;
        private string _tabViewItemName;
        private int _newMembesCount = 0;
        private bool _isFromDashboard =false;
        #endregion

        #region Properties
        public string TabViewItemName
        {
            get { return _tabViewItemName; }
            set
            {
                _tabViewItemName = value;
                OnPropertyChanged();
            }
        }
        public int NewMembersCount
        {
            get { return _newMembesCount; }
            set { _newMembesCount = value; OnPropertyChanged(); }
        }
        public bool IsFromDashboard
        {
            get { return _isFromDashboard; }
            set { _isFromDashboard = value; OnPropertyChanged(); }
        }

        #endregion


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
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


        public UsersView()
        {
            this.InitializeComponent();

            // Subscribe to the event that's raised when a qualifier value changes.
            ResourceContext.GetForCurrentView().QualifierValues.MapChanged += new MapChangedEventHandler<string, string>(QualifierValues_MapChanged);

            InitializeNavigationViewItems_PointerHandlers();

            InitializeNoWifiRefreshButtons_PointerHandlers_NetworkChange_PageLoadin();
        }
       

        private async Task RefreshGeneralUsersPivotContent()
        {
            try
            {
                if (usersViewModel.Users == null)
                {
                    if (IsNetworkAvailable())
                    {
                        await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                        {
                            if (NoWifiGeneralUsers.Visibility == Visibility.Visible)
                            {
                                NoWifiGeneralUsers.Visibility = Visibility.Collapsed;

                                fixedToastNotificationMessage.Text =
                                    TranslatingUI.UsersResourceMap.GetValue("txtToastConnWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                                fixedToastNotification.Show(6000);
                            }

                            loadingData.IsLoading = true;

                            HttpReponse httpReponse =
                                await UsersRepo.GetGeneralUsersAsync((30).ToString(), (0).ToString());

                            loadingData.IsLoading = false;


                            if (httpReponse.Response.Result != null)
                            {
                                if ((httpReponse.Response.Result as ObservableCollection<User>).Count > 0)
                                {
                                    usersViewModel.Users =
                                        (ObservableCollection<User>)httpReponse.Response.Result;
                                }
                                else
                                {
                                    EmptyUsersContent.Visibility = Visibility.Visible;
                                }
                            }
                            else
                            {
                                errorToastNotificationMessage.Text = httpReponse.ErrorMessage;
                                errorToastNotification.Show(6000);
                            }

                        });
                    }
                    else
                    {
                        NoWifiGeneralUsers.Visibility = Visibility.Visible;
                    }
                }

            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
        }

        private async Task RefreshNewMembersUsersPivotContent()
        {
            try
            {
                if (usersViewModel.NewMembersUsers == null)
                {
                    if (IsNetworkAvailable())
                    {
                        await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                        {
                            if (NoWifiNewMemebersUsers.Visibility == Visibility.Visible)
                            {
                                NoWifiNewMemebersUsers.Visibility = Visibility.Collapsed;

                                fixedToastNotificationMessage.Text =
                                     TranslatingUI.UsersResourceMap.GetValue("txtToastConnWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                                fixedToastNotification.Show(6000);
                            }

                            loadingData.IsLoading = true;

                            HttpReponse httpReponse =
                                await UsersRepo.GetNewMembersUsersAsync((10).ToString(), (0).ToString());

                            loadingData.IsLoading = false;

                            if (httpReponse.Response.Result != null)
                            {
                                if (((ObservableCollection<ProfileUser>)httpReponse.Response.Result).Count > 0)
                                {
                                    usersViewModel.NewMembersUsers =
                                        (ObservableCollection<ProfileUser>)httpReponse.Response.Result;

                                    NewMembersCount += 10;
                                }
                                else
                                {
                                    EmptyNewUsersContent.Visibility = Visibility.Visible;
                                }
                            }
                            else
                            {
                                errorToastNotificationMessage.Text = httpReponse.ErrorMessage;
                                errorToastNotification.Show(6000);
                            }
                        });
                    }
                    else
                    {
                        NoWifiNewMemebersUsers.Visibility = Visibility.Visible;
                    }
                }

            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }

        }


        # region InitializeNoWifiRefreshButtons_PointerHandlers_NetworkChange_PageLoadin

        private void InitializeNoWifiRefreshButtons_PointerHandlers_NetworkChange_PageLoadin()
        {
            this.Loading += UsersView_Loading;
            NetworkHelper.Instance.NetworkChanged += NetworkChanged;

            btnNoWifiGeneralUsers.Click += new RoutedEventHandler(BtnNoWifiGeneralUsers_Click);
            btnNoWifiNewMemUsers.Click += new RoutedEventHandler(BtnNoWifiNewMemUsers_Click);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                IsFromDashboard = true;
            }
        }

        private async void UsersView_Loading(FrameworkElement sender, object args)
        {
            await ActivateRefreshUITexts();

            if (IsFromDashboard)
            {
                (tabViewItemNewMembers.Resources["tabViewItemNewMembersPressed"] as Storyboard).Begin();
                TabViewItemName = newMembersItemText.Text;

                mainUsersPivot.SelectedIndex = 1;
                await RefreshNewMembersUsersPivotContent();
            }
            else
            {
                (tabViewItemGeneral.Resources["tabViewItemGeneralPressed"] as Storyboard).Begin();
                TabViewItemName = generalItemText.Text;

                mainUsersPivot.SelectedIndex = 0;
                await RefreshGeneralUsersPivotContent();
            }

        }

        private async void NetworkChanged(object sender, EventArgs e)
        {
            if ((TabViewItemName == generalItemText.Text))
            {
                await RefreshGeneralUsersPivotContent();
            }
            else if ((TabViewItemName == newMembersItemText.Text))
            {
                await RefreshNewMembersUsersPivotContent();
            }
        }

        private async void BtnNoWifiGeneralUsers_Click(object sender, RoutedEventArgs e)
        {
            await RefreshGeneralUsersPivotContent();
        }

        private async void BtnNoWifiNewMemUsers_Click(object sender, RoutedEventArgs e)
        {
            await RefreshNewMembersUsersPivotContent();
        }

        #endregion


        #region Initialize Tab View Items Pointer events

        private void InitializeNavigationViewItems_PointerHandlers()
        {
            //General navigation view item pointer events handlers
            tabViewItemGeneral.PointerEntered += new PointerEventHandler(TabViewItemGeneral_PointerEntered);
            tabViewItemGeneral.PointerExited += new PointerEventHandler(TabViewItemGeneral_PointerExited);
            tabViewItemGeneral.PointerPressed += new PointerEventHandler(TabViewItemGeneral_PointerPressed);
            grdViewUsers.SelectionChanged += GrdViewUsers_SelectionChanged;
            btnDetailsGoBack.Click += new RoutedEventHandler(BtnDetailsGoBack_Click);
            btnCaViErrCancel.Click += new RoutedEventHandler((s ,e) => errorDetailsCardView.IsOpen = false);

            //New members navigation view item pointer events handlers
            tabViewItemNewMembers.PointerEntered += TabViewItemNewMembers_PointerEntered;
            tabViewItemNewMembers.PointerExited += TabViewItemNewMembers_PointerExited;
            tabViewItemNewMembers.PointerPressed += TabViewItemNewMembers_PointerPressed;
        }

        #region General 
        private async void GrdViewUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (IsNetworkAvailable())
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                    {
                        swipBackGeneralUsers.IsOpen = false;

                        loadingGeneralUsersData.IsLoading = true;

                        var profileHttpResponse = await UsersRepo.GetUserDetailsProfileAsync(
                                ((User)grdViewUsers.SelectedItem).Id);

                        var ReqHttpResponse = await UsersRepo.GetUserDetailsRequestsAsync(
                                ((User)grdViewUsers.SelectedItem).Id);

                        usersViewModel.UserDetails = new UserDetails();

                        if (profileHttpResponse.Response.Result != null &&
                            ReqHttpResponse.Response.Result != null)
                        {
                            usersViewModel.UserDetails.ProfileUser =
                                (ProfileUser)profileHttpResponse.Response.Result;

                            if (((ObservableCollection<RequestUser>)ReqHttpResponse.Response.Result).Count > 0)
                            {
                                usersViewModel.UserDetails.RequestsUser =
                                    (ObservableCollection<RequestUser>)ReqHttpResponse.Response.Result;
                            }
                            else
                            {
                                EmptyUserRequests.Visibility = Visibility.Visible;
                            }

                            btnDetailsGoBack.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            swipBackGeneralUsers.IsOpen = true;

                            txtDetProfError.Text =
                                !string.IsNullOrEmpty(
                                    profileHttpResponse.ErrorMessage) ? profileHttpResponse.ErrorMessage : "";
                            txtDetProfError.Text =
                                !string.IsNullOrEmpty(
                                    profileHttpResponse.ErrorMessage) ? profileHttpResponse.ErrorMessage : "";

                            errorDetailsCardView.IsOpen = true;
                        }

                        loadingGeneralUsersData.IsLoading = false;
                    });
                }
                else
                {
                    errorToastNotificationMessage.Text =
                        TranslatingUI.UsersResourceMap.GetValue("txtToastNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                    errorToastNotification.Show(6000);
                }
            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
           
        }

        private void BtnDetailsGoBack_Click(object sender, RoutedEventArgs e)
        {
            swipBackGeneralUsers.IsOpen = true;

            btnDetailsGoBack.Visibility = Visibility.Collapsed;
        }

        private void TabViewItemGeneral_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (TabViewItemName != TranslatingUI.UsersResourceMap.GetValue("generalItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (tabViewItemGeneral.Resources["tabViewItemGeneralPointerOver"] as Storyboard).Begin();
            }
        }

        private void TabViewItemGeneral_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (TabViewItemName != TranslatingUI.UsersResourceMap.GetValue("generalItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (tabViewItemGeneral.Resources["tabViewItemGeneralNormal"] as Storyboard).Begin();
            }
        }

        private async void TabViewItemGeneral_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            (tabViewItemNewMembers.Resources["tabViewItemNewMembersNormal"] as Storyboard).Begin();

            (tabViewItemGeneral.Resources["tabViewItemGeneralPressed"] as Storyboard).Begin();
            TabViewItemName = generalItemText.Text;

            mainUsersPivot.SelectedIndex = 0;
            await RefreshGeneralUsersPivotContent();
        }

        private async void scrVieGeneralUsers_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            progGeneralUsers.IsActive = true;

            HttpReponse httpReponse =
                await UsersRepo.GetGeneralUsersAsync((30).ToString(), usersViewModel.Users.Count.ToString());

            progGeneralUsers.IsActive = false;


            if (httpReponse.Response.Result != null)
            {
                foreach (var user in httpReponse.Response.Result as ObservableCollection<User>)
                {
                    usersViewModel.Users.Add(user);
                }
            }
            else
            {
                errorToastNotificationMessage.Text = httpReponse.ErrorMessage;
                errorToastNotification.Show(6000);
            }
        }


        #endregion

        #region New Members

        private void TabViewItemNewMembers_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (TabViewItemName != TranslatingUI.UsersResourceMap.GetValue("newMembersItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (tabViewItemNewMembers.Resources["tabViewItemNewMembersPointerOver"] as Storyboard).Begin();
            }
        }

        private void TabViewItemNewMembers_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (TabViewItemName != TranslatingUI.UsersResourceMap.GetValue("newMembersItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (tabViewItemNewMembers.Resources["tabViewItemNewMembersNormal"] as Storyboard).Begin();
            }
        }

        private async void TabViewItemNewMembers_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            (tabViewItemGeneral.Resources["tabViewItemGeneralNormal"] as Storyboard).Begin();

            (tabViewItemNewMembers.Resources["tabViewItemNewMembersPressed"] as Storyboard).Begin();
            TabViewItemName = newMembersItemText.Text;

            mainUsersPivot.SelectedIndex = 1;
            await RefreshNewMembersUsersPivotContent();
        }

        private async void NewMemUserTemp_AcceptUserEvent(object sender, EventArgs e)
        {
            try
            {
                if (IsNetworkAvailable())
                {
                    loadingData.IsLoading = true;

                    var httpResponse =
                            await UsersRepo.AcceptNewMemberUserAsync((sender as NewMemUserTemp).ProfileUser.Id);

                    loadingData.IsLoading = false;

                    if (httpResponse.Response.Result != null)
                    {
                        usersViewModel.NewMembersUsers.Remove((sender as NewMemUserTemp).ProfileUser);

                        fixedToastNotificationMessage.Text =
                            TranslatingUI.UsersResourceMap.GetValue("txtToastNewMemUserAcc", TranslatingUI.ResourceContextObj).ValueAsString;
                        fixedToastNotification.Show(6000);
                    }
                    else
                    {
                        errorToastNotificationMessage.Text = httpResponse.ErrorMessage;
                        errorToastNotification.Show(6000);
                    }
                }
                else
                {
                    errorToastNotificationMessage.Text =
                         TranslatingUI.UsersResourceMap.GetValue("txtToastNewMemUserNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                    errorToastNotification.Show(6000);
                }
            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
           
        }

        private async void NewMemUserTemp_RefuseUserEvent(object sender, EventArgs e)
        {
            try
            {
                if (IsNetworkAvailable())
                {
                    var httpResponse =
                            await UsersRepo.RefuseNewMemberUserAsync((sender as NewMemUserTemp).ProfileUser.Id);

                    if (httpResponse.Response.Result != null)
                    {
                        usersViewModel.NewMembersUsers.Remove((sender as NewMemUserTemp).ProfileUser);
                        fixedToastNotificationMessage.Text =
                             TranslatingUI.UsersResourceMap.GetValue("txtToastNewMemUserRef", TranslatingUI.ResourceContextObj).ValueAsString;
                        fixedToastNotification.Show(6000);
                    }
                    else
                    {
                        errorToastNotificationMessage.Text = httpResponse.ErrorMessage;
                        errorToastNotification.Show(6000);
                    }
                }
                else
                {
                    errorToastNotificationMessage.Text =
                         TranslatingUI.UsersResourceMap.GetValue("txtToastNewMemUserNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                    errorToastNotification.Show(6000);
                }
            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
            
        }

        private async void scrVieNewMemUsers_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            try
            {
                progNewMemUsers.IsActive = true;

                HttpReponse httpReponse =
                    await UsersRepo.GetNewMembersUsersAsync((10).ToString(), NewMembersCount.ToString());

                progNewMemUsers.IsActive = false;

                if (httpReponse.Response.Result != null)
                {
                    foreach (var newMemUser in httpReponse.Response.Result as ObservableCollection<ProfileUser>)
                    {
                        usersViewModel.NewMembersUsers.Add(newMemUser);
                    }

                    NewMembersCount += 10;
                }
                else
                {
                    errorToastNotificationMessage.Text = httpReponse.ErrorMessage;
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

        #endregion


        #region Translating UI

        private void RefreshUIText()
        {
            GeneralUsersTranslating();
            NewMembersUsersAndCardViewsTranslating();
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

        private void GeneralUsersTranslating()
        {
            generalItemText.Text =
                TranslatingUI.UsersResourceMap.GetValue("generalItemText", TranslatingUI.ResourceContextObj).ValueAsString;
            newMembersItemText.Text =
                TranslatingUI.UsersResourceMap.GetValue("newMembersItemText", TranslatingUI.ResourceContextObj).ValueAsString;
            btnGoBackUsers.Text =
                TranslatingUI.UsersResourceMap.GetValue("btnGoBackUsers", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralProfileTitle.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtGeneralProfileTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralName.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtGeneralName", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralEmail.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtGeneralEmail", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralPhonNum.Text =
                 TranslatingUI.UsersResourceMap.GetValue("txtGeneralPhonNum", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralNatNum.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtGeneralNatNum", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCity.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtGeneralCity", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralDatJoin.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtGeneralDatJoin", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralFav.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtGeneralFav", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralSearchs.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtGeneralSearchs", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralReq.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtGeneralReq", TranslatingUI.ResourceContextObj).ValueAsString;
            txtEmptyUsers.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtEmptyUsers", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNoWifiGenUsers.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtNoWifiGenUsers", TranslatingUI.ResourceContextObj).ValueAsString;
        }

        private void NewMembersUsersAndCardViewsTranslating()
        {
            newMembersItemText.Text =
                TranslatingUI.UsersResourceMap.GetValue("newMembersItemText", TranslatingUI.ResourceContextObj).ValueAsString;
            txtEmptyNewUsers.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtEmptyNewUsers", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNoWifiNewMembers.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtNoWifiNewMembers", TranslatingUI.ResourceContextObj).ValueAsString;
            errorDetailsCardView.Header =
                TranslatingUI.UsersResourceMap.GetValue("errorDetailsCardViewHeader", TranslatingUI.ResourceContextObj).ValueAsString;
            txtCaViErrCancel.Text =
                TranslatingUI.UsersResourceMap.GetValue("txtCaViErrCancel", TranslatingUI.ResourceContextObj).ValueAsString;
        }


        #endregion
    }
}
