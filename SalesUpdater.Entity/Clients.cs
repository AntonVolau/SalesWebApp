using System.Collections.Generic;

namespace SalesUpdater.Entity
{
    public partial class Clients
    {
        public Clients()
        {
            this.Sales = new HashSet<Sales>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public virtual ICollection<Sales> Sales { get; set; }
    }
}
