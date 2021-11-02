using Herafi.Core.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herafi.Core.Repositories.RepoOperations
{
    interface IUsersRepoOps
    {
        //General users
        [Get("/users/get-general-users")]
        Task<HttpReponse> GetGeneralUsersOpAsync([AliasAs("page_size")] string pageSize, [AliasAs("offset")] string offset, [Header("Authorization")] string token);


        [Get("/users/get-user-details-profile")]
        Task<HttpReponse> GetUserDetailsProfileOpAsync([AliasAs("user_id")] string userId, [Header("Authorization")] string token);


        [Get("/users/get-user-details-requests")]
        Task<HttpReponse> GetUserDetailsRequestsOpAsync([AliasAs("user_id")] string userId, [Header("Authorization")] string token);




        //New members users
        [Get("/users/get-new-members-users-ids")]
        Task<HttpReponse> GetNewMembersUsersIds([AliasAs("page_size")] string pageSize, [AliasAs("offset")] string offset, [AliasAs("Authorization")] string token);

        [Get("/users/get-new-member-user")]
        Task<HttpReponse> GetNewMemberUserOpAsync([AliasAs("user_id")] string userId, [Header("Authorization")] string token);


        [Post("/users/accept-new-member-user")]
        Task<HttpReponse> AcceptNewMemberUserOpAsync([AliasAs("user_id")] string userId, [Header("Authorization")] string token);


        [Delete("/users/refuse-new-member-user")]
        Task<HttpReponse> RefuseNewMemberUserOpAsync([AliasAs("user_id")] string userId, [Header("Authorization")] string token);

    }
}
