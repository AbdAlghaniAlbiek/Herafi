using Herafi.Core.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herafi.Core.Repositories.RepoOperations
{
    public interface IAnalyzesRepoOps
    {
        //Profits section
        [Get("/analyzes/get-profits-years")]
        Task<HttpReponse> GetProfitsYearsOpAsync([Header("Authorization")] string token);


        [Get("/analyzes/get-profits-months")]
        Task<HttpReponse> GetProfitsMonthsOpAsync([AliasAs("year")] string year, [Header("Authorization")] string token);


        [Get("/analyzes/get-profits-details")]
        Task<HttpReponse> GetProfitsDetailsOpAsync([AliasAs("year")] string year, [AliasAs("month")] string month, [Header("Authorization")] string token);




        //Craftmen section
        [Get("/analyzes/get-craftmen-years")]
        Task<HttpReponse> GetCraftmenYearsOpAsync([Header("Authorization")] string token);


        [Get("/analyzes/get-craftmen-months")]
        Task<HttpReponse> GetCraftmenMonthsOpAsync([AliasAs("year")] string year, [Header("Authorization")] string token);


        [Get("/analyzes/get-craftmen-details")]
        Task<HttpReponse> GetCraftmenDetailsOpAsync([AliasAs("year")] string year, [AliasAs("month")] string month, [Header("Authorization")] string token);





        //Users section
        [Get("/analyzes/get-users-years")]
        Task<HttpReponse> GetUsersYearsOpAsync([Header("Authorization")] string token);


        [Get("/analyzes/get-users-months")]
        Task<HttpReponse> GetUsersMonthsOpAsync([AliasAs("year")] string year, [Header("Authorization")] string token);


        [Get("/analyzes/get-users-details")]
        Task<HttpReponse> GetUsersDetailsOpAsync([AliasAs("year")] string year, [AliasAs("month")] string month, [Header("Authorization")] string token);




        //Requests section
        [Get("/analyzes/get-requests-years")]
        Task<HttpReponse> GetRequestsYearsOpAsync([Header("Authorization")] string token);


        [Get("/analyzes/get-requests-months")]
        Task<HttpReponse> GetRequestsMonthsOpAsync([AliasAs("year")] string year, [Header("Authorization")] string token);


        [Get("/analyzes/get-requests-details")]
        Task<HttpReponse> GetRequestsDetailsOpAsync([AliasAs("year")] string year, [AliasAs("month")] string month, [Header("Authorization")] string token);





        //Reports section
        [Get("/analyzes/get-reports-years")]
        Task<HttpReponse> GetReportsYearsOpAsync([Header("Authorization")] string token);


        [Get("/analyzes/get-reports-months")]
        Task<HttpReponse> GetReportsMonthsOpAsync([AliasAs("year")] string year, [Header("Authorization")] string token);


        [Get("/analyzes/get-reports-details")]
        Task<HttpReponse> GetReportsDetailsOpAsync([AliasAs("year")] string year, [AliasAs("month")] string month, [Header("Authorization")] string token);
    }
}
