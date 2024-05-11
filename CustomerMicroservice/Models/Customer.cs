using System.ComponentModel.DataAnnotations;

namespace CustomerMicroservice.Models
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string userName { get; set; }

        [Required]
        [StringLength(100)]
        public string firstName { get; set; }

        [Required]
        [StringLength(100)]
        public string lastName { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }
    }
}
