namespace SalesUpdater.Web.Data.Models.Filters
{
    public class ClientFilterCoreModel : PagedListParameterCoreModel
    {
        public string Name { get; set; } = null;

        public string Surname { get; set; } = null;
    }
}