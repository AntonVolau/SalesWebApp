using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SalesUpdater.Web.Data.Models
{
    public class ClientViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(50, ErrorMessage = "Name length must be less than 50 characters")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Surname")]
        [StringLength(50, ErrorMessage = "Surname length must be less than 50 characters")]
        public string Surname { get; set; }
    }
}