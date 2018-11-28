using System.Collections.Generic;

namespace NblClassLibrary.Models
{
    public class District
    {
       
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int DivisionId { get; set; }
        public Division Division { get; set; }
        public List<Upazilla> Upazillas { get; set; }
        public District()
        {
            Division=new Division();
            Upazillas=new List<Upazilla>();
        }
        
    }
}