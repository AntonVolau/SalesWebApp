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

        private IManagerRepository Managers { get; }

        public ManagerDbReaderWriter(SalesContext context)
        {
            Context = context;

            var mapper = AutoMapper.CreateConfiguration().CreateMapper();

            Managers = new ManagerRepository(Context, mapper);
        }

        public async Task<IPagedList<ManagerDTO>> GetPagedListAsync(int number, int size,
            Expression<Func<ManagerDTO, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            return await Managers.GetPagedListAsync(number, size, predicate).ConfigureAwait(false);
        }

        public async Task<ManagerDTO> GetAsync(int id)
        {
            return await Managers.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<ManagerDTO> AddAsync(ManagerDTO manager)
        {
                if (await Managers.TryAddManagerAsync(manager).ConfigureAwait(false))
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

        public async Task<ManagerDTO> UpdateAsync(ManagerDTO manager)
        {
                if (await Managers.DoesManagerExistAsync(manager).ConfigureAwait(false))
                {
                    throw new ArgumentException("Manager already exist!");
                }

                var result = Managers.Update(manager);
                await Managers.SaveAsync().ConfigureAwait(false);

                return result;
        }

        public async Task DeleteAsync(int id)
        {
                await Managers.DeleteAsync(id).ConfigureAwait(false);
                await Managers.SaveAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<ManagerDTO>> FindAsync(Expression<Func<ManagerDTO, bool>> predicate)
        {
            return await Managers.FindAsync(predicate).ConfigureAwait(false);
        }
    }
}