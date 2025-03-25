using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToursAndTravells.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [DisplayName("Full Name")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [DisplayName("Email Address")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [DisplayName("Phone Number")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string UserPhone { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters.")]
        public string UserPassword { get; set; }

        // ✅ Define foreign keys explicitly
        public int CountryId { get; set; }
        public int StateId { get; set; }

        // ✅ Ensure explicit mapping to the correct FK
        [ForeignKey(nameof(CountryId))]
        public virtual Country? Country { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State? State { get; set; }
    }
}
