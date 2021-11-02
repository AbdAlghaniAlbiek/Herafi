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
    public class DashboardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ProfitDetails _profitDetails;
        private NewMembers _newMembers;



        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public DashboardViewModel()
        {
            //SampleData();
        }
        private void SampleData()
        {
            NewMembers = new NewMembers
            {
                NewMembersCraftmenNumber = "500",
                NewMembersUsersNumber = "500"
            };

            ProfitDetails = new ProfitDetails
            {
                PerDay = "4000",
                PerHour = "100",
                ProfitsPrecentage = "20",
                Total = "100000",
                Profits = new ObservableCollection<Profit>
                {
                     new Profit
                    {
                        Day = "0",
                        Paids = "0"
                    },
                    new Profit
                    {
                        Day = "1",
                        Paids = "100"
                    },
                    new Profit
                    {
                        Day = "2",
                        Paids = "200"
                    },
                    new Profit
                    {
                        Day = "3",
                        Paids =  "300"
                    },
                    new Profit
                    {
                        Day = "4",
                        Paids =  "500"
                    },
                    new Profit
                    {
                        Day = "5",
                        Paids =  "100"
                    },
                    new Profit
                    {
                        Day = "6",
                        Paids =  "800"
                    },
                    new Profit
                    {
                        Day = "7",
                        Paids = "400"
                    },
                    new Profit
                    {
                        Day = "8",
                        Paids = "300"
                    },
                    new Profit
                    {
                        Day = "9",
                        Paids = "100"
                    },
                    new Profit
                    {
                        Day = "10",
                        Paids = "500"
                    },
                    new Profit
                    {
                        Day = "11",
                        Paids = "600"
                    },
                    new Profit
                    {
                        Day = "12",
                        Paids = "1000"
                    },
                    new Profit
                    {
                        Day = "13",
                        Paids = "400"
                    },
                    new Profit
                    {
                        Day = "14",
                        Paids = "400"
                    },
                    new Profit
                    {
                        Day = "15",
                        Paids = "400"
                    },
                    new Profit
                    {
                        Day = "16",
                        Paids = "200"
                    },
                    new Profit
                    {
                        Day = "17",
                        Paids = "700"
                    },
                    new Profit
                    {
                        Day = "18",
                        Paids = "1000"
                    },
                    new Profit
                    {
                        Day = "19",
                        Paids = "500"
                    },
                    new Profit
                    {
                        Day = "20",
                        Paids = "500"
                    },
                    new Profit
                    {
                        Day = "21",
                        Paids = "400"
                    },
                    new Profit
                    {
                        Day = "22",
                        Paids = "500"
                    },
                    new Profit
                    {
                        Day = "23",
                        Paids = "400"
                    },
                    new Profit
                    {
                        Day = "24",
                        Paids = "500"
                    },
                    new Profit
                    {
                        Day = "25",
                        Paids = "800"
                    },
                    new Profit
                    {
                        Day = "27",
                        Paids = "600"
                    },
                    new Profit
                    {
                        Day = "28",
                        Paids = "700"
                    },
                    new Profit
                    {
                        Day = "29",
                        Paids = "800"
                    },
                    new Profit
                    {
                        Day = "30",
                        Paids = "1000"
                    },
                }
            };
        }



        public NewMembers NewMembers
        {
            get { return _newMembers; }
            set { _newMembers = value; OnPropertyChanged(); }
        }

        public ProfitDetails ProfitDetails
        {
            get { return _profitDetails; }
            set { _profitDetails = value; OnPropertyChanged(); }
        }
    }
}
