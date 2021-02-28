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
        [StringLength(30, ErrorMessage = "Name length must be less than 30 characters long")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Surname")]
        [StringLength(30, ErrorMessage = "Surname length must be less than 30 characters long")]
        public string Surname { get; set; }
    }
}