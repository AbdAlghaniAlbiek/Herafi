using Herafi.Core.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herafi.Core.Repositories.RepoOperations
{
    interface IDashboardRepoOps
    {
        [Get("/dashboard/get-profits-details")]
        Task<HttpReponse> GetProfitsDetailsAsync([Header("Authorization")] string token);


        [Get("/dashboard/get-new-members")]
        Task<HttpReponse> GetNewMembersAsync([Header("Authorization")] string token);
    }
}
