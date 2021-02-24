using System.Collections.Generic;

namespace SalesUpdater.Entity
{
    public partial class Managers
    {
        public Managers()
        {
            this.Sales = new HashSet<Sales>();
        }

        public int ID { get; set; }
        public string Surname { get; set; }
        public virtual ICollection<Sales> Sales { get; set; }
    }
}
