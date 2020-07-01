using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertAPI.DTOs.Responses
{
    public class ValidateTokenResponse
    {
        public string TokenString { get; set; }
        public string Login { get; set; }
        public int IdClient { get; set; }
        public string FirstName { get; set; }

    }
}
