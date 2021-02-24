using AutoMapper;
using SalesUpdater.Entity;
using SalesUpdater.Web.Data.Contracts.Repositories;
using SalesUpdater.Web.Data.Sales;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SalesUpdater.Web.Data.Repositories
{
    public class ProductRepository : GenericRepository<ProductCoreModel, Entity.Products>, IProductRepository
    {
        public ProductRepository(SalesContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<bool> TryAddUniqueProductAsync(ProductCoreModel productCoreModel)
        {
            if (await DoesProductExistAsync(productCoreModel).ConfigureAwait(false))
            {
                return false;
            }

            Add(productCoreModel);
            return true;
        }

        public async Task<int> GetIdAsync(string productName)
        {
            Expression<Func<ProductCoreModel, bool>> predicate = x => x.Name == productName;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.First().Id;
        }

        public async Task<bool> DoesProductExistAsync(ProductCoreModel productCoreModel)
        {
            Expression<Func<ProductCoreModel, bool>> predicate = x => x.Name == productCoreModel.Name;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.Any();
        }
    }
}