using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StefanPimentel_WebAPI.Data;
using StefanPimentel_WebAPI.Model;
using System.Collections;

namespace StefanPimentel_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {

            var products = await _context.Products.ToListAsync();

            return Ok(products);
        }
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return Ok(product);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpGet("search/{name}")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProducts(string name)
        {
            var products = await _context.Products.FirstOrDefaultAsync(p => p.Name.Contains(name));
            if (products == null)
            {
                return NotFound("Product name is not found!");
            }

            return Ok(products);
            //Get by product name
            ///api/Product/search/phone
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product updateProduct)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound("Product is not found!");
            }

            product.Name = updateProduct.Name;
            product.Price = updateProduct.Price;
            product.Stock = updateProduct.Stock;

            await _context.SaveChangesAsync();

            return Ok(product);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)

        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound("Product is not found!");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Product deleted successfully!");
        }
        [HttpGet("SearchPartialProductNane/{name}")]
        public async Task<IActionResult> SearchPartialProductName(string name)
        {
            var products = await _context.Products.Where(p => EF.Functions.Like(p.Name, $"%{name}%")).ToListAsync();

            if (products.Count == 0)
            {
                return NotFound("No products found with the given name.");
            }

            return Ok(products);
        }
    }
}