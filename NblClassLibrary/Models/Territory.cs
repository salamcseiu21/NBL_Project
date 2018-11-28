using System.Collections.Generic;
using NBL.Models;

namespace NblClassLibrary.Models
{
    public class Territory
    {
        public int TerritoryId { get; set; }
        public int RegionId { get; set; }
        public string TerritoryName { get; set; }
        public int AddedByUserId { get; set; } 
        public List<Upazilla> UpazillaList { get; set; }
        public Region Region { get; set; }

        public Territory()
        {
          Region=new Region();
        }
    }
}