using AllJobsRestAPI.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AllJobsRestAPI.Models.Users
{
    public class CreateRequest
    {
        [Required]
        public string PhoneNumber { get; set; }
        
        [Required]
        public string FullName { get; set; }

        [Required]
        public string UserIP { get; set; }

        //[Required]
        //[EnumDataType(typeof(Role))]
        //public string Role { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
