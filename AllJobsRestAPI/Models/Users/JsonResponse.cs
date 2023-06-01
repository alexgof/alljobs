using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AllJobsRestAPI.Models.Users
{
    public class JsonResponse
    {        

        [DataContract]
        public class JSONResponse
        {
            [DataMember(EmitDefaultValue = true, Order = 0)]
            public string Status { get; set; }

            [DataMember(EmitDefaultValue = false, Order = 1)]
            public string Token { get; set; }

            [DataMember(EmitDefaultValue = false, Order = 2)]
            public string Message { get; set; }

            public void SetError(string errorMessage, string status = "E")
            {
                Status = status;
                Message = errorMessage;
            }
        }               
    }
}
