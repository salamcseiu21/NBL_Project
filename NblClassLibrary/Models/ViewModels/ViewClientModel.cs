using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NblClassLibrary.Models.ViewModels
{
   public class ViewClientModel
    {
        public string CommercialName { get; set; }
        public string ClientName { get; set; }
        public decimal Transaction { get; set; } 
    }
}
