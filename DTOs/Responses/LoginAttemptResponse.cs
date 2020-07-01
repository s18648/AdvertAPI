using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertAPI.DTOs.Responses
{
    public class LoginAttemptResponse
    {
        public string Hash { get; set; }
        public string Salt { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public int IdClient { get; set; }
        public string TokenString { get; set; }
    }
}
