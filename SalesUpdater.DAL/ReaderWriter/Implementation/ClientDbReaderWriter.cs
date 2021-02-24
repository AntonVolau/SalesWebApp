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
    public class ClientDbReaderWriter : IClientDbReaderWriter
    {
        private SalesContext Context { get; }

        private ReaderWriterLockSlim Locker { get; }

        private IClientRepository Clients { get; }

        public ClientDbReaderWriter(SalesContext context, ReaderWriterLockSlim locker)
        {
            Context = context;

            Locker = locker;

            var mapper = AutoMapper.CreateConfiguration().CreateMapper();

            Clients = new ClientRepository(Context, mapper);
        }

        public async Task<IPagedList<ClientDTO>> GetUsingPagedListAsync(int number, int size,
            Expression<Func<ClientDTO, bool>> predicate = null,
            SortDirection sortDirection = SortDirection.Ascending)
        {
            return await Clients.GetUsingPagedListAsync(number, size, predicate).ConfigureAwait(false);
        }

        public async Task<ClientDTO> GetAsync(int id)
        {
            return await Clients.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<ClientDTO> AddAsync(ClientDTO client)
        {
            Locker.EnterWriteLock();
            try
            {
                if (await Clients.TryAddUniqueClientAsync(client).ConfigureAwait(false))
                {
                    await Clients.SaveAsync().ConfigureAwait(false);
                    return await GetAsync(await Clients.GetIdAsync(client.Name, client.Surname)
                        .ConfigureAwait(false)).ConfigureAwait(false);
                }
                else
                {
                    throw new ArgumentException("Client already exists!");
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

        public async Task<ClientDTO> UpdateAsync(ClientDTO client)
        {
            Locker.EnterWriteLock();
            try
            {
                if (await Clients.DoesClientExistAsync(client).ConfigureAwait(false))
                    throw new ArgumentException("Client already exists!");

                var result = Clients.Update(client);
                await Clients.SaveAsync().ConfigureAwait(false);

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
                await Clients.DeleteAsync(id).ConfigureAwait(false);
                await Clients.SaveAsync().ConfigureAwait(false);
            }
            finally
            {
                if (Locker.IsReadLockHeld)
                {
                    Locker.ExitReadLock();
                }
            }
        }

        public async Task<IEnumerable<ClientDTO>> FindAsync(Expression<Func<ClientDTO, bool>> predicate)
        {
            return await Clients.FindAsync(predicate).ConfigureAwait(false);
        }
    }
}