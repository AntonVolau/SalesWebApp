namespace SalesUpdater.Web.Data.Models.Filters
{
    public class ClientViewFilterModel : PagedListParameterViewModel
    {
        public string Name { get; set; } = null;

        public string Surname { get; set; } = null;
    }
}