using Microsoft.AspNetCore.Mvc;

namespace OrderProcessingSystem.Controllers
{
    public class OrderHistoryController : Controller
    {
        private readonly IOrderHistoryService _service;

        public OrderHistoryController(IOrderHistoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetHistory(Guid orderId)
        {
            var history = await _service.GetHistoryAsync(orderId);

            return Ok(history);
        }
    }
}
