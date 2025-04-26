using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Carts")]
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set;}

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt {get; set; } = DateTime.UtcNow;

        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        public Customer? Customer {get; set;}
    }
}