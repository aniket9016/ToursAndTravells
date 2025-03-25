using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToursAndTravells.Models
{
    public class State
    {
        [Key]
        public int StateId { get; set; }

        [Required]
        public string StateName { get; set; }

        // ✅ Proper FK Definition
        public int CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public virtual Country? Country { get; set; }

        public virtual ICollection<User>? Users { get; set; } = new List<User>();
    }
}
