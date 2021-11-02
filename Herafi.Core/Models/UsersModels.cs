using Newtonsoft.Json;
using Refit;
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
    public class UsersModels
    {
    }


    public class User : INotifyPropertyChanged
    {
        private string _id;
        private string _imagePath;
        private string _name;
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        [JsonProperty(PropertyName ="id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="image_path")]
        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; OnPropertyChanged(); }
        }

    }

    public class UserDetails : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ProfileUser _profileUser;
        private ObservableCollection<RequestUser> _requestsUser;


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public ProfileUser ProfileUser
        {
            get { return _profileUser; }
            set { _profileUser = value; OnPropertyChanged(); }
        }


        public ObservableCollection<RequestUser> RequestsUser
        {
            get { return _requestsUser; }
            set { _requestsUser = value; OnPropertyChanged(); }
        }
    }

    public class RequestUser : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _id;
        private string _name;
        private string _craftmanName;
        private string _process;
        private string _startDate;
        private string _endDate;
        private string _cost;
        private string _status;
        private string _hoursWork;
        private string _comment;
        private string _rating;


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        [JsonProperty(PropertyName ="id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="craftman_name")]
        public string CraftmanName
        {
            get { return _craftmanName; }
            set { _craftmanName = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="process")]
        public string Process
        {
            get { return _process; }
            set { _process = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="start_date")]
        public string StartDate
        {
            get { return _startDate; }
            set { _startDate = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="end_date")]
        public string EndDate
        {
            get { return _endDate; }
            set { _endDate = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="cost")]
        public string Cost
        {
            get { return _cost; }
            set { _cost = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="status")]
        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="hours_work")]
        public string HoursWork
        {
            get { return _hoursWork; }
            set { _hoursWork = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="comment")]
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="rating")]
        public string Rating
        {
            get { return _rating; }
            set { _rating = value; OnPropertyChanged(); }
        }
    }

    public class ProfileUser : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _name;
        private string _email;
        private string _phoneNumber;
        private string _nationalNumber;
        private string _city;
        private string _Searchs;
        private string _favourites;
        private string _requestsNum;
        private string _dateJoin;
        private string _profileImage;
        private string _personalIdentityImage;
        private string _id;


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        [JsonProperty(PropertyName ="id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="profile_image")]
        public string ProfileImage
        {
            get { return _profileImage; }
            set { _profileImage = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="personal_identity_image")]
        public string PersonalIdentityImage
        {
            get { return _personalIdentityImage; }
            set { _personalIdentityImage = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="date_join")]
        public string DateJoin
        {
            get { return _dateJoin; }
            set { _dateJoin = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="email")]
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="phone_number")]
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="national_number")]
        public string NationalNumber
        {
            get { return _nationalNumber; }
            set { _nationalNumber = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="city")]
        public string City
        {
            get { return _city; }
            set { _city = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="searchs")]
        public string Searchs
        {
            get { return _Searchs; }
            set { _Searchs = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="favourites")]
        public string Favourites
        {
            get { return _favourites; }
            set { _favourites = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="requests_num")]
        public string RequestsNum
        {
            get { return _requestsNum; }
            set { _requestsNum = value; OnPropertyChanged(); }
        }
    }

}
