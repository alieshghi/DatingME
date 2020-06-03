using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TodoApi.helper
{
    public static class Extentions
    {
        public static void AddApplicationError(this HttpResponse response, string message){
            response.Headers.Add("Application-Error",message);
            response.Headers.Add("Access-Control-Expose-Headers","Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin","*");

        }
        public static void AddPagination(this HttpResponse response,int pageSize, int totalPage
        ,int curentPage,int totalPageOfItems){
            var httpHeader = new PaginationHeader(totalPage, pageSize, curentPage, totalPageOfItems);
            var camelCaseFormater= new JsonSerializerSettings();
            camelCaseFormater.ContractResolver= new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination",JsonConvert.SerializeObject(httpHeader,camelCaseFormater));
            response.Headers.Add("Access-Control-Expose-Headers","Pagination");
            
        }
        public static int CalCauteAge(this DateTime birthDay){
            var age = DateTime.Today.Year - birthDay.Year;
            if (birthDay.AddYears(age)> DateTime.Today)
            {
                age--;                                
            }
            return age;
        }
    }
}