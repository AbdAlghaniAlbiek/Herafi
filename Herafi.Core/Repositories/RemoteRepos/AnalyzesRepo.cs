using Herafi.Core.Models;
using Herafi.Core.Security;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Herafi.Core.Repositories.RepoOperations;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Herafi.Core.Repositories.RemoteRepo
{
    public class AnalyzesRepo
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


        //Profits section
        public static async Task<HttpReponse> GetProfitsYearsAsync()
        {
            var analyzesApis = RestService.For<IAnalyzesRepoOps>(
               Common.URL,
               new RefitSettings
               {
                   ContentSerializer = new NewtonsoftJsonContentSerializer()
               });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse = 
                   await analyzesApis.GetProfitsYearsOpAsync(Common.TOKEN);

                ObservableCollection<string> years =
                    JsonConvert.DeserializeObject<ObservableCollection<string>>(
                        httpReponse.Response.Result.ToString());

                for (int i = 0; i < years.Count; i++)
                {
                    years[i] = AESCryptography.Decrypt(years[i]);
                }

                httpReponse.Response.Result = years;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token;
                }
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> GetProfitsMonthsAsync(string year)
        {
            var analyzesApis = RestService.For<IAnalyzesRepoOps>(
               Common.URL,
               new RefitSettings
               {
                   ContentSerializer = new NewtonsoftJsonContentSerializer()
               });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                   await analyzesApis.GetProfitsMonthsOpAsync(
                       AESCryptography.Encrypt(year), Common.TOKEN);

                ObservableCollection<string> months =
                    JsonConvert.DeserializeObject<ObservableCollection<string>>(
                        httpReponse.Response.Result.ToString());

                for (int i = 0; i < months.Count; i++)
                {
                    months[i] = AESCryptography.Decrypt(months[i]);
                }

                httpReponse.Response.Result = months;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token;
                }
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> GetProfitsDetailsAsync(string year, string month)
        {
            var analyzesApis = RestService.For<IAnalyzesRepoOps>(
               Common.URL,
               new RefitSettings
               {
                   ContentSerializer = new NewtonsoftJsonContentSerializer()
               });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                   await analyzesApis.GetProfitsDetailsOpAsync(
                       AESCryptography.Encrypt(year), AESCryptography.Encrypt(month), Common.TOKEN);

                ProfitsRadDetails profitRadDetails =
                    JsonConvert.DeserializeObject<ProfitsRadDetails>(httpReponse.Response.Result.ToString());


                profitRadDetails.PerYear = AESCryptography.Decrypt(profitRadDetails.PerYear);
                profitRadDetails.PerMonth = AESCryptography.Decrypt(profitRadDetails.PerMonth);
                profitRadDetails.PerDay = AESCryptography.Decrypt(profitRadDetails.PerDay);
                profitRadDetails.PerHour = AESCryptography.Decrypt(profitRadDetails.PerHour);
                profitRadDetails.Total = AESCryptography.Decrypt(profitRadDetails.Total);

                for (int i = 0; i < profitRadDetails.ProfitsRad.Count; i++)
                {
                    profitRadDetails.ProfitsRad[i].Day = AESCryptography.Decrypt(profitRadDetails.ProfitsRad[i].Day);
                    profitRadDetails.ProfitsRad[i].Paids = AESCryptography.Decrypt(profitRadDetails.ProfitsRad[i].Paids);
                }

                httpReponse.Response.Result = profitRadDetails;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token;
                }
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }



        //Craftmen section
        public static async Task<HttpReponse> GetCraftmenYearsAsync()
        {
            var analyzesApis = RestService.For<IAnalyzesRepoOps>(
               Common.URL,
               new RefitSettings
               {
                   ContentSerializer = new NewtonsoftJsonContentSerializer()
               });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                   await analyzesApis.GetCraftmenYearsOpAsync(Common.TOKEN);

                ObservableCollection<string> years =
                    JsonConvert.DeserializeObject<ObservableCollection<string>>(
                        httpReponse.Response.Result.ToString());

                for (int i = 0; i < years.Count; i++)
                {
                    years[i] = AESCryptography.Decrypt(years[i]);
                }

                httpReponse.Response.Result = years;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token;
                }
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> GetCraftmenMonthsAsync(string year)
        {
            var analyzesApis = RestService.For<IAnalyzesRepoOps>(
               Common.URL,
               new RefitSettings
               {
                   ContentSerializer = new NewtonsoftJsonContentSerializer()
               });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                   await analyzesApis.GetCraftmenMonthsOpAsync(
                       AESCryptography.Encrypt(year), Common.TOKEN);

                ObservableCollection<string> months =
                    JsonConvert.DeserializeObject<ObservableCollection<string>>(
                        httpReponse.Response.Result.ToString());

                for (int i = 0; i < months.Count; i++)
                {
                    months[i] = AESCryptography.Decrypt(months[i]);
                }

                httpReponse.Response.Result = months;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token;
                }
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> GetCraftmenDetailsAsync(string year, string month)
        {
            var analyzesApis = RestService.For<IAnalyzesRepoOps>(
               Common.URL,
               new RefitSettings
               {
                   ContentSerializer = new NewtonsoftJsonContentSerializer()
               });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                   await analyzesApis.GetCraftmenDetailsOpAsync(
                       AESCryptography.Encrypt(year), AESCryptography.Encrypt(month), Common.TOKEN);

                CraftmenRadDetails craftmenRadDetails =
                    JsonConvert.DeserializeObject<CraftmenRadDetails>(httpReponse.Response.Result.ToString());


                craftmenRadDetails.PerYear = AESCryptography.Decrypt(craftmenRadDetails.PerYear);
                craftmenRadDetails.PerMonth = AESCryptography.Decrypt(craftmenRadDetails.PerMonth);
                craftmenRadDetails.PerDay = AESCryptography.Decrypt(craftmenRadDetails.PerDay);
                craftmenRadDetails.PerHour = AESCryptography.Decrypt(craftmenRadDetails.PerHour);
                craftmenRadDetails.Total = AESCryptography.Decrypt(craftmenRadDetails.Total);

                for (int i = 0; i < craftmenRadDetails.CraftmenRad.Count; i++)
                {
                    craftmenRadDetails.CraftmenRad[i].Day = AESCryptography.Decrypt(craftmenRadDetails.CraftmenRad[i].Day);
                    craftmenRadDetails.CraftmenRad[i].CraftmenNumber = AESCryptography.Decrypt(craftmenRadDetails.CraftmenRad[i].CraftmenNumber);
                }

                httpReponse.Response.Result = craftmenRadDetails;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token;
                }
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }



        //Users section
        public static async Task<HttpReponse> GetUsersYearsAsync()
        {
            var analyzesApis = RestService.For<IAnalyzesRepoOps>(
               Common.URL,
               new RefitSettings
               {
                   ContentSerializer = new NewtonsoftJsonContentSerializer()
               });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                   await analyzesApis.GetUsersYearsOpAsync(Common.TOKEN);

                ObservableCollection<string> years =
                    JsonConvert.DeserializeObject<ObservableCollection<string>>(
                        httpReponse.Response.Result.ToString());

                for (int i = 0; i < years.Count; i++)
                {
                    years[i] = AESCryptography.Decrypt(years[i]);
                }

                httpReponse.Response.Result = years;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token;
                }
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> GetUsersMonthsAsync(string year)
        {
            var analyzesApis = RestService.For<IAnalyzesRepoOps>(
               Common.URL,
               new RefitSettings
               {
                   ContentSerializer = new NewtonsoftJsonContentSerializer()
               });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                   await analyzesApis.GetUsersMonthsOpAsync(
                       AESCryptography.Encrypt(year), Common.TOKEN);

                ObservableCollection<string> months =
                    JsonConvert.DeserializeObject<ObservableCollection<string>>(
                        httpReponse.Response.Result.ToString());

                for (int i = 0; i < months.Count; i++)
                {
                    months[i] = AESCryptography.Decrypt(months[i]);
                }

                httpReponse.Response.Result = months;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token;
                }
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> GetUsersDetailsAsync(string year, string month)
        {
            var analyzesApis = RestService.For<IAnalyzesRepoOps>(
               Common.URL,
               new RefitSettings
               {
                   ContentSerializer = new NewtonsoftJsonContentSerializer()
               });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                   await analyzesApis.GetUsersDetailsOpAsync(
                       AESCryptography.Encrypt(year), AESCryptography.Encrypt(month), Common.TOKEN);

                UsersRadDetails usersRadDetails =
                    JsonConvert.DeserializeObject<UsersRadDetails>(httpReponse.Response.Result.ToString());


                usersRadDetails.PerYear = AESCryptography.Decrypt(usersRadDetails.PerYear);
                usersRadDetails.PerMonth = AESCryptography.Decrypt(usersRadDetails.PerMonth);
                usersRadDetails.PerDay = AESCryptography.Decrypt(usersRadDetails.PerDay);
                usersRadDetails.PerHour = AESCryptography.Decrypt(usersRadDetails.PerHour);
                usersRadDetails.Total = AESCryptography.Decrypt(usersRadDetails.Total);

                for (int i = 0; i < usersRadDetails.UsersRad.Count; i++)
                {
                    usersRadDetails.UsersRad[i].Day = AESCryptography.Decrypt(usersRadDetails.UsersRad[i].Day);
                    usersRadDetails.UsersRad[i].UsersNumber = AESCryptography.Decrypt(usersRadDetails.UsersRad[i].UsersNumber);
                }

                httpReponse.Response.Result = usersRadDetails;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token;
                }
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }



        //Requests section
        public static async Task<HttpReponse> GetRequestsYearsAsync()
        {
            var analyzesApis = RestService.For<IAnalyzesRepoOps>(
               Common.URL,
               new RefitSettings
               {
                   ContentSerializer = new NewtonsoftJsonContentSerializer()
               });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                   await analyzesApis.GetRequestsYearsOpAsync(Common.TOKEN);

                ObservableCollection<string> years =
                    JsonConvert.DeserializeObject<ObservableCollection<string>>(
                        httpReponse.Response.Result.ToString());

                for (int i = 0; i < years.Count; i++)
                {
                    years[i] = AESCryptography.Decrypt(years[i]);
                }

                httpReponse.Response.Result = years;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token;
                }
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> GetRequestsMonthsAsync(string year)
        {
            var analyzesApis = RestService.For<IAnalyzesRepoOps>(
               Common.URL,
               new RefitSettings
               {
                   ContentSerializer = new NewtonsoftJsonContentSerializer()
               });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                   await analyzesApis.GetRequestsMonthsOpAsync(
                       AESCryptography.Encrypt(year), Common.TOKEN);

                ObservableCollection<string> months =
                    JsonConvert.DeserializeObject<ObservableCollection<string>>(
                        httpReponse.Response.Result.ToString());

                for (int i = 0; i < months.Count; i++)
                {
                    months[i] = AESCryptography.Decrypt(months[i]);
                }

                httpReponse.Response.Result = months;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token;
                }
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> GetRequestsDetailsAsync(string year, string month)
        {
            var analyzesApis = RestService.For<IAnalyzesRepoOps>(
               Common.URL,
               new RefitSettings
               {
                   ContentSerializer = new NewtonsoftJsonContentSerializer()
               });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                   await analyzesApis.GetRequestsDetailsOpAsync(
                       AESCryptography.Encrypt(year), AESCryptography.Encrypt(month), Common.TOKEN);

                RequestsRadDetails requestsRadDetails =
                    JsonConvert.DeserializeObject<RequestsRadDetails>(httpReponse.Response.Result.ToString());


                requestsRadDetails.PerYear = AESCryptography.Decrypt(requestsRadDetails.PerYear);
                requestsRadDetails.PerMonth = AESCryptography.Decrypt(requestsRadDetails.PerMonth);
                requestsRadDetails.PerDay = AESCryptography.Decrypt(requestsRadDetails.PerDay);
                requestsRadDetails.PerHour = AESCryptography.Decrypt(requestsRadDetails.PerHour);
                requestsRadDetails.Total = AESCryptography.Decrypt(requestsRadDetails.Total);

                for (int i = 0; i < requestsRadDetails.RequestsRad.Count; i++)
                {
                    requestsRadDetails.RequestsRad[i].Day = AESCryptography.Decrypt(requestsRadDetails.RequestsRad[i].Day);
                    requestsRadDetails.RequestsRad[i].RequestsNumber = AESCryptography.Decrypt(requestsRadDetails.RequestsRad[i].RequestsNumber);
                }

                httpReponse.Response.Result = requestsRadDetails;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token;
                }
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }



        //Reports section
        public static async Task<HttpReponse> GetReportsYearsAsync()
        {
            var analyzesApis = RestService.For<IAnalyzesRepoOps>(
               Common.URL,
               new RefitSettings
               {
                   ContentSerializer = new NewtonsoftJsonContentSerializer()
               });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                   await analyzesApis.GetReportsYearsOpAsync(Common.TOKEN);

                ObservableCollection<string> years =
                    JsonConvert.DeserializeObject<ObservableCollection<string>>(
                        httpReponse.Response.Result.ToString());

                for (int i = 0; i < years.Count; i++)
                {
                    years[i] = AESCryptography.Decrypt(years[i]);
                }

                httpReponse.Response.Result = years;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token;
                }
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> GetReportsMonthsAsync(string year)
        {
            var analyzesApis = RestService.For<IAnalyzesRepoOps>(
               Common.URL,
               new RefitSettings
               {
                   ContentSerializer = new NewtonsoftJsonContentSerializer()
               });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                   await analyzesApis.GetReportsMonthsOpAsync(
                       AESCryptography.Encrypt(year), Common.TOKEN);

                ObservableCollection<string> months =
                    JsonConvert.DeserializeObject<ObservableCollection<string>>(
                        httpReponse.Response.Result.ToString());

                for (int i = 0; i < months.Count; i++)
                {
                    months[i] = AESCryptography.Decrypt(months[i]);
                }

                httpReponse.Response.Result = months;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token;
                }
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> GetReportsDetailsAsync(string year, string month)
        {
            var analyzesApis = RestService.For<IAnalyzesRepoOps>(
               Common.URL,
               new RefitSettings
               {
                   ContentSerializer = new NewtonsoftJsonContentSerializer()
               });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                   await analyzesApis.GetReportsDetailsOpAsync(
                       AESCryptography.Encrypt(year), AESCryptography.Encrypt(month), Common.TOKEN);

                ReportsRadDetails reportsRadDetails =
                    JsonConvert.DeserializeObject<ReportsRadDetails>(httpReponse.Response.Result.ToString());


                reportsRadDetails.PerYear = AESCryptography.Decrypt(reportsRadDetails.PerYear);
                reportsRadDetails.PerMonth = AESCryptography.Decrypt(reportsRadDetails.PerMonth);
                reportsRadDetails.PerDay = AESCryptography.Decrypt(reportsRadDetails.PerDay);
                reportsRadDetails.PerHour = AESCryptography.Decrypt(reportsRadDetails.PerHour);
                reportsRadDetails.Total = AESCryptography.Decrypt(reportsRadDetails.Total);

                for (int i = 0; i < reportsRadDetails.ReportsRad.Count; i++)
                {
                    reportsRadDetails.ReportsRad[i].Day = AESCryptography.Decrypt(reportsRadDetails.ReportsRad[i].Day);
                    reportsRadDetails.ReportsRad[i].ReportsNumber = AESCryptography.Decrypt(reportsRadDetails.ReportsRad[i].ReportsNumber);
                }

                httpReponse.Response.Result = reportsRadDetails;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token;
                }
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

    }
}
