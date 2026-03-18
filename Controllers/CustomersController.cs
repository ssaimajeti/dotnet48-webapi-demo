using MicroProjectApplication.Models;
using MicroProjectApplication.Services.Interfaces;
using System.Web.Http;

namespace MicroProjectApplication.Controllers
{
    [RoutePrefix("api/customers")]
    public class CustomersController : ApiController
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_customerService.GetAll());
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var customer = _customerService.GetById(id);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(Customer customer)
        {
            var created = _customerService.Create(customer);
            if (created == null)
                return BadRequest("Invalid customer.");

            return Ok(created);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id, Customer customer)
        {
            var updated = _customerService.Update(id, customer);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var deleted = _customerService.Delete(id);
            if (!deleted)
                return NotFound();

            return Ok("Customer deleted successfully.");
        }
    }
}