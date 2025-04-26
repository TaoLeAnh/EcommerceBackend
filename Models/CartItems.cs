using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("CartItems")]
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get; set;}

        [Required]
        [Column(TypeName = "int")]
        public int Quantity { get; set; }

        [Required]
        [ForeignKey("Cart")]
        public int CartId { get; set; }

        public Cart? Cart { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public Product? Product { get; set; }
    }
}