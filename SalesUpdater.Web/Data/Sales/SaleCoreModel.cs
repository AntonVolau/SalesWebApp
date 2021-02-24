using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalesUpdater.Web.Data.Sales
{
    public class SaleCoreModel : CoreModel
    {
        public DateTime Date { get; set; }

        public ClientCoreModel Client { get; set; }

        public ProductCoreModel Product { get; set; }

        public decimal Sum { get; set; }

        public ManagerCoreModel Manager { get; set; }

        public SaleCoreModel()
        {
        }

        public SaleCoreModel(DateTime date, ClientCoreModel client, ProductCoreModel product, decimal sum,
            ManagerCoreModel manager)
        {
            Date = date;
            Client = client;
            Product = product;
            Sum = sum;
            Manager = manager;
        }
    }
}