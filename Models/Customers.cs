using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Customers")]
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get; set;}    

        [Required]
        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string FullName { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar(20)")]
        public string Phone { get; set; }
        
        [Required]
        [StringLength(255)]
        [Column(TypeName = "varchar(255)")]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string City { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string Country { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId {get; set;}

        public User? User {get; set;}
    }
}