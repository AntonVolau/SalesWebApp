namespace SalesUpdater.Entity
{
    public partial class Sales
    {
        public int ID { get; set; }
        public int ManagerId { get; set; }
        public int ProductId { get; set; }
        public int ClientId { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Sum { get; set; }

        public virtual Managers Managers { get; set; }
        public virtual Products Products { get; set; }
        public virtual Clients Clients { get; set; }
    }
}
