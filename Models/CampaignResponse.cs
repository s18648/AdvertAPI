using AdvertAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AdvertAPI.DTOs.Responses
{
    public class CampaignResponse
    {
        public int IdCampaign { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int FromIdBuilding { get; set; }
        public int ToldIdBuilding { get; set; }


        public List<Adds> ListOfAdds { get; set; }
        public Clients ClientsInfo { get; set; }



    }
}
