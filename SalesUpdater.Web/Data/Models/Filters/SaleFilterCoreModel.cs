using System;

namespace SalesUpdater.Web.Data.Models.Filters
{
    public class SaleFilterCoreModel : PagedListParameterCoreModel
    {
        public DateTime? DateFrom { get; set; } = null;
        public DateTime? DateTo { get; set; } = null;

        public string ClientName { get; set; }
        public string ClientSurname { get; set; }

        public string ProductName { get; set; }

        public decimal? SumFrom { get; set; } = null;
        public decimal? SumTo { get; set; } = null;

        public string ManagerSurname { get; set; }
    }
}