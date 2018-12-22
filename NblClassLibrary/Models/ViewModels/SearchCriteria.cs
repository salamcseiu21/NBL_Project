using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NblClassLibrary.Models.ViewModels
{
  public class SearchCriteria
    {
        public int? BranchId { get; set; }
        public string ClientName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; } 
    }
}
