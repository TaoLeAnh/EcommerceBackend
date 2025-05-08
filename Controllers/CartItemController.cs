using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/cart-items")]
    public class CartItemController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CartItemController(AppDbContext context)
        {
            _context =context;

        }

        [HttpPost]
        public async Task<ActionResult<CartItem>> CreateCartItem([FromBody] CreateCartItemRequest request)
        {
            try
            {
                var cartItem = new CartItem
                {
                Quantity = request.Quantity,
                CartId = request.CartId,
                ProductId = request.ProductId
                };
                _context.CartItems.Add(cartItem);
                await _context.SaveChangesAsync();
                return Ok(
                    new{
                        message = "Đã thêm sản phẩm vào giỏ hàng"
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi", error = ex.Message});
            }
        }

        [HttpGet("cart/{cartId}")]
        public async Task<ActionResult<CartItem>> GetCartItem(int cartId)
        {
            try
            {
                var cartItem = await _context.CartItems.Where(q => q.CartId == cartId).Include(q => q.Product).ToListAsync();
                return Ok(cartItem);
            }catch (Exception ex)
            {
                return StatusCode(500, new { message = "Không thể truy xuất giỏ hàng", error = ex.Message});
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CartItem>> UpdateCartItem(int id, [FromBody] UpdateCartItemRequest request)
        {
            try{
                var cartItem = await _context.CartItems.FindAsync(id);
                if(cartItem == null)
                    return NotFound(new { message = "Không tìm thấy sản phẩm trong giỏ hàng" });
                cartItem.Quantity = request.Quantity;
                await _context.SaveChangesAsync();
                return Ok(
                    new{
                        meessage = "Cập nhật thành công"
                    }
                );
            }catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật giỏ hàng", error = ex.Message});
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CartItem>> DeleteCartItem(int id)
        {
            try{
                var cartItem = await _context.CartItems.FindAsync(id);
                if(cartItem == null)
                    return NotFound(new { message = "Không tìm thấy sản phẩm trong giỏ hàng"});
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();

                return Ok(
                    new{
                        message = "Đã xóa sản phẩm khỏi giỏ hàng"
                    }
                );
            }catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa sản phẩm trong giỏ hàng", error = ex.Message});
            }
        }
        [HttpDelete("cart/{cartId}")]
        public async Task<IActionResult> DeleteAllCartItem(int cartId)
        {
            try{
                var cartItem = await _context.CartItems.Where(q => q.CartId == cartId).ToListAsync();
                if(!cartItem.Any())
                    return NotFound(new { message = "Giỏ hàng trống hoặc không tồn tại" });
                _context.CartItems.RemoveRange(cartItem);            
                await _context.SaveChangesAsync();
                return Ok(
                    new{
                        message = "Đã xóa toàn bộ giỏ hàng"
                    }
                );
            }catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa giỏ hàng", error = ex.Message});
            }
        }

    }
    public class CreateCartItemRequest{
        public int CartId {get; set;}
        public int ProductId {get; set;}
        public int Quantity {get; set;}
    }

    public class UpdateCartItemRequest{
        public int Quantity {get; set;}
    }
}