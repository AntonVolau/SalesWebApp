using AutoMapper;
using SalesUpdater.Entity;
using SalesUpdater.Web.Data.Contracts.Repositories;
using SalesUpdater.Web.Data.Sales;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SalesUpdater.Web.Data.Repositories
{
    public class ManagerRepository : GenericRepository<ManagerCoreModel, Entity.Managers>, IManagerRepository
    {
        public ManagerRepository(SalesContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<bool> TryAddUniqueManagerAsync(ManagerCoreModel managerDto)
        {
            if (await DoesManagerExistAsync(managerDto).ConfigureAwait(false))
            {
                return false;
            }

            Add(managerDto);
            return true;
        }

        public async Task<int> GetIdAsync(string managerLastName)
        {
            Expression<Func<ManagerCoreModel, bool>> predicate = x => x.Surname == managerLastName;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.First().Id;
        }

        public async Task<bool> DoesManagerExistAsync(ManagerCoreModel managerDto)
        {
            Expression<Func<ManagerCoreModel, bool>> predicate = x => x.Surname == managerDto.Surname;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.Any();
        }
    }
}