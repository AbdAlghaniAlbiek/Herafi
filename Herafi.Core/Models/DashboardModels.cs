using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Herafi.Core.Models
{
    class DashboardModels
    {
    }

    public class ProfitDetails : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Profit> _profits; 
        private string _profitsPercentage;
        private string _perHour;
        private string _perDay;
        private string _total;


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [JsonProperty(PropertyName = "per_day")]
        public string PerDay
        {
            get { return _perDay; }
            set { _perDay = value; OnPropertyChanged(); }
        }

        [JsonProperty(PropertyName = "per_hour")]
        public string PerHour
        {
            get { return _perHour; }
            set { _perHour = value; OnPropertyChanged(); }
        }

        [JsonProperty(PropertyName = "total")]
        public string Total
        {
            get { return _total; }
            set { _total = value; OnPropertyChanged(); }
        }

        [JsonProperty(PropertyName = "profits_precentage")]
        public string ProfitsPrecentage
        {
            get { return _profitsPercentage; }
            set { _profitsPercentage = value; OnPropertyChanged(); }
        }

        [JsonProperty(PropertyName = "profits")]
        public ObservableCollection<Profit> Profits
        {
            get { return _profits; }
            set { _profits = value; OnPropertyChanged(); }
        }
    }

    public class Profit : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _day;
        private string _paids;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [JsonProperty(PropertyName = "day")]
        public string Day
        {
            get { return _day; }
            set { _day = value; OnPropertyChanged(); }
        }

        [JsonProperty(PropertyName = "paids")]
        public string Paids
        {
            get { return _paids; }
            set { _paids = value; OnPropertyChanged(); }
        }
    }

    public class ModifiedProfit : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _day;
        private double _paids;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [JsonProperty(PropertyName = "day")]
        public string Day
        {
            get { return _day; }
            set { _day = value; OnPropertyChanged(); }
        }

        [JsonProperty(PropertyName = "paids")]
        public double Paids
        {
            get { return _paids; }
            set { _paids = value; OnPropertyChanged(); }
        }
    }

    public class NewMembers : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _newMembersUsersNumber;
        private string _newMembersCraftmenNumber;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        [JsonProperty(PropertyName = "new_members_users_num")]
        public string NewMembersUsersNumber
        {
            get { return _newMembersUsersNumber; }
            set { _newMembersUsersNumber = value; OnPropertyChanged(); }
        }

        [JsonProperty(PropertyName = "new_members_craftmen_num")]
        public string NewMembersCraftmenNumber
        {
            get { return _newMembersCraftmenNumber; }
            set { _newMembersCraftmenNumber = value; OnPropertyChanged(); }
        }
    }
}
