using Herafi.Core.Helpers;
using Herafi.Core.Models;
using Herafi.Core.Repositories.RepoOperations;
using Herafi.Core.Security;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herafi.Core.Repositories.RemoteRepo
{
    public class UsersRepo
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
                httpReponse = await
                    ex.GetContentAsAsync<HttpReponse>();
                httpReponse.ErrorMessage =
                    "The requested resource isn't exist in the server (error: 404)";
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                httpReponse = await ex.GetContentAsAsync<HttpReponse>();
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



        //General Users
        public static async Task<HttpReponse> GetGeneralUsersAsync(string pageSize, string offset)
        {
            var usersApis = RestService.For<IUsersRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                    await usersApis.GetGeneralUsersOpAsync(
                        AESCryptography.Encrypt(pageSize), AESCryptography.Encrypt(offset), Common.TOKEN);

                ObservableCollection<User> users =
                      JsonConvert.DeserializeObject<ObservableCollection<User>>(
                          httpReponse.Response.Result.ToString());

                for (int i = 0; i < users.Count; i++)
                {
                    users[i].Id = AESCryptography.Decrypt(users[i].Id);
                    users[i].Name = AESCryptography.Decrypt(users[i].Name);
                    users[i].ImagePath = AESCryptography.Decrypt(users[i].ImagePath);
                }

                httpReponse.Response.Result = users;


                Common.SUB_TOKEN = !string.IsNullOrEmpty(httpReponse.Response.Token as string) ?
                    httpReponse.Response.Token as string : Common.SUB_TOKEN;


                //if (string.IsNullOrEmpty(httpReponse.Response.Token as string))
                //{
                //    ObservableCollection<User> users =
                //        JsonConvert.DeserializeObject<ObservableCollection<User>>(
                //            httpReponse.Response.Result.ToString());

                //    for (int i = 0; i < users.Count; i++)
                //    {
                //        users[i].Id = AESCryptography.Decrypt(users[i].Id);
                //        users[i].Name = AESCryptography.Decrypt(users[i].Name);
                //        users[i].ImagePath = AESCryptography.Decrypt(users[i].ImagePath);
                //    }

                //    httpReponse.Response.Result = users;
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token as string, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        ObservableCollection<User> users =
                //            JsonConvert.DeserializeObject<ObservableCollection<User>>(
                //                 httpReponse.Response.Result.ToString());

                //        for (int i = 0; i < users.Count; i++)
                //        {
                //            users[i].Id = AESCryptography.Decrypt(users[i].Id);
                //            users[i].Name = AESCryptography.Decrypt(users[i].Name);
                //            users[i].ImagePath = AESCryptography.Decrypt(users[i].ImagePath);
                //        }

                //        httpReponse.Response.Result = users;

                //        Common.SUB_TOKEN = httpReponse.Response.Token as string;
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

        public static async Task<HttpReponse> GetUserDetailsProfileAsync(string userId)
        {
            var usersApis = RestService.For<IUsersRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;
            
            try
            {
                httpReponse =
                await usersApis.GetUserDetailsProfileOpAsync(
                    AESCryptography.Encrypt(userId), Common.TOKEN);

                ProfileUser userProfile =
                        JsonConvert.DeserializeObject<ProfileUser>(
                            httpReponse.Response.Result.ToString());


                userProfile.Id = AESCryptography.Decrypt(userProfile.Id);
                userProfile.Name = AESCryptography.Decrypt(userProfile.Name);
                userProfile.Email = AESCryptography.Decrypt(userProfile.Email);
                userProfile.PhoneNumber = AESCryptography.Decrypt(userProfile.PhoneNumber);
                userProfile.NationalNumber = AESCryptography.Decrypt(userProfile.NationalNumber);
                userProfile.City = AESCryptography.Decrypt(userProfile.City);
                userProfile.DateJoin = AESCryptography.Decrypt(userProfile.DateJoin);
                userProfile.RequestsNum = AESCryptography.Decrypt(userProfile.RequestsNum);
                userProfile.Searchs = AESCryptography.Decrypt(userProfile.Searchs);
                userProfile.ProfileImage = AESCryptography.Decrypt(userProfile.ProfileImage);
                userProfile.PersonalIdentityImage = AESCryptography.Decrypt(userProfile.PersonalIdentityImage);
                userProfile.Favourites = AESCryptography.Decrypt(userProfile.Favourites);

                httpReponse.Response.Result = userProfile;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token as string;
                }
                //if (string.IsNullOrEmpty(httpReponse.Response.Token as string))
                //{
                //    ProfileUser userProfile =
                //        JsonConvert.DeserializeObject<ProfileUser>(
                //            httpReponse.Response.Result.ToString());


                //    userProfile.Id = AESCryptography.Decrypt(userProfile.Id);
                //    userProfile.Name = AESCryptography.Decrypt(userProfile.Name);
                //    userProfile.Email = AESCryptography.Decrypt(userProfile.Email);
                //    userProfile.PhoneNumber = AESCryptography.Decrypt(userProfile.PhoneNumber);
                //    userProfile.NationalNumber = AESCryptography.Decrypt(userProfile.NationalNumber);
                //    userProfile.City = AESCryptography.Decrypt(userProfile.City);
                //    userProfile.DateJoin = AESCryptography.Decrypt(userProfile.DateJoin);
                //    userProfile.RequestsNum = AESCryptography.Decrypt(userProfile.RequestsNum);
                //    userProfile.Searchs = AESCryptography.Decrypt(userProfile.Searchs);
                //    userProfile.ProfileImage = AESCryptography.Decrypt(userProfile.ProfileImage);
                //    userProfile.PersonalIdentityImage = AESCryptography.Decrypt(userProfile.PersonalIdentityImage);
                //    userProfile.Favourites = AESCryptography.Decrypt(userProfile.Favourites);

                //    httpReponse.Response.Result = userProfile;
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token as string, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        ProfileUser userProfile =
                //            JsonConvert.DeserializeObject<ProfileUser>(
                //                httpReponse.Response.Result.ToString());


                //        userProfile.Id = AESCryptography.Decrypt(userProfile.Id);
                //        userProfile.Name = AESCryptography.Decrypt(userProfile.Name);
                //        userProfile.Email = AESCryptography.Decrypt(userProfile.Email);
                //        userProfile.PhoneNumber = AESCryptography.Decrypt(userProfile.PhoneNumber);
                //        userProfile.NationalNumber = AESCryptography.Decrypt(userProfile.NationalNumber);
                //        userProfile.City = AESCryptography.Decrypt(userProfile.City);
                //        userProfile.DateJoin = AESCryptography.Decrypt(userProfile.DateJoin);
                //        userProfile.RequestsNum = AESCryptography.Decrypt(userProfile.RequestsNum);
                //        userProfile.Searchs = AESCryptography.Decrypt(userProfile.Searchs);
                //        userProfile.ProfileImage = AESCryptography.Decrypt(userProfile.ProfileImage);
                //        userProfile.PersonalIdentityImage = AESCryptography.Decrypt(userProfile.PersonalIdentityImage);
                //        userProfile.Favourites = AESCryptography.Decrypt(userProfile.Favourites);

                //        httpReponse.Response.Result = userProfile;

                //        Common.SUB_TOKEN = httpReponse.Response.Token as string;
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

        public static async Task<HttpReponse> GetUserDetailsRequestsAsync(string userId)
        {
            var usersApis = RestService.For<IUsersRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
               httpReponse =
               await usersApis.GetUserDetailsRequestsOpAsync(
                   AESCryptography.Encrypt(userId), Common.TOKEN);

                ObservableCollection<RequestUser> userRequests =
                        JsonConvert.DeserializeObject<ObservableCollection<RequestUser>>(
                            httpReponse.Response.Result.ToString());


                for (int i = 0; i < userRequests.Count; i++)
                {
                    userRequests[i].Id = AESCryptography.Decrypt(userRequests[i].Id);
                    userRequests[i].Name = AESCryptography.Decrypt(userRequests[i].Name);
                    userRequests[i].CraftmanName = AESCryptography.Decrypt(userRequests[i].CraftmanName);
                    userRequests[i].Process = AESCryptography.Decrypt(userRequests[i].Process);
                    userRequests[i].StartDate = AESCryptography.Decrypt(userRequests[i].StartDate);
                    userRequests[i].EndDate = AESCryptography.Decrypt(userRequests[i].EndDate);
                    userRequests[i].Cost = AESCryptography.Decrypt(userRequests[i].Cost);
                    userRequests[i].Status = AESCryptography.Decrypt(userRequests[i].Status);
                    userRequests[i].HoursWork = !string.IsNullOrEmpty(userRequests[i].HoursWork) ? AESCryptography.Decrypt(userRequests[i].HoursWork) : "";
                    userRequests[i].Rating = AESCryptography.Decrypt(userRequests[i].Rating);
                    userRequests[i].Comment = AESCryptography.Decrypt(userRequests[i].Comment);
                }

               

                httpReponse.Response.Result = userRequests;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token as string;
                }
                //if (string.IsNullOrEmpty(httpReponse.Response.Token as string))
                //{
                //    RequestUser userRequest =
                //        JsonConvert.DeserializeObject<RequestUser>(
                //            httpReponse.Response.Result.ToString());


                //    userRequest.Id = AESCryptography.Decrypt(userRequest.Id);
                //    userRequest.Name = AESCryptography.Decrypt(userRequest.Name);
                //    userRequest.CraftmanName = AESCryptography.Decrypt(userRequest.CraftmanName);
                //    userRequest.Process = AESCryptography.Decrypt(userRequest.Process);
                //    userRequest.StartDate = AESCryptography.Decrypt(userRequest.StartDate);
                //    userRequest.EndDate = AESCryptography.Decrypt(userRequest.EndDate);
                //    userRequest.Cost = AESCryptography.Decrypt(userRequest.Cost);
                //    userRequest.Status = AESCryptography.Decrypt(userRequest.Status);
                //    userRequest.HoursWork = !string.IsNullOrEmpty(userRequest.HoursWork) ? AESCryptography.Decrypt(userRequest.HoursWork) : "";
                //    userRequest.Rating = AESCryptography.Decrypt(userRequest.Rating);
                //    userRequest.Comment = AESCryptography.Decrypt(userRequest.Comment);

                //    httpReponse.Response.Result = userRequest;
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token as string, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        RequestUser userRequest =
                //            JsonConvert.DeserializeObject<RequestUser>(
                //                httpReponse.Response.Result.ToString());


                //        userRequest.Id = AESCryptography.Decrypt(userRequest.Id);
                //        userRequest.Name = AESCryptography.Decrypt(userRequest.Name);
                //        userRequest.CraftmanName = AESCryptography.Decrypt(userRequest.CraftmanName);
                //        userRequest.Process = AESCryptography.Decrypt(userRequest.Process);
                //        userRequest.EndDate = AESCryptography.Decrypt(userRequest.EndDate);
                //        userRequest.Cost = AESCryptography.Decrypt(userRequest.Cost);
                //        userRequest.Status = AESCryptography.Decrypt(userRequest.Status);
                //        userRequest.HoursWork = AESCryptography.Decrypt(userRequest.HoursWork);
                //        userRequest.Rating = AESCryptography.Decrypt(userRequest.Rating);
                //        userRequest.Comment = AESCryptography.Decrypt(userRequest.Comment);

                //        httpReponse.Response.Result = userRequest;

                //        Common.SUB_TOKEN = httpReponse.Response.Token as string;
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



        //New members Users
        private static async Task<HttpReponse> GetNewMembersUser(IUsersRepoOps usersApis, string userId)
        {
            usersApis = RestService.For<IUsersRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                  await usersApis.GetNewMemberUserOpAsync(userId, Common.TOKEN);

                ProfileUser profile =
                     JsonConvert.DeserializeObject<ProfileUser>(httpReponse.Response.Result.ToString());

                profile.Id = AESCryptography.Decrypt(profile.Id);
                profile.Name = AESCryptography.Decrypt(profile.Name);
                profile.ProfileImage = AESCryptography.Decrypt(profile.ProfileImage);
                profile.PersonalIdentityImage = AESCryptography.Decrypt(profile.PersonalIdentityImage);
                profile.Email = AESCryptography.Decrypt(profile.Email);
                profile.PhoneNumber = AESCryptography.Decrypt(profile.PhoneNumber);
                profile.NationalNumber = AESCryptography.Decrypt(profile.NationalNumber);
                profile.DateJoin = AESCryptography.Decrypt(profile.DateJoin);
                profile.City = AESCryptography.Decrypt(profile.City);

                httpReponse.Response.Result = profile;

                Common.SUB_TOKEN = !string.IsNullOrEmpty(httpReponse.Response.Token as string) ?
                 httpReponse.Response.Token as string : Common.SUB_TOKEN;

                //if (string.IsNullOrEmpty(httpReponse.Response.Token as string))
                //{
                //    ProfileUser profile =
                //        JsonConvert.DeserializeObject<ProfileUser>(httpReponse.Response.Result.ToString());

                //    profile.Id = AESCryptography.Decrypt(profile.Id);
                //    profile.Name = AESCryptography.Decrypt(profile.Name);
                //    profile.ProfileImage = AESCryptography.Decrypt(profile.ProfileImage);
                //    profile.PersonalIdentityImage = AESCryptography.Decrypt(profile.Id);
                //    profile.Email = AESCryptography.Decrypt(profile.Name);
                //    profile.PhoneNumber = AESCryptography.Decrypt(profile.ProfileImage);
                //    profile.NationalNumber = AESCryptography.Decrypt(profile.Id);
                //    profile.DateJoin = AESCryptography.Decrypt(profile.Name);
                //    profile.City = AESCryptography.Decrypt(profile.ProfileImage);

                //    httpReponse.Response.Result = profile;
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //           JsonConvert.DeserializeObject<Token>(
                //           JWTAuthorization.Decode(httpReponse.Response.Token as string, Common.JWT_PUBLIC_KEY)).
                //           SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        ProfileUser profile =
                //        JsonConvert.DeserializeObject<ProfileUser>(httpReponse.Response.Result.ToString());

                //        profile.Id = AESCryptography.Decrypt(profile.Id);
                //        profile.Name = AESCryptography.Decrypt(profile.Name);
                //        profile.ProfileImage = AESCryptography.Decrypt(profile.ProfileImage);
                //        profile.PersonalIdentityImage = AESCryptography.Decrypt(profile.Id);
                //        profile.Email = AESCryptography.Decrypt(profile.Name);
                //        profile.PhoneNumber = AESCryptography.Decrypt(profile.ProfileImage);
                //        profile.NationalNumber = AESCryptography.Decrypt(profile.Id);
                //        profile.DateJoin = AESCryptography.Decrypt(profile.Name);
                //        profile.City = AESCryptography.Decrypt(profile.ProfileImage);

                //        httpReponse.Response.Result = profile;

                //        Common.SUB_TOKEN = httpReponse.Response.Token as string;
                //    }
                //    else
                //    {
                //        httpReponse.ErrorMessage =
                //           TranslatingUI.MainResourceMap.GetValue("txtToastNotFromServerErr", TranslatingUI.ResourceContextObj).ValueAsString;
                //    }
                //}
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }
        public static async Task<HttpReponse> GetNewMembersUsersAsync(string pageSize, string offset)
        {
            var usersApis = RestService.For<IUsersRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                    await usersApis.GetNewMembersUsersIds(
                        AESCryptography.Encrypt(pageSize), AESCryptography.Encrypt(offset), Common.TOKEN);

                ObservableCollection<string> newMemUsersIds =
                    JsonConvert.DeserializeObject<ObservableCollection<string>>(
                        httpReponse.Response.Result.ToString());

                ObservableCollection<ProfileUser> newMemUsers =
                    new ObservableCollection<ProfileUser>();

                for (int i = 0; i < newMemUsersIds.Count; i++)
                {
                    httpReponse = await GetNewMembersUser(usersApis, newMemUsersIds[i]);

                    if (httpReponse.ErrorMessage != "")
                    {
                        newMemUsers = null;
                        break;
                    }

                    newMemUsers.Add(httpReponse.Response.Result as ProfileUser);
                }

                httpReponse.Response.Result = newMemUsers;

                if (!string.IsNullOrEmpty(httpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token as string;
                }
                //if (string.IsNullOrEmpty(httpReponse.Response.Token as string))
                //{
                //    ObservableCollection<string> newMemUsersIds =
                //     JsonConvert.DeserializeObject<ObservableCollection<string>>(
                //         httpReponse.Response.Result.ToString());

                //    ObservableCollection<ProfileUser> newMemUsers =
                //        new ObservableCollection<ProfileUser>();

                //    for (int i = 0; i < newMemUsersIds.Count; i++)
                //    {
                //        httpReponse = await GetNewMembersUser(usersApis, newMemUsersIds[i]);

                //        if (httpReponse.ErrorMessage != "")
                //        {
                //            newMemUsers = null;
                //            break;
                //        }

                //        newMemUsers.Add(httpReponse.Response.Result as ProfileUser);
                //    }

                //    httpReponse.Response.Result = newMemUsers;
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token as string, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        Common.SUB_TOKEN = httpReponse.Response.Token as string;

                //        ObservableCollection<string> newMemUsersIds =
                //            JsonConvert.DeserializeObject<ObservableCollection<string>>(
                //                httpReponse.Response.Result.ToString());

                //        ObservableCollection<ProfileUser> newMemUsers =
                //            new ObservableCollection<ProfileUser>();

                //        for (int i = 0; i < newMemUsersIds.Count; i++)
                //        {
                //            httpReponse = await GetNewMembersUser(usersApis, newMemUsersIds[i]);

                //            if (httpReponse.ErrorMessage != "")
                //            {
                //                newMemUsers = null;
                //                break;
                //            }

                //            newMemUsers.Add(httpReponse.Response.Result as ProfileUser);
                //        }

                //        httpReponse.Response.Result = newMemUsers;
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

        public static async Task<HttpReponse> AcceptNewMemberUserAsync(string userId)
        {
            var usersApis =
                    RestService.For<IUsersRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                    await usersApis.AcceptNewMemberUserOpAsync(
                        AESCryptography.Encrypt(userId), Common.TOKEN);

                httpReponse.Response.Result =
                       AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

                if (!string.IsNullOrEmpty(httpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token as string;
                }

                //if (string.IsNullOrEmpty(httpReponse.Response.Token as string))
                //{
                //    httpReponse.Response.Result = 
                //        AESCryptography.Decrypt(httpReponse.Response.Result.ToString());
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token as string, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        string result =
                //            JsonConvert.DeserializeObject<string>(httpReponse.Response.Result.ToString());

                //        httpReponse.Response.Result = AESCryptography.Decrypt(result);

                //        Common.SUB_TOKEN = httpReponse.Response.Token as string;
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

        public static async Task<HttpReponse> RefuseNewMemberUserAsync(string userId)
        {
            var usersApis = RestService.For<IUsersRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                await usersApis.RefuseNewMemberUserOpAsync(
                    AESCryptography.Encrypt(userId), Common.TOKEN);

                httpReponse.Response.Result =
                      AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

                if (!string.IsNullOrEmpty(httpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token as string;
                }
                //if (string.IsNullOrEmpty(httpReponse.Response.Token as string))
                //{
                //    httpReponse.Response.Result =
                //        AESCryptography.Decrypt(httpReponse.Response.Result.ToString());
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token as string, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        string result =
                //            JsonConvert.DeserializeObject<string>(httpReponse.Response.Result.ToString());

                //        httpReponse.Response.Result = AESCryptography.Decrypt(result);

                //        Common.SUB_TOKEN = httpReponse.Response.Token as string;
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
