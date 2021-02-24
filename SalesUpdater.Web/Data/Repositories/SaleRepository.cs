using AutoMapper;
using SalesUpdater.Entity;
using SalesUpdater.Web.Data.Contracts.Repositories;
using SalesUpdater.Web.Data.Sales;

namespace SalesUpdater.Web.Data.Repositories
{
    public class SaleRepository : GenericRepository<SaleCoreModel, Entity.Sales>, ISaleRepository
    {
        public SaleRepository(SalesContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}