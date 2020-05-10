using System.Net.Mime;
using Microsoft.AspNetCore.Http;
namespace TodoApi.helper
{
    public static class Extentions
    {
        public static void AddApplicationError(this HttpResponse response, string message){
            response.Headers.Add("Application-Error",message);
            response.Headers.Add("Access-Control-Expose-Headers","Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin","*");

        }
    }
}