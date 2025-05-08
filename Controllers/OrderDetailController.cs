using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/order-details")]
    public class OrderDetailController : ControllerBase
    {
        private readonly AppDbContext _context;
        public OrderDetailController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var OrderDetail = await _context.OrderDetails.Include(o => o.Product).Include(o => o.Order).ToListAsync();
            return Ok(OrderDetail);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetail>> GetOrderDetail(int id)
        {
            var orderDetail = await _context.OrderDetails.Include(o => o.Product).Include(o => o.Order).FirstOrDefaultAsync(o => o.Id == id);
            if (orderDetail == null)
            {
                return NotFound( new { message = "Không tìm thấy chi tiết đơn hàng" });
            }
            return Ok(orderDetail);
        }

        [HttpGet("oder/{orderId}")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var orderDetail = await _context.OrderDetails.Include(o => o.Product).Where(o => o.OrderId == orderId).ToListAsync();
            if(orderDetail == null || !orderDetail.Any())
            {
                return NotFound(new {message = "Không tìm thấy chi tiết đơn hàng"});
            }

            var result = orderDetail.Select(d=> new OrderDetailResponse
            {
                ProductId = d.ProductId,
                ProductName = d.Product?.Name,
                ImageUrl = d.Product?.ImageUrl,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice,
                TotalPrice = d.Quantity * d.UnitPrice
            }).ToList();

            var grandTotal = result.Sum(d=> d.TotalPrice);

            return Ok(new {
                CartItems = result,
                TotalAmount = grandTotal
            });
        }
        [HttpPost]
        public async Task<ActionResult<OrderDetail>> CreateOrderDetail([FromBody] CreateOrderDetailRequest request)
        {
            try{
                var orderDetail = new OrderDetail
                {
                    OrderId = request.OrderId,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    UnitPrice = request.UnitPrice
                };
                _context.OrderDetails.Add(orderDetail);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Tạo chi tiết đơn hàng thành công" , data = request});
            }catch(Exception ex){
                return StatusCode(500, new {message =  "Lỗi khi tạo chi tiết đơn hàng", error = ex.Message});
        }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDetail>> UpdateOrderDetail(int id, [FromBody] UpdateOrderDetailRequest request)
        {
            var orderDetail = await _context.OrderDetails.FirstOrDefaultAsync(o=> o.Id == id);
            if(orderDetail == null)
            {
                return NotFound(new { message = "Không tìm thấy chi tiết đơn hàng" });
            }
            try
            {
                orderDetail.Quantity = request.Quantity;
                orderDetail.UnitPrice = request.UnitPrice;
                orderDetail.ProductId = request.ProductId;
                orderDetail.OrderId = request.OrderId;

                await _context.SaveChangesAsync();
                return Ok(new { message = "Cập nhật chi tiết đơn hàng thành công", data = orderDetail });
            }catch(Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật chi tiết đơn hàng", error = ex.Message });
            }
        }
        [HttpDelete("{id}")]
            public async Task<ActionResult<OrderDetail>> DeleteOrderDetail(int id)
            {
                var orderDetail = await _context.OrderDetails.FirstOrDefaultAsync(o => o.Id ==id);
                if(orderDetail == null)
                    return NotFound(new { message = "Không tìm thấy chi tiết đơn hàng" });
                try
                {
                    _context.OrderDetails.Remove(orderDetail);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Xóa chi tiết đơn hàng thành công" });
                }
                catch(Exception ex)
                {
                    return StatusCode(500, new { message = "Lỗi khi xóa chi tiết đơn hàng", error = ex.Message });
                }
            }
    }
    public class CreateOrderDetailRequest
    {
        public int OrderId { get; set;}
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
    public class UpdateOrderDetailRequest
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity {get ; set;}
        public decimal UnitPrice { get; set; }
    }
    public class OrderDetailResponse
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}