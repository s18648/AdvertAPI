using System;
using System.Collections.Generic;

namespace AdvertAPI.Entities
{
    public partial class Building
    {
        public Building()
        {
            CampaignFromIdBuildingNavigation = new HashSet<Campaign>();
            CampaignToldBuildingNavigation = new HashSet<Campaign>();
        }

        public int IdBuilding { get; set; }
        public string Street { get; set; }
        public int StreetNumber { get; set; }
        public string City { get; set; }
        public decimal Height { get; set; }

        public virtual ICollection<Campaign> CampaignFromIdBuildingNavigation { get; set; }
        public virtual ICollection<Campaign> CampaignToldBuildingNavigation { get; set; }
    }
}
