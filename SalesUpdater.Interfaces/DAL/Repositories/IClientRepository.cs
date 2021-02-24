using SalesUpdater.Interfaces.Core.DataTransferObject;
using System.Threading.Tasks;

namespace SalesUpdater.Interfaces.DAL.Repositories
{
    public interface IClientRepository : IRepository<ClientDTO>
    {
        void AddClientToDatabase(ClientDTO clientDto);

        int GetId(string name, string surname);

        Task<bool> TryAddUniqueClientAsync(ClientDTO customerCoreModel);

        Task<int> GetIdAsync(string customerFirstName, string customerLastName);

        Task<bool> DoesClientExistAsync(ClientDTO customerCoreModel);
    }
}
