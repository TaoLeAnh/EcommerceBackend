using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

        [StringLength(255)]
        [Column(TypeName = "varchar(255)")]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int Stock { get; set; }

        [StringLength(255)]
        [Column(TypeName = "varchar(255)")]
        public string ImageUrl { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow; 

        [Required]
        [ForeignKey("Category")]//khóa ngoài gắn đến Category
        public int CategoryId { get; set; }

        public Category? Category { get; set; } //tạo liên kết với Category b��ng CategoryId hay là JOIN
    }
}