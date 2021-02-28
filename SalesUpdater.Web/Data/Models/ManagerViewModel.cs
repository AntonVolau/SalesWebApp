using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SalesUpdater.Web.Data.Models
{
    public class ManagerViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Surname")]
        [StringLength(30, ErrorMessage = "Surname length must be less than 30 characters long")]
        public string Surname { get; set; }
    }
}