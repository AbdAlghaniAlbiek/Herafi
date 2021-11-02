using Herafi.Core.Helpers;
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
    public class SettingsRepo
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



        //Get admin profile and update it
        public static async Task<HttpReponse> GetAdminProfileAsync(string adminId)
        {
            var settingsApis = RestService.For<ISettingRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                 await settingsApis.GetAdminProfileOpAsync(
                    AESCryptography.Encrypt(adminId), Common.TOKEN);

                Admin adminProfile =
                        JsonConvert.DeserializeObject<Admin>(httpReponse.Response.Result.ToString());


                adminProfile.Id = AESCryptography.Decrypt(adminProfile.Id);
                adminProfile.Name = AESCryptography.Decrypt(adminProfile.Name);
                adminProfile.Email = AESCryptography.Decrypt(adminProfile.Email);
                adminProfile.PhoneNumber = AESCryptography.Decrypt(adminProfile.PhoneNumber);
                adminProfile.NationalNumber = AESCryptography.Decrypt(adminProfile.NationalNumber);
                adminProfile.City = AESCryptography.Decrypt(adminProfile.City);
                adminProfile.DateJoin = AESCryptography.Decrypt(adminProfile.DateJoin);
                adminProfile.ProfileImage = AESCryptography.Decrypt(adminProfile.ProfileImage);
                adminProfile.PersonalIdentityImage = AESCryptography.Decrypt(adminProfile.PersonalIdentityImage);


                httpReponse.Response.Result = adminProfile;


                if (!string.IsNullOrEmpty(httpReponse.Response.Token as string))
                {
                    Common.SUB_TOKEN = httpReponse.Response.Token as string;
                }
                //if (string.IsNullOrEmpty(httpReponse.Response.Token))
                //{
                //    Admin adminProfile =
                //        JsonConvert.DeserializeObject<Admin>(httpReponse.Response.Result.ToString());


                //    adminProfile.Id = AESCryptography.Decrypt(adminProfile.Id);
                //    adminProfile.Name = AESCryptography.Decrypt(adminProfile.Name);
                //    adminProfile.Email = AESCryptography.Decrypt(adminProfile.Email);
                //    adminProfile.PhoneNumber = AESCryptography.Decrypt(adminProfile.PhoneNumber);
                //    adminProfile.NationalNumber = AESCryptography.Decrypt(adminProfile.NationalNumber);
                //    adminProfile.City = AESCryptography.Decrypt(adminProfile.City);
                //    adminProfile.DateJoin = AESCryptography.Decrypt(adminProfile.DateJoin);
                //    adminProfile.ProfileImage = AESCryptography.Decrypt(adminProfile.ProfileImage);
                //    adminProfile.PersonalIdentityImage = AESCryptography.Decrypt(adminProfile.PersonalIdentityImage);


                //    httpReponse.Response.Result = adminProfile;
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt(
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        Admin adminProfile =
                //            JsonConvert.DeserializeObject<Admin>(
                //            httpReponse.Response.Result.ToString());


                //        adminProfile.Id = AESCryptography.Decrypt(adminProfile.Id);
                //        adminProfile.Name = AESCryptography.Decrypt(adminProfile.Name);
                //        adminProfile.Email = AESCryptography.Decrypt(adminProfile.Email);
                //        adminProfile.PhoneNumber = AESCryptography.Decrypt(adminProfile.PhoneNumber);
                //        adminProfile.NationalNumber = AESCryptography.Decrypt(adminProfile.NationalNumber);
                //        adminProfile.City = AESCryptography.Decrypt(adminProfile.City);
                //        adminProfile.DateJoin = AESCryptography.Decrypt(adminProfile.DateJoin);
                //        adminProfile.ProfileImage = AESCryptography.Decrypt(adminProfile.ProfileImage);
                //        adminProfile.PersonalIdentityImage = AESCryptography.Decrypt(adminProfile.PersonalIdentityImage);


                //        httpReponse.Response.Result = adminProfile;

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

        public static async Task<HttpReponse> UpdateAdminProfileAsync(Admin admin)
        {
            var settingsApis = RestService.For<ISettingRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            admin.Name = AESCryptography.Encrypt(admin.Name);
            admin.Email = AESCryptography.Encrypt(admin.Email);
            admin.PhoneNumber = AESCryptography.Encrypt(admin.PhoneNumber);
            admin.NationalNumber = AESCryptography.Encrypt(admin.NationalNumber);
            admin.City = AESCryptography.Encrypt(admin.City);
            admin.DateJoin = AESCryptography.Encrypt(admin.DateJoin);
            admin.ProfileImage = AESCryptography.Encrypt(admin.ProfileImage);
            admin.PersonalIdentityImage = AESCryptography.Encrypt(admin.PersonalIdentityImage);

            HttpReponse httpReponse = null;
           
            try
            {
                 httpReponse =
                   await settingsApis.UpdateAdminProfileOpAsync(admin, Common.TOKEN);

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


        //Add facebook account to main account
        public static async Task<HttpReponse> RequestFacebookIdAsync()
        {
            var settingsApis = RestService.For<ISettingRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {

                 httpReponse =
                   await settingsApis.RequestFacebookIdOpAsync(Common.TOKEN);

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
                //             JsonConvert.DeserializeObject<string>(httpReponse.Response.Result.ToString());

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

        public static async Task<HttpReponse> AddFacebookAccountAsync(string adminId, string facebookId)
        {
            var settingsApis = RestService.For<ISettingRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            var httpReponse =
                    await settingsApis.AddFacebookAccountOpAsync(
                        AESCryptography.Encrypt(adminId), AESCryptography.Encrypt(facebookId), Common.TOKEN);

            try
            {

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


        //Add microsoft account to main account
        public static async Task<HttpReponse> RequestMicrosoftIdAsync()
        {
            var settingsApis = RestService.For<ISettingRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                    await settingsApis.RequestMicrosoftIdOpAsync(Common.TOKEN);

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

        public static async Task<HttpReponse> AddMicrosoftAccountAsync(string adminId, string microsoftId)
        {
            var settingsApis = RestService.For<ISettingRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                   await settingsApis.AddMicrosoftAccountOpAsync(
                       AESCryptography.Encrypt(adminId), AESCryptography.Encrypt(microsoftId), Common.TOKEN);

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
    }
}
