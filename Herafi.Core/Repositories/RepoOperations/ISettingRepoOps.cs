using Herafi.Core.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herafi.Core.Repositories.RepoOperations
{
    interface ISettingRepoOps
    {
        //get admin profile and update it
        [Get("/settings/get-admin-profile")]
        Task<HttpReponse> GetAdminProfileOpAsync([AliasAs("admin_id")] string adminId, [Header("Authorization")] string token);


        [Put("/settings/update-admin-profile")]
        Task<HttpReponse> UpdateAdminProfileOpAsync([Body] Admin admin, [Header("Authorization")] string token);



        //Add facebook account to main account
        [Get("/settings/request-facebook-id")]
        Task<HttpReponse> RequestFacebookIdOpAsync([Header("Authorization")] string token);


        [Put("/settings/add-facebook-account")]
        Task<HttpReponse> AddFacebookAccountOpAsync([AliasAs("admin_id")] string adminId, [AliasAs("facebook_id")] string facebookId, [Header("Authorization")] string token);



        //Add microsoft account to main account
        [Get("/settings/request-microsoft-id")]
        Task<HttpReponse> RequestMicrosoftIdOpAsync([Header("Authorization")] string token);

        [Put("/settings/add-microsoft-account")]
        Task<HttpReponse> AddMicrosoftAccountOpAsync([AliasAs("admin_id")] string adminId, [AliasAs("microsoft_id")] string microsoftId, [Header("Authorization")] string token);
    }
}
