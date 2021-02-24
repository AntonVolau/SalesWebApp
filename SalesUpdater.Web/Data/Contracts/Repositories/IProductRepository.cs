using SalesUpdater.Web.Data.Sales;
using System.Threading.Tasks;

namespace SalesUpdater.Web.Data.Contracts.Repositories
{
    public interface IProductRepository : IGenericRepository<ProductCoreModel>
    {
        Task<bool> TryAddUniqueProductAsync(ProductCoreModel productCoreModel);

        Task<int> GetIdAsync(string productName);

        Task<bool> DoesProductExistAsync(ProductCoreModel productCoreModel);
    }
}
