namespace SalesUpdater.Interfaces.Core.DataTransferObject
{
    public class ProductDTO : CoreModel
    {
        public string Name { get; set; }
        public ProductDTO()
        {
        }
        public ProductDTO(string name)
        {
            Name = name;
        }
    }
}
