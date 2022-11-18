using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        public string Full_Name { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Is_Active { get; set; }
        public string Password { get; set; }
    }
}