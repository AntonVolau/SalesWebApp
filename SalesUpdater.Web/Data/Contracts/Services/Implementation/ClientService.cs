using SalesUpdater.Entity;
using SalesUpdater.Interfaces.Core.DataTransferObject;
using SalesUpdater.Web.Data.Models.Filters;
using SalesUpdater.DAL.ReaderWriter;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using X.PagedList;
using SalesUpdater.DAL.ReaderWriter.Implementation;

namespace SalesUpdater.Web.Data.Contracts.Services.Implementation
{
    public class ClientService : IClientService, IDisposable
    {
        private SalesContext Context { get; }

        private ReaderWriterLockSlim Locker { get; }

        private IClientDbReaderWriter ClientDbReaderWriter { get; }

        public ClientService()
        {
            Context = new SalesContext();

            Locker = new ReaderWriterLockSlim();

            ClientDbReaderWriter = new ClientDbReaderWriter(Context, Locker);
        }

        public async Task<IPagedList<ClientDTO>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<ClientDTO, bool>> predicate = null,
            SortDirection sortDirection = SortDirection.Ascending)
        {
            return await ClientDbReaderWriter.GetUsingPagedListAsync(pageNumber, pageSize, predicate)
                .ConfigureAwait(false);
        }

        public async Task<IPagedList<ClientDTO>> Filter(ClientFilterCoreModel clientFilterCoreModel,
            int pageSize, SortDirection sortDirection = SortDirection.Ascending)
        {
            if (clientFilterCoreModel.Name == null && clientFilterCoreModel.Surname == null)
            {
                return await GetUsingPagedListAsync(clientFilterCoreModel.Page ?? 1, pageSize)
                    .ConfigureAwait(false);
            }

            return await GetUsingPagedListAsync(
                    clientFilterCoreModel.Page ?? 1, pageSize,
                    x => x.Name.Contains(clientFilterCoreModel.Name) ||
                         x.Surname.Contains(clientFilterCoreModel.Surname))
                .ConfigureAwait(false);
        }

        public async Task<ClientDTO> GetAsync(int id)
        {
            return await ClientDbReaderWriter.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<ClientDTO> AddAsync(ClientDTO model)
        {
            return await ClientDbReaderWriter.AddAsync(model).ConfigureAwait(false);
        }

        public async Task<ClientDTO> UpdateAsync(ClientDTO model)
        {
            return await ClientDbReaderWriter.UpdateAsync(model).ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            await ClientDbReaderWriter.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ClientDTO>> FindAsync(Expression<Func<ClientDTO, bool>> predicate)
        {
            return await ClientDbReaderWriter.FindAsync(predicate).ConfigureAwait(false);
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