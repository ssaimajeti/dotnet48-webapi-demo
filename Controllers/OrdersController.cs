using MicroProjectApplication.Models;
using MicroProjectApplication.Services.Interfaces;
using System.Web.Http;

namespace MicroProjectApplication.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_orderService.GetAll());
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var order = _orderService.GetById(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(Order order)
        {
            var created = _orderService.Create(order);
            if (created == null)
                return BadRequest("Invalid order. Check CustomerId, ProductId, and Quantity.");

            return Ok(created);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var deleted = _orderService.Delete(id);
            if (!deleted)
                return NotFound();

            return Ok("Order deleted successfully.");
        }
    }
}