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

        private ReaderWriterLockSlim Locker { get; }

        private IProductRepository Products { get; }

        public ProductDbReaderWriter(SalesContext context, ReaderWriterLockSlim locker)
        {
            Context = context;

            Locker = locker;

            var mapper = AutoMapper.CreateConfiguration().CreateMapper();

            Products = new ProductRepository(Context, mapper);
        }

        public async Task<IPagedList<ProductDTO>> GetUsingPagedListAsync(int number, int size,
            Expression<Func<ProductDTO, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            return await Products.GetUsingPagedListAsync(number, size, predicate).ConfigureAwait(false);
        }

        public async Task<ProductDTO> GetAsync(int id)
        {
            return await Products.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<ProductDTO> AddAsync(ProductDTO product)
        {
            Locker.EnterWriteLock();
            try
            {
                if (await Products.TryAddUniqueProductAsync(product).ConfigureAwait(false))
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
            finally
            {
                if (Locker.IsWriteLockHeld)
                {
                    Locker.ExitWriteLock();
                }
            }
        }

        public async Task<ProductDTO> UpdateAsync(ProductDTO product)
        {
            Locker.EnterWriteLock();
            try
            {
                if (await Products.DoesProductExistAsync(product).ConfigureAwait(false))
                    throw new ArgumentException("Product already exists!");

                var result = Products.Update(product);
                await Products.SaveAsync().ConfigureAwait(false);

                return result;
            }
            finally
            {
                if (Locker.IsWriteLockHeld)
                {
                    Locker.ExitWriteLock();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            Locker.EnterReadLock();
            try
            {
                await Products.DeleteAsync(id).ConfigureAwait(false);
                await Products.SaveAsync().ConfigureAwait(false);
            }
            finally
            {
                if (Locker.IsReadLockHeld)
                {
                    Locker.ExitReadLock();
                }
            }
        }

        public async Task<IEnumerable<ProductDTO>> FindAsync(Expression<Func<ProductDTO, bool>> predicate)
        {
            return await Products.FindAsync(predicate).ConfigureAwait(false);
        }
    }
}