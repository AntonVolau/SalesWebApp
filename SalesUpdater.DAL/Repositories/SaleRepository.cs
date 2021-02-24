using AutoMapper;
using SalesUpdater.Entity;
using SalesUpdater.Interfaces.Core.DataTransferObject;
using SalesUpdater.Interfaces.DAL.Repositories;

namespace SalesUpdater.DAL.Repositories
{
    public class SaleRepository : Repository<SaleDTO, Sales>, ISaleRepository
    {
        public SaleRepository(SalesContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
