using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class Order
    {
        [Key]
        public string Id { get; set; }
        public string Ticker { get; set; }
        public decimal LimitPrice { get; set; }
        public int Quantity { get; set; }
        public bool Filled { get; set; } = false;
        public decimal Price { get; set; }
    }
}
