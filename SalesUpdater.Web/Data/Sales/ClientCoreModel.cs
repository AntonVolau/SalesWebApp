namespace SalesUpdater.Web.Data.Sales
{
    public class ClientCoreModel : CoreModel
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public ClientCoreModel()
        {
        }

        public ClientCoreModel(string firstName, string lastName)
        {
            Name = firstName;
            Surname = lastName;
        }
    }
}