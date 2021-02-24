using SalesUpdater.DAL.ReaderWriter;
using SalesUpdater.DAL.ReaderWriter.Implementation;
using SalesUpdater.Entity;
using SalesUpdater.Interfaces.Core.DataTransferObject;
using SalesUpdater.Web.Data.Models.Filters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using X.PagedList;

namespace SalesUpdater.Web.Data.Contracts.Services.Implementation
{
    public class ProductService : IProductService, IDisposable
    {
        private SalesContext Context { get; }

        private ReaderWriterLockSlim Locker { get; }

        private IProductDbReaderWriter ProductDbReaderWriter { get; }

        public ProductService()
        {
            Context = new SalesContext();

            Locker = new ReaderWriterLockSlim();

            ProductDbReaderWriter = new ProductDbReaderWriter(Context, Locker);
        }

        public async Task<IPagedList<ProductDTO>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<ProductDTO, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            return await ProductDbReaderWriter.GetUsingPagedListAsync(pageNumber, pageSize, predicate)
                .ConfigureAwait(false);
        }

        public async Task<IPagedList<ProductDTO>> Filter(ProductFilterCoreModel productFilterCoreModel,
            int pageSize, SortDirection sortDirection = SortDirection.Ascending)
        {
            if (productFilterCoreModel.Name == null)
            {
                return await GetUsingPagedListAsync(productFilterCoreModel.Page ?? 1, pageSize)
                    .ConfigureAwait(false);
            }

            return await GetUsingPagedListAsync(productFilterCoreModel.Page ?? 1,
                    pageSize, x => x.Name.Contains(productFilterCoreModel.Name))
                .ConfigureAwait(false);
        }

        public async Task<ProductDTO> GetAsync(int id)
        {
            return await ProductDbReaderWriter.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<ProductDTO> AddAsync(ProductDTO model)
        {
            return await ProductDbReaderWriter.AddAsync(model).ConfigureAwait(false);
        }

        public async Task<ProductDTO> UpdateAsync(ProductDTO model)
        {
            return await ProductDbReaderWriter.UpdateAsync(model).ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            await ProductDbReaderWriter.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ProductDTO>> FindAsync(Expression<Func<ProductDTO, bool>> predicate)
        {
            return await ProductDbReaderWriter.FindAsync(predicate).ConfigureAwait(false);
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Locker.Dispose();
                    Context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}