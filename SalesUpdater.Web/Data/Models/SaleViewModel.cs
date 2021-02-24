using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SalesUpdater.Web.Data.Models
{
    public class SaleViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public ClientViewModel Client { get; set; }

        public ProductViewModel Product { get; set; }

        [Required]
        [Display(Name = "Sum")]
        public double Sum { get; set; }

        public ManagerViewModel Manager { get; set; }
    }
}