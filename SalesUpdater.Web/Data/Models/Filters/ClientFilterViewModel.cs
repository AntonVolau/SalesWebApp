namespace SalesUpdater.Web.Data.Models.Filters
{
    public class ClientFilterViewModel : PagedListParameterViewModel
    {
        public string Name { get; set; } = null;

        public string Surname { get; set; } = null;
    }
}