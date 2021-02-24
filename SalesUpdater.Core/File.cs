using SalesUpdater.Interfaces.Core;

namespace SalesUpdater.Core
{
    public class File : IFile
    {
        public string Date { get; set; }
        public string Client { get; set; }
        public string Product { get; set; }
        public string Price { get; set; }
    }
}
