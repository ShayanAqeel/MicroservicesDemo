using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductMicroservice.Data;
using ProductMicroservice.Models;
using System.Security.Claims;

namespace ProductMicroservice.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductDbContext dbContext;

        public ProductController(ProductDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var userName = User.Identity.Name;

            // retrieve role from token
            var roleClaims = User.FindFirst(ClaimTypes.Role);
            var role = roleClaims?.Value;

            var products = dbContext.Products
                .Where(product => product.userName == userName)
                .ToListAsync();

            return Ok(new { UserName = userName, Role = role, Products = products });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(AddProductRequest addProductRequest)
        {
            var userName = User.Identity.Name;
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                name = addProductRequest.name,
                description = addProductRequest.description,
                price = addProductRequest.price,
                userName = userName,
            };

            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
            return Ok(product);
        }
    }
}
