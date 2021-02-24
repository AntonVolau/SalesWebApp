namespace SalesUpdater.Web.Data.Sales
{
    public class ProductCoreModel : CoreModel
    {
        public string Name { get; set; }

        public ProductCoreModel()
        {
        }

        public ProductCoreModel(string name)
        {
            Name = name;
        }
    }
}