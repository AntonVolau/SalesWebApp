using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SalesUpdater.Web.Data.Models
{
    public class ProductViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(50, ErrorMessage = "Name length must be less than 50 characters")]
        public string Name { get; set; }
    }
}