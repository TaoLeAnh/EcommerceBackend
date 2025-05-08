using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;
        public OrderController(AppDbContext context)
        {
            _context =context;

        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {   
                //Lấy giỏ hàng của khách hàng
                var cart = await _context.Carts.Include(q=>q.Customer).Include(q=> q.CartItems).ThenInclude(q=> q.Product).FirstOrDefaultAsync(q => q.CustomerId == request.CustomerId);
                if(cart == null || cart.CartItems.Count == 0 || !cart.CartItems.Any())
                {
                    return BadRequest(new { message = "Giỏ hàng không tồn tại hoặc rỗng" });
                }
                //Tính tổng tiền của giỏ hàng
                decimal totalAmount = cart.CartItems.Sum(c=> c.Quantity * c.Product.Price);
                
                //Tạo đơn hàng mới
                var order= new Order
                {
                    CustomerId = request.CustomerId,
                    TotalAmount = totalAmount,
                    Status = "Pending",
                    OrderDate = DateTime.Now,
                    Customer = cart.Customer
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                //Tạo danh sách các sản phẩm trong đơn hàng
                var orderItems = cart.CartItems.Select(c=> new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    UnitPrice = c.Product.Price
                }).ToList();
                _context.OrderDetails.AddRange(orderItems);
                
                //Xóa các sản phẩm trong giỏ hàng sau khi tạo đơn hàng
                _context.CartItems.RemoveRange(cart.CartItems); 
                await _context.SaveChangesAsync();
                return Ok(new { message = "Đơn hàng thành công ", orderId = order.Id,TotalAmount = totalAmount});
            }catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tạo đơn hàng", error = ex.Message});
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders.Include(o=>o.OrderDetails).ThenInclude(o=>o.Product).ToListAsync();

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _context.Orders.Include(o=>o.OrderDetails).ThenInclude(o=>o.Product).FirstOrDefaultAsync(o=> o.Id == id);
            if(order == null)
                return NotFound(new { message = "Không tìm thấy đơn hàng" });
            return Ok(order);
        }
    }
    public class CreateOrderRequest
    {
        public int CustomerId { get; set; }
    }
}