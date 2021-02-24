using System.Collections.Generic;

namespace SalesUpdater.Entity
{
    public partial class Products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Products()
        {
            this.Sales = new HashSet<Sales>();
        }

        public int ID { get; set; }
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public virtual ICollection<Sales> Sales { get; set; }
    }
}
