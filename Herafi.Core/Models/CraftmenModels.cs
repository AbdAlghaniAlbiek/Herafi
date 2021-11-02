using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Refit;
using Newtonsoft.Json;

namespace Herafi.Core.Models
{
    public class CraftmenModels
    {

    }

    public class Craftman : INotifyPropertyChanged
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

    public class CraftmanDetails : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Profile _profile;
        private ObservableCollection<Craft> _crafts;
        private ObservableCollection<string> _certifications;
        private ObservableCollection<Request> _requests;
        private ObservableCollection<Project> _projects;


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [JsonProperty(PropertyName ="profile")]
        public Profile Profile
        {
            get { return _profile; }
            set { _profile = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="crafts")]
        public ObservableCollection<Craft> Crafts
        {
            get { return _crafts; }
            set { _crafts = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="certifications")]
        public ObservableCollection<string> Certifications
        {
            get { return _certifications; }
            set { _certifications = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="requests")]
        public ObservableCollection<Request> Requests
        {
            get { return _requests; }
            set { _requests = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="projects")]
        public ObservableCollection<Project> Projects
        {
            get { return _projects; }
            set { _projects = value; OnPropertyChanged(); }
        }
    }

    public class NewMemberCraftman : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Profile _profile;
        private ObservableCollection<Craft> _crafts;
        private ObservableCollection<string> _certifications;
        private string _level = "Normal";
        private bool _isThereCertifications;

      
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [JsonProperty(PropertyName = "is_there_certifications")]
        public bool IsThereCertifications
        {
            get { return _isThereCertifications; }
            set { _isThereCertifications = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName = "level")]
        public string Level
        {
            get { return _level; }
            set { _level = value; OnPropertyChanged(); }
        }




        [JsonProperty(PropertyName ="profile")]
        public Profile Profile
        {
            get { return _profile; }
            set { _profile = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="crafts")]
        public ObservableCollection<Craft> Crafts
        {
            get { return _crafts; }
            set { _crafts = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="certifications")]
        public ObservableCollection<string> Certifications
        {
            get { return _certifications; }
            set { _certifications = value; OnPropertyChanged(); }
        }

    }

    public class ReportCraftman : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        private string _id;
        private string _profileImage;
        private ObservableCollection<Report> _reports;
        


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


        [JsonProperty(PropertyName ="name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="reports")]
        public ObservableCollection<Report> Reports
        {
            get { return _reports; }
            set { _reports = value; OnPropertyChanged(); }
        }

    }



    public class Craft : INotifyPropertyChanged
    {
        private ObservableCollection<string> _skills;
        private string _name;
        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        [JsonProperty(PropertyName ="name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        [JsonProperty(PropertyName ="skills")]
        public ObservableCollection<string> Skills
        {
            get { return _skills; }
            set { _skills = value; OnPropertyChanged(); }
        }
    }

    public class Profile : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _name;
        private string _email;
        private string _phoneNumber;
        private string _nationalNumber;
        private string _city;
        private string _lowest_cost;
        private string _highestCost;
        private string _status;
        private string _level;
        private string _usersSearchs;
        private string _usersFavourites;
        private string _craftsNum;
        private string _blocksNum;
        private string _certificationsNum;
        private string _projectsNum;
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


        [JsonProperty(PropertyName ="lowest_cost")]
        public string LowestCost
        {
            get { return _lowest_cost; }
            set { _lowest_cost = value; OnPropertyChanged(); }
        }

        [JsonProperty(PropertyName ="highest_cost")]
        public string HighestCost
        {
            get { return _highestCost; }
            set { _highestCost = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="status")]
        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="level")]
        public string Level
        {
            get { return _level; }
            set { _level = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="users_searchs")]
        public string UsersSearchs
        {
            get { return _usersSearchs; }
            set { _usersSearchs = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="users_favourites")]
        public string UsersFavourites
        {
            get { return _usersFavourites; }
            set { _usersFavourites = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="blocks_num")]
        public string BlocksNum
        {
            get { return _blocksNum; }
            set { _blocksNum = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="crafts_num")]
        public string CraftsNum
        {
            get { return _craftsNum; }
            set { _craftsNum = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="certifications_num")]
        public string CertificationsNum
        {
            get { return _certificationsNum; }
            set { _certificationsNum = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="projects_num")]
        public string ProjectsNum
        {
            get { return _projectsNum; }
            set { _projectsNum = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="requests_num")]
        public string RequestsNum
        {
            get { return _requestsNum; }
            set { _requestsNum = value; OnPropertyChanged(); }
        }


    }

    public class Project : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _id;
        private string _name;
        private string _userName;
        private string _process;
        private string _startDate;
        private string _endDate;
        private string _cost;
        private string _status;
        private string _hoursWork;
        private string _comment;
        private string _rating;
        private ObservableCollection<string> _projectImages;


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


        [JsonProperty(PropertyName ="user_name")]
        public string Username
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(); }
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


        [JsonProperty(PropertyName ="project_images")]
        public ObservableCollection<string> ProjectImages
        {
            get { return _projectImages; }
            set { _projectImages = value; OnPropertyChanged(); }
        }

    }

    public class Request : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _id;
        private string _name;
        private string _userName;
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


        [JsonProperty(PropertyName ="user_name")]
        public string Username
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(); }
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

    public class Report : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _userName;
        private string _requestId;
        private string _comment;
        private ObservableCollection<string> _problems;
        private string _number;


        [JsonProperty(PropertyName ="number")]
        public string Number
        {
            get { return _number; }
            set { _number = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="user_name")]
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="request_id")]
        public string RequestId
        {
            get { return _requestId; }
            set { _requestId = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="comment")]
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="problems")]
        public ObservableCollection<string> Problems
        {
            get { return _problems; }
            set { _problems = value; OnPropertyChanged(); }
        }

    }

    public class ReportedNumbers : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _blockingNumber;
        private string _firingNum;



        [JsonProperty(PropertyName = "blocking_num")]
        public string BlockingNumber
        {
            get { return _blockingNumber; }
            set { _blockingNumber = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName = "firing_num")]
        public string FiringNumber
        {
            get { return _firingNum; }
            set { _firingNum = value; OnPropertyChanged(); }
        }

    }
}
