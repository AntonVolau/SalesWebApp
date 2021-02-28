using AutoMapper;
using SalesUpdater.Entity;
using SalesUpdater.Interfaces.Core.DataTransferObject;
using SalesUpdater.Interfaces.DAL.Repositories;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SalesUpdater.DAL.Repositories
{
    public class ProductRepository : Repository<ProductDTO, Products>, IProductRepository
    {
        public ProductRepository(SalesContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public void AddProductToDatabase(ProductDTO productDto)
        {
            Expression<Func<ProductDTO, bool>> predicate = x => x.Name == productDto.Name;

            if (Find(predicate).Any()) return;

            Add(productDto);
        }

        public int GetId(string productName)
        {
            Expression<Func<ProductDTO, bool>> predicate = x => x.Name == productName;

            return Find(predicate).First().ID;
        }
        public async Task<bool> TryAddProductAsync(ProductDTO productCoreModel)
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
            Expression<Func<ProductDTO, bool>> predicate = x => x.Name == productName;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.First().ID;
        }

        public async Task<bool> DoesProductExistAsync(ProductDTO productCoreModel)
        {
            Expression<Func<ProductDTO, bool>> predicate = x => x.Name == productCoreModel.Name;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.Any();
        }
    }
}
