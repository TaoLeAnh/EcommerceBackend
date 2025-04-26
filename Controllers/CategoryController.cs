// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Models;

// namespace Controllers
// {
//     [ApiController]
//     [Route("api/categories")]
//     public class CategoryController : ControllerBase
//     {
//         private readonly AppDbContext _context;
        
//         public CategoryController(AppDbContext context)
//         {
//             _context = context;
//         }

//         [HttpPost]
//         public async Task<ActionResult<Category>> CreateCategory([FromBody] CreateCategoryRequest request)//lưu thông tin lấy từ Body FE vào request
//         {
//             try{
//                 var category = new Category{
//                 Name = request.Name,
//                 Description = request.Description
//                 };
//                 _context.Categories.Add(category);//thêm vào bảng Category sau khi lấy dữ liệu từ request
//                 await _context.SaveChangesAsync();//lưu vào database
//                 return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
//             }catch(Exceoption ex){
//                 return BadRequest(ex.Message);
//             }
//         }
//     }
//     public class CreateCategoryRequest
//     {
//         public string Name { get; set; }
//         public string? Description { get; set; }
//     }
// }