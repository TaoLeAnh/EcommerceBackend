using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/carts")]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CartController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> CreateCart([FromBody] CreateCartRequest request)
        {   
            var customer= await _context.Carts.FirstOrDefaultAsync(u => u.CustomerId == request.CustomerId);
            if(customer != null)
                return BadRequest("Giỏ hàng đã tồn tại");
        
            var cart = new Cart
            {
                CreatedAt = DateTime.UtcNow,
                CustomerId = request.CustomerId
            };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return Ok(cart);
        }
        //lấy toàn bộ sản phẩm
        [HttpGet("{customerId}")]
        public async Task<ActionResult<Cart>> GetCart(int customerId)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(u => u.CustomerId == customerId);
            if (cart == null)
                return NotFound();  
            return Ok(cart);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Cart>> DeleteCart(int id)
        {
            var cart =await _context.Carts.FirstOrDefaultAsync(u => u.Id == id);
            if(cart == null)
                return NotFound();
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return Ok(
                new{
                    message ="Xóa giỏ hàng thành công"
                }
            );
        }     
    }
    public class CreateCartRequest
    {
        public int CustomerId { get; set; }
    }
}