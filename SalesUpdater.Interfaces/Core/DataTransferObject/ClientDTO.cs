namespace SalesUpdater.Interfaces.Core.DataTransferObject
{
    public class ClientDTO : CoreModel
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public ClientDTO()
        {
        }

        public ClientDTO(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }
    }
}
