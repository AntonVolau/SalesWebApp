using SalesUpdater.Interfaces.Core.DataTransferObject;
using System.Threading.Tasks;

namespace SalesUpdater.Interfaces.DAL.Repositories
{
    public interface IClientRepository : IRepository<ClientDTO>
    {
        void AddClientToDatabase(ClientDTO clientDto);

        int GetId(string name, string surname);

        Task<bool> TryAddClientAsync(ClientDTO customerCoreModel);

        Task<int> GetIdAsync(string clientName, string clientSurname);

        Task<bool> DoesClientExistAsync(ClientDTO clientCoreModel);
    }
}
