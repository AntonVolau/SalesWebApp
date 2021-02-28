using SalesUpdater.Interfaces.Core.DataTransferObject;
using SalesUpdater.Web.Data.Models.Filters;

namespace SalesUpdater.Web.Data.Contracts.Services
{
    public interface IClientService : IService<ClientDTO, ClientCoreFilterModel>
    {
    }
}
