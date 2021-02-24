using SalesUpdater.Interfaces.Core.DataTransferObject;
using SalesUpdater.Web.Data.Models.Filters;

namespace SalesUpdater.Web.Data.Contracts.Services
{
    public interface IProductService : IService<ProductDTO, ProductFilterCoreModel>
    {
    }
}
