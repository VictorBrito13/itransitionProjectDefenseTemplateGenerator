using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ItransitionTemplates.Models
{
    [Index(nameof(Email), IsUnique = true, Name = "IDX_User_Email")]
    public class User {
        public ulong UserId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        [MaxLength(256)]
        public string Password { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Response> responses { get; set; }
    }
}