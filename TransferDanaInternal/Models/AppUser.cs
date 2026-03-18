using System.ComponentModel.DataAnnotations;

namespace TransferDanaInternal.Models
{
    public class AppUser
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
