namespace SalesUpdater.Interfaces.Core
{
    public interface IFile
    {
        string Date { get; set; }
        string Client { get; set; }
        string Product { get; set; }
        string Price { get; set; }
    }
}
