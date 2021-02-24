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
    public class ManagerDbReaderWriter : IManagerDbReaderWriter
    {
        private SalesContext Context { get; }

        private ReaderWriterLockSlim Locker { get; }

        private IManagerRepository Managers { get; }

        public ManagerDbReaderWriter(SalesContext context, ReaderWriterLockSlim locker)
        {
            Context = context;

            Locker = locker;

            var mapper = AutoMapper.CreateConfiguration().CreateMapper();

            Managers = new ManagerRepository(Context, mapper);
        }

        public async Task<IPagedList<ManagerDTO>> GetUsingPagedListAsync(int number, int size,
            Expression<Func<ManagerDTO, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            return await Managers.GetUsingPagedListAsync(number, size, predicate).ConfigureAwait(false);
        }

        public async Task<ManagerDTO> GetAsync(int id)
        {
            return await Managers.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<ManagerDTO> AddAsync(ManagerDTO manager)
        {
            Locker.EnterWriteLock();
            try
            {
                if (await Managers.TryAddUniqueManagerAsync(manager).ConfigureAwait(false))
                {
                    await Managers.SaveAsync().ConfigureAwait(false);
                    return await GetAsync(await Managers.GetIdAsync(manager.Surname)
                        .ConfigureAwait(false)).ConfigureAwait(false);
                }
                else
                {
                    throw new ArgumentException("Manager already exists!");
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

        public async Task<ManagerDTO> UpdateAsync(ManagerDTO manager)
        {
            Locker.EnterWriteLock();
            try
            {
                if (await Managers.DoesManagerExistAsync(manager).ConfigureAwait(false))
                    throw new ArgumentException("Manager already exists!");

                var result = Managers.Update(manager);
                await Managers.SaveAsync().ConfigureAwait(false);

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
                await Managers.DeleteAsync(id).ConfigureAwait(false);
                await Managers.SaveAsync().ConfigureAwait(false);
            }
            finally
            {
                if (Locker.IsReadLockHeld)
                {
                    Locker.ExitReadLock();
                }
            }
        }

        public async Task<IEnumerable<ManagerDTO>> FindAsync(Expression<Func<ManagerDTO, bool>> predicate)
        {
            return await Managers.FindAsync(predicate).ConfigureAwait(false);
        }
    }
}