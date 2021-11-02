using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Herafi.Core.Models;
using System.Collections.ObjectModel;

namespace Herafi.Core.ViewModels
{
    public class AnalyzesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<string> _profitsYears;
        private ObservableCollection<string> _profitsMonths;
        private ProfitsRadDetails _profitsRadDetails;

        private ObservableCollection<string> _craftmenYears;
        private ObservableCollection<string> _craftmenMonths;
        private CraftmenRadDetails _craftmenRadDetails;

        private ObservableCollection<string> _usersYears;
        private ObservableCollection<string> _usersMonths;
        private UsersRadDetails _usersRadDetails;

        private ObservableCollection<string> _requestsYears;
        private ObservableCollection<string> _requestsMonths;
        private RequestsRadDetails _requestsRadDetails;

        private ObservableCollection<string> _reportsYears;
        private ObservableCollection<string> _reportsMonths;
        private ReportsRadDetails _reportsRadDetails;





        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AnalyzesViewModel()
        {
            //SampleData();
        }

        private void SampleData()
        {
            ProfitsYears = new ObservableCollection<string>
            {
                "2021",
                "2022"
            };

            ProfitsMonths = new ObservableCollection<string>
            {
                "1",
                "2",
                "5",
                "6",
                "8",
                "12"
            };

            ProfitsRadDetails = new ProfitsRadDetails
            {
                PerYear = "10023500",
                PerMonth = "100230",
                PerDay = "14000",
                PerHour = "2500",
                Total = "189235000",
                ProfitsRad = new ObservableCollection<ProfitRad>
                {
                    new ProfitRad
                    {
                        Day = "0",
                        Paids = "0"
                    },
                    new ProfitRad
                    {
                        Day = "1",
                        Paids = "100"
                    },
                    new ProfitRad
                    {
                        Day = "2",
                        Paids = "200"
                    },
                    new ProfitRad
                    {
                        Day = "3",
                        Paids = "300"
                    },
                    new ProfitRad
                    {
                        Day = "4",
                        Paids = "500"
                    },
                    new ProfitRad
                    {
                        Day = "5",
                        Paids = "100"
                    },
                    new ProfitRad
                    {
                        Day = "6",
                        Paids = "800"
                    },
                    new ProfitRad
                    {
                        Day = "7",
                        Paids = "400"
                    },
                    new ProfitRad
                    {
                        Day = "8",
                        Paids = "300"
                    },
                    new ProfitRad
                    {
                        Day = "9",
                        Paids = "100"
                    },
                    new ProfitRad
                    {
                        Day = "10",
                        Paids = "500"
                    },
                    new ProfitRad
                    {
                        Day = "11",
                        Paids = "600"
                    },
                    new ProfitRad
                    {
                        Day = "12",
                        Paids = "1000"
                    },
                    new ProfitRad
                    {
                        Day = "13",
                        Paids = "400"
                    },
                    new ProfitRad
                    {
                        Day = "14",
                        Paids = "400"
                    },
                    new ProfitRad
                    {
                        Day = "15",
                        Paids = "400"
                    },
                    new ProfitRad
                    {
                        Day = "16",
                        Paids = "200"
                    },
                    new ProfitRad
                    {
                        Day = "17",
                        Paids = "700"
                    },
                    new ProfitRad
                    {
                        Day = "18",
                        Paids = "1000"
                    },
                    new ProfitRad
                    {
                        Day = "19",
                        Paids = "500"
                    },
                    new ProfitRad
                    {
                        Day = "20",
                        Paids = "500"
                    },
                    new ProfitRad
                    {
                        Day = "21",
                        Paids = "400"
                    },
                    new ProfitRad
                    {
                        Day = "22",
                        Paids = "500"
                    },
                    new ProfitRad
                    {
                        Day = "23",
                        Paids = "400"
                    },
                    new ProfitRad
                    {
                        Day = "24",
                        Paids = "500"
                    },
                    new ProfitRad
                    {
                        Day = "25",
                        Paids = "800"
                    },
                    new ProfitRad
                    {
                        Day = "27",
                        Paids = "600"
                    },
                    new ProfitRad
                    {
                        Day = "28",
                        Paids = "700"
                    },
                    new ProfitRad
                    {
                        Day = "29",
                        Paids = "800"
                    },
                    new ProfitRad
                    {
                        Day = "30",
                        Paids = "1000"
                    },
                }
            };



            CraftmenYears = new ObservableCollection<string>
            {
                "2021",
                "2022"
            };

            CraftmenMonths = new ObservableCollection<string>
            {
                "1",
                "2",
                "5",
                "6",
                "8",
                "12"
            };

            CraftmenRadDetails = new CraftmenRadDetails
            {
                PerYear = "10023500",
                PerMonth = "100230",
                PerDay = "14000",
                PerHour = "2500",
                Total = "189235000",
                CraftmenRad = new ObservableCollection<CraftmanRad>
                {
                    new CraftmanRad
                    {
                        Day = "0",
                        CraftmenNumber = "0"
                    },
                    new CraftmanRad
                    {
                        Day = "1",
                        CraftmenNumber = "100"
                    },
                    new CraftmanRad
                    {
                        Day = "2",
                        CraftmenNumber = "200"
                    },
                    new CraftmanRad
                    {
                        Day = "3",
                        CraftmenNumber = "300"
                    },
                    new CraftmanRad
                    {
                        Day = "4",
                        CraftmenNumber = "500"
                    },
                    new CraftmanRad
                    {
                        Day = "5",
                        CraftmenNumber = "100"
                    },
                    new CraftmanRad
                    {
                        Day = "6",
                        CraftmenNumber = "800"
                    },
                    new CraftmanRad
                    {
                        Day = "7",
                        CraftmenNumber = "400"
                    },
                    new CraftmanRad
                    {
                        Day = "8",
                        CraftmenNumber = "300"
                    },
                    new CraftmanRad
                    {
                        Day = "9",
                        CraftmenNumber = "100"
                    },
                    new CraftmanRad
                    {
                        Day = "10",
                        CraftmenNumber = "500"
                    },
                    new CraftmanRad
                    {
                        Day = "11",
                        CraftmenNumber = "600"
                    },
                    new CraftmanRad
                    {
                        Day = "12",
                        CraftmenNumber = "1000"
                    },
                    new CraftmanRad
                    {
                        Day = "13",
                        CraftmenNumber = "400"
                    },
                    new CraftmanRad
                    {
                        Day = "14",
                        CraftmenNumber = "400"
                    },
                    new CraftmanRad
                    {
                        Day = "15",
                        CraftmenNumber = "400"
                    },
                    new CraftmanRad
                    {
                        CraftmenNumber = "200"
                    },
                    new CraftmanRad
                    {
                        Day = "17",
                        CraftmenNumber = "700"
                    },
                    new CraftmanRad
                    {
                        Day = "18",
                        CraftmenNumber = "1000"
                    },
                    new CraftmanRad
                    {
                        Day = "19",
                        CraftmenNumber = "500"
                    },
                    new CraftmanRad
                    {
                        Day = "20",
                        CraftmenNumber = "500"
                    },
                    new CraftmanRad
                    {
                        Day = "21",
                        CraftmenNumber = "400"
                    },
                    new CraftmanRad
                    {
                        Day = "22",
                        CraftmenNumber = "500"
                    },
                    new CraftmanRad
                    {
                        Day = "23",
                        CraftmenNumber = "400"
                    },
                    new CraftmanRad
                    {
                        Day = "24",
                        CraftmenNumber = "500"
                    },
                    new CraftmanRad
                    {
                        Day = "25",
                        CraftmenNumber = "800"
                    },
                    new CraftmanRad
                    {
                        Day = "27",
                        CraftmenNumber = "600"
                    },
                    new CraftmanRad
                    {
                        Day = "28",
                        CraftmenNumber = "700"
                    },
                    new CraftmanRad
                    {
                        Day = "29",
                        CraftmenNumber = "800"
                    },
                    new CraftmanRad
                    {
                        Day = "30",
                        CraftmenNumber = "1000"
                    },
                }
            };



            UsersYears = new ObservableCollection<string>
            {
                "2021",
                "2022"
            };

            UsersMonths = new ObservableCollection<string>
            {
                "1",
                "2",
                "5",
                "6",
                "8",
                "12"
            };

            UsersRadDetails = new UsersRadDetails
            {
                PerYear = "10023500",
                PerMonth = "100230",
                PerDay = "14000",
                PerHour = "2500",
                Total = "189235000",
                UsersRad = new ObservableCollection<UserRad>
                {
                    new UserRad
                    {
                        Day = "0",
                        UsersNumber = "0"
                    },
                    new UserRad
                    {
                        Day = "1",
                        UsersNumber = "100"
                    },
                    new UserRad
                    {
                        Day = "2",
                        UsersNumber = "200"
                    },
                    new UserRad
                    {
                        Day = "3",
                        UsersNumber = "300"
                    },
                    new UserRad
                    {
                        Day = "4",
                        UsersNumber = "500"
                    },
                    new UserRad
                    {
                        Day = "5",
                        UsersNumber = "100"
                    },
                    new UserRad
                    {
                        Day = "6",
                        UsersNumber = "800"
                    },
                    new UserRad
                    {
                        Day = "7",
                        UsersNumber = "400"
                    },
                    new UserRad
                    {
                        Day = "8",
                        UsersNumber = "300"
                    },
                    new UserRad
                    {
                        Day = "9",
                        UsersNumber = "100"
                    },
                    new UserRad
                    {
                        Day = "10",
                        UsersNumber = "500"
                    },
                    new UserRad
                    {
                        Day = "11",
                        UsersNumber = "600"
                    },
                    new UserRad
                    {
                        Day = "12",
                        UsersNumber = "1000"
                    },
                    new UserRad
                    {
                        Day = "13",
                        UsersNumber = "400"
                    },
                    new UserRad
                    {
                        Day = "14",
                        UsersNumber = "400"
                    },
                    new UserRad
                    {
                        Day = "15",
                        UsersNumber = "400"
                    },
                    new UserRad
                    {
                        Day = "16",
                        UsersNumber = "200"
                    },
                    new UserRad
                    {
                        Day = "17",
                        UsersNumber = "700"
                    },
                    new UserRad
                    {
                        Day = "18",
                        UsersNumber = "1000"
                    },
                    new UserRad
                    {
                        Day = "19",
                        UsersNumber = "500"
                    },
                    new UserRad
                    {
                        Day = "20",
                        UsersNumber = "500"
                    },
                    new UserRad
                    {
                        Day = "21",
                        UsersNumber = "400"
                    },
                    new UserRad
                    {
                        Day = "22",
                        UsersNumber = "500"
                    },
                    new UserRad
                    {
                        Day = "23",
                        UsersNumber = "400"
                    },
                    new UserRad
                    {
                        Day = "24",
                        UsersNumber = "500"
                    },
                    new UserRad
                    {
                        Day = "25",
                        UsersNumber = "800"
                    },
                    new UserRad
                    {
                        Day = "27",
                        UsersNumber = "600"
                    },
                    new UserRad
                    {
                        Day = "28",
                        UsersNumber = "700"
                    },
                    new UserRad
                    {
                        Day = "29",
                        UsersNumber = "800"
                    },
                    new UserRad
                    {
                        Day = "30",
                        UsersNumber = "1000"
                    },
                }
            };



            RequestsYears = new ObservableCollection<string>
            {
                "2021",
                "2022"
            };

            RequestsMonths = new ObservableCollection<string>
            {
                "1",
                "2",
                "5",
                "6",
                "8",
                "12"
            };

            RequestsRadDetails = new RequestsRadDetails
            {
                PerYear = "10023500",
                PerMonth = "100230",
                PerDay = "14000",
                PerHour = "2500",
                Total = "189235000",
                RequestsRad = new ObservableCollection<RequestRad>
                {
                    new RequestRad
                    {
                        Day = "0",
                        RequestsNumber = "0"
                    },
                    new RequestRad
                    {
                        Day = "1",
                        RequestsNumber = "100"
                    },
                    new RequestRad
                    {
                        Day = "2",
                        RequestsNumber = "200"
                    },
                    new RequestRad
                    {
                        Day = "3",
                        RequestsNumber = "300"
                    },
                    new RequestRad
                    {
                        Day = "4",
                        RequestsNumber = "500"
                    },
                    new RequestRad
                    {
                        Day = "5",
                        RequestsNumber = "100"
                    },
                    new RequestRad
                    {
                        Day = "6",
                        RequestsNumber = "800"
                    },
                    new RequestRad
                    {
                        Day = "7",
                        RequestsNumber = "400"
                    },
                    new RequestRad
                    {
                        Day = "8",
                        RequestsNumber = "300"
                    },
                    new RequestRad
                    {
                        Day = "9",
                        RequestsNumber = "100"
                    },
                    new RequestRad
                    {
                        Day = "10",
                        RequestsNumber = "500"
                    },
                    new RequestRad
                    {
                        Day = "11",
                        RequestsNumber = "600"
                    },
                    new RequestRad
                    {
                        Day = "12",
                        RequestsNumber = "1000"
                    },
                    new RequestRad
                    {
                        Day = "13",
                        RequestsNumber = "400"
                    },
                    new RequestRad
                    {
                        Day = "14",
                        RequestsNumber = "400"
                    },
                    new RequestRad
                    {
                        Day = "15",
                        RequestsNumber = "400"
                    },
                    new RequestRad
                    {
                        Day = "16",
                        RequestsNumber = "200"
                    },
                    new RequestRad
                    {
                        Day = "17",
                        RequestsNumber = "700"
                    },
                    new RequestRad
                    {
                        Day = "18",
                        RequestsNumber = "1000"
                    },
                    new RequestRad
                    {
                        Day = "19",
                        RequestsNumber = "500"
                    },
                    new RequestRad
                    {
                        Day = "20",
                        RequestsNumber = "500"
                    },
                    new RequestRad
                    {
                        Day = "21",
                        RequestsNumber = "400"
                    },
                    new RequestRad
                    {
                        Day = "22",
                        RequestsNumber = "500"
                    },
                    new RequestRad
                    {
                        Day = "23",
                        RequestsNumber = "400"
                    },
                    new RequestRad
                    {
                        Day = "24",
                        RequestsNumber = "500"
                    },
                    new RequestRad
                    {
                        Day = "25",
                        RequestsNumber = "800"
                    },
                    new RequestRad
                    {
                        Day = "27",
                        RequestsNumber = "600"
                    },
                    new RequestRad
                    {
                        Day = "28",
                        RequestsNumber = "700"
                    },
                    new RequestRad
                    {
                        Day = "29",
                        RequestsNumber = "800"
                    },
                    new RequestRad
                    {
                        Day = "30",
                        RequestsNumber = "1000"
                    },
                }
            };



            ReportsYears = new ObservableCollection<string>
            {
                "2021",
                "2022"
            };

            ReportsMonths = new ObservableCollection<string>
            {
                "1",
                "2",
                "5",
                "6",
                "8",
                "12"
            };

            ReportsRadDetails = new ReportsRadDetails
            {
                PerYear = "10023500",
                PerMonth = "100230",
                PerDay = "14000",
                PerHour = "2500",
                Total = "189235000",
                ReportsRad = new ObservableCollection<ReportRad>
                {
                    new ReportRad
                    {
                        Day = "0",
                        ReportsNumber = "0"
                    },
                    new ReportRad
                    {
                        Day = "1",
                        ReportsNumber = "100"
                    },
                    new ReportRad
                    {
                        Day = "2",
                        ReportsNumber = "200"
                    },
                    new ReportRad
                    {
                        Day = "3",
                        ReportsNumber = "300"
                    },
                    new ReportRad
                    {
                        Day = "4",
                        ReportsNumber = "500"
                    },
                    new ReportRad
                    {
                        Day = "5",
                        ReportsNumber = "100"
                    },
                    new ReportRad
                    {
                        Day = "6",
                        ReportsNumber = "800"
                    },
                    new ReportRad
                    {
                        Day = "7",
                        ReportsNumber = "400"
                    },
                    new ReportRad
                    {
                        Day = "8",
                        ReportsNumber = "300"
                    },
                    new ReportRad
                    {
                        Day = "9",
                        ReportsNumber = "100"
                    },
                    new ReportRad
                    {
                        Day = "10",
                        ReportsNumber = "500"
                    },
                    new ReportRad
                    {
                        Day = "11",
                        ReportsNumber = "600"
                    },
                    new ReportRad
                    {
                        Day = "12",
                        ReportsNumber = "1000"
                    },
                    new ReportRad
                    {
                        Day = "13",
                        ReportsNumber = "400"
                    },
                    new ReportRad
                    {
                        Day = "14",
                        ReportsNumber = "400"
                    },
                    new ReportRad
                    {
                        Day = "15",
                        ReportsNumber = "400"
                    },
                    new ReportRad
                    {
                        Day = "16",
                        ReportsNumber = "200"
                    },
                    new ReportRad
                    {
                        Day = "17",
                        ReportsNumber = "700"
                    },
                    new ReportRad
                    {
                        Day = "18",
                        ReportsNumber = "1000"
                    },
                    new ReportRad
                    {
                        Day = "19",
                        ReportsNumber = "500"
                    },
                    new ReportRad
                    {
                        Day = "20",
                        ReportsNumber = "500"
                    },
                    new ReportRad
                    {
                        Day = "21",
                        ReportsNumber = "400"
                    },
                    new ReportRad
                    {
                        Day = "22",
                        ReportsNumber = "500"
                    },
                    new ReportRad
                    {
                        Day = "23",
                        ReportsNumber = "400"
                    },
                    new ReportRad
                    {
                        Day = "24",
                        ReportsNumber = "500"
                    },
                    new ReportRad
                    {
                        Day = "25",
                        ReportsNumber = "800"
                    },
                    new ReportRad
                    {
                        Day = "27",
                        ReportsNumber = "600"
                    },
                    new ReportRad
                    {
                        Day = "28",
                        ReportsNumber = "700"
                    },
                    new ReportRad
                    {
                        Day = "29",
                        ReportsNumber = "800"
                    },
                    new ReportRad
                    {
                        Day = "30",
                        ReportsNumber = "1000"
                    },
                }
            };
        }



        //Profits section
        public ObservableCollection<string> ProfitsYears
        {
            get { return _profitsYears; }
            set { _profitsYears = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> ProfitsMonths
        {
            get { return _profitsMonths; }
            set { _profitsMonths = value; OnPropertyChanged(); }
        }

        public ProfitsRadDetails ProfitsRadDetails
        {
            get { return _profitsRadDetails; }
            set { _profitsRadDetails = value; OnPropertyChanged(); }
        }


        //Users section
        public ObservableCollection<string> CraftmenYears
        {
            get { return _craftmenYears; }
            set { _craftmenYears = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> CraftmenMonths
        {
            get { return _craftmenMonths; }
            set { _craftmenMonths = value; OnPropertyChanged(); }
        }

        public CraftmenRadDetails CraftmenRadDetails
        {
            get { return _craftmenRadDetails; }
            set { _craftmenRadDetails = value; OnPropertyChanged(); }
        }


        //Users section
        public ObservableCollection<string> UsersYears
        {
            get { return _usersYears; }
            set { _usersYears = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> UsersMonths
        {
            get { return _usersMonths; }
            set { _usersMonths = value; OnPropertyChanged(); }
        }

        public UsersRadDetails UsersRadDetails
        {
            get { return _usersRadDetails; }
            set { _usersRadDetails = value; OnPropertyChanged(); }
        }


        //Requests section
        public ObservableCollection<string> RequestsYears
        {
            get { return _requestsYears; }
            set { _requestsYears = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> RequestsMonths
        {
            get { return _requestsMonths; }
            set { _requestsMonths = value; OnPropertyChanged(); }
        }

        public RequestsRadDetails RequestsRadDetails
        {
            get { return _requestsRadDetails; }
            set { _requestsRadDetails = value; OnPropertyChanged(); }
        }


        //Reports section
        public ObservableCollection<string> ReportsYears
        {
            get { return _reportsYears; }
            set { _reportsYears = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> ReportsMonths
        {
            get { return _reportsMonths; }
            set { _reportsMonths = value; OnPropertyChanged(); }
        }

        public ReportsRadDetails ReportsRadDetails
        {
            get { return _reportsRadDetails; }
            set { _reportsRadDetails = value; OnPropertyChanged(); }
        }

    }
}
