namespace Sender.Models.DTO
{
    public class PurchaseOrderRequest
    {
        public string Ticker { get; set; }
        public decimal LimitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
