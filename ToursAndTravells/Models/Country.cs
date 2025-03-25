using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToursAndTravells.Models
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        [Required]
        public string CountryName { get; set; }

        // ✅ Explicitly map the one-to-many relationship
        public virtual ICollection<State> States { get; set; } = new List<State>();

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
