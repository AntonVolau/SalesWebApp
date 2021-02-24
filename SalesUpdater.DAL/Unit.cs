using SalesUpdater.DAL.Repositories;
using SalesUpdater.Entity;
using SalesUpdater.Interfaces.Core.DataTransferObject;
using SalesUpdater.Interfaces.DAL;
using SalesUpdater.Interfaces.DAL.Repositories;
using System.Collections.Generic;
using System.Threading;

namespace SalesUpdater.DAL
{
    public class Unit : IUnit
    {
        private SalesContext Context { get; }
        private ReaderWriterLockSlim Locker { get; }

        private IClientRepository Clients { get; }
        private IManagerRepository Managers { get; }
        private IProductRepository Products { get; }
        private ISaleRepository Sales { get; }

        public Unit(SalesContext context, ReaderWriterLockSlim locker)
        {
            Context = context;
            Locker = locker;

            var mapper = AutoMapper.CreateConfiguration().CreateMapper();
            Clients = new ClientRepository(Context, mapper);
            Managers = new ManagerRepository(Context, mapper);
            Products = new ProductRepository(Context, mapper);
            Sales = new SaleRepository(Context, mapper);
        }

        public void Add(params SaleDTO[] models)
        {
            Locker.EnterWriteLock();
            try
            {
                foreach (var sale in models)
                {
                    Clients.AddClientToDatabase(sale.Clients);
                    Clients.Save();
                    sale.Clients.ID = Clients.GetId(sale.Clients.Name, sale.Clients.Surname);

                    Managers.AddManagerToDatabase(sale.Managers);
                    Managers.Save();
                    sale.Managers.ID = Managers.GetId(sale.Managers.Surname);

                    Products.AddProductToDatabase(sale.Products);
                    Products.Save();
                    sale.Products.ID = Products.GetId(sale.Products.Name);

                    Sales.Add(sale);
                    Sales.Save();
                }
            }
            finally
            {
                Locker.ExitWriteLock();
            }
        }

        public IEnumerable<SaleDTO> GetAll()
        {
            return Sales.GetAll();
        }
    }
}
