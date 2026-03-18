using MicroProjectApplication.Models;
using MicroProjectApplication.Services.Interfaces;
using System.Web.Http;

namespace MicroProjectApplication.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_productService.GetAll());
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var product = _productService.GetById(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(Product product)
        {
            var created = _productService.Create(product);
            if (created == null)
                return BadRequest("Invalid product.");

            return Ok(created);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id, Product product)
        {
            var updated = _productService.Update(id, product);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var deleted = _productService.Delete(id);
            if (!deleted)
                return NotFound();

            return Ok("Product deleted successfully.");
        }
    }
}