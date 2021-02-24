namespace SalesUpdater.Interfaces.Core.DataTransferObject
{
    public class ManagerDTO : CoreModel
    {
        public string Surname { get; set; }
        public ManagerDTO()
        {
        }
        public ManagerDTO(string surname)
        {
            Surname = surname;
        }
    }
}
