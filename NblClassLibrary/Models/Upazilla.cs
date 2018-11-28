﻿using NBL.Models;

namespace NblClassLibrary.Models
{
    public class Upazilla
    {

      
        public int UpazillaId { get; set; }
        public string UpazillaName { get; set; }
        public int DistrictId { get; set; }

        public District District { get; set; }

        public Upazilla()
        {
            District=new District();
        }
    }
}