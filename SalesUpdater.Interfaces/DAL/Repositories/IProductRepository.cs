using SalesUpdater.Interfaces.Core.DataTransferObject;
using System.Threading.Tasks;

namespace SalesUpdater.Interfaces.DAL.Repositories
{
    public interface IProductRepository : IRepository<ProductDTO>
    {
        void AddProductToDatabase(ProductDTO productDto);

        int GetId(string productName);

        Task<bool> TryAddProductAsync(ProductDTO productCoreModel);

        Task<int> GetIdAsync(string productName);

        Task<bool> DoesProductExistAsync(ProductDTO productCoreModel);
    }
}
