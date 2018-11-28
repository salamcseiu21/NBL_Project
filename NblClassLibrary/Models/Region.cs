using System;
using System.Collections.Generic;
using NBL.Models;

namespace NblClassLibrary.Models
{
    public class Region
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public int DivisionId { get; set; }
        public string IsAssigned { get; set; }
        public string IsCurrent { get; set; }
        public DateTime SysDateTime { get; set; } 
        public Division Division { get; set; }
        public List<Territory> Territories { get; set; }
        public List<District> Districts { get; set; }
        public int BranchId { get; set; }   
        public Branch Branch { get; set; }
        public Region()
        {
            Districts=new List<District>();
            Division=new Division(); 
            Territories=new List<Territory>();
            Branch=new Branch();
        }

    }
}