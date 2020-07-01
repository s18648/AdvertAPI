using System;
using System.Collections.Generic;

namespace AdvertAPI.Entities
{
    public partial class Campaign
    {
        public Campaign()
        {
            Banner = new HashSet<Banner>();
        }

        public int IdCampaign { get; set; }
        public int IdClient { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PricePerSquareMeter { get; set; }
        public int FromIdBuilding { get; set; }
        public int ToldBuilding { get; set; }

        public virtual Building FromIdBuildingNavigation { get; set; }
        public virtual Client IdClientNavigation { get; set; }
        public virtual Building ToldBuildingNavigation { get; set; }
        public virtual ICollection<Banner> Banner { get; set; }
    }
}
