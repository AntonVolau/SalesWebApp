using SalesUpdater.DAL.Repositories;
using SalesUpdater.Entity;
using SalesUpdater.Interfaces.Core.DataTransferObject;
using SalesUpdater.Interfaces.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using X.PagedList;

namespace SalesUpdater.DAL.ReaderWriter.Implementation
{
    public class ProductDbReaderWriter : IProductDbReaderWriter
    {
        private SalesContext Context { get; }

        private IProductRepository Products { get; }

        public ProductDbReaderWriter(SalesContext context)
        {
            Context = context;

            var mapper = AutoMapper.CreateConfiguration().CreateMapper();

            Products = new ProductRepository(Context, mapper);
        }

        public async Task<IPagedList<ProductDTO>> GetPagedListAsync(int number, int size,
            Expression<Func<ProductDTO, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            return await Products.GetPagedListAsync(number, size, predicate).ConfigureAwait(false);
        }

        public async Task<ProductDTO> GetAsync(int id)
        {
            return await Products.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<ProductDTO> AddAsync(ProductDTO product)
        {
                if (await Products.TryAddProductAsync(product).ConfigureAwait(false))
                {
                    await Products.SaveAsync().ConfigureAwait(false);
                    return await GetAsync(await Products.GetIdAsync(product.Name)
                        .ConfigureAwait(false)).ConfigureAwait(false);
                }
                else
                {
                    throw new ArgumentException("Product already exists!");
                }
        }

        public async Task<ProductDTO> UpdateAsync(ProductDTO product)
        {
                if (await Products.DoesProductExistAsync(product).ConfigureAwait(false))
                    throw new ArgumentException("Product already exists!");

                var result = Products.Update(product);
                await Products.SaveAsync().ConfigureAwait(false);

                return result;
        }

        public async Task DeleteAsync(int id)
        {
                await Products.DeleteAsync(id).ConfigureAwait(false);
                await Products.SaveAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<ProductDTO>> FindAsync(Expression<Func<ProductDTO, bool>> predicate)
        {
            return await Products.FindAsync(predicate).ConfigureAwait(false);
        }
    }
}