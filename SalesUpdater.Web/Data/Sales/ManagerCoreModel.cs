namespace SalesUpdater.Web.Data.Sales
{
    public class ManagerCoreModel : CoreModel
    {
        public string Surname { get; set; }

        public ManagerCoreModel()
        {
        }

        public ManagerCoreModel(string lastName)
        {
            Surname = lastName;
        }
    }
}