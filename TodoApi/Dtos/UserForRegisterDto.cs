using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dtos
{
    public class UserForRegisterDto
    {
        public UserForRegisterDto()
        {
            this.Created = DateTime.Now;
            this.LastActive = DateTime.Now;
        }
        [Required]  
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string KnownAs { get; set; }        
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        [Required]
       
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
    }
}