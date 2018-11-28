using System;
using System.ComponentModel.DataAnnotations;
using NBL.Models;

namespace NblClassLibrary.Models.ViewModels
{
    public class ViewClient:Client
    {
         
        public string ClientTypeName { get; set; }
        public decimal Discount { get; set; } 
        public string DivisionName { get; set; }
        public string DistrictName { get; set; }
        public string UpazillaName { get; set; }
        public string PostOfficeName { get; set; }
        public string PostCode { get; set; }
        public string UserName { get; set; }
        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }
        [Display(Name = "Branch")]
        public string BranchName { get; set; }
        public decimal TotalDebitAmount { get; set; }

    }
}