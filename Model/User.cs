using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_System.Model
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(26)]
        public string Full_Name { get; set; }

        [Required]
        public string Email { get; set; }
        [Required, MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        public bool Is_Active { get; set; } = true;
        public ICollection<Book> Books { get; set; }

    }
}
