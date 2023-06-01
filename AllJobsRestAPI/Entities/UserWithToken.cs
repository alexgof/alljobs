using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllJobsRestAPI.Entities
{
    public class UserWithToken
    {
        public int ID { get; set; }
        public string UserIP { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }        
        public string Password { get; set; }

        public string Token { get; set; }
    }
}
