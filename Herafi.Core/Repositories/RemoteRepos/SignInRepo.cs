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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Herafi.Core.Repositories.RemoteRepo
{
    public class SignInRepo
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



        public static async Task<HttpReponse> GetCitiesAsync()
        {
            var signInApis = RestService.For<ISignInRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                    await signInApis.GetCitiesOpAsync();

                ObservableCollection<City> cities =
                        JsonConvert.DeserializeObject<ObservableCollection<City>>(
                        httpReponse.Response.Result.ToString());

                for (int i = 0; i < cities.Count; i++)
                {
                    cities[i].Id = AESCryptography.Decrypt(cities[i].Id);
                    cities[i].Name = AESCryptography.Decrypt(cities[i].Name);
                }

                httpReponse.Response.Result = cities;
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }


        //Sign up admin (and with optionally with facebook and microsoft accounts)
        public static async Task<HttpReponse> SignUpAdminAsync(Admin admin)
        {
            var signInApis = RestService.For<ISignInRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });


            admin.Name = AESCryptography.Encrypt(admin.Name);
            admin.Email = AESCryptography.Encrypt(admin.Email);
            admin.PhoneNumber = AESCryptography.Encrypt(admin.PhoneNumber);
            admin.Password = AESCryptography.Encrypt(admin.Password);
            admin.NationalNumber = AESCryptography.Encrypt(admin.NationalNumber);
            admin.City = AESCryptography.Encrypt(admin.City);

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                    await signInApis.SignUpAdminOpAsync(admin);

                httpReponse.Response.Result = 
                    AESCryptography.Decrypt(httpReponse.Response.Result.ToString());
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> RequestFacebookIdAsync()
        {
            var signInApis = RestService.For<ISignInRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                    await signInApis.RequestFacebookIdOpAsync();

                string facebookId =
                    JsonConvert.DeserializeObject<string>(httpReponse.Response.Result.ToString());

                httpReponse.Response.Result = AESCryptography.Decrypt(facebookId);
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> SignUpWidthFacebookIdAdminAsync(Admin admin)
        {
            var signInApis = RestService.For<ISignInRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            admin.Name = AESCryptography.Encrypt(admin.Name);
            admin.Email = AESCryptography.Encrypt(admin.Email);
            admin.PhoneNumber = AESCryptography.Encrypt(admin.PhoneNumber);
            admin.Password = AESCryptography.Encrypt(admin.Password);
            admin.NationalNumber = AESCryptography.Encrypt(admin.NationalNumber);
            admin.City = AESCryptography.Encrypt(admin.City);
            admin.FacebookId = AESCryptography.Encrypt(admin.FacebookId);

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                   await signInApis.SignUpWithFacebookIdOpAsync(admin);

                //Get only admin id
                httpReponse.Response.Result = 
                    AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

                Common.SUB_TOKEN = !string.IsNullOrEmpty(httpReponse.Response.Token) ?
                    httpReponse.Response.Token : Common.SUB_TOKEN;
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> RequestMicrosoftIdAsync()
        {
            var signInApis = RestService.For<ISignInRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                   await signInApis.RequestMicrosoftIdOpAsync();

                httpReponse.Response.Result = 
                    AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

                Common.SUB_TOKEN = !string.IsNullOrEmpty(httpReponse.Response.Token) ?
                    httpReponse.Response.Token : Common.SUB_TOKEN;
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> SignUpWidthMicrosoftIdAdminAsync(Admin admin)
        {
            var signInApis =
                    RestService.For<ISignInRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            admin.Name = AESCryptography.Encrypt(admin.Name);
            admin.Email = AESCryptography.Encrypt(admin.Email);
            admin.PhoneNumber = AESCryptography.Encrypt(admin.PhoneNumber);
            admin.Password = AESCryptography.Encrypt(admin.Password);
            admin.NationalNumber = AESCryptography.Encrypt(admin.NationalNumber);
            admin.City = AESCryptography.Encrypt(admin.City);
            admin.MicrosoftId = AESCryptography.Encrypt(admin.MicrosoftId);

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                   await signInApis.SignUpWithMicrosoftIdOpAsync(admin);

                //Get admin id
                httpReponse.Response.Result = 
                    AESCryptography.Decrypt(httpReponse.Response.Result.ToString());
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }


        //Verify admin 
        public static async Task<HttpReponse> VerifyAdminAsync(string code)
        {
            var signInApis = RestService.For<ISignInRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                   await signInApis.VerifyIdentityOpAsync(new Dictionary<string, object>() { { "code", AESCryptography.Encrypt(code) } });

                httpReponse.Response.Result =
                      AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

                Common.SUB_TOKEN = !string.IsNullOrEmpty(httpReponse.Response.Token as string) ?
                    httpReponse.Response.Token as string : Common.SUB_TOKEN;


              //if (string.IsNullOrEmpty(httpReponse.Response.Token as string))
                //{
                //    httpReponse.Response.Result =
                //        AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

                //    Common.SUB_TOKEN = !string.IsNullOrEmpty(httpReponse.Response.Token as string) ? 
                //        httpReponse.Response.Token as string : Common.SUB_TOKEN;
                //}
                //else
                //{
                //    if (AESCryptography.Decrypt( 
                //        JsonConvert.DeserializeObject<Token>(
                //        JWTAuthorization.Decode(httpReponse.Response.Token as string, Common.JWT_PUBLIC_KEY)).
                //        SecretKeyword) == AESCryptography.Decrypt(Common.SECRET_KEYWORD))
                //    {
                //        httpReponse.Response.Result =
                //            AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

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



        //uploading profile and personal identity images
        public static async Task<HttpReponse> UploadProfileImageAsync(string adminId, StreamPart profileImage)
        {
            var signInApis = RestService.For<ISignInRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                    await signInApis.UploadProfileImageOpAsync(
                        AESCryptography.Encrypt(adminId), profileImage, Common.TOKEN);

                httpReponse.Response.Result =
                       AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

                Common.SUB_TOKEN = !string.IsNullOrEmpty(httpReponse.Response.Token) ?
                    httpReponse.Response.Token : Common.SUB_TOKEN;

            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> UploadPersonalIdentityImageAsync(string adminId, StreamPart personalIdentityImage)
        {
            var signInApis =  RestService.For<ISignInRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                    await signInApis.UploadPersonalIdentityImageOpAsync(
                        AESCryptography.Encrypt(adminId), personalIdentityImage, Common.TOKEN);

                httpReponse.Response.Result =
                        AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

                Common.SUB_TOKEN = !string.IsNullOrEmpty(httpReponse.Response.Token) ?
                    httpReponse.Response.Token : Common.SUB_TOKEN;


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
                //        httpReponse.Response.Result = 
                //            AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

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



        //Sign in admin and forget password process
        public static async Task<HttpReponse> SignInAdminAsync(Admin admin)
        {
            admin.Email = AESCryptography.Encrypt(admin.Email);
            admin.Password = AESCryptography.Encrypt(admin.Password);

            var signInApis = RestService.For<ISignInRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                 httpReponse =
                    await signInApis.SignInAdminOpAsync(admin);

                httpReponse.Response.Result =
                    AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

                Common.SUB_TOKEN = !string.IsNullOrEmpty(httpReponse.Response.Token) ?
                    httpReponse.Response.Token : Common.SUB_TOKEN;

                //if (string.IsNullOrEmpty(httpReponse.Response.Token))
                //{
                //    httpReponse.Response.Result =
                //        AESCryptography.Decrypt(httpReponse.Response.Result.ToString());
                //}
                //else
                //{
                //    if (await JWTAuthorization.ValidateToken(httpReponse.Response.Token))
                //    {
                //        httpReponse.Response.Result =
                //            AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

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

        public static async Task<HttpReponse> DirectSignInAdminAsync(string adminId)
        {
            var signInApis = RestService.For<ISignInRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                  await signInApis.DirectSignInAdminOpAsync(new Dictionary<string, object> { { "admin_id", AESCryptography.Encrypt(adminId) } });

                //Success result
                httpReponse.Response.Result = 
                    AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

                Common.SUB_TOKEN = httpReponse.Response.Token as string;


                //if (string.IsNullOrEmpty(httpReponse.Response.Token as string))
                //{
                //    string successResult =
                //         JsonConvert.DeserializeObject<string>(httpReponse.Response.Result.ToString());

                //    httpReponse.Response.Result = AESCryptography.Decrypt(successResult);
                //}
                //else
                //{
                //    if (await JWTAuthorization.ValidateToken(httpReponse.Response.Token))
                //    {
                //        string successResult =
                //            JsonConvert.DeserializeObject<string>(httpReponse.Response.Result.ToString());

                //        httpReponse.Response.Result = AESCryptography.Decrypt(successResult);

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

        public static async Task<HttpReponse> DirectSignInWithFacebookIdAsync(string facebookId)
        {
            var signInApis =
                    RestService.For<ISignInRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                   await signInApis.DirectSignInWithFacebookIdOpAsync(
                       new Dictionary<string, object> { { "facebook_id", AESCryptography.Encrypt(facebookId) } });

                //Get Admin ID
                httpReponse.Response.Result =
                    AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

                Common.SUB_TOKEN = !string.IsNullOrEmpty(httpReponse.Response.Token) ?
                    httpReponse.Response.Token : Common.SUB_TOKEN;


                //if (string.IsNullOrEmpty(httpReponse.Response.Token))
                //{
                //    httpReponse.Response.Result =
                //        AESCryptography.Decrypt(httpReponse.Response.Result.ToString());
                //}
                //else
                //{
                //    if (await JWTAuthorization.ValidateToken(httpReponse.Response.Token))
                //    {
                //        httpReponse.Response.Result =
                //            AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

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

        public static async Task<HttpReponse> DirectSignInWithMicrosoftIdAsync(string microsoftId)
        {
            var signInApis = RestService.For<ISignInRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                   await signInApis.DirectSignInWithMicrosoftIdOpAsync(new Dictionary<string, object> { { "microsoft_id", AESCryptography.Encrypt(microsoftId) } });

                //Get Admin id
                httpReponse.Response.Result = 
                    AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

                Common.SUB_TOKEN = !string.IsNullOrEmpty(httpReponse.Response.Token) ?
                    httpReponse.Response.Token : Common.SUB_TOKEN;


                //if (string.IsNullOrEmpty(httpReponse.Response.Token as string))
                //{
                //    string adminId =
                //        JsonConvert.DeserializeObject<string>(httpReponse.Response.Result.ToString());

                //    httpReponse.Response.Result = AESCryptography.Decrypt(adminId);
                //}
                //else
                //{
                //    if (await JWTAuthorization.ValidateToken(httpReponse.Response.Token))
                //    {
                //        string adminId =
                //            JsonConvert.DeserializeObject<string>(httpReponse.Response.Result.ToString());

                //        httpReponse.Response.Result = AESCryptography.Decrypt(adminId);

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


        //Reset password if the admin forget his password
        public static async Task<HttpReponse> CheckIdentityAsync(string cerdiential)
        {
            var signInApis = RestService.For<ISignInRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                  await signInApis.CheckIdentityOpAsync(
                      new Dictionary<string, object> { { "cerdiential", AESCryptography.Encrypt(cerdiential) } });

                httpReponse.Response.Result = 
                    AESCryptography.Decrypt(httpReponse.Response.Result.ToString());
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> VerificationIdentityAsync(string code)
        {
            var signInApis = RestService.For<ISignInRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                  await signInApis.VerificationIdentityOpAsync(
                      new Dictionary<string, object> { { "code", AESCryptography.Encrypt(code) } });

                httpReponse.Response.Result = 
                    AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

                Common.SUB_TOKEN = !string.IsNullOrEmpty(httpReponse.Response.Token) ?
                  httpReponse.Response.Token : Common.SUB_TOKEN;
            }
            catch (ApiException ex)
            {
                httpReponse = await HandlingErrorMessage(httpReponse, ex);
            }

            return httpReponse;
        }

        public static async Task<HttpReponse> ResetPasswordAsync(string adminId, string newPassword)
        {
            var signInApis = RestService.For<ISignInRepoOps>(
                Common.URL,
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer()
                });

            HttpReponse httpReponse = null;

            try
            {
                httpReponse =
                  await signInApis.ResetPasswordOpAsync(
                      new Dictionary<string, object> { { "admin_id", AESCryptography.Encrypt(adminId) }, { "new_password", AESCryptography.Encrypt(newPassword) } },
                      Common.TOKEN);

                httpReponse.Response.Result = 
                    AESCryptography.Decrypt(httpReponse.Response.Result.ToString());

                Common.SUB_TOKEN = !string.IsNullOrEmpty(httpReponse.Response.Token) ?
                    httpReponse.Response.Token : Common.SUB_TOKEN;

                //if (string.IsNullOrEmpty(httpReponse.Response.Token as string))
                //{
                //    string result =
                //        JsonConvert.DeserializeObject<string>(httpReponse.Response.Result.ToString());

                //    httpReponse.Response.Result = AESCryptography.Decrypt(result);
                //}
                //else
                //{
                //    if (await JWTAuthorization.ValidateToken(httpReponse.Response.Token))
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
