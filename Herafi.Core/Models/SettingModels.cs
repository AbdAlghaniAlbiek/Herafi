using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Herafi.Core.Models
{
    public class SettingModels
    {

    }


    public class Admin : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _id;
        private string _name;
        private string _email;
        private string _phoneNumber;
        private string _dateJoin;
        private string _nationalNumber;
        private string _city;
        private string _profileImage;
        private string _personalIdentityImage;
        private string _facebookId;
        private string _microsoftId;
        private string _password;

        
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName = "password")]
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName = "email")]
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName = "phone_number")]
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName = "date_join")]
        public string DateJoin
        {
            get { return _dateJoin; }
            set { _dateJoin = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName = "national_number")]
        public string NationalNumber
        {
            get { return _nationalNumber; }
            set { _nationalNumber = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName = "city")]
        public string City
        {
            get { return _city; }
            set { _city = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName = "profile_image")]
        public string ProfileImage
        {
            get { return _profileImage; }
            set { _profileImage = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName = "personal_identity_image")]
        public string PersonalIdentityImage
        {
            get { return _personalIdentityImage; }
            set { _personalIdentityImage = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName = "facebook_id")]
        public string FacebookId
        {
            get { return _facebookId; }
            set { _facebookId = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName = "microsoft_id")]
        public string MicrosoftId
        {
            get { return _microsoftId; }
            set { _microsoftId = value; OnPropertyChanged(); }
        }
    }



    public class City:INotifyPropertyChanged
    {
        private string _id;
        private string _name;
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

    }
}
