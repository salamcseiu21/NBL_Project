using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NblClassLibrary.Models
{
    public class Territory
    {
        public int TerritoryId { get; set; }
        [Required]
        [Display(Name = "Region")]
        public int RegionId { get; set; }
        [Required]
        [Display(Name = "Territory Name")]
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