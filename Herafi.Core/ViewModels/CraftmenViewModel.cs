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
    public class CraftmenViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Craftman> _craftmen;
        private ObservableCollection<NewMemberCraftman> _newMemberCraftmen;
        private ObservableCollection<ReportCraftman> _reportBlockingCraftmen;
        private ObservableCollection<ReportCraftman> _reportFiringCraftmen;
        private CraftmanDetails _craftmanDetails;
        private ReportedNumbers _reportedNumbers;


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CraftmenViewModel()
        {
            //SampleData();
        }

        private void SampleData()
        {
            Craftmen = new ObservableCollection<Craftman>()
            {
                new Craftman(){Name = "Ahmad Shaban"},
                new Craftman(){Name = "Ahmad Shaban"},
                new Craftman(){Name = "Ahmad Shaban"},
                new Craftman(){Name = "Ahmad Shaban"},
                new Craftman(){Name = "Ahmad Shaban"},
                new Craftman(){Name = "Ahmad Shaban"},
                new Craftman(){Name = "Ahmad Shaban"},
                new Craftman(){Name = "Ahmad Shaban"},
                new Craftman(){Name = "Ahmad Shaban"},
                new Craftman(){Name = "Ahmad Shaban"},
                new Craftman(){Name = "Ahmad Shaban"},
                new Craftman(){Name = "Ahmad Shaban"},
                new Craftman(){Name = "Ahmad Shaban"},
                new Craftman(){Name = "Ahmad Shaban"}
            };

            CraftmanDetails = new CraftmanDetails()
            {
                Profile = new Profile()
                {
                    PersonalIdentityImage = "",
                    ProfileImage = "",
                    Name = "Ahmad Shaban",
                    Email = "ahmadshaban@gmail.com",
                    PhoneNumber = "0996663521",
                    DateJoin = "20/1/2021",
                    NationalNumber = "02050878965",
                    City = "Aleppo",
                    LowestCost = "4000",
                    HighestCost = "8000",
                    Status = "Available",
                    Level = "Normal",
                    UsersSearchs = "50",
                    UsersFavourites = "24",
                    BlocksNum = "2",
                    CertificationsNum = "2",
                    CraftsNum = "1",
                    ProjectsNum = "3",
                    RequestsNum = "3"
                },
                Certifications = new ObservableCollection<string>()
                {
                    "",
                    ""
                },
                Crafts = new ObservableCollection<Craft>()
                {
                    new Craft()
                    {
                        Name = "Electronics",
                        Skills = new ObservableCollection<string>()
                        {
                            "Ghasalat",
                            "Baradat",
                            "Ghasalat",
                            "Ghasalat",
                            "Ghasalat",
                            "Ghasalat",
                            "Ghasalat",
                            "Ghasalat",
                            "Ghasalat"
                        }
                    },
                    new Craft()
                    {
                        Name = "Water Tamded",
                        Skills = new ObservableCollection<string>()
                        {
                            "Ghasalat",
                            "Baradat",
                            "Ghasalat",
                            "Ghasalat",
                            "Ghasalat",
                            "Ghasalat",
                        }
                    }
                },
                Projects = new ObservableCollection<Project>()
                {
                    new Project()
                    {
                        Name = "Tamded network water",
                        Process = "Wage + cost of repair",
                        StartDate = "21/11/2021",
                        EndDate = "22/12/2021",
                        Cost = "126000",
                        Comment = "This is request was amazing and this craftsman make a great job and he's offering multi services that help me to solve my problem",
                        Status = "Finished",
                        HoursWork = "6",
                        Rating = "3.0",
                        Username = "Mouhammad Najjar",
                        Id = "125487",
                        ProjectImages = new ObservableCollection<string>()
                        {
                            "",
                            "",
                            "",
                            ""
                        }
                    },
                    new Project()
                    {
                        Name = "Tamded network water",
                        Process = "Wage + cost of repair",
                        StartDate = "21/11/2021",
                        EndDate = "22/12/2021",
                        Cost = "126000",
                        Comment = "This is request was amazing and this craftsman make a great job and he's offering multi services that help me to solve my problem",
                        Status = "Finished",
                        HoursWork = "6",
                        Rating = "3",
                        Username = "Mouhammad Najjar",
                        Id = "125487",
                        ProjectImages = new ObservableCollection<string>()
                        {
                            "",
                            "",
                            "",
                            ""
                        }
                    }
                },
                Requests = new ObservableCollection<Request>()
                {
                    new Request()
                    {
                        Name = "Tamded network water",
                        Process = "Wage + cost of repair",
                        StartDate = "21/11/2021",
                        EndDate = "22/12/2021",
                        Cost = "126000",
                        Comment = "This is request was amazing and this craftsman make a great job and he's offering multi services that help me to solve my problem",
                        Status = "Finished",
                        HoursWork = "6",
                        Rating = "3",
                        Username = "Mouhammad Najjar"
                    },
                    new Request()
                    {
                        Name = "Tamded network water",
                        Process = "Wage + cost of repair",
                        StartDate = "21/11/2021",
                        EndDate = "22/12/2021",
                        Cost = "126000",
                        Comment = "This is request was amazing and this craftsman make a great job and he's offering multi services that help me to solve my problem",
                        Status = "Finished",
                        HoursWork = "6",
                        Rating = "3",
                        Username = "Mouhammad Najjar"
                    }
                }

            };

            NewMemberCraftmen = new ObservableCollection<NewMemberCraftman>()
            {
                new NewMemberCraftman()
                { 
                    Profile = new Profile()
                    {
                        Id = "2410",
                        ProfileImage = "",
                        Name = "Ahmad Shaban",
                        PersonalIdentityImage = "",
                        Email = "ahmadshaban@gmail.com",
                        PhoneNumber = "0996663521",
                        NationalNumber = "02036589754",
                        DateJoin = "20/1/2021",
                        City = "Aleppo",
                        LowestCost = "4000",
                        HighestCost = "8000",
                        CraftsNum = "3",
                        CertificationsNum = "2",
                    },

                    Crafts = new ObservableCollection<Craft>()
                    {
                        new Craft()
                        {
                            Name = "Electronics",
                            Skills = new ObservableCollection<string>()
                            {
                                "Ghasalat",
                                "Baradat",
                                "Ghasalat",
                                "Ghasalat",
                                "Ghasalat",
                                "Ghasalat",
                                "Ghasalat",
                                "Ghasalat",
                                "Ghasalat"
                            }
                        },
                        new Craft()
                        {
                            Name = "Water Tamded",
                            Skills = new ObservableCollection<string>()
                            {
                                "Ghasalat",
                                "Baradat",
                                "Ghasalat",
                                "Ghasalat",
                                "Ghasalat",
                                "Ghasalat",
                            }
                        }
                    },
                    
                    Certifications = new ObservableCollection<string>()
                    {
                        "",
                        ""
                    }
                },
                new NewMemberCraftman()
                {
                     Profile = new Profile()
                    {
                        Id = "2410",
                        ProfileImage = "",
                        Name = "Ahmad Shaban",
                        PersonalIdentityImage = "",
                        Email = "ahmadshaban@gmail.com",
                        PhoneNumber = "0996663521",
                        NationalNumber = "02036589754",
                        DateJoin = "20/1/2021",
                        City = "Aleppo",
                        LowestCost = "4000",
                        HighestCost = "8000",
                        CraftsNum = "3",
                        CertificationsNum = "2",
                    },
                    Crafts = new ObservableCollection<Craft>()
                    {
                         new Craft()
                        {
                            Name = "Electronics",
                            Skills = new ObservableCollection<string>()
                            {
                                "Ghasalat",
                                "Baradat",
                                "Ghasalat",
                                "Ghasalat",
                                "Ghasalat",
                                "Ghasalat",
                                "Ghasalat",
                                "Ghasalat",
                                "Ghasalat"
                            }
                        },
                        new Craft()
                        {
                            Name = "Water Tamded",
                            Skills = new ObservableCollection<string>()
                            {
                                "Ghasalat",
                                "Baradat",
                                "Ghasalat",
                                "Ghasalat",
                                "Ghasalat",
                                "Ghasalat",
                            }
                        }
                    },
                    Certifications = new ObservableCollection<string>()
                    {
                        "",
                        ""
                    }
                }
            };

            ReportBlockingCraftmen = new ObservableCollection<ReportCraftman>()
            {
                new ReportCraftman()
                {
                    Id = "12485",
                    Name = "Ahmad Najjar",
                    ProfileImage = "",
                    Reports = new ObservableCollection<Report>()
                    {
                        new Report()
                        {
                            Number = "1",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                        new Report()
                        {
                            Number = "2",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                        new Report()
                        {
                            Number = "3",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                        new Report()
                        {
                            Number = "4",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                    }
                },
                new ReportCraftman()
                {
                    Id = "12485",
                    Name = "Ahmad Najjar",
                    ProfileImage = "",
                    Reports = new ObservableCollection<Report>()
                    {
                        new Report()
                        {
                            Number = "1",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                        new Report()
                        {
                            Number = "2",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                        new Report()
                        {
                            Number = "3",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                        new Report()
                        {
                            Number = "4",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                    }
                },
                new ReportCraftman()
                {
                    Id = "12485",
                    Name = "Ahmad Najjar",
                    ProfileImage = "",
                    Reports = new ObservableCollection<Report>()
                    {
                        new Report()
                        {
                            Number = "1",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                        new Report()
                        {
                            Number = "2",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                        new Report()
                        {
                            Number = "3",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                        new Report()
                        {
                            Number = "4",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                    }
                },
            };

            ReportFiringCraftmen = new ObservableCollection<ReportCraftman>()
            {
                new ReportCraftman()
                {
                    Id = "12485",
                    Name = "Ahmad Najjar",
                    ProfileImage = "",
                    Reports = new ObservableCollection<Report>()
                    {
                        new Report()
                        {
                            Number = "1",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                        new Report()
                        {
                            Number = "2",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                        new Report()
                        {
                            Number = "3",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                        new Report()
                        {
                            Number = "4",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                    }
                },
                new ReportCraftman()
                {
                    Id = "12485",
                    Name = "Ahmad Najjar",
                    ProfileImage = "",
                    Reports = new ObservableCollection<Report>()
                    {
                        new Report()
                        {
                            Number = "1",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                        new Report()
                        {
                            Number = "2",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                        new Report()
                        {
                            Number = "3",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                        new Report()
                        {
                            Number = "4",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                    }
                },
                new ReportCraftman()
                {
                    Id = "12485",
                    Name = "Ahmad Najjar",
                    ProfileImage = "",
                    Reports = new ObservableCollection<Report>()
                    {
                        new Report()
                        {
                            Number = "1",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                        new Report()
                        {
                            Number = "2",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                        new Report()
                        {
                            Number = "3",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                        new Report()
                        {
                            Number = "4",
                            UserName = "Ahmad Shaban",
                            RequestId = "1235",
                            Problems = new ObservableCollection<string>()
                            {
                                "Price",
                                "Dealing",
                                "Lates",
                                "Speed"
                            },
                            Comment = "This is request was trash and this craftman make a bad job and he isn't offering any services that help me to solve my problem in I suffer from now, to be honest he don't any thing from his job"
                        },
                    }
                },
            };

            ReportedNumbers = new ReportedNumbers()
            {
                BlockingNumber = "85",
                FiringNumber = "12"
            };
        }


        public ObservableCollection<Craftman> Craftmen
        {
            get { return _craftmen; }
            set { _craftmen = value; OnPropertyChanged(); }
        }

        public ObservableCollection<NewMemberCraftman> NewMemberCraftmen
        {
            get { return _newMemberCraftmen; }
            set { _newMemberCraftmen = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ReportCraftman> ReportBlockingCraftmen
        {
            get { return _reportBlockingCraftmen; }
            set { _reportBlockingCraftmen = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ReportCraftman> ReportFiringCraftmen
        {
            get { return _reportFiringCraftmen; }
            set { _reportFiringCraftmen = value; OnPropertyChanged(); }
        }

        public CraftmanDetails CraftmanDetails
        {
            get { return _craftmanDetails; }
            set { _craftmanDetails = value; OnPropertyChanged(); }
        }

        public ReportedNumbers ReportedNumbers
        {
            get { return _reportedNumbers; }
            set { _reportedNumbers = value; OnPropertyChanged(); }
        }
    }
}
