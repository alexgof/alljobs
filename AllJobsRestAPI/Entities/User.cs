using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AllJobsRestAPI.Entities
{
    public class User
    {
        
        public int ID { get; set; }
        public string UserIP { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        //public Role Role { get; set; }
        public string Password { get; set; }
        //[JsonIgnore]
        //public string PasswordHash { get; set; }
    }
}
