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

        private ReaderWriterLockSlim Locker { get; }

        private ISaleDbReaderWriter SaleDbReaderWriter { get; }

        public SaleService()
        {
            Context = new SalesContext();

            Locker = new ReaderWriterLockSlim();

            SaleDbReaderWriter = new SaleDbReaderWriter(Context, Locker);
        }

        public async Task<IPagedList<SaleDTO>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<SaleDTO, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            return await SaleDbReaderWriter.GetUsingPagedListAsync(pageNumber, pageSize, predicate)
                .ConfigureAwait(false);
        }

        public async Task<IPagedList<SaleDTO>> Filter(SaleFilterCoreModel saleFilterCoreModel,
            int pageSize, SortDirection sortDirection = SortDirection.Ascending)
        {
            if (saleFilterCoreModel.ClientName == null &&
                saleFilterCoreModel.ClientSurname == null &&
                saleFilterCoreModel.DateFrom == null &&
                saleFilterCoreModel.DateTo == null &&
                saleFilterCoreModel.ManagerSurname == null &&
                saleFilterCoreModel.ProductName == null &&
                saleFilterCoreModel.SumFrom == null &&
                saleFilterCoreModel.SumTo == null)
            {
                return await GetUsingPagedListAsync(saleFilterCoreModel.Page ?? 1, pageSize)
                    .ConfigureAwait(false);
            }

            return await GetUsingPagedListAsync(
                saleFilterCoreModel.Page ?? 1, pageSize, x =>
                    (x.Date >= saleFilterCoreModel.DateFrom && x.Date <= saleFilterCoreModel.DateTo) &&
                    (x.Sum >= saleFilterCoreModel.SumFrom && x.Sum <= saleFilterCoreModel.SumTo) &&
                    x.Clients.Name.Contains(saleFilterCoreModel.ClientName) &&
                    x.Clients.Surname.Contains(saleFilterCoreModel.ClientSurname) &&
                    x.Managers.Surname.Contains(saleFilterCoreModel.ManagerSurname) &&
                    x.Products.Name.Contains(saleFilterCoreModel.ProductName)).ConfigureAwait(false);

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