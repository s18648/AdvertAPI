using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertAPI.DTOs.Requests
{
    public class RecordTokenRequest
    {
        public int IdClient { get; set; }
        public string refreshTokenValue { get; set; }
    }
}
