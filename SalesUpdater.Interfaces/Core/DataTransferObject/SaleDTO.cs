using System;

namespace SalesUpdater.Interfaces.Core.DataTransferObject
{
    public class SaleDTO : CoreModel
    {
        public DateTime Date { get; set; }

        public ClientDTO Clients { get; set; }

        public ProductDTO Products { get; set; }

        public decimal Sum { get; set; }

        public ManagerDTO Managers { get; set; }

        public SaleDTO()
        {
        }

        public SaleDTO(DateTime date, ClientDTO client, ProductDTO product, decimal sum, ManagerDTO manager)
        {
            Date = date;
            Clients = client;
            Products = product;
            Sum = sum;
            Managers = manager;
        }
    }
}
