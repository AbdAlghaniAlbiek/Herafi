using Herafi.Core.Models;
using Herafi.Core.Repositories.RepoOperations;
using Herafi.Core.Security;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herafi.Core.Repositories.RemoteRepo
{
    public class DashboardRepo
    {
        private static async Task<HttpReponse> HandlingErrorMessage(HttpReponse httpReponse, ApiException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                httpReponse = await
                   ex.GetContentAsAsync<HttpReponse>();
                httpReponse.ErrorMessage =
                    AESCryptography.Decrypt(httpReponse.ErrorMessage);

                if (!string.IsNullOrEmpty(httpReponse.Response.Token))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token;
                }

                //if (!string.IsNullOrEmpty(httpReponse.Response.Token))
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        Common.SUB_TOKEN = httpReponse.Response.Token;
                //    }
                //    else
                //    {
                //        httpReponse.ErrorMessage =
                //            TranslatingUI.MainResourceMap.GetValue("txtToastNotFromServerErr", TranslatingUI.ResourceContextObj).ValueAsString;
                //    }
                //}
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                httpReponse = new HttpReponse()
                {
                    Response = new Response { Result = null, Token = string.Empty },
                    ErrorMessage = string.Empty
                };
                httpReponse.ErrorMessage =
                    "The requested resource isn't exist in the server (error: 404)";
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                httpReponse = new HttpReponse()
                {
                    Response = new Response { Result = null, Token = string.Empty },
                    ErrorMessage = string.Empty
                };
                httpReponse.ErrorMessage =
                    "Error from the server (error: 500)";
            }
            else
            {
                httpReponse = await ex.GetContentAsAsync<HttpReponse>();
                httpReponse.ErrorMessage = ex.Message;
            }

            return httpReponse;
        }


        public async static Task<HttpReponse> GetProfitsDetailsAsync()
        {
            var dashboardApis = RestService.For<IDashboardRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse profDetHttpReponse = null;

            try
            {
                profDetHttpReponse =
                    await dashboardApis.GetProfitsDetailsAsync(Common.TOKEN);

                ////Modifying new Members data
                //var newMembers =
                //        JsonConvert.DeserializeObject<NewMembers>(newMemHttpReponse.Response.Result.ToString());
                
                //newMembers.NewMembersUsers = AESCryptography.Decrypt(newMembers.NewMembersUsers);
                //newMembers.NewMembersCraftmen = AESCryptography.Decrypt(newMembers.NewMembersCraftmen);


                //Modifying profits details data
                var profitsDetails =
                        JsonConvert.DeserializeObject<ProfitDetails>(profDetHttpReponse.Response.Result.ToString());

                profitsDetails.PerDay = AESCryptography.Decrypt(profitsDetails.PerDay);
                profitsDetails.PerHour = AESCryptography.Decrypt(profitsDetails.PerHour);
                profitsDetails.Total = AESCryptography.Decrypt(profitsDetails.Total);
                profitsDetails.ProfitsPrecentage = AESCryptography.Decrypt(profitsDetails.ProfitsPrecentage);

                for (int i = 0; i < profitsDetails.Profits.Count; i++)
                {
                    profitsDetails.Profits[i].Day = AESCryptography.Decrypt(profitsDetails.Profits[i].Day);
                    profitsDetails.Profits[i].Paids = AESCryptography.Decrypt(profitsDetails.Profits[i].Paids);
                }

                profDetHttpReponse.Response.Result = profitsDetails;

                if (!string.IsNullOrEmpty(profDetHttpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = profDetHttpReponse.Response.Token as string;
                }

            }
            catch (ApiException ex)
            {
                profDetHttpReponse = await HandlingErrorMessage(profDetHttpReponse, ex);
            }

            return profDetHttpReponse;
        }

        public async static Task<HttpReponse> GetNewMembersAsync()
        {
            var dashboardApis = RestService.For<IDashboardRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse newMemHttpReponse = null;

            try
            {
                newMemHttpReponse =
                    await dashboardApis.GetNewMembersAsync(Common.TOKEN);

                //Modifying new Members data
                var newMembers =
                        JsonConvert.DeserializeObject<NewMembers>(newMemHttpReponse.Response.Result.ToString());

                newMembers.NewMembersUsersNumber = AESCryptography.Decrypt(newMembers.NewMembersUsersNumber);
                newMembers.NewMembersCraftmenNumber = AESCryptography.Decrypt(newMembers.NewMembersCraftmenNumber);


                newMemHttpReponse.Response.Result = newMembers;

                if (!string.IsNullOrEmpty(newMemHttpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = newMemHttpReponse.Response.Token as string;
                }

            }
            catch (ApiException ex)
            {
                newMemHttpReponse = await HandlingErrorMessage(newMemHttpReponse, ex);
            }

            return newMemHttpReponse;
        }
    }
}
