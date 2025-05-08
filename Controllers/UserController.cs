// using Microsoft.AspNetCore.UseAuthorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
// using Data;

namespace Controllers
{
    [ApiController]
    [Route("api/users")]//Đường dẫn, định tuyến
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;//đọc dữ liệu hay ghi dữ liệu thông qua context

        public UserController(AppDbContext context){//context này là chung của cả service
            _context =context;
        }

        //Tạo mới người dùng(User)
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserRequest request) //async sử lý bất đồng bộ, khi có nhiều luồng cùng chạy phải đợi kết quả trc xong mới chạy tiếp;
            {
                //Kiểm tra xem UserName đã tồn tại hay chưa
                if (await _context.Users.AnyAsync(u => u.Username == request.Username))      
                    return BadRequest("Username đã tồn tại");

                var user = new User{
                    Username = request.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    FullName = request.FullName,
                    Role = "nhanvien",
                    Phone = request.Phone,
                    IsActive = true
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUser),new {id = user.Id}, user);
            }
            
        //Lấy người dùng
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id){
            var user = await _context.Users.FindAsync(id);
            if(user == null)
                return NotFound();
            return user;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<User>> UpdateUser([FromRoute] int id, [FromBody] UpdateUserRequest update){
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if(user == null)
                return NotFound();
            user.Username = update.Username;
            user.Password = BCrypt.Net.BCrypt.HashPassword(update.Password);
            user.FullName = update.FullName;
            // user.Role = update.Role;
            user.Phone = update.Phone;
            user.CreatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(user);
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<User>> DeleteUser([FromRoute] int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    

    public class CreateUserRequest{
        public String Username {get; set; }
        public String Password {get; set; }
        public String FullName {get; set; }
        public String Phone {get; set; }
    }

    public class UpdateUserRequest{
        public String Username {get; set; }
        public String Password {get; set; }
        public String FullName {get; set; }
        public String Phone {get; set; }
    }
}