using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Herafi.Core.Models;
using Herafi.Core.Repositories.RepoOperations;
using Herafi.Core.Security;
using Refit;
using Newtonsoft.Json;
using Herafi.Core.Helpers;

namespace Herafi.Core.Repositories.RemoteRepo
{
    public class CraftmenRepo
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


        //General craftmen
        public static async Task<HttpReponse> GetGeneralCraftmenAsync(string pageSize, string offset)
        {
            var craftmenApis = RestService.For<ICraftmenRepoOps>(
                Common.URL,
                new RefitSettings {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                    await craftmenApis.GetGeneralCraftmenOpAsync(
                        AESCryptography.Encrypt(pageSize), AESCryptography.Encrypt(offset), Common.TOKEN);

                ObservableCollection<Craftman> craftmen =
                       JsonConvert.DeserializeObject<ObservableCollection<Craftman>>(
                           httpReponse.Response.Result.ToString());

                for (int i = 0; i < craftmen.Count; i++)
                {
                    craftmen[i].Id = AESCryptography.Decrypt(craftmen[i].Id);
                    craftmen[i].Name = AESCryptography.Decrypt(craftmen[i].Name);
                    craftmen[i].ImagePath = AESCryptography.Decrypt(craftmen[i].ImagePath);
                }

                httpReponse.Response.Result = craftmen;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token;
                }

                //if (string.IsNullOrEmpty(httpReponse.Response.Token))
                //{
                //    ObservableCollection<Craftman> craftmen =
                //        JsonConvert.DeserializeObject<ObservableCollection<Craftman>>(
                //            httpReponse.Response.Result.ToString());

                //    for (int i = 0; i < craftmen.Count; i++)
                //    {
                //        craftmen[i].Id = AESCryptography.Decrypt(craftmen[i].Id);
                //        craftmen[i].Name = AESCryptography.Decrypt(craftmen[i].Name);
                //        craftmen[i].ImagePath = AESCryptography.Decrypt(craftmen[i].ImagePath);
                //    }

                //    httpReponse.Response.Result = craftmen;
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        ObservableCollection<Craftman> craftmen =
                //            JsonConvert.DeserializeObject<ObservableCollection<Craftman>>(
                //                httpReponse.Response.Result.ToString());

                //        for (int i = 0; i < craftmen.Count; i++)
                //        {
                //            craftmen[i].Id = AESCryptography.Decrypt(craftmen[i].Id);
                //            craftmen[i].Name = AESCryptography.Decrypt(craftmen[i].Name);
                //            craftmen[i].ImagePath = AESCryptography.Decrypt(craftmen[i].ImagePath);
                //        }

                //        httpReponse.Response.Result = craftmen;

                //        Common.SUB_TOKEN = httpReponse.Response.Token;
                //    }
                //    else
                //    {
                //        httpReponse.ErrorMessage =
                //            TranslatingUI.MainResourceMap.GetValue("txtToastNotFromServerErr", TranslatingUI.ResourceContextObj).ValueAsString;
                //    }
                //}
            }
            catch(ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> GetCraftmanDetailsProfileAsync(string craftmanId)
        {
            var craftmenApis = RestService.For<ICraftmenRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                    await craftmenApis.GetCraftmanDetailsProfileOpAsync(
                        AESCryptography.Encrypt(craftmanId), Common.TOKEN);

                Profile craftmanProfile =
                       JsonConvert.DeserializeObject<Profile>(
                           httpReponse.Response.Result.ToString());

                craftmanProfile.Id = AESCryptography.Decrypt(craftmanProfile.Id);
                craftmanProfile.Name = AESCryptography.Decrypt(craftmanProfile.Name);
                craftmanProfile.Email = AESCryptography.Decrypt(craftmanProfile.Email);
                craftmanProfile.PhoneNumber = AESCryptography.Decrypt(craftmanProfile.PhoneNumber);
                craftmanProfile.NationalNumber = AESCryptography.Decrypt(craftmanProfile.NationalNumber);
                craftmanProfile.City = AESCryptography.Decrypt(craftmanProfile.City);
                craftmanProfile.DateJoin = AESCryptography.Decrypt(craftmanProfile.DateJoin);
                craftmanProfile.Level = AESCryptography.Decrypt(craftmanProfile.Level);
                craftmanProfile.Status = AESCryptography.Decrypt(craftmanProfile.Status);
                craftmanProfile.BlocksNum = AESCryptography.Decrypt(craftmanProfile.BlocksNum);
                craftmanProfile.CraftsNum = AESCryptography.Decrypt(craftmanProfile.CraftsNum);
                craftmanProfile.CertificationsNum = AESCryptography.Decrypt(craftmanProfile.CertificationsNum);
                craftmanProfile.ProjectsNum = AESCryptography.Decrypt(craftmanProfile.ProjectsNum);
                craftmanProfile.RequestsNum = AESCryptography.Decrypt(craftmanProfile.RequestsNum);
                craftmanProfile.LowestCost = AESCryptography.Decrypt(craftmanProfile.LowestCost);
                craftmanProfile.HighestCost = AESCryptography.Decrypt(craftmanProfile.HighestCost);
                craftmanProfile.UsersSearchs = AESCryptography.Decrypt(craftmanProfile.UsersSearchs);
                craftmanProfile.ProfileImage = AESCryptography.Decrypt(craftmanProfile.ProfileImage);
                craftmanProfile.PersonalIdentityImage = AESCryptography.Decrypt(craftmanProfile.PersonalIdentityImage);
                craftmanProfile.UsersFavourites = AESCryptography.Decrypt(craftmanProfile.UsersFavourites);

                httpReponse.Response.Result = craftmanProfile;

                Common.SUB_TOKEN = !string.IsNullOrWhiteSpace(httpReponse.Response.Token) ?
                       httpReponse.Response.Token : Common.SUB_TOKEN;

                //if (string.IsNullOrEmpty(httpReponse.Response.Token))
                //{
                //    Profile craftmanProfile =
                //        JsonConvert.DeserializeObject<Profile>(
                //            httpReponse.Response.Result.ToString());


                //    craftmanProfile.Id = AESCryptography.Decrypt(craftmanProfile.Id);
                //    craftmanProfile.Name = AESCryptography.Decrypt(craftmanProfile.Name);
                //    craftmanProfile.Email = AESCryptography.Decrypt(craftmanProfile.Email);
                //    craftmanProfile.PhoneNumber = AESCryptography.Decrypt(craftmanProfile.PhoneNumber);
                //    craftmanProfile.NationalNumber = AESCryptography.Decrypt(craftmanProfile.NationalNumber);
                //    craftmanProfile.City = AESCryptography.Decrypt(craftmanProfile.City);
                //    craftmanProfile.DateJoin = AESCryptography.Decrypt(craftmanProfile.DateJoin);
                //    craftmanProfile.Level = AESCryptography.Decrypt(craftmanProfile.Level);
                //    craftmanProfile.Status = AESCryptography.Decrypt(craftmanProfile.Status);
                //    craftmanProfile.BlocksNum = AESCryptography.Decrypt(craftmanProfile.BlocksNum);
                //    craftmanProfile.CraftsNum = AESCryptography.Decrypt(craftmanProfile.CraftsNum);
                //    craftmanProfile.CertificationsNum = AESCryptography.Decrypt(craftmanProfile.CertificationsNum);
                //    craftmanProfile.ProjectsNum = AESCryptography.Decrypt(craftmanProfile.ProjectsNum);
                //    craftmanProfile.RequestsNum = AESCryptography.Decrypt(craftmanProfile.RequestsNum);
                //    craftmanProfile.LowestCost = AESCryptography.Decrypt(craftmanProfile.LowestCost);
                //    craftmanProfile.HighestCost = AESCryptography.Decrypt(craftmanProfile.HighestCost);
                //    craftmanProfile.UsersSearchs = AESCryptography.Decrypt(craftmanProfile.UsersSearchs);
                //    craftmanProfile.ProfileImage = AESCryptography.Decrypt(craftmanProfile.ProfileImage);
                //    craftmanProfile.PersonalIdentityImage = AESCryptography.Decrypt(craftmanProfile.PersonalIdentityImage);
                //    craftmanProfile.UsersFavourites = AESCryptography.Decrypt(craftmanProfile.UsersFavourites);

                //    httpReponse.Response.Result = craftmanProfile;
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        Profile craftmanProfile =
                //            JsonConvert.DeserializeObject<Profile>(
                //                httpReponse.Response.Result.ToString());


                //        craftmanProfile.Id = AESCryptography.Decrypt(craftmanProfile.Id);
                //        craftmanProfile.Name = AESCryptography.Decrypt(craftmanProfile.Name);
                //        craftmanProfile.Email = AESCryptography.Decrypt(craftmanProfile.Email);
                //        craftmanProfile.PhoneNumber = AESCryptography.Decrypt(craftmanProfile.PhoneNumber);
                //        craftmanProfile.NationalNumber = AESCryptography.Decrypt(craftmanProfile.NationalNumber);
                //        craftmanProfile.City = AESCryptography.Decrypt(craftmanProfile.City);
                //        craftmanProfile.DateJoin = AESCryptography.Decrypt(craftmanProfile.DateJoin);
                //        craftmanProfile.Level = AESCryptography.Decrypt(craftmanProfile.Level);
                //        craftmanProfile.Status = AESCryptography.Decrypt(craftmanProfile.Status);
                //        craftmanProfile.BlocksNum = AESCryptography.Decrypt(craftmanProfile.BlocksNum);
                //        craftmanProfile.CraftsNum = AESCryptography.Decrypt(craftmanProfile.CraftsNum);
                //        craftmanProfile.CertificationsNum = AESCryptography.Decrypt(craftmanProfile.CertificationsNum);
                //        craftmanProfile.ProjectsNum = AESCryptography.Decrypt(craftmanProfile.ProjectsNum);
                //        craftmanProfile.RequestsNum = AESCryptography.Decrypt(craftmanProfile.RequestsNum);
                //        craftmanProfile.LowestCost = AESCryptography.Decrypt(craftmanProfile.LowestCost);
                //        craftmanProfile.UsersSearchs = AESCryptography.Decrypt(craftmanProfile.UsersSearchs);
                //        craftmanProfile.ProfileImage = AESCryptography.Decrypt(craftmanProfile.ProfileImage);
                //        craftmanProfile.PersonalIdentityImage = AESCryptography.Decrypt(craftmanProfile.PersonalIdentityImage);
                //        craftmanProfile.UsersFavourites = AESCryptography.Decrypt(craftmanProfile.UsersFavourites);

                //        httpReponse.Response.Result = craftmanProfile;

                //        Common.SUB_TOKEN = httpReponse.Response.Token;
                //    }
                //    else
                //    {
                //        httpReponse.ErrorMessage =
                //            TranslatingUI.MainResourceMap.GetValue("txtToastNotFromServerErr", TranslatingUI.ResourceContextObj).ValueAsString;
                //    }
                //}
            }
            catch(ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> GetCraftmanDetailsCraftsAsync(string craftmanId)
        {
            var craftmenApis =
                    RestService.For<ICraftmenRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse = await craftmenApis.GetCraftmanDetailsCraftsOpAsync(
                    AESCryptography.Encrypt(craftmanId), Common.TOKEN);

                ObservableCollection<Craft> craftmanCrafts =
                    JsonConvert.DeserializeObject<ObservableCollection<Craft>>(
                            httpReponse.Response.Result.ToString());


                for (int i = 0; i < craftmanCrafts.Count; i++)
                {
                    craftmanCrafts[i].Name = AESCryptography.Decrypt(craftmanCrafts[i].Name);

                    for (int j = 0; j < craftmanCrafts[i].Skills.Count; j++)
                    {
                        craftmanCrafts[i].Skills[j] = AESCryptography.Decrypt(craftmanCrafts[i].Skills[j]);
                    }
                }

                httpReponse.Response.Result = craftmanCrafts;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token as string;
                }

                //if (string.IsNullOrEmpty(httpReponse.Response.Token))
                //{
                //    ObservableCollection<Craft> craftmanCrafts =
                //    JsonConvert.DeserializeObject<ObservableCollection<Craft>>(
                //            httpReponse.Response.Result.ToString());


                //    for (int i = 0; i < craftmanCrafts.Count; i++)
                //    {
                //        craftmanCrafts[i].Name = AESCryptography.Decrypt(craftmanCrafts[i].Name);

                //        for (int j = 0; j < craftmanCrafts[i].Skills.Count; j++)
                //        {
                //            craftmanCrafts[i].Skills[j] = AESCryptography.Decrypt(craftmanCrafts[i].Skills[j]);
                //        }
                //    }

                //    httpReponse.Response.Result = craftmanCrafts;
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        ObservableCollection<Craft> craftmanCrafts =
                //    JsonConvert.DeserializeObject<ObservableCollection<Craft>>(
                //            httpReponse.Response.Result.ToString());


                //        for (int i = 0; i < craftmanCrafts.Count; i++)
                //        {
                //            craftmanCrafts[i].Name = AESCryptography.Decrypt(craftmanCrafts[i].Name);

                //            for (int j = 0; j < craftmanCrafts[i].Skills.Count; j++)
                //            {
                //                craftmanCrafts[i].Skills[j] = AESCryptography.Decrypt(craftmanCrafts[i].Skills[j]);
                //            }
                //        }

                //        httpReponse.Response.Result = craftmanCrafts;

                //        Common.SUB_TOKEN = httpReponse.Response.Token;
                //    }
                //    else
                //    {
                //        httpReponse.ErrorMessage =
                //            TranslatingUI.MainResourceMap.GetValue("txtToastNotFromServerErr", TranslatingUI.ResourceContextObj).ValueAsString;
                //    }
                //}
            }
            catch(ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> GetCraftmanDetailsCertificationsAsync(string craftmanId)
        {
            var craftmenApis =
                    RestService.For<ICraftmenRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                    await craftmenApis.GetCraftmanDetailsCertificationsOpAsync(
                        AESCryptography.Encrypt(craftmanId), Common.TOKEN);

                ObservableCollection<string> craftmanCertifications =
                      JsonConvert.DeserializeObject<ObservableCollection<string>>(
                          httpReponse.Response.Result.ToString());


                for (int i = 0; i < craftmanCertifications.Count; i++)
                {
                    craftmanCertifications[i] = AESCryptography.Decrypt(craftmanCertifications[i]);
                }

                httpReponse.Response.Result = craftmanCertifications;


                if (!string.IsNullOrEmpty(httpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token as string;
                }

                //if (string.IsNullOrEmpty(httpReponse.Response.Token))
                //{
                //    ObservableCollection<string> craftmanCertifications =
                //        JsonConvert.DeserializeObject<ObservableCollection<string>>(
                //            httpReponse.Response.Result.ToString());


                //    for (int i = 0; i < craftmanCertifications.Count; i++)
                //    {
                //        craftmanCertifications[i] = AESCryptography.Decrypt(craftmanCertifications[i]);
                //    }

                //    httpReponse.Response.Result = craftmanCertifications;
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        ObservableCollection<string> craftmanCertifications =
                //            JsonConvert.DeserializeObject<ObservableCollection<string>>(
                //                httpReponse.Response.Result.ToString());


                //        for (int i = 0; i < craftmanCertifications.Count; i++)
                //        {
                //            craftmanCertifications[i] = AESCryptography.Decrypt(craftmanCertifications[i]);
                //        }

                //        httpReponse.Response.Result = craftmanCertifications;

                //        Common.SUB_TOKEN = httpReponse.Response.Token;
                //    }
                //    else
                //    {
                //        httpReponse.ErrorMessage =
                //            TranslatingUI.MainResourceMap.GetValue("txtToastNotFromServerErr", TranslatingUI.ResourceContextObj).ValueAsString;
                //    }
                //}
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex); 
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> GetCraftmanDetailsRequestsAsync(string craftmanId)
        {
            var craftmenApis =
                    RestService.For<ICraftmenRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse = await craftmenApis.GetCraftmanDetailsRequestsOpAsync(
                    AESCryptography.Encrypt(craftmanId), Common.TOKEN);


                ObservableCollection<Request> craftmanRequests =
                    JsonConvert.DeserializeObject<ObservableCollection<Request>>(
                        httpReponse.Response.Result.ToString());

                for (int i = 0; i < craftmanRequests.Count; i++)
                {
                    craftmanRequests[i].Comment = AESCryptography.Decrypt(craftmanRequests[i].Comment);
                    craftmanRequests[i].Cost = AESCryptography.Decrypt(craftmanRequests[i].Cost);
                    craftmanRequests[i].StartDate = AESCryptography.Decrypt(craftmanRequests[i].StartDate);
                    craftmanRequests[i].EndDate = AESCryptography.Decrypt(craftmanRequests[i].EndDate);
                    craftmanRequests[i].HoursWork = !string.IsNullOrEmpty(craftmanRequests[i].HoursWork) ? AESCryptography.Decrypt(craftmanRequests[i].HoursWork) : "";
                    craftmanRequests[i].Id = AESCryptography.Decrypt(craftmanRequests[i].Id);
                    craftmanRequests[i].Name = AESCryptography.Decrypt(craftmanRequests[i].Name);
                    craftmanRequests[i].Process = AESCryptography.Decrypt(craftmanRequests[i].Process);
                    craftmanRequests[i].Rating = AESCryptography.Decrypt(craftmanRequests[i].Rating);
                    craftmanRequests[i].Status = AESCryptography.Decrypt(craftmanRequests[i].Status);
                    craftmanRequests[i].Username = AESCryptography.Decrypt(craftmanRequests[i].Username);
                }

                httpReponse.Response.Result = craftmanRequests;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token as string;
                }

                //if (string.IsNullOrEmpty(httpReponse.Response.Token))
                //{
                //    ObservableCollection<Request> craftmanRequests =
                //    JsonConvert.DeserializeObject<ObservableCollection<Request>>(
                //        httpReponse.Response.Result.ToString());


                //    for (int i = 0; i < craftmanRequests.Count; i++)
                //    {
                //        craftmanRequests[i].Comment = AESCryptography.Decrypt(craftmanRequests[i].Comment);
                //        craftmanRequests[i].Cost = AESCryptography.Decrypt(craftmanRequests[i].Cost);
                //        craftmanRequests[i].StartDate = AESCryptography.Decrypt(craftmanRequests[i].StartDate);
                //        craftmanRequests[i].EndDate = AESCryptography.Decrypt(craftmanRequests[i].EndDate);
                //        craftmanRequests[i].HoursWork = !string.IsNullOrEmpty(craftmanRequests[i].HoursWork) ? AESCryptography.Decrypt(craftmanRequests[i].HoursWork) : "";
                //        craftmanRequests[i].Id = AESCryptography.Decrypt(craftmanRequests[i].Id);
                //        craftmanRequests[i].Name = AESCryptography.Decrypt(craftmanRequests[i].Name);
                //        craftmanRequests[i].Process = AESCryptography.Decrypt(craftmanRequests[i].Process);
                //        craftmanRequests[i].Rating = AESCryptography.Decrypt(craftmanRequests[i].Rating);
                //        craftmanRequests[i].Status = AESCryptography.Decrypt(craftmanRequests[i].Status);
                //        craftmanRequests[i].Username = AESCryptography.Decrypt(craftmanRequests[i].Username);
                //    }

                //    httpReponse.Response.Result = craftmanRequests;
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        ObservableCollection<Request> craftmanRequests =
                //    JsonConvert.DeserializeObject<ObservableCollection<Request>>(
                //        httpReponse.Response.Result.ToString());


                //        for (int i = 0; i < craftmanRequests.Count; i++)
                //        {
                //            craftmanRequests[i].Comment = AESCryptography.Decrypt(craftmanRequests[i].Comment);
                //            craftmanRequests[i].Cost = AESCryptography.Decrypt(craftmanRequests[i].Cost);
                //            craftmanRequests[i].StartDate = AESCryptography.Decrypt(craftmanRequests[i].StartDate);
                //            craftmanRequests[i].EndDate = AESCryptography.Decrypt(craftmanRequests[i].EndDate);
                //            craftmanRequests[i].HoursWork = !string.IsNullOrEmpty(craftmanRequests[i].HoursWork) ? AESCryptography.Decrypt(craftmanRequests[i].HoursWork) : "";
                //            craftmanRequests[i].Id = AESCryptography.Decrypt(craftmanRequests[i].Id);
                //            craftmanRequests[i].Name = AESCryptography.Decrypt(craftmanRequests[i].Name);
                //            craftmanRequests[i].Process = AESCryptography.Decrypt(craftmanRequests[i].Process);
                //            craftmanRequests[i].Rating = AESCryptography.Decrypt(craftmanRequests[i].Rating);
                //            craftmanRequests[i].Status = AESCryptography.Decrypt(craftmanRequests[i].Status);
                //            craftmanRequests[i].Username = AESCryptography.Decrypt(craftmanRequests[i].Username);
                //        }

                //        httpReponse.Response.Result = craftmanRequests;

                //        Common.SUB_TOKEN = httpReponse.Response.Token;
                //    }
                //    else
                //    {
                //        httpReponse.ErrorMessage =
                //            TranslatingUI.MainResourceMap.GetValue("txtToastNotFromServerErr", TranslatingUI.ResourceContextObj).ValueAsString;
                //    }
                //}
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage (httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> GetCraftmanDetailsProjectsAsync(string craftmanId)
        {
            var craftmenApis = RestService.For<ICraftmenRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse = await craftmenApis.GetCraftmanDetailsProjectsOpAsync(
                   AESCryptography.Encrypt(craftmanId), Common.TOKEN);


                ObservableCollection<Project> craftmanProjects =
                  JsonConvert.DeserializeObject<ObservableCollection<Project>>(
                      httpReponse.Response.Result.ToString());


                for (int i = 0; i < craftmanProjects.Count; i++)
                {
                    craftmanProjects[i].Comment = AESCryptography.Decrypt(craftmanProjects[i].Comment);
                    craftmanProjects[i].Cost = AESCryptography.Decrypt(craftmanProjects[i].Cost);
                    craftmanProjects[i].StartDate = AESCryptography.Decrypt(craftmanProjects[i].StartDate);
                    craftmanProjects[i].EndDate = AESCryptography.Decrypt(craftmanProjects[i].EndDate);
                    craftmanProjects[i].HoursWork = !string.IsNullOrEmpty(craftmanProjects[i].HoursWork) ? AESCryptography.Decrypt(craftmanProjects[i].HoursWork) : "";
                    craftmanProjects[i].Id = AESCryptography.Decrypt(craftmanProjects[i].Id);
                    craftmanProjects[i].Name = AESCryptography.Decrypt(craftmanProjects[i].Name);
                    craftmanProjects[i].Process = AESCryptography.Decrypt(craftmanProjects[i].Process);
                    craftmanProjects[i].Rating = AESCryptography.Decrypt(craftmanProjects[i].Rating);
                    craftmanProjects[i].Status = AESCryptography.Decrypt(craftmanProjects[i].Status);
                    craftmanProjects[i].Username = AESCryptography.Decrypt(craftmanProjects[i].Username);

                    for (int j = 0; j < craftmanProjects[i].ProjectImages.Count; j++)
                    {
                        craftmanProjects[i].ProjectImages[j] = AESCryptography.Decrypt(craftmanProjects[i].ProjectImages[j]);
                    }
                }

                httpReponse.Response.Result = craftmanProjects;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token as string;
                }


                //if (string.IsNullOrEmpty(httpReponse.Response.Token))
                //{
                //    ObservableCollection<Project> craftmanProjects =
                //   JsonConvert.DeserializeObject<ObservableCollection<Project>>(
                //       httpReponse.Response.Result.ToString());


                //    for (int i = 0; i < craftmanProjects.Count; i++)
                //    {
                //        craftmanProjects[i].Comment = AESCryptography.Decrypt(craftmanProjects[i].Comment);
                //        craftmanProjects[i].Cost = AESCryptography.Decrypt(craftmanProjects[i].Cost);
                //        craftmanProjects[i].StartDate = AESCryptography.Decrypt(craftmanProjects[i].StartDate);
                //        craftmanProjects[i].EndDate = AESCryptography.Decrypt(craftmanProjects[i].EndDate);
                //        craftmanProjects[i].HoursWork = !string.IsNullOrEmpty(craftmanProjects[i].HoursWork) ? AESCryptography.Decrypt(craftmanProjects[i].HoursWork) : "";
                //        craftmanProjects[i].Id = AESCryptography.Decrypt(craftmanProjects[i].Id);
                //        craftmanProjects[i].Name = AESCryptography.Decrypt(craftmanProjects[i].Name);
                //        craftmanProjects[i].Process = AESCryptography.Decrypt(craftmanProjects[i].Process);
                //        craftmanProjects[i].Rating = AESCryptography.Decrypt(craftmanProjects[i].Rating);
                //        craftmanProjects[i].Status = AESCryptography.Decrypt(craftmanProjects[i].Status);
                //        craftmanProjects[i].Username = AESCryptography.Decrypt(craftmanProjects[i].Username);

                //        for (int j = 0; j < craftmanProjects[i].ProjectImages.Count; j++)
                //        {
                //            craftmanProjects[i].ProjectImages[j] = AESCryptography.Decrypt(craftmanProjects[i].ProjectImages[j]);
                //        }
                //    }

                //    httpReponse.Response.Result = craftmanProjects;
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        ObservableCollection<Project> craftmanProjects =
                //   JsonConvert.DeserializeObject<ObservableCollection<Project>>(
                //       httpReponse.Response.Result.ToString());


                //        for (int i = 0; i < craftmanProjects.Count; i++)
                //        {
                //            craftmanProjects[i].Comment = AESCryptography.Decrypt(craftmanProjects[i].Comment);
                //            craftmanProjects[i].Cost = AESCryptography.Decrypt(craftmanProjects[i].Cost);
                //            craftmanProjects[i].StartDate = AESCryptography.Decrypt(craftmanProjects[i].StartDate);
                //            craftmanProjects[i].EndDate = AESCryptography.Decrypt(craftmanProjects[i].EndDate);
                //            craftmanProjects[i].HoursWork = AESCryptography.Decrypt(craftmanProjects[i].HoursWork);
                //            craftmanProjects[i].Id = AESCryptography.Decrypt(craftmanProjects[i].Id);
                //            craftmanProjects[i].Name = AESCryptography.Decrypt(craftmanProjects[i].Name);
                //            craftmanProjects[i].Process = AESCryptography.Decrypt(craftmanProjects[i].Process);
                //            craftmanProjects[i].Rating = AESCryptography.Decrypt(craftmanProjects[i].Rating);
                //            craftmanProjects[i].Status = AESCryptography.Decrypt(craftmanProjects[i].Status);
                //            craftmanProjects[i].Username = AESCryptography.Decrypt(craftmanProjects[i].Username);

                //            for (int j = 0; j < craftmanProjects[i].ProjectImages.Count; j++)
                //            {
                //                craftmanProjects[i].ProjectImages[j] = AESCryptography.Decrypt(craftmanProjects[i].ProjectImages[j]);
                //            }
                //        }

                //        httpReponse.Response.Result = craftmanProjects;

                //        Common.SUB_TOKEN = httpReponse.Response.Token;
                //    }
                //    else
                //    {
                //        httpReponse.ErrorMessage =
                //            TranslatingUI.MainResourceMap.GetValue("txtToastNotFromServerErr", TranslatingUI.ResourceContextObj).ValueAsString;
                //    }
                //}
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }




        //New members craftmen
        private async static Task<HttpReponse> GetNewMemberCraftman(ICraftmenRepoOps craftmenApis, string craftmanId)
        {
            craftmenApis = RestService.For<ICraftmenRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;
            
            try
            {
                 httpReponse =
                    await craftmenApis.GetNewMemberCraftmanOpAsync(craftmanId, Common.TOKEN);

                NewMemberCraftman newMemCraftman =
                       JsonConvert.DeserializeObject<NewMemberCraftman>(
                           httpReponse.Response.Result.ToString());

                newMemCraftman.Profile.ProfileImage = AESCryptography.Decrypt(newMemCraftman.Profile.ProfileImage);
                newMemCraftman.Profile.Name = AESCryptography.Decrypt(newMemCraftman.Profile.Name);
                newMemCraftman.Profile.PersonalIdentityImage = AESCryptography.Decrypt(newMemCraftman.Profile.PersonalIdentityImage);
                newMemCraftman.Profile.Id = AESCryptography.Decrypt(newMemCraftman.Profile.Id);
                newMemCraftman.Profile.Email = AESCryptography.Decrypt(newMemCraftman.Profile.Email);
                newMemCraftman.Profile.PhoneNumber = AESCryptography.Decrypt(newMemCraftman.Profile.PhoneNumber);
                newMemCraftman.Profile.NationalNumber = AESCryptography.Decrypt(newMemCraftman.Profile.NationalNumber);
                newMemCraftman.Profile.DateJoin = AESCryptography.Decrypt(newMemCraftman.Profile.DateJoin);
                newMemCraftman.Profile.City = AESCryptography.Decrypt(newMemCraftman.Profile.City);
                newMemCraftman.Profile.LowestCost = AESCryptography.Decrypt(newMemCraftman.Profile.LowestCost);
                newMemCraftman.Profile.HighestCost = AESCryptography.Decrypt(newMemCraftman.Profile.HighestCost);
                newMemCraftman.Profile.CraftsNum = AESCryptography.Decrypt(newMemCraftman.Profile.CraftsNum);
                newMemCraftman.Profile.CertificationsNum = AESCryptography.Decrypt(newMemCraftman.Profile.CertificationsNum);


                for (int j = 0; j < newMemCraftman.Crafts.Count; j++)
                {
                    newMemCraftman.Crafts[j].Name = AESCryptography.Decrypt(newMemCraftman.Crafts[j].Name);

                    for (int k = 0; k < newMemCraftman.Crafts[j].Skills.Count; k++)
                    {
                        newMemCraftman.Crafts[j].Skills[k] = AESCryptography.Decrypt(newMemCraftman.Crafts[j].Skills[k]);
                    }
                }

                for (int j = 0; j < newMemCraftman.Certifications.Count; j++)
                {
                    newMemCraftman.Certifications[j] = AESCryptography.Decrypt(newMemCraftman.Certifications[j]);
                }

                httpReponse.Response.Result = newMemCraftman;


                if (!string.IsNullOrEmpty(httpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token as string;
                }

                //if (string.IsNullOrEmpty(httpReponse.Response.Token))
                //{
                //    NewMemberCraftman newMemCraftman =
                //        JsonConvert.DeserializeObject<NewMemberCraftman>(
                //            httpReponse.Response.Result.ToString());

                //    newMemCraftman.Profile.ProfileImage = AESCryptography.Decrypt(newMemCraftman.Profile.ProfileImage);
                //    newMemCraftman.Profile.Name = AESCryptography.Decrypt(newMemCraftman.Profile.Name);
                //    newMemCraftman.Profile.PersonalIdentityImage = AESCryptography.Decrypt(newMemCraftman.Profile.PersonalIdentityImage);
                //    newMemCraftman.Profile.Id = AESCryptography.Decrypt(newMemCraftman.Profile.Id);
                //    newMemCraftman.Profile.Email = AESCryptography.Decrypt(newMemCraftman.Profile.Email);
                //    newMemCraftman.Profile.PhoneNumber = AESCryptography.Decrypt(newMemCraftman.Profile.PhoneNumber);
                //    newMemCraftman.Profile.NationalNumber = AESCryptography.Decrypt(newMemCraftman.Profile.DateJoin);
                //    newMemCraftman.Profile.DateJoin = AESCryptography.Decrypt(newMemCraftman.Profile.DateJoin);
                //    newMemCraftman.Profile.City = AESCryptography.Decrypt(newMemCraftman.Profile.City);
                //    newMemCraftman.Profile.LowestCost = AESCryptography.Decrypt(newMemCraftman.Profile.LowestCost);
                //    newMemCraftman.Profile.HighestCost = AESCryptography.Decrypt(newMemCraftman.Profile.HighestCost);


                //    for (int j = 0; j < newMemCraftman.Crafts.Count; j++)
                //    {
                //        newMemCraftman.Crafts[j].Name = AESCryptography.Decrypt(newMemCraftman.Crafts[j].Name);

                //        for (int k = 0; k < newMemCraftman.Crafts[j].Skills.Count; k++)
                //        {
                //            newMemCraftman.Crafts[j].Skills[k] = AESCryptography.Decrypt(newMemCraftman.Crafts[j].Skills[k]);
                //        }
                //    }

                //    for (int j = 0; j < newMemCraftman.Certifications.Count; j++)
                //    {
                //        newMemCraftman.Certifications[j] = AESCryptography.Decrypt(newMemCraftman.Certifications[j]);
                //    }

                //    httpReponse.Response.Result = newMemCraftman;
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //       JsonConvert.DeserializeObject<Token>(
                //       JWTAuthorization.Decode(httpReponse.Response.Token, Common.JWT_PUBLIC_KEY)).
                //       SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        NewMemberCraftman newMemCraftman =
                //        JsonConvert.DeserializeObject<NewMemberCraftman>(
                //            httpReponse.Response.Result.ToString());

                //        newMemCraftman.Profile.ProfileImage = AESCryptography.Decrypt(newMemCraftman.Profile.ProfileImage);
                //        newMemCraftman.Profile.Name = AESCryptography.Decrypt(newMemCraftman.Profile.Name);
                //        newMemCraftman.Profile.PersonalIdentityImage = AESCryptography.Decrypt(newMemCraftman.Profile.PersonalIdentityImage);
                //        newMemCraftman.Profile.Id = AESCryptography.Decrypt(newMemCraftman.Profile.Id);
                //        newMemCraftman.Profile.Email = AESCryptography.Decrypt(newMemCraftman.Profile.Email);
                //        newMemCraftman.Profile.PhoneNumber = AESCryptography.Decrypt(newMemCraftman.Profile.PhoneNumber);
                //        newMemCraftman.Profile.NationalNumber = AESCryptography.Decrypt(newMemCraftman.Profile.DateJoin);
                //        newMemCraftman.Profile.DateJoin = AESCryptography.Decrypt(newMemCraftman.Profile.DateJoin);
                //        newMemCraftman.Profile.City = AESCryptography.Decrypt(newMemCraftman.Profile.City);
                //        newMemCraftman.Profile.LowestCost = AESCryptography.Decrypt(newMemCraftman.Profile.LowestCost);
                //        newMemCraftman.Profile.HighestCost = AESCryptography.Decrypt(newMemCraftman.Profile.HighestCost);


                //        for (int j = 0; j < newMemCraftman.Crafts.Count; j++)
                //        {
                //            newMemCraftman.Crafts[j].Name = AESCryptography.Decrypt(newMemCraftman.Crafts[j].Name);

                //            for (int k = 0; k < newMemCraftman.Crafts[j].Skills.Count; k++)
                //            {
                //                newMemCraftman.Crafts[j].Skills[k] = AESCryptography.Decrypt(newMemCraftman.Crafts[j].Skills[k]);
                //            }
                //        }

                //        for (int j = 0; j < newMemCraftman.Certifications.Count; j++)
                //        {
                //            newMemCraftman.Certifications[j] = AESCryptography.Decrypt(newMemCraftman.Certifications[j]);
                //        }

                //        httpReponse.Response.Result = newMemCraftman;

                //        Common.SUB_TOKEN = httpReponse.Response.Token;

                //    }
                //    else
                //    {
                //        httpReponse.ErrorMessage =
                //            TranslatingUI.MainResourceMap.GetValue("txtToastNotFromServerErr", TranslatingUI.ResourceContextObj).ValueAsString;
                //    }
                //}
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }
        public static async Task<HttpReponse> GetNewMembersCraftmenAsync(string pageSize, string offset)
        {
            var craftmenApis = RestService.For<ICraftmenRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

           try
            {
                 httpReponse =
                    await craftmenApis.GetNewMembersCraftmenIdsOpAsync(
                        AESCryptography.Encrypt(pageSize), AESCryptography.Encrypt(offset), Common.TOKEN);

                ObservableCollection<string> craftmenIds =
                        JsonConvert.DeserializeObject<ObservableCollection<string>>(
                            httpReponse.Response.Result.ToString());

                //Decryption
                for (int i = 0; i < craftmenIds.Count; i++)
                {
                    craftmenIds[i] = AESCryptography.Decrypt(craftmenIds[i]);
                }

                ObservableCollection<NewMemberCraftman> newMemCraftmen =
                        new ObservableCollection<NewMemberCraftman>();

                for (int i = 0; i < craftmenIds.Count; i++)
                {
                    httpReponse = await GetNewMemberCraftman(craftmenApis, AESCryptography.Encrypt(craftmenIds[i]));

                    if (!string.IsNullOrEmpty(httpReponse.ErrorMessage))
                    {
                        newMemCraftmen = null;
                        break;
                    }

                    newMemCraftmen.Add(httpReponse.Response.Result as NewMemberCraftman);
                }


                //This process for make medium radio button UnEnabled if this craftman doesn't have any certifications
                for (int i = 0; i < newMemCraftmen.Count; i++)
                {
                    if (newMemCraftmen[i].Certifications.Count == 0)
                    {
                        newMemCraftmen[i].IsThereCertifications = false;
                    }
                    else
                    {
                        newMemCraftmen[i].IsThereCertifications = true;
                    }
                }

                httpReponse.Response.Result = newMemCraftmen;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token as string;
                }

                //if (string.IsNullOrEmpty(httpReponse.Response.Token))
                //{
                //    ObservableCollection<string> craftmenIds =
                //        JsonConvert.DeserializeObject<ObservableCollection<string>>(
                //            httpReponse.Response.Result.ToString());

                //    //Decryption
                //    for (int i = 0; i < craftmenIds.Count; i++)
                //    {
                //        craftmenIds[i] = AESCryptography.Decrypt(craftmenIds[i]);
                //    }


                //    ObservableCollection<NewMemberCraftman> newMemCraftmen =
                //            new ObservableCollection<NewMemberCraftman>();

                //    for (int i = 0; i < craftmenIds.Count; i++)
                //    {
                //        httpReponse = await GetNewMemberCraftman(craftmenApis, craftmenIds[i]);

                //        if (httpReponse.ErrorMessage != null)
                //        {
                //            newMemCraftmen = null;
                //            break;
                //        }

                //        newMemCraftmen.Add(httpReponse.Response.Result as NewMemberCraftman);
                //    }

                //    httpReponse.Response.Result = newMemCraftmen;
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        ObservableCollection<string> craftmenIds =
                //         JsonConvert.DeserializeObject<ObservableCollection<string>>(
                //             httpReponse.Response.Result.ToString());

                //        //Decryption
                //        for (int i = 0; i < craftmenIds.Count; i++)
                //        {
                //            craftmenIds[i] = AESCryptography.Decrypt(craftmenIds[i]);
                //        }


                //        ObservableCollection<NewMemberCraftman> newMemCraftmen =
                //                new ObservableCollection<NewMemberCraftman>();

                //        for (int i = 0; i < craftmenIds.Count; i++)
                //        {
                //            httpReponse = await GetNewMemberCraftman(craftmenApis, craftmenIds[i]);

                //            if (httpReponse.ErrorMessage != null)
                //            {
                //                newMemCraftmen = null;
                //                break;
                //            }

                //            newMemCraftmen.Add(httpReponse.Response.Result as NewMemberCraftman);
                //        }

                //        httpReponse.Response.Result = newMemCraftmen;

                //        Common.SUB_TOKEN = httpReponse.Response.Token;
                //    }
                //    else
                //    {
                //        httpReponse.ErrorMessage =
                //               TranslatingUI.MainResourceMap.GetValue("txtToastNotFromServerErr", TranslatingUI.ResourceContextObj).ValueAsString;
                //    }
                //}
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }


        public static async Task<HttpReponse> AcceptNewMemberCraftmanAsync(string craftmanId, string level)
        {
            var craftmenApis = RestService.For<ICraftmenRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                    await craftmenApis.AcceptNewMemberCraftmanOpAsync(
                        AESCryptography.Encrypt(craftmanId), AESCryptography.Encrypt(level), Common.TOKEN);

                httpReponse.Response.Result = 
                    AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

                if (!string.IsNullOrEmpty(httpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token as string;
                }
                //if (string.IsNullOrEmpty(httpReponse.Response.Token))
                //{
                //    string result =
                //        JsonConvert.DeserializeObject<string>(httpReponse.Response.Result.ToString());

                //    httpReponse.Response.Result = AESCryptography.Decrypt(result);
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        string result =
                //            JsonConvert.DeserializeObject<string>(httpReponse.Response.Result.ToString());

                //        httpReponse.Response.Result = AESCryptography.Decrypt(result);

                //        Common.SUB_TOKEN = httpReponse.Response.Token;
                //    }
                //    else
                //    {
                //        httpReponse.ErrorMessage =
                //            TranslatingUI.MainResourceMap.GetValue("txtToastNotFromServerErr", TranslatingUI.ResourceContextObj).ValueAsString;
                //    }
                //}
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> RefuseNewMemberCraftmanAsync(string craftmanId)
        {
            var craftmenApis = RestService.For<ICraftmenRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                   await craftmenApis.RefuseNewMemberCraftmanOpAsync(
                       AESCryptography.Encrypt(craftmanId), Common.TOKEN);


                httpReponse.Response.Result = AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

                if (!string.IsNullOrEmpty(httpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token as string;
                }
                //if (string.IsNullOrEmpty(httpReponse.Response.Token))
                //{
                //    string result =
                //        JsonConvert.DeserializeObject<string>(httpReponse.Response.Result.ToString());

                //    httpReponse.Response.Result = AESCryptography.Decrypt(result);
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        string result =
                //            JsonConvert.DeserializeObject<string>(httpReponse.Response.Result.ToString());

                //        httpReponse.Response.Result = AESCryptography.Decrypt(result);

                //        Common.SUB_TOKEN = httpReponse.Response.Token;
                //    }
                //    else
                //    {
                //        httpReponse.ErrorMessage =
                //            TranslatingUI.MainResourceMap.GetValue("txtToastNotFromServerErr", TranslatingUI.ResourceContextObj).ValueAsString;
                //    }
                //}
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }




        //Reported craftmen (blocking and firing)
        public static async Task<HttpReponse> GetBlockingsFiringsCraftmenNumberAsync()
        {
            var craftmenApis = RestService.For<ICraftmenRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                }); ;

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                    await craftmenApis.GetBlockingsFiringsCraftmenNumberOpAsync(Common.TOKEN);

                ReportedNumbers reportedCraftmen =
                       JsonConvert.DeserializeObject<ReportedNumbers>(
                           httpReponse.Response.Result.ToString());

                reportedCraftmen.BlockingNumber = AESCryptography.Decrypt(reportedCraftmen.BlockingNumber);
                reportedCraftmen.FiringNumber = AESCryptography.Decrypt(reportedCraftmen.FiringNumber);

                httpReponse.Response.Result = reportedCraftmen;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token as string;
                }
                //if (string.IsNullOrEmpty(httpReponse.Response.Token))
                //{
                //    ReportedNumbers reportedCraftmen =
                //        JsonConvert.DeserializeObject<ReportedNumbers>(
                //            httpReponse.Response.Result.ToString());

                //    reportedCraftmen.BlockingNumber = AESCryptography.Decrypt(reportedCraftmen.BlockingNumber);
                //    reportedCraftmen.FiringNumber = AESCryptography.Decrypt(reportedCraftmen.FiringNumber);

                //    httpReponse.Response.Result = reportedCraftmen;
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        ReportedNumbers reportedCraftmen =
                //        JsonConvert.DeserializeObject<ReportedNumbers>(
                //            httpReponse.Response.Result.ToString());

                //        reportedCraftmen.BlockingNumber = AESCryptography.Decrypt(reportedCraftmen.BlockingNumber);
                //        reportedCraftmen.FiringNumber = AESCryptography.Decrypt(reportedCraftmen.FiringNumber);

                //        httpReponse.Response.Result = reportedCraftmen;

                //        Common.SUB_TOKEN = httpReponse.Response.Token;
                //    }
                //    else
                //    {
                //        httpReponse.ErrorMessage =
                //            TranslatingUI.MainResourceMap.GetValue("txtToastNotFromServerErr", TranslatingUI.ResourceContextObj).ValueAsString;
                //    }
                //}
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        private static async Task<HttpReponse> GetReportedBlockingCraftman(ICraftmenRepoOps craftmenApis, string craftmanId)
        {
            craftmenApis = RestService.For<ICraftmenRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                   await craftmenApis.GetReportedCraftmanOpAsync(craftmanId, Common.TOKEN);

                ReportCraftman reportCraftman =
                        JsonConvert.DeserializeObject<ReportCraftman>(
                            httpReponse.Response.Result.ToString());

                reportCraftman.Id = AESCryptography.Decrypt(reportCraftman.Id);
                reportCraftman.Name = AESCryptography.Decrypt(reportCraftman.Name);
                reportCraftman.ProfileImage = AESCryptography.Decrypt(reportCraftman.ProfileImage);

                for (int j = 0; j < reportCraftman.Reports.Count; j++)
                {
                    reportCraftman.Reports[j].Comment = AESCryptography.Decrypt(reportCraftman.Reports[j].Comment);
                    reportCraftman.Reports[j].UserName = AESCryptography.Decrypt(reportCraftman.Reports[j].UserName);
                    reportCraftman.Reports[j].RequestId = AESCryptography.Decrypt(reportCraftman.Reports[j].RequestId);
                    reportCraftman.Reports[j].Number = AESCryptography.Decrypt(reportCraftman.Reports[j].Number);

                    for (int k = 0; k < reportCraftman.Reports[j].Problems.Count; k++)
                    {
                        reportCraftman.Reports[j].Problems[k] = AESCryptography.Decrypt(reportCraftman.Reports[j].Problems[k]);
                    }
                }

                httpReponse.Response.Result = reportCraftman;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token as string;
                }

                //if (string.IsNullOrEmpty(httpReponse.Response.Token))
                //{
                //    ReportCraftman reportCraftman =
                //        JsonConvert.DeserializeObject<ReportCraftman>(
                //            httpReponse.Response.Result.ToString());

                //    reportCraftman.Id = AESCryptography.Decrypt(reportCraftman.Id);
                //    reportCraftman.Name = AESCryptography.Decrypt(reportCraftman.Name);
                //    reportCraftman.ProfileImage = AESCryptography.Decrypt(reportCraftman.ProfileImage);

                //    for (int j = 0; j < reportCraftman.Reports.Count; j++)
                //    {
                //        reportCraftman.Reports[j].Comment = AESCryptography.Decrypt(reportCraftman.Reports[j].Comment);
                //        reportCraftman.Reports[j].UserName = AESCryptography.Decrypt(reportCraftman.Reports[j].UserName);
                //        reportCraftman.Reports[j].RequestId = AESCryptography.Decrypt(reportCraftman.Reports[j].Comment);
                //        reportCraftman.Reports[j].Number = AESCryptography.Decrypt(reportCraftman.Reports[j].Comment);

                //        for (int k = 0; k < reportCraftman.Reports[j].Problems.Count; k++)
                //        {
                //            reportCraftman.Reports[j].Problems[k] = AESCryptography.Decrypt(reportCraftman.Reports[j].Problems[k]);
                //        }
                //    }

                //    httpReponse.Response.Result = reportCraftman;
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //       JsonConvert.DeserializeObject<Token>(
                //       JWTAuthorization.Decode(httpReponse.Response.Token, Common.JWT_PUBLIC_KEY)).
                //       SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        ReportCraftman reportCraftman =
                //        JsonConvert.DeserializeObject<ReportCraftman>(
                //            httpReponse.Response.Result.ToString());

                //        reportCraftman.Id = AESCryptography.Decrypt(reportCraftman.Id);
                //        reportCraftman.Name = AESCryptography.Decrypt(reportCraftman.Name);
                //        reportCraftman.ProfileImage = AESCryptography.Decrypt(reportCraftman.ProfileImage);

                //        for (int j = 0; j < reportCraftman.Reports.Count; j++)
                //        {
                //            reportCraftman.Reports[j].Comment = AESCryptography.Decrypt(reportCraftman.Reports[j].Comment);
                //            reportCraftman.Reports[j].UserName = AESCryptography.Decrypt(reportCraftman.Reports[j].UserName);
                //            reportCraftman.Reports[j].RequestId = AESCryptography.Decrypt(reportCraftman.Reports[j].Comment);
                //            reportCraftman.Reports[j].Number = AESCryptography.Decrypt(reportCraftman.Reports[j].Comment);

                //            for (int k = 0; k < reportCraftman.Reports[j].Problems.Count; k++)
                //            {
                //                reportCraftman.Reports[j].Problems[k] = AESCryptography.Decrypt(reportCraftman.Reports[j].Problems[k]);
                //            }
                //        }

                //        httpReponse.Response.Result = reportCraftman;

                //        Common.SUB_TOKEN = httpReponse.Response.Token;
                //    }
                //    else
                //    {
                //        httpReponse.ErrorMessage =
                //            TranslatingUI.MainResourceMap.GetValue("txtToastNotFromServerErr", TranslatingUI.ResourceContextObj).ValueAsString;
                //    }
                //}
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }
        public static async Task<HttpReponse> GetReportedBlockingCraftmenAsync(string pageSize, string offset)
        {
            var craftmenApis = RestService.For<ICraftmenRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });


            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                   await craftmenApis.GetReportedBlockingCraftmenIdsOpAsync(
                       AESCryptography.Encrypt(pageSize), AESCryptography.Encrypt(offset), Common.TOKEN);

                ObservableCollection<string> blockingCraftmenIds =
                    JsonConvert.DeserializeObject<ObservableCollection<string>>(
                        httpReponse.Response.Result.ToString());

                ObservableCollection<ReportCraftman> reportCraftmen = new
                    ObservableCollection<ReportCraftman>();


                for (int i = 0; i < blockingCraftmenIds.Count; i++)
                {
                    httpReponse = await GetReportedBlockingCraftman(craftmenApis, blockingCraftmenIds[i].ToString());

                    if (httpReponse.ErrorMessage != "")
                    {
                        reportCraftmen = null;
                        break;
                    }

                    reportCraftmen.Add(httpReponse.Response.Result as ReportCraftman);
                }

                httpReponse.Response.Result = reportCraftmen;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token as string;
                }
                //if (string.IsNullOrEmpty(httpReponse.Response.Token))
                //{
                //    ObservableCollection<string> blockingCraftmenIds =
                //    JsonConvert.DeserializeObject<ObservableCollection<string>>(
                //        httpReponse.Response.Result.ToString());

                //    ObservableCollection<ReportCraftman> reportCraftmen = new
                //        ObservableCollection<ReportCraftman>();


                //    for (int i = 0; i < blockingCraftmenIds.Count; i++)
                //    {
                //        httpReponse = await GetReportedBlockingCraftman(craftmenApis, blockingCraftmenIds[i]);

                //        if (httpReponse.ErrorMessage != "")
                //        {
                //            reportCraftmen = null;
                //            break;
                //        }

                //        reportCraftmen.Add(httpReponse.Response.Result as ReportCraftman);
                //    }

                //    httpReponse.Response.Result = reportCraftmen;
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        ObservableCollection<string> blockingCraftmenIds =
                //            JsonConvert.DeserializeObject<ObservableCollection<string>>(
                //                httpReponse.Response.Result.ToString());

                //        ObservableCollection<ReportCraftman> reportCraftmen = new
                //            ObservableCollection<ReportCraftman>();


                //        for (int i = 0; i < blockingCraftmenIds.Count; i++)
                //        {
                //            httpReponse = await GetReportedBlockingCraftman(craftmenApis, blockingCraftmenIds[i]);

                //            if (httpReponse.ErrorMessage != "")
                //            {
                //                reportCraftmen = null;
                //                break;
                //            }

                //            reportCraftmen.Add(httpReponse.Response.Result as ReportCraftman);
                //        }

                //        httpReponse.Response.Result = reportCraftmen;

                //        Common.SUB_TOKEN = httpReponse.Response.Token;
                //    }
                //    else
                //    {
                //        httpReponse.ErrorMessage =
                //            TranslatingUI.MainResourceMap.GetValue("txtToastNotFromServerErr", TranslatingUI.ResourceContextObj).ValueAsString;
                //    }
                //}
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }
        public static async Task<HttpReponse> GetReportedFiringCraftmenAsync(string pageSize, string offset)
        {
            var craftmenApis = RestService.For<ICraftmenRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                    await craftmenApis.GetReportedFiringCraftmenIdsOpAsync(
                        AESCryptography.Encrypt(pageSize), AESCryptography.Encrypt(offset), Common.TOKEN);

                ObservableCollection<string> firingCraftmenIds =
                    JsonConvert.DeserializeObject<ObservableCollection<string>>(
                        httpReponse.Response.Result.ToString());

                ObservableCollection<ReportCraftman> reportCraftmen = new
                    ObservableCollection<ReportCraftman>();


                for (int i = 0; i < firingCraftmenIds.Count; i++)
                {
                    httpReponse = await GetReportedBlockingCraftman(craftmenApis, firingCraftmenIds[i]);

                    if (httpReponse.ErrorMessage != "")
                    {
                        reportCraftmen = null;
                        break;
                    }

                    reportCraftmen.Add(httpReponse.Response.Result as ReportCraftman);
                }

                httpReponse.Response.Result = reportCraftmen;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token as string;
                }

                //if (string.IsNullOrEmpty(httpReponse.Response.Token))
                //{
                //    ObservableCollection<string> blockingCraftmenIds =
                //    JsonConvert.DeserializeObject<ObservableCollection<string>>(
                //        httpReponse.Response.Result.ToString());

                //    ObservableCollection<ReportCraftman> reportCraftmen = new
                //        ObservableCollection<ReportCraftman>();


                //    for (int i = 0; i < blockingCraftmenIds.Count; i++)
                //    {
                //        httpReponse = await GetReportedBlockingCraftman(craftmenApis, blockingCraftmenIds[i]);

                //        if (httpReponse.ErrorMessage != "")
                //        {
                //            reportCraftmen = null;
                //            break;
                //        }

                //        reportCraftmen.Add(httpReponse.Response.Result as ReportCraftman);
                //    }

                //    httpReponse.Response.Result = reportCraftmen;
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        ObservableCollection<string> blockingCraftmenIds =
                //            JsonConvert.DeserializeObject<ObservableCollection<string>>(
                //                httpReponse.Response.Result.ToString());

                //        ObservableCollection<ReportCraftman> reportCraftmen = new
                //            ObservableCollection<ReportCraftman>();


                //        for (int i = 0; i < blockingCraftmenIds.Count; i++)
                //        {
                //            httpReponse = await GetReportedBlockingCraftman(craftmenApis, blockingCraftmenIds[i]);

                //            if (httpReponse.ErrorMessage != "")
                //            {
                //                reportCraftmen = null;
                //                break;
                //            }

                //            reportCraftmen.Add(httpReponse.Response.Result as ReportCraftman);
                //        }

                //        httpReponse.Response.Result = reportCraftmen;

                //        Common.SUB_TOKEN = httpReponse.Response.Token;
                //    }
                //    else
                //    {
                //        httpReponse.ErrorMessage =
                //            TranslatingUI.MainResourceMap.GetValue("txtToastNotFromServerErr", TranslatingUI.ResourceContextObj).ValueAsString;
                //    }
                //}
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> BlockingCraftmanAsync(string craftmanId)
        {
            var craftmenApis = RestService.For<ICraftmenRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                   await craftmenApis.BlockingCraftmanOpAsync(
                       AESCryptography.Encrypt(craftmanId), Common.TOKEN);

                httpReponse.Response.Result = 
                    AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

                if (!string.IsNullOrEmpty(httpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token as string;
                }

                //if (string.IsNullOrEmpty(httpReponse.Response.Token))
                //{
                //    httpReponse.Response.Result = AESCryptography.Decrypt(httpReponse.Response.Result.ToString());
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        string result =
                //            JsonConvert.DeserializeObject<string>(httpReponse.Response.Result.ToString());

                //        httpReponse.Response.Result = AESCryptography.Decrypt(result);

                //        Common.SUB_TOKEN = httpReponse.Response.Token;
                //    }
                //    else
                //    {
                //        httpReponse.ErrorMessage =
                //            TranslatingUI.MainResourceMap.GetValue("txtToastNotFromServerErr", TranslatingUI.ResourceContextObj).ValueAsString;
                //    }
                //}
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> FiringCraftmanAsync(string craftmanId)
        {
            var craftmenApis = RestService.For<ICraftmenRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                    await craftmenApis.FiringCraftmanOpAsync(
                        AESCryptography.Encrypt(craftmanId), Common.TOKEN);

                httpReponse.Response.Result = 
                    AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

                if (!string.IsNullOrEmpty(httpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token as string;
                }
                //if (string.IsNullOrEmpty(httpReponse.Response.Token))
                //{
                //    string result =
                //        JsonConvert.DeserializeObject<string>(httpReponse.Response.Result.ToString());

                //    httpReponse.Response.Result = AESCryptography.Decrypt(httpReponse.Response.Result.ToString());
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        string result =
                //            JsonConvert.DeserializeObject<string>(httpReponse.Response.Result.ToString());

                //        httpReponse.Response.Result = AESCryptography.Decrypt(result);

                //        Common.SUB_TOKEN = httpReponse.Response.Token;
                //    }
                //    else
                //    {
                //        httpReponse.ErrorMessage =
                //            TranslatingUI.MainResourceMap.GetValue("txtToastNotFromServerErr", TranslatingUI.ResourceContextObj).ValueAsString;
                //    }
                //}
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

    }
}
