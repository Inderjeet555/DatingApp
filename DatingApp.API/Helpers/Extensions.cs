using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static void AddPagination(this HttpResponse response, int currentPage, int itemPerPage, int totalIems, int totalPages)
        {
            var pagiNationHeader = new PaginationHeader(currentPage, itemPerPage, totalIems, totalPages);
            var cmaelCaseFormatter = new JsonSerializerSettings();
            cmaelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(pagiNationHeader, cmaelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }

        public static int CalculateAge(this DateTime datetime)
        {
            var age = DateTime.Today.Year - datetime.Year;

            if(datetime.AddYears(age) > DateTime.Today)
                age--;

                return age;
        }


    }
}