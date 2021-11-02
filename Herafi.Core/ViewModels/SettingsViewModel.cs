using Herafi.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Herafi.Core.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Admin _admin;

        

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SettingsViewModel()
        {
            //SampleDate();
        }

        private void SampleDate()
        {
            Admin = new Admin()
            {
                Id = "1",
                Name = "Ahmad Shaban",
                Email = "ahmadShaban@gmail.com",
                PhoneNumber = "0996663521",
                NationalNumber = "02050878965",
                City = "Aleppo",
                DateJoin = "20/11/2021",
                ProfileImage = "",
                PersonalIdentityImage = ""
            };
        }


        public Admin Admin
        {
            get { return _admin; }
            set { _admin = value; OnPropertyChanged(); }
        }
    }
}
