using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;
using Herafi.Core.Models;

namespace Herafi.Core.Repositories.RepoOperations
{
    public interface ICraftmenRepoOps
    {
        //General Craftmen
        [Get("/craftmen/get-general-craftmen")]
        Task<HttpReponse> GetGeneralCraftmenOpAsync([AliasAs("page_size")] string pageSize, [AliasAs("offset")] string offset, [Header("Authorization")] string token);


        [Get("/craftmen/get-craftman-details-profile")]
        Task<HttpReponse> GetCraftmanDetailsProfileOpAsync([AliasAs("craftman_id")] string craftmanId, [Header("Authorization")] string token);


        [Get("/craftmen/get-craftman-details-crafts")]
        Task<HttpReponse> GetCraftmanDetailsCraftsOpAsync([AliasAs("craftman_id")] string craftmanId, [Header("Authorization")] string token);


        [Get("/craftmen/get-craftman-details-certifications")]
        Task<HttpReponse> GetCraftmanDetailsCertificationsOpAsync([AliasAs("craftman_id")] string craftmanId, [Header("Authorization")] string token);


        [Get("/craftmen/get-craftman-details-requests")]
        Task<HttpReponse> GetCraftmanDetailsRequestsOpAsync([AliasAs("craftman_id")] string craftmanId, [Header("Authorization")] string token);


        [Get("/craftmen/get-craftman-details-projects")]
        Task<HttpReponse> GetCraftmanDetailsProjectsOpAsync([AliasAs("craftman_id")] string craftmanId, [Header("Authorization")] string token);




        //New members craftmen
        [Get("/craftmen/get-new-members-craftmen-ids")]
        Task<HttpReponse> GetNewMembersCraftmenIdsOpAsync([AliasAs("page_size")] string pageSize, [AliasAs("offset")] string offset, [Header("Authorization")] string token);


        [Get("/craftmen/get-new-member-craftman")]
        Task<HttpReponse> GetNewMemberCraftmanOpAsync([AliasAs("craftman_id")] string craftmanId, [Header("Authorization")] string token);


        [Post("/craftmen/accept-new-member-craftman")]
        Task<HttpReponse> AcceptNewMemberCraftmanOpAsync([AliasAs("craftman_id")] string craftmanId, [AliasAs("level")] string level, [Header("Authorization")] string token);


        [Delete("/craftmen/refuse-new-member-craftman")]
        Task<HttpReponse> RefuseNewMemberCraftmanOpAsync([AliasAs("craftman_id")] string craftmanId, [Header("Authorization")] string token);




        //Reported craftmen (for blocking and firing)
        [Get("/craftmen/get-blockings-firings-craftmen-number")]
        Task<HttpReponse> GetBlockingsFiringsCraftmenNumberOpAsync([Header("Authorization")] string token);


        [Get("/craftmen/get-reported-blocking-craftmen-ids")]
        Task<HttpReponse> GetReportedBlockingCraftmenIdsOpAsync([AliasAs("page_size")] string pageSize, [AliasAs("offset")] string offset, [Header("Authorization")] string token);


        [Get("/craftmen/get-reported-firing-craftmen-ids")]
        Task<HttpReponse> GetReportedFiringCraftmenIdsOpAsync([AliasAs("page_size")] string pageSize, [AliasAs("offset")] string offset, [Header("Authorization")] string token);


        [Get("/craftmen/get-reported-craftman")]
        Task<HttpReponse> GetReportedCraftmanOpAsync([AliasAs("craftman_id")] string craftmanId, [Header("Authorization")] string token);


        [Put("/craftmen/blocking-craftman")]
        Task<HttpReponse> BlockingCraftmanOpAsync([AliasAs("craftman_id")] string craftmanId, [Header("Authorization")] string token);


        [Delete("/craftmen/firing-craftman")]
        Task<HttpReponse> FiringCraftmanOpAsync([AliasAs("craftman_id")] string craftmanId, [Header("Authorization")] string token);
    }
}
