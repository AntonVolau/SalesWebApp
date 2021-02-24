using System;
using System.ComponentModel.DataAnnotations;

namespace SalesUpdater.Web.Data.Models.Filters
{
    public class SaleFilterViewModel : PagedListParameterViewModel
    {
        [DataType(DataType.Date)]
        public DateTime? DateFrom { get; set; } = null;
        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; } = null;

        public string ClientName { get; set; }
        public string ClientSurname { get; set; }

        public string ProductName { get; set; }

        public decimal? SumFrom { get; set; } = null;
        public decimal? SumTo { get; set; } = null;

        public string ManagerSurname { get; set; }
    }
}