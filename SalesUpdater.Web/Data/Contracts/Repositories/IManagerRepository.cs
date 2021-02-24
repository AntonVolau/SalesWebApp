using SalesUpdater.Web.Data.Sales;
using System.Threading.Tasks;

namespace SalesUpdater.Web.Data.Contracts.Repositories
{
    interface IManagerRepository : IGenericRepository<ManagerCoreModel>
    {
        Task<bool> TryAddUniqueManagerAsync(ManagerCoreModel managerCoreModel);

        Task<int> GetIdAsync(string managerLastName);

        Task<bool> DoesManagerExistAsync(ManagerCoreModel managerCoreModel);
    }
}
