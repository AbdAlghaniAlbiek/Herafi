using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Herafi.Core.Models
{
    class AnalyzesModels
    {
    }

    public class ProfitRad : INotifyPropertyChanged
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

    public class ModifiedProfitRad : INotifyPropertyChanged
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

    public class ProfitsRadDetails : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _perYear;
        private string _perMonth;
        private string _perDay;
        private string _perHour;
        private string _total;
        private ObservableCollection<ProfitRad> _profitsRad;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [JsonProperty(PropertyName = "per_year")]
        public string PerYear
        {
            get { return _perYear; }
            set { _perYear = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName = "per_month")]
        public string PerMonth
        {
            get { return _perMonth; }
            set { _perMonth = value; OnPropertyChanged(); }
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

        [JsonProperty(PropertyName = "profits_rad")]
        public ObservableCollection<ProfitRad> ProfitsRad
        {
            get { return _profitsRad; }
            set { _profitsRad = value; OnPropertyChanged(); }
        }
    }



    public class CraftmanRad : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _day;
        private string _craftmenNumber;

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


        [JsonProperty(PropertyName = "craftmen_number")]
        public string CraftmenNumber
        {
            get { return _craftmenNumber; }
            set { _craftmenNumber = value; OnPropertyChanged(); }
        }

    }

    public class ModifiedCraftmanRad : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _day;
        private double _craftmenNumber;

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


        [JsonProperty(PropertyName = "craftmen_number")]
        public double CraftmenNumber
        {
            get { return _craftmenNumber; }
            set { _craftmenNumber = value; OnPropertyChanged(); }
        }

    }

    public class CraftmenRadDetails : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _perYear;
        private string _perMonth;
        private string _perDay;
        private string _perHour;
        private string _total;
        private ObservableCollection<CraftmanRad> _craftmenRad;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [JsonProperty(PropertyName = "per_year")]
        public string PerYear
        {
            get { return _perYear; }
            set { _perYear = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName = "per_month")]
        public string PerMonth
        {
            get { return _perMonth; }
            set { _perMonth = value; OnPropertyChanged(); }
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

        [JsonProperty(PropertyName = "craftmen_rad")]
        public ObservableCollection<CraftmanRad> CraftmenRad
        {
            get { return _craftmenRad; }
            set { _craftmenRad = value; OnPropertyChanged(); }
        }
    }




    public class UserRad : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _day;
        private string _usersNumber;

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


        [JsonProperty(PropertyName = "users_number")]
        public string UsersNumber
        {
            get { return _usersNumber; }
            set { _usersNumber = value; OnPropertyChanged(); }
        }

    }

    public class ModifiedUserRad : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _day;
        private double _usersNumber;

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


        [JsonProperty(PropertyName = "users_number")]
        public double UsersNumber
        {
            get { return _usersNumber; }
            set { _usersNumber = value; OnPropertyChanged(); }
        }

    }

    public class UsersRadDetails : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _perYear;
        private string _perMonth;
        private string _perDay;
        private string _perHour;
        private string _total;
        private ObservableCollection<UserRad> _usersRad;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [JsonProperty(PropertyName = "per_year")]
        public string PerYear
        {
            get { return _perYear; }
            set { _perYear = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName = "per_month")]
        public string PerMonth
        {
            get { return _perMonth; }
            set { _perMonth = value; OnPropertyChanged(); }
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

        [JsonProperty(PropertyName = "users_rad")]
        public ObservableCollection<UserRad> UsersRad
        {
            get { return _usersRad; }
            set { _usersRad = value; OnPropertyChanged(); }
        }
    }




    public class RequestRad : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _day;
        private string _requestsNumber;

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


        [JsonProperty(PropertyName = "requests_number")]
        public string RequestsNumber
        {
            get { return _requestsNumber; }
            set { _requestsNumber = value; OnPropertyChanged(); }
        }

    }

    public class ModifiedRequestRad : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _day;
        private double _requestsNumber;

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


        [JsonProperty(PropertyName = "requests_number")]
        public double RequestsNumber
        {
            get { return _requestsNumber; }
            set { _requestsNumber = value; OnPropertyChanged(); }
        }

    }

    public class RequestsRadDetails : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _perYear;
        private string _perMonth;
        private string _perDay;
        private string _perHour;
        private string _total;
        private ObservableCollection<RequestRad> _requestsRad;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [JsonProperty(PropertyName = "per_year")]
        public string PerYear
        {
            get { return _perYear; }
            set { _perYear = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName = "per_month")]
        public string PerMonth
        {
            get { return _perMonth; }
            set { _perMonth = value; OnPropertyChanged(); }
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

        [JsonProperty(PropertyName = "requests_rad")]
        public ObservableCollection<RequestRad> RequestsRad
        {
            get { return _requestsRad; }
            set { _requestsRad = value; OnPropertyChanged(); }
        }
    }




    public class ReportRad : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _day;
        private string _reportsNumber;

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


        [JsonProperty(PropertyName = "reports_number")]
        public string ReportsNumber
        {
            get { return _reportsNumber; }
            set { _reportsNumber = value; OnPropertyChanged(); }
        }

    }

    public class ModifiedReportRad : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _day;
        private double _reportsNumber;

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


        [JsonProperty(PropertyName = "reports_number")]
        public double ReportsNumber
        {
            get { return _reportsNumber; }
            set { _reportsNumber = value; OnPropertyChanged(); }
        }

    }

    public class ReportsRadDetails : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _perYear;
        private string _perMonth;
        private string _perDay;
        private string _perHour;
        private string _total;
        private ObservableCollection<ReportRad> _reportsRad;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [JsonProperty(PropertyName = "per_year")]
        public string PerYear
        {
            get { return _perYear; }
            set { _perYear = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName = "per_month")]
        public string PerMonth
        {
            get { return _perMonth; }
            set { _perMonth = value; OnPropertyChanged(); }
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

        [JsonProperty(PropertyName = "reports_rad")]
        public ObservableCollection<ReportRad> ReportsRad
        {
            get { return _reportsRad; }
            set { _reportsRad = value; OnPropertyChanged(); }
        }
    }
}
