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
    public class SaleService : ISaleService, IDisposable
    {
        private SalesContext Context { get; }

        private ISaleDbReaderWriter SaleDbReaderWriter { get; }

        public SaleService()
        {
            Context = new SalesContext();

            SaleDbReaderWriter = new SaleDbReaderWriter(Context);
        }

        public async Task<IPagedList<SaleDTO>> GetPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<SaleDTO, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            return await SaleDbReaderWriter.GetPagedListAsync(pageNumber, pageSize, predicate)
                .ConfigureAwait(false);
        }

        public async Task<IPagedList<SaleDTO>> Filter(SaleCoreFilterModel saleCoreFilterModel,
            int pageSize, SortDirection sortDirection = SortDirection.Ascending)
        {
            if (saleCoreFilterModel.ClientName == null &&
                saleCoreFilterModel.ClientSurname == null &&
                saleCoreFilterModel.DateFrom == null &&
                saleCoreFilterModel.DateTo == null &&
                saleCoreFilterModel.ManagerSurname == null &&
                saleCoreFilterModel.ProductName == null &&
                saleCoreFilterModel.SumFrom == null &&
                saleCoreFilterModel.SumTo == null)
            {
                return await GetPagedListAsync(saleCoreFilterModel.Page ?? 1, pageSize)
                    .ConfigureAwait(false);
            }

            return await GetPagedListAsync(
                saleCoreFilterModel.Page ?? 1, pageSize, x =>
                    (x.Date >= saleCoreFilterModel.DateFrom && x.Date <= saleCoreFilterModel.DateTo) &&
                    (x.Sum >= saleCoreFilterModel.SumFrom && x.Sum <= saleCoreFilterModel.SumTo) &&
                    x.Clients.Name.Contains(saleCoreFilterModel.ClientName) &&
                    x.Clients.Surname.Contains(saleCoreFilterModel.ClientSurname) &&
                    x.Managers.Surname.Contains(saleCoreFilterModel.ManagerSurname) &&
                    x.Products.Name.Contains(saleCoreFilterModel.ProductName)).ConfigureAwait(false);

        }

        public async Task<SaleDTO> GetAsync(int id)
        {
            return await SaleDbReaderWriter.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<SaleDTO> AddAsync(SaleDTO model)
        {
            return await SaleDbReaderWriter.AddAsync(model).ConfigureAwait(false);
        }

        public async Task<SaleDTO> UpdateAsync(SaleDTO model)
        {
            return await SaleDbReaderWriter.UpdateAsync(model).ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            await SaleDbReaderWriter.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<SaleDTO>> FindAsync(Expression<Func<SaleDTO, bool>> predicate)
        {
            return await SaleDbReaderWriter.FindAsync(predicate).ConfigureAwait(false);
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
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