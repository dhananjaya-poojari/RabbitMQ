namespace Common.Models
{
    public class PurchaseOrderSent
    {
        public PurchaseOrderSent(string id)
        {
            Id = id;
        }
        public string Id { get; private set; }
    }
}
