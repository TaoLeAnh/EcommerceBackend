using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Categories")]
    public class Category 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get; set;}

        [Required]// tự check notnull
        [StringLength (100)]// tự check giới hạn độ dài
        [Column(TypeName = "nvarchar(100)")]
        public string Name {get; set;}

        [StringLength (255)]
        [Column(TypeName = "nvarchar(255)")]
        public string? Description {get; set;}//dấu hỏi chấm là cho phép null

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

    }
}