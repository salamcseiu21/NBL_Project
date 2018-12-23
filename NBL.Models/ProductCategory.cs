using System.ComponentModel.DataAnnotations;

namespace NBL.Models
{
    public class ProductCategory
    {

        public int ProductCategoryId { get; set; }
        [Display(Name = "Category Name")]
        public string ProductCategoryName { get; set; }
        [Display(Name = "Brand")]
        public int CompanyId { get; set; }

    }
}