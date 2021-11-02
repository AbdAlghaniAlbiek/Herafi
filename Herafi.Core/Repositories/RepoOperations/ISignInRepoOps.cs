using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;
using Herafi.Core.Models;

namespace Herafi.Core.Repositories.RepoOperations
{
    interface ISignInRepoOps
    {
        [Get("/sign-in/get-cities")]
        Task<HttpReponse> GetCitiesOpAsync();

        //Sign up 
        [Post("/sign-in/sign-up-admin")]
        Task<HttpReponse> SignUpAdminOpAsync([Body] Admin admin);

        //Facebook sign up
        [Get("/sign-in/request-facebook-id")]
        Task<HttpReponse> RequestFacebookIdOpAsync();

        [Post("/sign-in/sign-up-facebook-id")]
        Task<HttpReponse> SignUpWithFacebookIdOpAsync([Body] Admin admin);


        //Microsoft sign up
        [Get("/sign-in/request-microsoft-id")]
        Task<HttpReponse> RequestMicrosoftIdOpAsync();

        [Post("/sign-in/sign-up-microsoft-id")]
        Task<HttpReponse> SignUpWithMicrosoftIdOpAsync([Body] Admin admin);


        //Verify
        [Post("/sign-in/verify-admin")]
        Task<HttpReponse> VerifyIdentityOpAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> code);



        //upload images
        [Multipart]
        [Post("/sign-in/upload-profile-image")]
        Task<HttpReponse> UploadProfileImageOpAsync([AliasAs("admin_id")] string adminId, [AliasAs("photo")] StreamPart stream, [Header("Authorization")] string token);

        [Multipart]
        [Post("/sign-in/upload-personal-identity-image")]
        Task<HttpReponse> UploadPersonalIdentityImageOpAsync([AliasAs("admin_id")]string adminId, [AliasAs("photo")] StreamPart stream, [Header("Authorization")] string token);



        //Sign in
        [Post("/sign-in/sign-in-admin")]
        Task<HttpReponse> SignInAdminOpAsync([Body] Admin admin);

        [Post("/sign-in/direct-sign-in-admin")]
        Task<HttpReponse> DirectSignInAdminOpAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> adminId);

        [Post("/sign-in/direct-sign-in-facebook-id")]
        Task<HttpReponse> DirectSignInWithFacebookIdOpAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object>  facebookId);

        [Post("/sign-in/direct-sign-in-microsoft-id")]
        Task<HttpReponse> DirectSignInWithMicrosoftIdOpAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object>  microsoftId);



        //Forget password
        [Post("/sign-in/check-identity")]
        Task<HttpReponse> CheckIdentityOpAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> cerdiential);

        [Post("/sign-in/verification-identity")]
        Task<HttpReponse> VerificationIdentityOpAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> code);

        [Put("/sign-in/reset-password")]
        Task<HttpReponse> ResetPasswordOpAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data, [Header("Authorization")] string token);

    }
}
