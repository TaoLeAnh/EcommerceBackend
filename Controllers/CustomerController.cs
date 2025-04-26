using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers
{
   [ApiController]
   [Route("api/customers")]

   public class CustomerController : ControllerBase
   {
    private readonly AppDbContext _context;

    public CustomerController(AppDbContext context){
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> CreateCustomer([FromBody] CreateCustomerRequest request)
    {
        if (await _context.Customers.AnyAsync(c => c.FullName == request.FullName))
            return BadRequest("Customer đã tồn tại");
        
        var Customer = new Customer{
            FullName = request.FullName,
            Phone = request.Phone,
            Address = request.Address,
            City = request.City,
            Country = request.Country
        };
        _context.Customers.Add(Customer);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCustomer), new {id = Customer.Id}, Customer);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if(customer == null)
            return NotFound();
        return customer;
    }
   } 
   public class CreateCustomerRequest
   {    
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}