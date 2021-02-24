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
    public class ClientRepository : GenericRepository<ClientCoreModel, Entity.Clients>, IClientRepository
    {
        public ClientRepository(SalesContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<bool> TryAddUniqueClientAsync(ClientCoreModel ClientCoreModel)
        {
            if (await DoesClientExistAsync(ClientCoreModel).ConfigureAwait(false))
            {
                return false;
            }

            Add(ClientCoreModel);
            return true;
        }

        public async Task<int> GetIdAsync(string ClientFirstName, string ClientLastName)
        {
            Expression<Func<ClientCoreModel, bool>> predicate = x =>
                x.Name == ClientFirstName && x.Surname == ClientLastName;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.First().Id;
        }

        public async Task<bool> DoesClientExistAsync(ClientCoreModel ClientCoreModel)
        {
            Expression<Func<ClientCoreModel, bool>> predicate = x =>
                x.Surname == ClientCoreModel.Surname && x.Name == ClientCoreModel.Name;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.Any();
        }
    }
}