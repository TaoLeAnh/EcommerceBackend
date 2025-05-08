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
        
        var customer = new Customer{
            FullName = request.FullName,
            Phone = request.Phone,
            Address = request.Address,
            City = request.City,
            Country = request.Country,
            UserId = request.UserId
        };
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCustomer), new {id = customer.Id}, customer);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if(customer == null)
            return NotFound();
        return Ok(customer);
    }
    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult<Customer>> UpdateCustomer([FromRoute] int id, [FromBody] UpdateCustomerRequest request)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(u => u.Id == id);
        if(customer == null)
            return NotFound();
        customer.FullName = request.FullName;
        customer.Phone = request.Phone;
        customer.Address = request.Address;
        customer.City = request.City;
        customer.Country = request.Country;

        await _context.SaveChangesAsync();
        return Ok(customer);
    }
    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult<Customer>> DeleteCustomer([FromRoute] int id)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(u => u.Id == id);
        if(customer == null)
            return NotFound();
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return NoContent();
    }
   } 
   public class CreateCustomerRequest
   {    
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int UserId { get; set; }
    }

    public class UpdateCustomerRequest
    {    
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int UserId { get; set; }
    }
}