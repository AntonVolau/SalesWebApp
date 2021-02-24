using AutoMapper;
using SalesUpdater.Entity;
using SalesUpdater.Interfaces.Core.DataTransferObject;
using SalesUpdater.Interfaces.DAL.Repositories;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SalesUpdater.DAL.Repositories
{
    public class ManagerRepository : Repository<ManagerDTO, Managers>, IManagerRepository
    {
        public ManagerRepository(SalesContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public void AddManagerToDatabase(ManagerDTO managerDto)
    {
        Expression<Func<ManagerDTO, bool>> predicate = x => x.Surname == managerDto.Surname;

        if (Find(predicate).Any()) return;

        Add(managerDto);
    }

    public int GetId(string managerLastName)
    {
        Expression<Func<ManagerDTO, bool>> predicate = x => x.Surname == managerLastName;

        return Find(predicate).First().ID;
    }
        public async Task<bool> TryAddUniqueManagerAsync(ManagerDTO managerDto)
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
            Expression<Func<ManagerDTO, bool>> predicate = x => x.Surname == managerLastName;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.First().ID;
        }

        public async Task<bool> DoesManagerExistAsync(ManagerDTO managerDto)
        {
            Expression<Func<ManagerDTO, bool>> predicate = x => x.Surname == managerDto.Surname;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.Any();
        }
    }
}
