using Common.Client;
using Common.Data;
using Common.Models;
using MassTransit;

namespace Sender.RabbitMQ
{
    public class PurchaseOrderSentConsumer(StockClient stockClient, AppDBContext appDBContext) : IConsumer<PurchaseOrderSent>
    {
        public async Task Consume(ConsumeContext<PurchaseOrderSent> context)
        {
            var order = await appDBContext.Orders.FindAsync(context.Message.Id); // this will return null always as we are InMemory Db
            if (order == null)
            {
                return;
            }
            var stockResponse = await stockClient.GetDataForTicker(order.Ticker);

            var lastPrice = stockResponse.Price.High;
            if (lastPrice <= order.LimitPrice)
            {
                order.Filled = true;
                order.Price = lastPrice;
            }
        }
    }
}
