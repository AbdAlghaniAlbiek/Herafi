using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Herafi.Core.Models;
using System.Collections.ObjectModel;

namespace Herafi.UWP.Converters
{
    public class AnalyzesRadConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter as string == "Profits")
            {
                ObservableCollection<ModifiedProfitRad> modProfits =
                    new ObservableCollection<ModifiedProfitRad>();

                for (int i = 0; i < (value as ObservableCollection<ProfitRad>).Count; i++)
                {
                    modProfits.Add(
                        new ModifiedProfitRad
                        {
                            Day = (value as ObservableCollection<ProfitRad>)[i].Day,
                            Paids = double.Parse((value as ObservableCollection<ProfitRad>)[i].Paids)
                        });
                }

                //Very important to clear memeory
                (value as ObservableCollection<ProfitRad>).Clear();

                return modProfits;
            }

            else if (parameter as string == "Craftmen")
            {
                ObservableCollection<ModifiedCraftmanRad> modCraftmen =
                    new ObservableCollection<ModifiedCraftmanRad>();

                for (int i = 0; i < (value as ObservableCollection<CraftmanRad>).Count; i++)
                {
                    modCraftmen.Add(
                        new ModifiedCraftmanRad
                        {
                            Day = (value as ObservableCollection<CraftmanRad>)[i].Day,
                            CraftmenNumber = double.Parse((value as ObservableCollection<CraftmanRad>)[i].CraftmenNumber)
                        });
                }

                //Very important to clear memeory
                (value as ObservableCollection<CraftmanRad>).Clear();

                return modCraftmen;
            }

            else if (parameter as string == "Users")
            {
                ObservableCollection<ModifiedUserRad> modUsers =
                    new ObservableCollection<ModifiedUserRad>();

                for (int i = 0; i < (value as ObservableCollection<UserRad>).Count; i++)
                {
                    modUsers.Add(
                        new ModifiedUserRad
                        {
                            Day = (value as ObservableCollection<UserRad>)[i].Day,
                            UsersNumber = double.Parse((value as ObservableCollection<UserRad>)[i].UsersNumber)
                        });
                }

                //Very important to clear memeory
                (value as ObservableCollection<UserRad>).Clear();

                return modUsers;
            }

            else if (parameter as string == "Requests")
            {
                ObservableCollection<ModifiedRequestRad> modRequests =
                    new ObservableCollection<ModifiedRequestRad>();

                for (int i = 0; i < (value as ObservableCollection<RequestRad>).Count; i++)
                {
                    modRequests.Add(
                        new ModifiedRequestRad
                        {
                            Day = (value as ObservableCollection<RequestRad>)[i].Day,
                            RequestsNumber = double.Parse((value as ObservableCollection<RequestRad>)[i].RequestsNumber)
                        });
                }

               //Very important to clear memeory
               (value as ObservableCollection<RequestRad>).Clear();

                return modRequests;
            }

            else if (parameter as string == "Reports")
            {
                ObservableCollection<ModifiedReportRad> modReports =
                    new ObservableCollection<ModifiedReportRad>();

                for (int i = 0; i < (value as ObservableCollection<ReportRad>).Count; i++)
                {
                    modReports.Add(
                        new ModifiedReportRad
                        {
                            Day = (value as ObservableCollection<ReportRad>)[i].Day,
                            ReportsNumber = double.Parse((value as ObservableCollection<ReportRad>)[i].ReportsNumber)
                        });
                }

               //Very important to clear memeory
               (value as ObservableCollection<ReportRad>).Clear();

                return modReports;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
