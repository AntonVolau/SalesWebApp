using SalesUpdater.Interfaces.Core.DataTransferObject;
using System.Threading.Tasks;

namespace SalesUpdater.Interfaces.DAL.Repositories
{
    public interface IManagerRepository: IRepository<ManagerDTO>
    {
        void AddManagerToDatabase(ManagerDTO managerDto);

        int GetId(string managerSurname);

        Task<bool> TryAddManagerAsync(ManagerDTO managerCoreModel);

        Task<int> GetIdAsync(string managerLastName);

        Task<bool> DoesManagerExistAsync(ManagerDTO managerCoreModel);
    }
}
