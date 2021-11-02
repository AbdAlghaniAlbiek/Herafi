using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Microsoft.Toolkit.Uwp.Notifications;
using Herafi.Core.Repositories.RemoteRepo;
using Herafi.Core.Models;
using System.Diagnostics;
using Windows.UI.Notifications;
using Herafi.Core.Helpers;

namespace Herafi.Background.Notifications
{
    public sealed class NewMembers : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral; // Note: defined at class scope so that we can mark it complete inside the OnCancel() callback if we choose to support cancellation

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var cost = BackgroundWorkCost.CurrentBackgroundWorkCost;
            if (cost == BackgroundWorkCostValue.High)
            {
                Debug.WriteLine("Background task aborted (Cost is high)");
            }
            else
            {
                taskInstance.Canceled += (s, e) =>
                {
                    Debug.WriteLine("Background task is canceled");
                };


                _deferral = taskInstance.GetDeferral();

                try
                {
                    var newMembersResponse = await DashboardRepo.GetNewMembersAsync();

                    if (newMembersResponse.Response.Result != null)
                    {
                        if (StaticValues.NEW_CRAFTMEN == 0 || StaticValues.NEW_USERS == 0)
                        {
                            StaticValues.NEW_CRAFTMEN =
                                int.Parse((newMembersResponse.Response.Result as Core.Models.NewMembers).NewMembersCraftmenNumber);
                            StaticValues.NEW_USERS =
                                int.Parse((newMembersResponse.Response.Result as Core.Models.NewMembers).NewMembersUsersNumber);
                        }
                        else
                        {
                            if (StaticValues.NEW_CRAFTMEN != int.Parse((newMembersResponse.Response.Result as Core.Models.NewMembers).NewMembersCraftmenNumber))
                            {
                                var toast = new ToastNotification(
                                    new ToastContentBuilder()
                                    .AddText("Hearfi App", hintMaxLines: 1)
                                    .AddText(string.Format("You have {0} new craftmen, please check them",
                                    (int.Parse((newMembersResponse.Response.Result as Core.Models.NewMembers).NewMembersCraftmenNumber)) - StaticValues.NEW_CRAFTMEN).ToString()).GetXml());

                                var notifier = ToastNotificationManager.CreateToastNotifier();
                                notifier.Show(toast);

                                StaticValues.NEW_CRAFTMEN = int.Parse((newMembersResponse.Response.Result as Core.Models.NewMembers).NewMembersCraftmenNumber);
                            }

                            if (StaticValues.NEW_USERS != int.Parse((newMembersResponse.Response.Result as Core.Models.NewMembers).NewMembersUsersNumber))
                            {
                                var toast = new ToastNotification(
                                   new ToastContentBuilder()
                                   .AddText("Hearfi App", hintMaxLines: 1)
                                   .AddText(string.Format("You have {0} new Users, please check them",
                                   (int.Parse((newMembersResponse.Response.Result as Core.Models.NewMembers).NewMembersUsersNumber)) - StaticValues.NEW_USERS).ToString()).GetXml());

                                var notifier = ToastNotificationManager.CreateToastNotifier();
                                notifier.Show(toast);

                                StaticValues.NEW_USERS = int.Parse((newMembersResponse.Response.Result as Core.Models.NewMembers).NewMembersUsersNumber);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Background task error: " + ex.Message);
                }
                finally
                {
                    _deferral.Complete();
                }
            }
        }
    }
}
