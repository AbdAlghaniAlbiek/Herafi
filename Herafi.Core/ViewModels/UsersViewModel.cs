using Herafi.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Herafi.Core.ViewModels
{
    public class UsersViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<User> _users;
        private UserDetails _userDetails;
        private ObservableCollection<ProfileUser> _newMembersUsers;

       
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UsersViewModel()
        {
            //SampleData();
        }

        private void SampleData()
        {
            Users = new ObservableCollection<User>()
            {
                new User(){Name = "Ahmad Shaban"},
                new User(){Name = "Ahmad Shaban"},
                new User(){Name = "Ahmad Shaban"},
                new User(){Name = "Ahmad Shaban"},
                new User(){Name = "Ahmad Shaban"},
                new User(){Name = "Ahmad Shaban"},
                new User(){Name = "Ahmad Shaban"},
                new User(){Name = "Ahmad Shaban"},
                new User(){Name = "Ahmad Shaban"},
                new User(){Name = "Ahmad Shaban"},
                new User(){Name = "Ahmad Shaban"},
                new User(){Name = "Ahmad Shaban"},
                new User(){Name = "Ahmad Shaban"},
                new User(){Name = "Ahmad Shaban"}
            };

            UserDetails = new UserDetails()
            {
                ProfileUser = new ProfileUser()
                {
                    Id = "1",
                    Name = "Ahmad Shaban",
                    Email = "ahmadshaban@gmail.com",
                    PhoneNumber = "0996663521",
                    DateJoin = "20/11/2021",
                    NationalNumber = "02050878965",
                    City = "Aleppo",
                    Favourites = "24",
                    Searchs = "50",
                    RequestsNum = "3",
                    ProfileImage = "",
                    PersonalIdentityImage = ""
                },
                RequestsUser = new ObservableCollection<RequestUser>()
                {
                    new RequestUser()
                    {
                        Id = "125487",
                        Name = "Tamded network water",
                        CraftmanName = "Mouhammad Najjar",
                        Process = "Wage + cost of repair",
                        StartDate = "21/11/2021",
                        EndDate = "22/12/2021",
                        Cost = "126000",
                        Status = "Finished",
                        HoursWork = "6",
                        Rating = "3",
                        Comment = "This is request was amazing and this craftsman make a great job and he's offering multi services that help me to solve my problem"
                    },
                    new RequestUser()
                    {
                        Id = "125487",
                        Name = "Tamded network water",
                        CraftmanName = "Mouhammad Najjar",
                        Process = "Wage + cost of repair",
                        StartDate = "21/11/2021",
                        EndDate = "22/12/2021",
                        Cost = "126000",
                        Status = "Finished",
                        HoursWork = "6",
                        Rating = "3",
                        Comment = "This is request was amazing and this craftsman make a great job and he's offering multi services that help me to solve my problem"
                    },
                    new RequestUser()
                    {
                        Id = "125487",
                        Name = "Tamded network water",
                        CraftmanName = "Mouhammad Najjar",
                        Process = "Wage + cost of repair",
                        StartDate = "21/11/2021",
                        EndDate = "22/12/2021",
                        Cost = "126000",
                        Status = "Finished",
                        HoursWork = "6",
                        Rating = "3",
                        Comment = "This is request was amazing and this craftsman make a great job and he's offering multi services that help me to solve my problem"
                    }
                }
            };

            NewMembersUsers = new ObservableCollection<ProfileUser>()
            {
                new ProfileUser()
                {
                    Id = "1",
                    Name = "Ahamd Shaban",
                    Email = "ahamdshaban@gmail.com",
                    PhoneNumber = "0996663521",
                    City = "Aleppo",
                    DateJoin = "20/11/2021",
                    NationalNumber = "02050878965",
                    PersonalIdentityImage = "",
                    ProfileImage = ""
                },
                new ProfileUser()
                {
                    Id = "1",
                    Name = "Ahamd Shaban",
                    Email = "ahamdshaban@gmail.com",
                    PhoneNumber = "0996663521",
                    City = "Aleppo",
                    DateJoin = "20/11/2021",
                    NationalNumber = "02050878965",
                    PersonalIdentityImage = "",
                    ProfileImage = ""
                },
                new ProfileUser()
                {
                    Id = "1",
                    Name = "Ahamd Shaban",
                    Email = "ahamdshaban@gmail.com",
                    PhoneNumber = "0996663521",
                    City = "Aleppo",
                    DateJoin = "20/11/2021",
                    NationalNumber = "02050878965",
                    PersonalIdentityImage = "",
                    ProfileImage = ""
                }
            };
        }

   
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set { _users = value; OnPropertyChanged(); }
        }

        public UserDetails UserDetails
        {
            get { return _userDetails; }
            set { _userDetails = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ProfileUser> NewMembersUsers
        {
            get { return _newMembersUsers; }
            set { _newMembersUsers = value; OnPropertyChanged(); }
        }
    }
}
