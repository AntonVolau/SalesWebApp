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

        public int GetId(string clientName, string clientSurname)
        {
            Expression<Func<ClientDTO, bool>> predicate = x =>
                x.Name == clientName && x.Surname == clientSurname;

            return Find(predicate).First().ID;
        }
        public async Task<bool> TryAddClientAsync(ClientDTO clientCoreModel)
        {
            if (await DoesClientExistAsync(clientCoreModel).ConfigureAwait(false))
            {
                return false;
            }

            Add(clientCoreModel);
            return true;
        }

        public async Task<int> GetIdAsync(string clientName, string clientSurname)
        {
            Expression<Func<ClientDTO, bool>> predicate = x =>
                x.Name == clientName && x.Surname == clientSurname;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.First().ID;
        }

        public async Task<bool> DoesClientExistAsync(ClientDTO clientCoreModel)
        {
            Expression<Func<ClientDTO, bool>> predicate = x =>
                x.Surname == clientCoreModel.Surname && x.Name == clientCoreModel.Name;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.Any();
        }
    }
}
