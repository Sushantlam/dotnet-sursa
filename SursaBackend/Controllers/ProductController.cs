using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SursaBackend.AppDbContext;
using SursaBackend.Models;

namespace SursaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly SursaBackendDbContext _dbContext;
        public ProductController( SursaBackendDbContext dbContext)
        {
          
            _dbContext = dbContext;
        }

        [HttpPost("create")]
        public ActionResult<CreateProduct> Create(CreateProduct request)
        {
        

            
            var newProduct = new CreateProduct
            {
                Course = request.Course,
                Tutor = request.Tutor,
                Time = request.Time
            };

           
            _dbContext.Creates.Add(newProduct);
            _dbContext.SaveChanges();

            return Ok(newProduct);
        }

        [HttpGet("product")]
        public ActionResult<CreateProduct> GetAll()
        {


           var getAll = _dbContext.Creates.ToList();

            return Ok(getAll);
        }

        [HttpPut("update/{id}")]
        public IActionResult Update(Guid id, CreateProduct updatedProduct)
        {
            var existingProduct = _dbContext.Creates.FirstOrDefault(p => p.Id == id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            
            existingProduct.Course = updatedProduct.Course;
            existingProduct.Tutor = updatedProduct.Tutor;
            existingProduct.Time = updatedProduct.Time;

            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                
                throw;
            }

            return Ok(existingProduct); 
        }

        //get by id
        [HttpGet("product/{id}")]
        public ActionResult<CreateProduct> GetById(Guid id)
        {
            var product = _dbContext.Creates.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound(); 
            }

            return Ok(product);
        }

        //delete the product
        [HttpDelete("product/{id}")]
        public ActionResult Delete(Guid id)
        {
            var product = _dbContext.Creates.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound(); 
            }

            _dbContext.Creates.Remove(product);
            _dbContext.SaveChanges();

            return NoContent(); // Return 204 No Content upon successful deletion
        }

    }
}
