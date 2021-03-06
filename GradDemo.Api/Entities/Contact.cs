using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GradDemo.Api.Entities
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        [MaxLength(20)]
        public string ContactNumber { get; set; }

        public virtual IdentityUser User { get; set; }
    }
}
