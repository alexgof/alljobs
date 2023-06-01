using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AllJobsRestAPI.Models.Users
{
    [DataContract]
    public class JsonResponseWithUserID
    {
        public class UserData
        {
            public string ID { get; set; }
        }

       
        [DataMember(EmitDefaultValue = true, Order = 0)]
        public string Status { get; set; }

        [DataMember(EmitDefaultValue = true, Order = 1)]
        public UserData User { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 2)]
        public string Message { get; set; }

        public void SetError(string errorMessage, string status = "E")
        {
            Status = status;
            Message = errorMessage;
        }
        
    }
}
