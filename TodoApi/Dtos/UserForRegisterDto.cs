using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dtos
{
    public class UserForRegisterDto
    {
        [Required]  
         public string UserName { get; set; }
         public string Password { get; set; }
    }
}