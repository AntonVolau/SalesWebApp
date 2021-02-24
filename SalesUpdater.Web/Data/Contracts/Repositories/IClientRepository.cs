using SalesUpdater.Web.Data.Sales;
using System.Threading.Tasks;

namespace SalesUpdater.Web.Data.Contracts.Repositories
{
    interface IClientRepository : IGenericRepository<ClientCoreModel>
    {
        Task<bool> TryAddUniqueClientAsync(ClientCoreModel ClientCoreModel);

        Task<int> GetIdAsync(string ClientFirstName, string ClientLastName);

        Task<bool> DoesClientExistAsync(ClientCoreModel ClientCoreModel);
    }
}
