using CustomerMicroservice.Data;
using CustomerMicroservice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace CustomerMicroservice.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerDbContext dbContext;

        public CustomerController(CustomerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var username = User.Identity.Name;

            // Retrieve role from token
            var roleClaim = User.FindFirst(ClaimTypes.Role);
            var role = roleClaim?.Value;

            var customers = await dbContext.Customers
                .Where(c => c.userName == username)
                .ToListAsync();

            return Ok(new { UserName = username, Role = role, Customers = customers });
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCustomer([FromRoute] Guid id)
        {
            var customer = await dbContext.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(AddCustomerRequest addCustomerRequest)
        {
            var userName = User.Identity.Name;
            var customer = new Customer()
            {
                Id = Guid.NewGuid(),
                firstName = addCustomerRequest.firstName,
                lastName = addCustomerRequest.lastName,
                email = addCustomerRequest.email,
                userName = userName,
            };

            await dbContext.Customers.AddAsync(customer);
            await dbContext.SaveChangesAsync();
            return Ok(customer);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> EditCustomer([FromRoute] Guid id ,EditCustomerRequest editCustomerRequest)
        {

            var customer = dbContext.Customers.Find(id);

            if (customer != null)
            {
                customer.firstName = editCustomerRequest.firstName;
                customer.lastName = editCustomerRequest.lastName;  
                customer.email = editCustomerRequest.email;

                await dbContext.SaveChangesAsync();
                return Ok(customer);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id)
        {
            var customer = dbContext.Customers.Find(id);
            if (customer != null)
            {
                dbContext.Customers.Remove(customer);
                await dbContext.SaveChangesAsync();
                return Ok(customer);
            }

            return NotFound();
        }

        //public async Task<bool> UsernameExistsInAuthService(string username)
        //{
        //    // Replace with actual service implementation (HttpClient or dedicated service class)
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://your-auth-microservice-address/"); // Replace with actual URL
        //        var response = await client.GetAsync($"/api/users/exists/{username}");
        //        response.EnsureSuccessStatusCode(); // Handle errors if needed

        //        var contentString = await response.Content.ReadAsStringAsync();
        //        return JsonConvert.DeserializeObject<bool>(contentString); // Assuming a simple JSON response
        //    }
        //}
    }
}
