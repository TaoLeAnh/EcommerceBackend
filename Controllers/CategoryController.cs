using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] CreateCategoryRequest request)//lưu thông tin lấy từ Body FE vào request
        {
            try{
                var category = new Category{
                Name = request.Name,
                Description = request.Description
                };
                _context.Categories.Add(category);//thêm vào bảng Category sau khi lấy dữ liệu từ request
                await _context.SaveChangesAsync();//lưu vào database
                return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
            }catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }
        // /Lấy 1 Category theo Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(q => q.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        // /Lấy tất cả Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }
        // /Cập nhật Category
        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> UpdateCategory(int id, [FromBody] UpdateCategoryRequest request)//request lay tu FE
        {
            var category = await _context.Categories.FirstOrDefaultAsync(q => q.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            category.Name = request.Name;
            category.Description = request.Description;

            await _context.SaveChangesAsync();
            return Ok(
                new{
                    message = "Cập nhật thành công"
                }
            );
        }
        // /Xóa Category
        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> DeleteCategory(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(q=> q.Id == id);
            if(category == null)
                return NotFound();
            
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Ok(
                new{
                    message = "Xóa thành công"
                }
            );
    }
    }
    public class CreateCategoryRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
    public class UpdateCategoryRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}