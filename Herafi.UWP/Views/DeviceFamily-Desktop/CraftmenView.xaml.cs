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
using Microsoft.Toolkit.Uwp.Connectivity;
using System.Net.NetworkInformation;
using Herafi.Core.Security;
using Herafi.Core.Helpers;
using Herafi.Core.Repositories;
using Herafi.Core.ViewModels;
using Microsoft.Toolkit.Uwp.Helpers;
using Herafi.Core.Repositories.RemoteRepo;
using System.Collections.ObjectModel;
using Herafi.Core.Models;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.ApplicationModel.Resources.Core;
using MaterialLibs.Controls;
using System.Diagnostics;
using Herafi.UWP.Resources.Templates.CraftmenTemplates;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Herafi.UWP.Views.DeviceFamily_Desktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CraftmenView : Page,INotifyPropertyChanged
    {
        #region Fields
        private NewMemberCraftman _newMemCraftman;
        private string _tabViewItemName;
        private int _newMembersCount = 0;
        private int _reportedBlockingCount = 0;
        private int _reportedFiringCount = 0;
        private bool _isFromDashboard = false;
        public event PropertyChangedEventHandler PropertyChanged;
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
            get { return _newMembersCount; }
            set { _newMembersCount = value; OnPropertyChanged(); }
        }
        public int ReportedBlockingCount
        {
            get { return _reportedBlockingCount; }
            set { _reportedBlockingCount = value; OnPropertyChanged(); }
        }
        public int ReportedFiringCount
        {
            get { return _reportedFiringCount; }
            set { _reportedFiringCount = value; OnPropertyChanged(); }
        }
        public NewMemberCraftman NewMemCraftman
        {
            get { return _newMemCraftman; }
            set { _newMemCraftman = value; OnPropertyChanged(); }
        }
        public bool IsFromDashboard
        {
            get { return _isFromDashboard; }
            set { _isFromDashboard = value; OnPropertyChanged(); }
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


        public CraftmenView()
        {
            this.InitializeComponent();

            // Subscribe to the event that's raised when a qualifier value changes.
            ResourceContext.GetForCurrentView().QualifierValues.MapChanged += new MapChangedEventHandler<string, string>(QualifierValues_MapChanged);

            InitializeNavigationViewItems_PointerHandlers();

            InitializeNoWifiRefreshButtons_PointerHandlers_NetworkChange_PageLoadin();
        }
       

        private async Task RefreshGeneralCraftmenPivotContent()
        {
            try
            {
                if (craftmenViewModel.Craftmen == null)
                {
                    if (IsNetworkAvailable())
                    {
                        await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                        {
                            if (NoWifiGeneral.Visibility == Visibility.Visible)
                            {
                                NoWifiGeneral.Visibility = Visibility.Collapsed;

                                fixedToastNotificationMessage.Text =
                                    TranslatingUI.CraftmenResourceMap.GetValue("txtToastConnWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                                fixedToastNotification.Show(6000);
                            }

                            loadingData.IsLoading = true;

                            HttpReponse httpReponse =
                                await CraftmenRepo.GetGeneralCraftmenAsync((30).ToString(), (0).ToString());

                            loadingData.IsLoading = false;

                            if (httpReponse.Response.Result != null)
                            {
                                if ((httpReponse.Response.Result as ObservableCollection<Craftman>).Count > 0)
                                {
                                    craftmenViewModel.Craftmen =
                                        (ObservableCollection<Craftman>)httpReponse.Response.Result;
                                }
                                else
                                {
                                    EmptyCraftmenContent.Visibility = Visibility.Visible;
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
                        NoWifiGeneral.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
           
        }

        private async Task RefreshNewMembersCraftmenPivotContent()
        {
            try
            {
                if (craftmenViewModel.NewMemberCraftmen == null)
                {
                    if (IsNetworkAvailable())
                    {
                        await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                        {
                            if (NoWifiNewMembers.Visibility == Visibility.Visible)
                            {
                                NoWifiNewMembers.Visibility = Visibility.Collapsed;

                                fixedToastNotificationMessage.Text =
                                    TranslatingUI.CraftmenResourceMap.GetValue("txtToastConnWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                                fixedToastNotification.Show(6000);
                            }

                            loadingData.IsLoading = true;

                            HttpReponse httpReponse =
                                await CraftmenRepo.GetNewMembersCraftmenAsync((10).ToString(), (0).ToString());

                            loadingData.IsLoading = false;

                            if (httpReponse.Response.Result != null)
                            {
                                if ((httpReponse.Response.Result as ObservableCollection<NewMemberCraftman>).Count > 0)
                                {
                                    craftmenViewModel.NewMemberCraftmen =
                                        (ObservableCollection<NewMemberCraftman>)httpReponse.Response.Result;

                                    NewMembersCount += 10;
                                }
                                else
                                {
                                    EmpNewMemCraftmenContent.Visibility = Visibility.Visible;
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
                        NoWifiNewMembers.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
            
        }

        private async Task RefreshReportBlockingCraftmenPivotContent()
        {
            try
            {
                if (craftmenViewModel.ReportBlockingCraftmen == null)
                {
                    if (IsNetworkAvailable())
                    {
                        await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                        {
                            if (NoWifiReportsBlocking.Visibility == Visibility.Visible)
                            {
                                NoWifiReportsBlocking.Visibility = Visibility.Collapsed;

                                fixedToastNotificationMessage.Text =
                                    TranslatingUI.CraftmenResourceMap.GetValue("txtToastConnWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                                fixedToastNotification.Show(6000);
                            }

                            loadingData.IsLoading = true;

                            HttpReponse httpReponse =
                                await CraftmenRepo.GetReportedBlockingCraftmenAsync((10).ToString(), (0).ToString());

                            HttpReponse httpReponse2 =
                                await CraftmenRepo.GetBlockingsFiringsCraftmenNumberAsync();

                            loadingData.IsLoading = false;

                            if (httpReponse.Response.Result != null && httpReponse2.Response.Result != null)
                            {
                                craftmenViewModel.ReportedNumbers =
                                    (ReportedNumbers)httpReponse2.Response.Result;

                                if ((httpReponse.Response.Result as ObservableCollection<ReportCraftman>).Count > 0)
                                {
                                    craftmenViewModel.ReportBlockingCraftmen =
                                        (ObservableCollection<ReportCraftman>)httpReponse.Response.Result;

                                    ReportedBlockingCount += 10;
                                }
                                else
                                {
                                    EmpBlocCraftmenContent.Visibility = Visibility.Visible;
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
                        NoWifiReportsBlocking.Visibility = Visibility.Visible;
                    }
                }

            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
        }

        private async Task RefreshReportFiringCraftmenPivotContent()
        {
            try
            {
                if (craftmenViewModel.ReportFiringCraftmen == null)
                {
                    if (IsNetworkAvailable())
                    {
                        await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                        {
                            if (NoWifiReportsFiring.Visibility == Visibility.Visible)
                            {
                                NoWifiReportsFiring.Visibility = Visibility.Collapsed;

                                fixedToastNotificationMessage.Text =
                                    TranslatingUI.CraftmenResourceMap.GetValue("txtToastConnWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                                fixedToastNotification.Show(6000);
                            }

                            loadingData.IsLoading = true;

                            HttpReponse httpReponse =
                                await CraftmenRepo.GetReportedFiringCraftmenAsync((10).ToString(), (0).ToString());

                            loadingData.IsLoading = false;

                            if (httpReponse.Response.Result != null)
                            {
                                if ((httpReponse.Response.Result as ObservableCollection<ReportCraftman>).Count > 0)
                                {
                                    craftmenViewModel.ReportFiringCraftmen =
                                        (ObservableCollection<ReportCraftman>)httpReponse.Response.Result;

                                    ReportedFiringCount += 10;
                                }
                                else
                                {
                                    EmpFirCraftmenContent.Visibility = Visibility.Visible;
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
                        NoWifiReportsFiring.Visibility = Visibility.Visible;
                    }
                }

            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
        }


        #region Initialize no wifif refresh buttons pointer handlers && Network change && page loading  

        private void InitializeNoWifiRefreshButtons_PointerHandlers_NetworkChange_PageLoadin()
        {
            NetworkHelper.Instance.NetworkChanged += NetworkChanged;
            this.Loaded += CraftmenView_Loaded;

            btnNoWifiGeneralCraftman.Click += new RoutedEventHandler(BtnNoWifiGeneralCraftman_Click);
            btnNoWifiNewMemCraftmen.Click += new RoutedEventHandler(BtnNoWifiNewMemCraftmen_Click);
            btnNoWifiRepBloCraftmen.Click += new RoutedEventHandler(BtnNoWifiRepBloCraftmen_Click);
            btnNoWifiRepFirCraftmen.Click += new RoutedEventHandler(BtnNoWifiRepFirCraftmen_Click);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                IsFromDashboard = true;
            }
        }

        private async void CraftmenView_Loaded(object sender, RoutedEventArgs e)
        {
            await ActivateRefreshUITexts();

            if (IsFromDashboard)
            {
                (tabViewItemNewMembers.Resources["tabViewItemNewMembersPressed"] as Storyboard).Begin();
                TabViewItemName = newMembersItemText.Text;

                mainCraftmenPivot.SelectedIndex = 1;
                await RefreshNewMembersCraftmenPivotContent();
            }
            else
            {
                (tabViewItemGeneral.Resources["tabViewItemGeneralPressed"] as Storyboard).Begin();
                TabViewItemName = generalItemText.Text;

                mainCraftmenPivot.SelectedIndex = 0;
                await RefreshGeneralCraftmenPivotContent();
            }
        }
      
        private async void NetworkChanged(object sender, EventArgs e)
        {
            if ((TabViewItemName == generalItemText.Text))
            {
                await RefreshGeneralCraftmenPivotContent();
            }

            else if ((TabViewItemName == newMembersItemText.Text))
            {
                await RefreshNewMembersCraftmenPivotContent();
            }

            else if ((TabViewItemName == reportsItemText.Text) &&
                (blocFirCraftmenPivot.SelectedIndex == 0))
            {
                await RefreshReportBlockingCraftmenPivotContent();
            }

            else if ((TabViewItemName == reportsItemText.Text) &&
               (blocFirCraftmenPivot.SelectedIndex == 1))
            {
                await RefreshReportFiringCraftmenPivotContent();
            }
        }

        #region No Wifi buttons refresh

        private async void BtnNoWifiGeneralCraftman_Click(object sender, RoutedEventArgs e)
        {
            await RefreshGeneralCraftmenPivotContent();
        }

        private async void BtnNoWifiNewMemCraftmen_Click(object sender, RoutedEventArgs e)
        {
            await RefreshNewMembersCraftmenPivotContent();
        }

        private async void BtnNoWifiRepBloCraftmen_Click(object sender, RoutedEventArgs e)
        {
            await RefreshReportBlockingCraftmenPivotContent();
        }

        private async void BtnNoWifiRepFirCraftmen_Click(object sender, RoutedEventArgs e)
        {
            await RefreshReportFiringCraftmenPivotContent();
        }

        #endregion

        #endregion


        #region Initialize Tab View Items Pointer events

        private void InitializeNavigationViewItems_PointerHandlers()
        {
            //General navigation view item
            tabViewItemGeneral.PointerEntered += new PointerEventHandler(TabViewItemGeneral_PointerEntered);
            tabViewItemGeneral.PointerExited += new PointerEventHandler(TabViewItemGeneral_PointerExited);
            tabViewItemGeneral.PointerPressed += new PointerEventHandler(TabViewItemGeneral_PointerPressed);
            grdVieGeneralCraftmen.SelectionChanged += GrdVieGeneralCraftmen_SelectionChanged;
            btnDetailsGoBack.Click += new RoutedEventHandler( BtnDetailsGoBack_Click);
            btnCaViErrCancel.Click += new RoutedEventHandler((s, e) => errorDetailsCardView.IsOpen = false);

            //New members navigation view item 
            tabViewItemNewMembers.PointerEntered += new PointerEventHandler(TabViewItemNewMembers_PointerEntered);
            tabViewItemNewMembers.PointerExited += new PointerEventHandler(TabViewItemNewMembers_PointerExited);
            tabViewItemNewMembers.PointerPressed += new PointerEventHandler(TabViewItemNewMembers_PointerPressed);

            //Reports navigation view item and blocking/firing pivot items 
            tabViewItemReports.PointerEntered += new PointerEventHandler(TabViewItemReports_PointerEntered);
            tabViewItemReports.PointerExited += new PointerEventHandler(TabViewItemReports_PointerExited);
            tabViewItemReports.PointerPressed += new PointerEventHandler(TabViewItemReports_PointerPressed);

            //Refused Card view events
            btnCaViRefuseMessageCancel.Click += new RoutedEventHandler((s, e) => refuseMessageCardView.IsOpen = false);
            btnCaViRefuseMessageSend.Click += new RoutedEventHandler(BtnCaViRefuseMessageSend_Click);
            txtBoxCaViRefuseMessage.TextChanged += (s, e) => txtCaViRefuseMessageErr.Visibility = Visibility.Collapsed;

        }

        #region General 

        private async void GrdVieGeneralCraftmen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (IsNetworkAvailable())
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                    {
                        swipBackGeneralCraftmen.IsOpen = false;

                        loadCraftmDetailsData.IsLoading = true;

                        var profileHttpReponse = await CraftmenRepo.GetCraftmanDetailsProfileAsync(
                                ((Craftman)grdVieGeneralCraftmen.SelectedItem).Id);

                        var craftsHttpResponse = await CraftmenRepo.GetCraftmanDetailsCraftsAsync(
                                ((Craftman)grdVieGeneralCraftmen.SelectedItem).Id);

                        var certsHttpResponse = await CraftmenRepo.GetCraftmanDetailsCertificationsAsync(
                                ((Craftman)grdVieGeneralCraftmen.SelectedItem).Id);

                        var reqHttpResponse = await CraftmenRepo.GetCraftmanDetailsRequestsAsync(
                                ((Craftman)grdVieGeneralCraftmen.SelectedItem).Id);

                        var projHttpResponse = await CraftmenRepo.GetCraftmanDetailsProjectsAsync(
                                ((Craftman)grdVieGeneralCraftmen.SelectedItem).Id);

                        loadCraftmDetailsData.IsLoading = false;

                        craftmenViewModel.CraftmanDetails = new CraftmanDetails();

                        if (profileHttpReponse.Response.Result != null &&
                            craftsHttpResponse.Response.Result != null &&
                            certsHttpResponse.Response.Result != null &&
                            reqHttpResponse.Response.Result != null &&
                            projHttpResponse.Response.Result != null)
                        {
                            craftmenViewModel.CraftmanDetails.Profile =
                                 (Profile)profileHttpReponse.Response.Result;

                            craftmenViewModel.CraftmanDetails.Crafts =
                                (ObservableCollection<Craft>)craftsHttpResponse.Response.Result;

                            craftmenViewModel.CraftmanDetails.Certifications =
                               (ObservableCollection<string>)certsHttpResponse.Response.Result;

                            //Check if there is requests for this craftman or not
                            if (((ObservableCollection<Request>)reqHttpResponse.Response.Result).Count > 0)
                            {
                                craftmenViewModel.CraftmanDetails.Requests =
                                    (ObservableCollection<Request>)reqHttpResponse.Response.Result;
                            }
                            else
                            {
                                EmptyCraftmanRequests.Visibility = Visibility.Visible;
                            }

                            //Check if there is projects for this craftman or not
                            if (((ObservableCollection<Project>)projHttpResponse.Response.Result).Count > 0)
                            {
                                craftmenViewModel.CraftmanDetails.Projects =
                                    (ObservableCollection<Project>)projHttpResponse.Response.Result;
                            }
                            else
                            {
                                EmptyCraftmanProjects.Visibility = Visibility.Visible;
                            }

                            btnDetailsGoBack.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            swipBackGeneralCraftmen.IsOpen = true;

                            txtDetProfError.Text = !string.IsNullOrEmpty(profileHttpReponse.ErrorMessage) ? profileHttpReponse.ErrorMessage : "";
                            txtDetCraftsError.Text = !string.IsNullOrEmpty(craftsHttpResponse.ErrorMessage) ? craftsHttpResponse.ErrorMessage : "";
                            txtDetCertError.Text = !string.IsNullOrEmpty(certsHttpResponse.ErrorMessage) ? certsHttpResponse.ErrorMessage : "";
                            txtDetReqError.Text = !string.IsNullOrEmpty(reqHttpResponse.ErrorMessage) ? reqHttpResponse.ErrorMessage : "";
                            txtDetProjError.Text = !string.IsNullOrEmpty(projHttpResponse.ErrorMessage) ? projHttpResponse.ErrorMessage : "";

                            errorDetailsCardView.IsOpen = true;
                        }
                    });
                }
                else
                {
                    errorToastNotificationMessage.Text =
                        TranslatingUI.CraftmenResourceMap.GetValue("txtToastNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                    errorToastNotification.Show(6000);
                }
            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
           
        }

        private void TabViewItemGeneral_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (TabViewItemName != TranslatingUI.CraftmenResourceMap.GetValue("generalItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (tabViewItemGeneral.Resources["tabViewItemGeneralPointerOver"] as Storyboard).Begin();
            }
        }

        private void TabViewItemGeneral_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (TabViewItemName != TranslatingUI.CraftmenResourceMap.GetValue("generalItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (tabViewItemGeneral.Resources["tabViewItemGeneralNormal"] as Storyboard).Begin();
            }
        }

        private async void TabViewItemGeneral_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            (tabViewItemNewMembers.Resources["tabViewItemNewMembersNormal"] as Storyboard).Begin();
            (tabViewItemReports.Resources["tabViewItemReportsNormal"] as Storyboard).Begin();

            (tabViewItemGeneral.Resources["tabViewItemGeneralPressed"] as Storyboard).Begin();
            TabViewItemName = generalItemText.Text;

            mainCraftmenPivot.SelectedIndex = 0;
            await RefreshGeneralCraftmenPivotContent();
        }

        private void BtnDetailsGoBack_Click(object sender, RoutedEventArgs e)
        {
            swipBackGeneralCraftmen.IsOpen = true;

            btnDetailsGoBack.Visibility = Visibility.Collapsed;
        }

        private async void scrVieGeneralCraftmen_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            try
            {
                if (scrVieGeneralCraftmen.VerticalOffset == scrVieGeneralCraftmen.Height)
                {
                    progGeneralCraftmen.IsActive = true;

                    HttpReponse httpReponse =
                        await CraftmenRepo.GetGeneralCraftmenAsync(
                            (30).ToString(), craftmenViewModel.Craftmen.Count.ToString());

                    progGeneralCraftmen.IsActive = false;

                    if (httpReponse.Response.Result != null)
                    {
                        foreach (var craftman in httpReponse.Response.Result as ObservableCollection<Craftman>)
                        {
                            craftmenViewModel.Craftmen.Add(craftman);
                        }
                    }
                    else
                    {
                        errorToastNotificationMessage.Text = httpReponse.ErrorMessage;
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

        #region New Members

        private void TabViewItemNewMembers_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (TabViewItemName != TranslatingUI.CraftmenResourceMap.GetValue("newMembersItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (tabViewItemNewMembers.Resources["tabViewItemNewMembersPointerOver"] as Storyboard).Begin();
            }
        }

        private void TabViewItemNewMembers_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (TabViewItemName != TranslatingUI.CraftmenResourceMap.GetValue("newMembersItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (tabViewItemNewMembers.Resources["tabViewItemNewMembersNormal"] as Storyboard).Begin();
            }
        }

        private async void TabViewItemNewMembers_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            (tabViewItemReports.Resources["tabViewItemReportsNormal"] as Storyboard).Begin();
            (tabViewItemGeneral.Resources["tabViewItemGeneralNormal"] as Storyboard).Begin();

            (tabViewItemNewMembers.Resources["tabViewItemNewMembersPressed"] as Storyboard).Begin();
            TabViewItemName = newMembersItemText.Text;

            mainCraftmenPivot.SelectedIndex = 1;
            await RefreshNewMembersCraftmenPivotContent();
        }

        private async void NewMemCraftmanTemp_AcceptButtonEvent(object sender, EventArgs e)
        {
            try
            {
                if (IsNetworkAvailable())
                {
                    loadingData.IsLoading = true;

                    var httpResponse = await CraftmenRepo.AcceptNewMemberCraftmanAsync(
                            ((NewMemCraftmanTemp)sender).NewMemeberCraftman.Profile.Id,
                            ((NewMemCraftmanTemp)sender).NewMemeberCraftman.Level);

                    loadingData.IsLoading = false;

                    if (httpResponse.Response.Result != null)
                    {
                        craftmenViewModel.NewMemberCraftmen.Remove(((NewMemCraftmanTemp)sender).NewMemeberCraftman);

                        fixedToastNotificationMessage.Text =
                            TranslatingUI.CraftmenResourceMap.GetValue("txtToastNewMemCraftmanAcc", TranslatingUI.ResourceContextObj).ValueAsString;
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
                        TranslatingUI.CraftmenResourceMap.GetValue("txtToastNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                    errorToastNotification.Show(6000);
                }
            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
           
        }

        private void NewMemCraftmanTemp_RefuseButtonEvent(object sender, EventArgs e)
        {
            NewMemCraftman = (sender as NewMemCraftmanTemp).NewMemeberCraftman;
            refuseMessageCardView.IsOpen = true;
        }

        private async void BtnCaViRefuseMessageSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtBoxCaViRefuseMessage.Text))
                {
                    if (IsNetworkAvailable())
                    {
                        refuseMessageCardView.IsOpen = false;

                        loadingData.IsLoading = true;

                        var httpResponse =
                                await CraftmenRepo.RefuseNewMemberCraftmanAsync(NewMemCraftman.Profile.Id);

                        loadingData.IsLoading = false;

                        if (httpResponse.Response.Result != null)
                        {
                            craftmenViewModel.NewMemberCraftmen.Remove(NewMemCraftman);

                            fixedToastNotificationMessage.Text =
                                TranslatingUI.CraftmenResourceMap.GetValue("txtToastNewMemCraftmanRef", TranslatingUI.ResourceContextObj).ValueAsString;
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
                            TranslatingUI.CraftmenResourceMap.GetValue("txtToastNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                        errorToastNotification.Show(6000);
                    }
                }
                else
                {
                    txtCaViRefuseMessageErr.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
        }

        private async void scrVieNewMemCraftmen_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            try
            {
                if (scrVieNewMemCraftmen.VerticalOffset == scrVieNewMemCraftmen.Height)
                {
                    progNewMemCraftmen.IsActive = true;

                    HttpReponse httpReponse =
                        await CraftmenRepo.GetNewMembersCraftmenAsync((10).ToString(), NewMembersCount.ToString());

                    progNewMemCraftmen.IsActive = false;

                    if (httpReponse.Response.Result != null)
                    {
                        foreach (var newMemCraftman in httpReponse.Response.Result as ObservableCollection<NewMemberCraftman>)
                        {
                            craftmenViewModel.NewMemberCraftmen.Add(newMemCraftman);
                        }

                        NewMembersCount += 10;
                    }
                    else
                    {
                        errorToastNotificationMessage.Text = httpReponse.ErrorMessage;
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

        #region Reports

        private async void blocFirCraftmenPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (blocFirCraftmenPivot.SelectedIndex == 0)
            {
                await RefreshReportBlockingCraftmenPivotContent();
            }
            else
            {
                await RefreshReportFiringCraftmenPivotContent();
            }
        }

        private void TabViewItemReports_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (TabViewItemName != TranslatingUI.CraftmenResourceMap.GetValue("reportsItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (tabViewItemReports.Resources["tabViewItemReportsPointerOver"] as Storyboard).Begin();
            }
        }

        private void TabViewItemReports_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (TabViewItemName != TranslatingUI.CraftmenResourceMap.GetValue("reportsItemText", TranslatingUI.ResourceContextObj).ValueAsString)
            {
                (tabViewItemReports.Resources["tabViewItemReportsNormal"] as Storyboard).Begin();
            }
        }

        private async void TabViewItemReports_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            (tabViewItemGeneral.Resources["tabViewItemGeneralNormal"] as Storyboard).Begin();
            (tabViewItemNewMembers.Resources["tabViewItemNewMembersNormal"] as Storyboard).Begin();

            (tabViewItemReports.Resources["tabViewItemReportsPressed"] as Storyboard).Begin();
            TabViewItemName = reportsItemText.Text;

            mainCraftmenPivot.SelectedIndex = 2;
            await RefreshReportBlockingCraftmenPivotContent();
        }

        private async void ReportCraftmanTemp_ReportCraftmanEvent(object sender, EventArgs e)
        {
            try
            {
                if (IsNetworkAvailable())
                {
                    if ((sender as ReportCraftmanTemp).ButtonText == "Block" || (sender as ReportCraftmanTemp).ButtonText == "إيقاف")
                    {
                        loadingData.IsLoading = true;

                        var httpReponse =
                                await CraftmenRepo.BlockingCraftmanAsync((sender as ReportCraftmanTemp).ReportCraftman.Id);

                        loadingData.IsLoading = false;

                        if (httpReponse.Response.Result != null)
                        {
                            craftmenViewModel.ReportBlockingCraftmen.Remove(
                                (sender as ReportCraftmanTemp).ReportCraftman);

                            fixedToastNotificationMessage.Text =
                                TranslatingUI.CraftmenResourceMap.GetValue("txtToastRepBloc", TranslatingUI.ResourceContextObj).ValueAsString;
                            fixedToastNotification.Show(6000);
                        }
                        else
                        {
                            errorToastNotificationMessage.Text = httpReponse.ErrorMessage;
                            errorToastNotification.Show(6000);
                        }
                    }
                    else //Fire || طرد
                    {
                        loadingData.IsLoading = true;

                        var httpReponse =
                                await CraftmenRepo.FiringCraftmanAsync((sender as ReportCraftmanTemp).ReportCraftman.Id);

                        loadingData.IsLoading = false;

                        if (httpReponse.Response.Result != null)
                        {
                            craftmenViewModel.ReportFiringCraftmen.Remove(
                                (sender as ReportCraftmanTemp).ReportCraftman);

                            fixedToastNotificationMessage.Text =
                                TranslatingUI.CraftmenResourceMap.GetValue("txtToastRepFir", TranslatingUI.ResourceContextObj).ValueAsString;
                            fixedToastNotification.Show(6000);
                        }
                        else
                        {
                            errorToastNotificationMessage.Text = httpReponse.ErrorMessage;
                            errorToastNotification.Show(6000);
                        }
                    }
                }
                else
                {
                    errorToastNotificationMessage.Text =
                        TranslatingUI.CraftmenResourceMap.GetValue("txtToastNoWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                    errorToastNotification.Show(6000);
                }
            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
           
        }

        private async void scrVieRepBlockedCraftmen_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            try
            {
                if (scrVieRepBlockedCraftmen.VerticalOffset == scrVieRepBlockedCraftmen.Height)
                {
                    progRepBlockedCraftmen.IsActive = true;

                    HttpReponse httpReponse =
                        await CraftmenRepo.GetReportedBlockingCraftmenAsync((10).ToString(), ReportedBlockingCount.ToString());

                    progRepBlockedCraftmen.IsActive = false;

                    if (httpReponse.Response.Result != null)
                    {
                        foreach (var repBlockingCraftman in httpReponse.Response.Result as ObservableCollection<ReportCraftman>)
                        {
                            craftmenViewModel.ReportBlockingCraftmen.Add(repBlockingCraftman);
                        }

                        ReportedBlockingCount += 10;
                    }
                    else
                    {
                        errorToastNotificationMessage.Text = httpReponse.ErrorMessage;
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

        private async void scrVieRepFiringCraftmen_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            try
            {
                if (scrVieRepFiringCraftmen.VerticalOffset == scrVieRepFiringCraftmen.Height)
                {
                    progRepFiringCraftmen.IsActive = true;

                    HttpReponse httpReponse =
                        await CraftmenRepo.GetReportedFiringCraftmenAsync((10).ToString(), ReportedFiringCount.ToString());

                    progRepFiringCraftmen.IsActive = false;

                    if (httpReponse.Response.Result != null)
                    {
                        foreach (var repFiringCraftman in httpReponse.Response.Result as ObservableCollection<ReportCraftman>)
                        {
                            craftmenViewModel.ReportFiringCraftmen.Add(repFiringCraftman);
                        }

                        ReportedFiringCount += 10;
                    }
                    else
                    {
                        errorToastNotificationMessage.Text = httpReponse.ErrorMessage;
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

        #endregion


        #region Translating UI

        private void RefreshUIText()
        {
            GeneralCraftmenTranslating();
            NewMembersCraftmenTranslating();
            ReportedCraftmenAndCardViewsTranslating();
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

        private void GeneralCraftmenTranslating()
        {
            generalItemText.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("generalItemText", TranslatingUI.ResourceContextObj).ValueAsString;
            newMembersItemText.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("newMembersItemText", TranslatingUI.ResourceContextObj).ValueAsString;
            reportsItemText.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("reportsItemText", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGoBackCraftmen.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtGoBackCraftmen", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralName.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralName", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralEmail.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralEmail", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralPhonNum.Text =
                 TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralPhonNum", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralDatJoin.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralDatJoin", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralNatNum.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralNatNum", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralDatJoin.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralDatJoin", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCity.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralCity", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralTaxi.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralTaxi", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralStatus.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralStatus", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralLevel.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralLevel", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralUsersSear.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralUsersSear", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralUsersFav.Text =
                    TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralUsersFav", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralBlocks.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralBlocks", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCrafts.Text =
                    TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralCrafts", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralCert.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralCert", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralReq.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralReq", TranslatingUI.ResourceContextObj).ValueAsString;
            txtGeneralProj.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtGeneralProj", TranslatingUI.ResourceContextObj).ValueAsString;
            txtEmpCraftmanReqests.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtEmpCraftmanReqests", TranslatingUI.ResourceContextObj).ValueAsString;
            txtEmpCraftmanProjects.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtEmpCraftmanProjects", TranslatingUI.ResourceContextObj).ValueAsString;
            txtEmptyCraftmen.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtEmptyCraftmen", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNoWifiGeneral.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtNoWifiGeneral", TranslatingUI.ResourceContextObj).ValueAsString;
            txtBtnNoWifiGeneral.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtBtnNoWifiGeneral", TranslatingUI.ResourceContextObj).ValueAsString;
        }

        private void NewMembersCraftmenTranslating()
        {
            newMembersItemText.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("newMembersItemText", TranslatingUI.ResourceContextObj).ValueAsString;
            txtEmptyNewMemCraftmen.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtEmptyNewMemCraftmen", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNoWifiNewMembers.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtNoWifiNewMembers", TranslatingUI.ResourceContextObj).ValueAsString;
            txtBtnNoWifiNewMembers.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtBtnNoWifiNewMembers", TranslatingUI.ResourceContextObj).ValueAsString;

            refuseMessageCardView.Header =
               TranslatingUI.CraftmenResourceMap.GetValue("refuseMessageCardView", TranslatingUI.ResourceContextObj).ValueAsString;
            txtCaViRefuseMessageDesc.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtCaViRefuseMessageDesc", TranslatingUI.ResourceContextObj).ValueAsString;
            txtCaViRefuseMessageErr.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtCaViRefuseMessageErr", TranslatingUI.ResourceContextObj).ValueAsString;
            txtBoxCaViRefuseMessage.PlaceholderText =
               TranslatingUI.CraftmenResourceMap.GetValue("txtBoxCaViRefuseMessage", TranslatingUI.ResourceContextObj).ValueAsString;
            txtCaViRefuseMessageCancel.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtCaViRefuseMessageCancel", TranslatingUI.ResourceContextObj).ValueAsString;
            txtCaViRefuseMessageSend.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtCaViRefuseMessageSend", TranslatingUI.ResourceContextObj).ValueAsString;
        }

        private void ReportedCraftmenAndCardViewsTranslating()
        {
            reportsItemText.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("reportsItemText", TranslatingUI.ResourceContextObj).ValueAsString;
            txtRepBlocking.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtRepBlocking", TranslatingUI.ResourceContextObj).ValueAsString;
            txtEmptyBlocCraftmen.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtEmptyBlocCraftmen", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNoWifiRepBlocking.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtNoWifiRepBlocking", TranslatingUI.ResourceContextObj).ValueAsString;
            txtBtnNoWifiReportsBlocking.Text =
                  TranslatingUI.CraftmenResourceMap.GetValue("txtBtnNoWifiReportsBlocking", TranslatingUI.ResourceContextObj).ValueAsString;

            txtRepFiring.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtRepFiring", TranslatingUI.ResourceContextObj).ValueAsString;
            txtEmptyFirCraftmen.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtEmptyFirCraftmen", TranslatingUI.ResourceContextObj).ValueAsString;
            txtNoWifiReportsFiring.Text =
                TranslatingUI.CraftmenResourceMap.GetValue("txtNoWifiReportsFiring", TranslatingUI.ResourceContextObj).ValueAsString;
            txtBtnNoWifiReportsFiring.Text =
                  TranslatingUI.CraftmenResourceMap.GetValue("txtBtnNoWifiReportsFiring", TranslatingUI.ResourceContextObj).ValueAsString;

            errorDetailsCardView.Header = 
                TranslatingUI.CraftmenResourceMap.GetValue("errorDetailsCardViewHeader", TranslatingUI.ResourceContextObj).ValueAsString;
            txtCaViErrCancel.Text =
               TranslatingUI.CraftmenResourceMap.GetValue("txtCaViErrCancel", TranslatingUI.ResourceContextObj).ValueAsString;

        }

        #endregion

    }
}
