using Common.Client;
using Common.Data;
using Common.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Sender.Models.DTO;
using System;

namespace Sender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly StockClient _stockClient;
        private readonly AppDBContext _appDBContext;
        private readonly IPublishEndpoint _publishEndpoint;
        public StockController(StockClient stockClient, AppDBContext appDBContext, IPublishEndpoint publishEndpoint)
        {
            _stockClient = stockClient;
            _appDBContext = appDBContext;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("/{ticker}")]
        public async Task<IResult> Get(string ticker)
        {
            var stockRes = await _stockClient.GetDataForTicker(ticker);

            return stockRes != null ? Results.Ok(stockRes) : Results.NotFound(); ;
        }

        [HttpPost]
        public async Task<IResult> Post([FromBody] PurchaseOrderRequest orderRequest)
        {
            string id = Guid.NewGuid().ToString();
            var order = new Order
            {
                Id = id,
                Ticker = orderRequest.Ticker,
                LimitPrice = orderRequest.LimitPrice,
                Quantity = orderRequest.Quantity,
            };
            _appDBContext.Orders.Add(order);

            await _appDBContext.SaveChangesAsync();

            await _publishEndpoint.Publish(new PurchaseOrderSent(id));

            return Results.Created();
        }
    }
}
