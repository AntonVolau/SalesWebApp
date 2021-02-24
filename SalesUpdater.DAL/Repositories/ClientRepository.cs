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
    public class ClientRepository : Repository<ClientDTO, Clients>, IClientRepository
    {
        public ClientRepository(SalesContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public void AddClientToDatabase(ClientDTO clientDto)
        {
            Expression<Func<ClientDTO, bool>> predicate = x =>
                x.Surname == clientDto.Surname && x.Name == clientDto.Name;

            if (!Find(predicate).Any())
            {
                Add(clientDto);
            }
        }

        public int GetId(string ClientFirstName, string ClientLastName)
        {
            Expression<Func<ClientDTO, bool>> predicate = x =>
                x.Name == ClientFirstName && x.Surname == ClientLastName;

            return Find(predicate).First().ID;
        }
        public async Task<bool> TryAddUniqueClientAsync(ClientDTO customerCoreModel)
        {
            if (await DoesClientExistAsync(customerCoreModel).ConfigureAwait(false))
            {
                return false;
            }

            Add(customerCoreModel);
            return true;
        }

        public async Task<int> GetIdAsync(string customerFirstName, string customerLastName)
        {
            Expression<Func<ClientDTO, bool>> predicate = x =>
                x.Name == customerFirstName && x.Surname == customerLastName;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.First().ID;
        }

        public async Task<bool> DoesClientExistAsync(ClientDTO customerCoreModel)
        {
            Expression<Func<ClientDTO, bool>> predicate = x =>
                x.Surname == customerCoreModel.Surname && x.Name == customerCoreModel.Name;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.Any();
        }
    }
}
