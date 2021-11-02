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
using Herafi.Core.Repositories.RemoteRepo;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.Helpers;
using Herafi.Core.Models;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Herafi.UWP.Views.DeviceFamily_Desktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AnalyzesView : Page
    {
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

        public AnalyzesView()
        {
            this.InitializeComponent();

            //Initializing pointer pressed for all buttons of this view
            Initialize_PointerEvents();

            // Subscribe to the event that's raised when a qualifier value changes.
            ResourceContext.GetForCurrentView().QualifierValues.MapChanged += new MapChangedEventHandler<string, string>(QualifierValues_MapChanged);
        }

        private void Initialize_PointerEvents()
        {
            this.Loading += AnalyzesView_Loading;
            NetworkHelper.Instance.NetworkChanged += NetworkChanged;
            btnNoWifiAnalyzes.Click += new RoutedEventHandler(BtnNoWifiAnalyzes_Click);
            btnCaViErrCancel.Click += new RoutedEventHandler((s, e) => errorDetailsCardView.IsOpen = false);

            comBoxProfitsMonth.SelectionChanged += new SelectionChangedEventHandler(ComBoxProfitsMonth_SelectionChanged);
            comBoxProfitsYear.SelectionChanged += new SelectionChangedEventHandler(ComBoxProfitsYear_SelectionChanged);
            comBoxCraftmenMonth.SelectionChanged += new SelectionChangedEventHandler(ComBoxCraftmenMonth_SelectionChanged);
            comBoxCraftmenYear.SelectionChanged += new SelectionChangedEventHandler(ComBoxCraftmenYear_SelectionChanged);
            comBoxUsersMonth.SelectionChanged += new SelectionChangedEventHandler(ComBoxUsersMonth_SelectionChanged);
            comBoxUsersYear.SelectionChanged += new SelectionChangedEventHandler(ComBoxUsersYear_SelectionChanged);
            comBoxRequestsMonth.SelectionChanged += new SelectionChangedEventHandler(ComBoxRequestsMonth_SelectionChanged);
            comBoxRequestsYear.SelectionChanged += new SelectionChangedEventHandler(ComBoxRequestsYear_SelectionChanged);
            comBoxReportsMonth.SelectionChanged += new SelectionChangedEventHandler(ComBoxReportsMonth_SelectionChanged);
            comBoxReportsYear.SelectionChanged += new SelectionChangedEventHandler(ComBoxReportsYear_SelectionChanged);

            craftmenUsersPivot.SelectionChanged += new SelectionChangedEventHandler(CraftmenUsersPivot_SelectionChanged);
            requestsReportsPivot.SelectionChanged += new SelectionChangedEventHandler(RequestsReportsPivot_SelectionChanged);
        }


        private async void CraftmenUsersPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (craftmenUsersPivot.SelectedIndex == 1)
                {
                    if (analyzesViewModel.UsersRadDetails == null)
                    {
                        if (IsNetworkAvailable())
                        {
                            var usersYearsHttpReponse = await AnalyzesRepo.GetUsersYearsAsync();

                            if (usersYearsHttpReponse.Response.Result != null)
                            {
                                analyzesViewModel.UsersYears =
                                  (ObservableCollection<string>)usersYearsHttpReponse.Response.Result;

                                var usersMonthsHttpResponse =
                                        await AnalyzesRepo.GetUsersMonthsAsync(comBoxUsersYear.Items[0] as string);

                                if (usersMonthsHttpResponse.Response.Result != null)
                                {
                                    analyzesViewModel.UsersMonths =
                                        usersMonthsHttpResponse.Response.Result as ObservableCollection<string>;



                                    var usersDetailsHttpResponse =
                                        await AnalyzesRepo.GetUsersDetailsAsync(
                                            comBoxUsersYear.Items[0] as string, comBoxUsersMonth.Items[0] as string);

                                    if (usersDetailsHttpResponse.Response.Result != null)
                                    {
                                        analyzesViewModel.UsersRadDetails =
                                            usersDetailsHttpResponse.Response.Result as UsersRadDetails;
                                    }
                                    else
                                    {
                                        errorToastNotificationMessage.Text = usersDetailsHttpResponse.ErrorMessage;
                                        errorToastNotification.Show(6000);
                                    }
                                }
                                else
                                {
                                    errorToastNotificationMessage.Text = usersMonthsHttpResponse.ErrorMessage;
                                    errorToastNotification.Show(6000);
                                }
                            }
                            else
                            {
                                errorToastNotificationMessage.Text = usersYearsHttpReponse.ErrorMessage;
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
                }
            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
        }

        private async void RequestsReportsPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (requestsReportsPivot.SelectedIndex == 1)
                {
                    if (analyzesViewModel.ReportsRadDetails == null)
                    {
                        if (IsNetworkAvailable())
                        {
                            var reportsYearsHttpReponse = await AnalyzesRepo.GetReportsYearsAsync();

                            if (reportsYearsHttpReponse.Response.Result != null)
                            {
                                analyzesViewModel.ReportsYears =
                                   (ObservableCollection<string>)reportsYearsHttpReponse.Response.Result;



                                var reportsMonthsHttpReponse =
                                        await AnalyzesRepo.GetReportsMonthsAsync(comBoxReportsYear.Items[0] as string);

                                if (reportsMonthsHttpReponse.Response.Result != null)
                                {
                                    analyzesViewModel.ReportsMonths =
                                        (ObservableCollection<string>)reportsMonthsHttpReponse.Response.Result;



                                    var reportsDetailsHttpResponse =
                                            await AnalyzesRepo.GetReportsDetailsAsync(
                                                comBoxReportsYear.Items[0] as string, comBoxReportsMonth.Items[0] as string);

                                    if (reportsDetailsHttpResponse.Response.Result != null)
                                    {
                                        analyzesViewModel.ReportsRadDetails =
                                            reportsDetailsHttpResponse.Response.Result as ReportsRadDetails;
                                    }
                                    else
                                    {
                                        errorToastNotificationMessage.Text = reportsDetailsHttpResponse.ErrorMessage;
                                        errorToastNotification.Show(6000);
                                    }
                                }
                                else
                                {
                                    errorToastNotificationMessage.Text = reportsMonthsHttpReponse.ErrorMessage;
                                    errorToastNotification.Show(6000);
                                }
                            }
                            else
                            {
                                errorToastNotificationMessage.Text = reportsYearsHttpReponse.ErrorMessage;
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
                }

            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
        }


        #region Combo boxes events

        private async Task ComboxSelectionMonthChange(string chartType)
        {
            try
            {
                HttpReponse httpResponse;

                if (IsNetworkAvailable())
                {
                    if (chartType == "Profits")
                    {
                        if (comBoxProfitsYear.SelectedItem == null)
                        {
                            httpResponse = await AnalyzesRepo.GetProfitsDetailsAsync(
                                (string)comBoxProfitsYear.PlaceholderText, (string)comBoxProfitsMonth.SelectedItem);
                        }
                        else
                        {
                            if (comBoxProfitsMonth.SelectedItem == null)
                            {
                                httpResponse = await AnalyzesRepo.GetProfitsDetailsAsync(
                                    (string)comBoxProfitsYear.SelectedItem, (string)comBoxProfitsMonth.PlaceholderText);
                            }
                            else
                            {
                                httpResponse = await AnalyzesRepo.GetProfitsDetailsAsync(
                                   (string)comBoxProfitsYear.SelectedItem, (string)comBoxProfitsMonth.SelectedItem);
                            }

                        }

                    }
                    else if (chartType == "Craftmen")
                    {
                        if (comBoxCraftmenYear.SelectedItem == null)
                        {
                            httpResponse = await AnalyzesRepo.GetCraftmenDetailsAsync(
                                (string)comBoxCraftmenYear.PlaceholderText, (string)comBoxCraftmenMonth.SelectedItem);
                        }
                        else
                        {
                            if (comBoxCraftmenMonth.SelectedItem == null)
                            {
                                httpResponse = await AnalyzesRepo.GetCraftmenDetailsAsync(
                                    (string)comBoxCraftmenYear.SelectedItem, (string)comBoxCraftmenMonth.PlaceholderText);
                            }
                            else
                            {
                                httpResponse = await AnalyzesRepo.GetCraftmenDetailsAsync(
                                    (string)comBoxCraftmenYear.SelectedItem, (string)comBoxCraftmenMonth.SelectedItem);
                            }

                        }
                    }

                    else if (chartType == "Users")
                    {
                        if (comBoxUsersYear.SelectedItem == null)
                        {
                            httpResponse = await AnalyzesRepo.GetUsersDetailsAsync(
                                (string)comBoxUsersYear.PlaceholderText, (string)comBoxUsersMonth.SelectedItem);
                        }
                        else
                        {
                            if (comBoxUsersMonth.SelectedItem == null)
                            {
                                httpResponse = await AnalyzesRepo.GetUsersDetailsAsync(
                                    (string)comBoxUsersYear.SelectedItem, (string)comBoxUsersMonth.PlaceholderText);
                            }
                            else
                            {
                                httpResponse = await AnalyzesRepo.GetUsersDetailsAsync(
                                   (string)comBoxUsersYear.SelectedItem, (string)comBoxUsersMonth.SelectedItem);
                            }

                        }

                    }

                    else if (chartType == "Requests")
                    {
                        if (comBoxRequestsYear.SelectedItem == null)
                        {
                            httpResponse = await AnalyzesRepo.GetRequestsDetailsAsync(
                                (string)comBoxRequestsYear.PlaceholderText, (string)comBoxRequestsMonth.SelectedItem);
                        }
                        else
                        {
                            if (comBoxRequestsMonth.SelectedItem == null)
                            {
                                httpResponse = await AnalyzesRepo.GetRequestsDetailsAsync(
                                    (string)comBoxRequestsYear.SelectedItem, (string)comBoxRequestsMonth.PlaceholderText);
                            }
                            else
                            {
                                httpResponse = await AnalyzesRepo.GetRequestsDetailsAsync(
                                   (string)comBoxRequestsYear.SelectedItem, (string)comBoxRequestsMonth.SelectedItem);
                            }

                        }

                    }

                    else //Reports
                    {
                        if (comBoxReportsYear.SelectedItem == null)
                        {
                            httpResponse = await AnalyzesRepo.GetReportsDetailsAsync(
                                (string)comBoxReportsYear.PlaceholderText, (string)comBoxReportsMonth.SelectedItem);
                        }
                        else
                        {
                            if (comBoxReportsMonth.SelectedItem == null)
                            {
                                httpResponse = await AnalyzesRepo.GetReportsDetailsAsync(
                                    (string)comBoxReportsYear.SelectedItem, (string)comBoxReportsMonth.PlaceholderText);
                            }
                            else
                            {
                                httpResponse = await AnalyzesRepo.GetReportsDetailsAsync(
                                    (string)comBoxReportsYear.SelectedItem, (string)comBoxReportsMonth.SelectedItem);
                            }

                        }

                    }

                    if (httpResponse.Response.Result != null)
                    {
                        if (chartType == "Profits")
                        {
                            analyzesViewModel.ProfitsRadDetails =
                                (ProfitsRadDetails)httpResponse.Response.Result;
                        }
                        else if (chartType == "Craftmen")
                        {
                            analyzesViewModel.CraftmenRadDetails =
                                (CraftmenRadDetails)httpResponse.Response.Result;
                        }
                        else if (chartType == "Users")
                        {
                            analyzesViewModel.UsersRadDetails =
                                (UsersRadDetails)httpResponse.Response.Result;
                        }
                        else if (chartType == "Requests")
                        {
                            analyzesViewModel.RequestsRadDetails =
                                 (RequestsRadDetails)httpResponse.Response.Result;
                        }
                        else //Reports
                        {
                            analyzesViewModel.ReportsRadDetails =
                                 (ReportsRadDetails)httpResponse.Response.Result;
                        }
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

        private async Task ComboxSelectionYearChange(string chartType)
        {
            try
            {
                HttpReponse httpResponse;

                if (IsNetworkAvailable())
                {
                    if (chartType == "Profits")
                    {
                        httpResponse = await AnalyzesRepo.GetProfitsMonthsAsync(
                            (string)comBoxProfitsYear.SelectedItem);

                        if (httpResponse.Response.Result != null)
                        {
                            analyzesViewModel.ProfitsMonths =
                                httpResponse.Response.Result as ObservableCollection<string>;

                            httpResponse = await AnalyzesRepo.GetProfitsDetailsAsync(
                                (string)comBoxProfitsYear.SelectedItem, (string)comBoxProfitsMonth.Items[0]);

                            if (httpResponse.Response.Result != null)
                            {
                                analyzesViewModel.ProfitsRadDetails =
                                    httpResponse.Response.Result as ProfitsRadDetails;
                            }
                        }
                    }
                    else if (chartType == "Craftmen")
                    {
                        httpResponse = await AnalyzesRepo.GetCraftmenMonthsAsync(
                            (string)comBoxCraftmenYear.SelectedItem);

                        if (httpResponse.Response.Result != null)
                        {
                            analyzesViewModel.CraftmenMonths =
                                httpResponse.Response.Result as ObservableCollection<string>;

                            httpResponse = await AnalyzesRepo.GetCraftmenDetailsAsync(
                                (string)comBoxCraftmenYear.SelectedItem, (string)comBoxCraftmenMonth.Items[0]);

                            if (httpResponse.Response.Result != null)
                            {
                                analyzesViewModel.CraftmenRadDetails =
                                    httpResponse.Response.Result as CraftmenRadDetails;
                            }
                        }
                    }
                    else if (chartType == "Users")
                    {
                        httpResponse = await AnalyzesRepo.GetUsersMonthsAsync(
                             (string)comBoxUsersYear.SelectedItem);

                        if (httpResponse.Response.Result != null)
                        {
                            analyzesViewModel.UsersMonths =
                                httpResponse.Response.Result as ObservableCollection<string>;

                            httpResponse = await AnalyzesRepo.GetUsersDetailsAsync(
                                (string)comBoxUsersYear.SelectedItem, (string)comBoxUsersMonth.Items[0]);

                            if (httpResponse.Response.Result != null)
                            {
                                analyzesViewModel.UsersRadDetails =
                                    httpResponse.Response.Result as UsersRadDetails;
                            }
                        }
                    }
                    else if (chartType == "Requests")
                    {
                        httpResponse = await AnalyzesRepo.GetRequestsMonthsAsync(
                             (string)comBoxRequestsYear.SelectedItem);

                        if (httpResponse.Response.Result != null)
                        {
                            analyzesViewModel.RequestsMonths =
                                httpResponse.Response.Result as ObservableCollection<string>;

                            httpResponse = await AnalyzesRepo.GetRequestsDetailsAsync(
                                (string)comBoxRequestsYear.SelectedItem, (string)comBoxRequestsMonth.Items[0]);

                            if (httpResponse.Response.Result != null)
                            {
                                analyzesViewModel.RequestsRadDetails =
                                    httpResponse.Response.Result as RequestsRadDetails;
                            }
                        }
                    }
                    else //Reports
                    {
                        httpResponse = await AnalyzesRepo.GetReportsMonthsAsync(
                             (string)comBoxReportsYear.SelectedItem);

                        if (httpResponse.Response.Result != null)
                        {
                            analyzesViewModel.ReportsMonths =
                                httpResponse.Response.Result as ObservableCollection<string>;

                            httpResponse = await AnalyzesRepo.GetReportsDetailsAsync(
                                (string)comBoxReportsYear.SelectedItem, (string)comBoxReportsMonth.Items[0]);

                            if (httpResponse.Response.Result != null)
                            {
                                analyzesViewModel.ReportsRadDetails =
                                    httpResponse.Response.Result as ReportsRadDetails;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(httpResponse.ErrorMessage))
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


        //Selection Year
        private async void ComBoxProfitsYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ComboxSelectionYearChange("Profits");
        }
        private async void ComBoxCraftmenYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ComboxSelectionYearChange("Craftmen");
        }
        private async void ComBoxUsersYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ComboxSelectionYearChange("Users");
        }
        private async void ComBoxRequestsYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ComboxSelectionYearChange("Requests");
        }
        private async void ComBoxReportsYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ComboxSelectionYearChange("Reports");

        }


        //Selection Month
        private async void ComBoxProfitsMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ComboxSelectionMonthChange("Profits");
        }
        private async void ComBoxCraftmenMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ComboxSelectionMonthChange("Craftmen");
        }
        private async void ComBoxUsersMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ComboxSelectionMonthChange("Users");
        }
        private async void ComBoxRequestsMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ComboxSelectionMonthChange("Requests");
        }
        private async void ComBoxReportsMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ComboxSelectionMonthChange("Reports");
        }


        #endregion

        #region Networking change and page Loading event 

        private async Task LoadingChartsData()
        {
            try
            {
                var profitsHttpReponse =
                   await AnalyzesRepo.GetProfitsDetailsAsync(
                       analyzesViewModel.ProfitsYears[0], comBoxProfitsMonth.Items[0] as string);

                var craftmenHttpReponse =
                       await AnalyzesRepo.GetCraftmenDetailsAsync(
                           analyzesViewModel.CraftmenYears[0], comBoxCraftmenMonth.Items[0] as string);

                var requestsHttpReponse =
                       await AnalyzesRepo.GetRequestsDetailsAsync(
                           analyzesViewModel.RequestsYears[0], comBoxRequestsMonth.Items[0] as string);


                loadingData.IsLoading = false;

                if (profitsHttpReponse.Response.Result != null && craftmenHttpReponse.Response.Result != null &&
                    requestsHttpReponse.Response.Result != null)
                {
                    analyzesViewModel.ProfitsRadDetails = profitsHttpReponse.Response.Result as ProfitsRadDetails;
                    analyzesViewModel.CraftmenRadDetails = craftmenHttpReponse.Response.Result as CraftmenRadDetails;
                    analyzesViewModel.RequestsRadDetails = requestsHttpReponse.Response.Result as RequestsRadDetails;
                }
                else
                {
                    txtProfitsError.Text =
                        !string.IsNullOrEmpty(profitsHttpReponse.ErrorMessage) ? profitsHttpReponse.ErrorMessage : "";

                    txtCraftmenError.Text =
                        !string.IsNullOrEmpty(craftmenHttpReponse.ErrorMessage) ? craftmenHttpReponse.ErrorMessage : "";

                    txtRequestsError.Text =
                        !string.IsNullOrEmpty(requestsHttpReponse.ErrorMessage) ? requestsHttpReponse.ErrorMessage : "";

                    errorDetailsCardView.IsOpen = true;
                }
            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
        }

        private async Task RefreshDashboardContent()
        {
            try
            {
                if (analyzesViewModel.ProfitsYears == null && analyzesViewModel.CraftmenYears == null && analyzesViewModel.RequestsYears == null &&
               analyzesViewModel.ProfitsMonths == null && analyzesViewModel.CraftmenMonths == null && analyzesViewModel.RequestsMonths == null)
                {
                    if (IsNetworkAvailable())
                    {
                        if (NoWifiAnalyzes.Visibility == Visibility.Visible)
                        {
                            NoWifiAnalyzes.Visibility = Visibility.Collapsed;

                            fixedToastNotificationMessage.Text =
                                TranslatingUI.UsersResourceMap.GetValue("txtToastConnWifi", TranslatingUI.ResourceContextObj).ValueAsString;
                            fixedToastNotification.Show(6000);
                        }

                        loadingData.IsLoading = true;

                        var profitsYearsHttpReponse = await AnalyzesRepo.GetProfitsYearsAsync();
                        var craftmenYearsHttpReponse = await AnalyzesRepo.GetCraftmenYearsAsync();
                        var requestsYearsHttpReponse = await AnalyzesRepo.GetRequestsYearsAsync();


                        if (profitsYearsHttpReponse.Response.Result != null && craftmenYearsHttpReponse.Response.Result != null &&
                            requestsYearsHttpReponse.Response.Result != null)
                        {
                            analyzesViewModel.ProfitsYears =
                                (ObservableCollection<string>)profitsYearsHttpReponse.Response.Result;

                            analyzesViewModel.CraftmenYears =
                               (ObservableCollection<string>)craftmenYearsHttpReponse.Response.Result;

                            analyzesViewModel.RequestsYears =
                               (ObservableCollection<string>)requestsYearsHttpReponse.Response.Result;



                            var profitsMonthsHttpReponse =
                                    await AnalyzesRepo.GetProfitsMonthsAsync(analyzesViewModel.ProfitsYears[0]);

                            var craftmenMonthsHttpReponse =
                                    await AnalyzesRepo.GetCraftmenMonthsAsync(analyzesViewModel.CraftmenYears[0]);

                            var requestsMonthsHttpReponse =
                                    await AnalyzesRepo.GetRequestsMonthsAsync(analyzesViewModel.RequestsYears[0]);


                            if (profitsMonthsHttpReponse.Response.Result != null && craftmenMonthsHttpReponse.Response.Result != null &&
                                requestsMonthsHttpReponse.Response.Result != null)
                            {
                                analyzesViewModel.ProfitsMonths =
                                    (ObservableCollection<string>)profitsMonthsHttpReponse.Response.Result;

                                analyzesViewModel.CraftmenMonths =
                                    (ObservableCollection<string>)craftmenMonthsHttpReponse.Response.Result;

                                analyzesViewModel.RequestsMonths =
                                    (ObservableCollection<string>)requestsMonthsHttpReponse.Response.Result;

                                await LoadingChartsData();
                            }
                            else
                            {
                                txtProfitsError.Text =
                                    !string.IsNullOrEmpty(profitsMonthsHttpReponse.ErrorMessage) ? profitsMonthsHttpReponse.ErrorMessage : "";
                                txtCraftmenError.Text =
                                   !string.IsNullOrEmpty(craftmenMonthsHttpReponse.ErrorMessage) ? craftmenMonthsHttpReponse.ErrorMessage : "";
                                txtRequestsError.Text =
                                   !string.IsNullOrEmpty(requestsMonthsHttpReponse.ErrorMessage) ? requestsMonthsHttpReponse.ErrorMessage : "";

                                errorDetailsCardView.IsOpen = true;
                            }
                        }
                        else
                        {
                            txtProfitsError.Text =
                                   !string.IsNullOrEmpty(profitsYearsHttpReponse.ErrorMessage) ? profitsYearsHttpReponse.ErrorMessage : "";
                            txtCraftmenError.Text =
                               !string.IsNullOrEmpty(craftmenYearsHttpReponse.ErrorMessage) ? craftmenYearsHttpReponse.ErrorMessage : "";
                            txtRequestsError.Text =
                               !string.IsNullOrEmpty(requestsYearsHttpReponse.ErrorMessage) ? requestsYearsHttpReponse.ErrorMessage : "";

                            errorDetailsCardView.IsOpen = true;
                        }

                    }
                    else
                    {
                        NoWifiAnalyzes.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                errorToastNotificationMessage.Text = ex.Message;
                errorToastNotification.Show(6000);
            }
        }    

        private async void AnalyzesView_Loading(FrameworkElement sender, object args)
        {
            //Translating this view
            await ActivateRefreshUIText();


            await RefreshDashboardContent();
        }
       
        private async void NetworkChanged(object sender, EventArgs e)
        {
            await RefreshDashboardContent();
        }

        private async void BtnNoWifiAnalyzes_Click(object sender, RoutedEventArgs e)
        {
            await RefreshDashboardContent();
        }

        #endregion

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
           txtProfitsTitle.Text =
                TranslatingUI.AnalyzesResourceMap.GetValue("txtProfitsTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            profitsYear.Text =
                TranslatingUI.AnalyzesResourceMap.GetValue("txtYear", TranslatingUI.ResourceContextObj).ValueAsString;
            profitsMonth.Text =
                TranslatingUI.AnalyzesResourceMap.GetValue("txtMonth", TranslatingUI.ResourceContextObj).ValueAsString;
            txtProfitsCalcTitle.Text =
                TranslatingUI.AnalyzesResourceMap.GetValue("txtProfitsCalcTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            txtProfitsCalcPerYear.Text =
                TranslatingUI.AnalyzesResourceMap.GetValue("txtPerYear", TranslatingUI.ResourceContextObj).ValueAsString;
            txtProfitsCalcPerMonth.Text =
                TranslatingUI.AnalyzesResourceMap.GetValue("txtPerMonth", TranslatingUI.ResourceContextObj).ValueAsString;
            txtProfitsCalcPerDay.Text =
                TranslatingUI.AnalyzesResourceMap.GetValue("txtPerDay", TranslatingUI.ResourceContextObj).ValueAsString;
            txtProfitsCalcPerHour.Text =
                TranslatingUI.AnalyzesResourceMap.GetValue("txtPerHour", TranslatingUI.ResourceContextObj).ValueAsString;
            profitsMark.Text =
                TranslatingUI.AnalyzesResourceMap.GetValue("profitsMark", TranslatingUI.ResourceContextObj).ValueAsString;


            txtUsersCraftmenTitle.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtUsersCraftmenTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            txtCraftmenHeader.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtCraftmenHeader", TranslatingUI.ResourceContextObj).ValueAsString;
            craftmenYear.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtYear", TranslatingUI.ResourceContextObj).ValueAsString;
            craftmenMonth.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtMonth", TranslatingUI.ResourceContextObj).ValueAsString;
            txtCraftmenCalcTitle.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtCraftmenCalcTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            txtCraftmenCalcPerYear.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtPerYear", TranslatingUI.ResourceContextObj).ValueAsString;
            txtCraftmenCalcPerMonth.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtPerMonth", TranslatingUI.ResourceContextObj).ValueAsString;
            txtCraftmenCalcPerDay.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtPerDay", TranslatingUI.ResourceContextObj).ValueAsString;
            txtCraftmenCalcPerHour.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtPerHour", TranslatingUI.ResourceContextObj).ValueAsString;
            craftmenMark.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("craftmenMark", TranslatingUI.ResourceContextObj).ValueAsString;


            txtUsersHeader.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtUsersHeader", TranslatingUI.ResourceContextObj).ValueAsString;
            usersYear.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtYear", TranslatingUI.ResourceContextObj).ValueAsString;
            usersMonth.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtMonth", TranslatingUI.ResourceContextObj).ValueAsString;
            txtUsersCalcTitle.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtUsersCalcTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            txtUsersCalcPerYear.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtPerYear", TranslatingUI.ResourceContextObj).ValueAsString;
            txtUsersCalcPerMonth.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtPerMonth", TranslatingUI.ResourceContextObj).ValueAsString;
            txtUsersCalcPerDay.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtPerDay", TranslatingUI.ResourceContextObj).ValueAsString;
            txtUsersCalcPerHour.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtPerHour", TranslatingUI.ResourceContextObj).ValueAsString;
            usersMark.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("usersMark", TranslatingUI.ResourceContextObj).ValueAsString;


            txtRequestsReportsTitle.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtRequestsReportsTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            txtRequestsHeader.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtRequestsHeader", TranslatingUI.ResourceContextObj).ValueAsString;
            requestsYear.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtYear", TranslatingUI.ResourceContextObj).ValueAsString;
            requestsMonth.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtMonth", TranslatingUI.ResourceContextObj).ValueAsString;
            txtRequestsCalcTitle.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtRequestsCalcTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            txtRequestsCalcPerYear.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtPerYear", TranslatingUI.ResourceContextObj).ValueAsString;
            txtRequestsCalcPerMonth.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtPerMonth", TranslatingUI.ResourceContextObj).ValueAsString;
            txtRequestsCalcPerDay.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtPerDay", TranslatingUI.ResourceContextObj).ValueAsString;
            txtRequestsCalcPerHour.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtPerHour", TranslatingUI.ResourceContextObj).ValueAsString;
            requestsMark.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("requestsMark", TranslatingUI.ResourceContextObj).ValueAsString;

            
            txtReportsHeader.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtReportsHeader", TranslatingUI.ResourceContextObj).ValueAsString;
            reportsYear.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtYear", TranslatingUI.ResourceContextObj).ValueAsString;
            reportsMonth.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtMonth", TranslatingUI.ResourceContextObj).ValueAsString;
            txtReportsCalcTitle.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtReportsCalcTitle", TranslatingUI.ResourceContextObj).ValueAsString;
            txtReportsCalcPerYear.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtPerYear", TranslatingUI.ResourceContextObj).ValueAsString;
            txtReportsCalcPerMonth.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtPerMonth", TranslatingUI.ResourceContextObj).ValueAsString;
            txtReportsCalcPerDay.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtPerDay", TranslatingUI.ResourceContextObj).ValueAsString;
            txtReportsCalcPerHour.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("txtPerHour", TranslatingUI.ResourceContextObj).ValueAsString;
            reportsMark.Text =
                            TranslatingUI.AnalyzesResourceMap.GetValue("reportsMark", TranslatingUI.ResourceContextObj).ValueAsString;

            errorDetailsCardView.Header =
                 TranslatingUI.AnalyzesResourceMap.GetValue("errorDetailsCardView", TranslatingUI.ResourceContextObj).ValueAsString;
            txtCaViErrCancel.Text =
                 TranslatingUI.AnalyzesResourceMap.GetValue("txtCaViErrCancel", TranslatingUI.ResourceContextObj).ValueAsString;

        }

        #endregion
    }
}
