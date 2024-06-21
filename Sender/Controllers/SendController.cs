using Microsoft.AspNetCore.Mvc;
using Sender.Models;
using Sender.RabbitMQ;

namespace Sender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendController : ControllerBase
    {
        private readonly RabbitMQSendMessageDirectlyToQueue _rabbitMQSendMessageDirectlyToQueue;
        private readonly RabbitMQSendMessageToExchange _rabbitMQSendMessageToExchange;
        public SendController(RabbitMQSendMessageDirectlyToQueue rabbitMQSendMessageDirectlyToQueue, RabbitMQSendMessageToExchange rabbitMQSendMessageToExchange)
        {
            _rabbitMQSendMessageDirectlyToQueue = rabbitMQSendMessageDirectlyToQueue;
            _rabbitMQSendMessageToExchange = rabbitMQSendMessageToExchange;
        }

        [HttpPost("toQueue")]
        public HttpResponseMessage SendToQueue([FromBody] Message message)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                _rabbitMQSendMessageDirectlyToQueue.Send(message.message);
                response.StatusCode = System.Net.HttpStatusCode.Accepted;
            }
            catch (Exception ex)
            {
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Content = new StringContent(ex.Message);
            }
            return response;
        }
        [HttpPost("toExchange")]
        public HttpResponseMessage SendToExchange([FromBody] Message message)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                _rabbitMQSendMessageToExchange.Send(message.message);
                response.StatusCode = System.Net.HttpStatusCode.Accepted;
            }
            catch (Exception ex)
            {
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Content = new StringContent(ex.Message);
            }
            return response;
        }
    }

}
