using Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Library_System.Model
{
    public class Book
    {
        public Guid Id { get; set; }
        [Required]
        public string Book_Name { get; set; }

        [Required,ForeignKey(nameof(library))]

        public Guid Library_Id { get; set; }

        [ForeignKey(nameof(User))]
        public Guid ?Reserved_User_Id { get; set; }
        [Required]
        public Book_Type_Enum Type { get; set; }

        [MaxLength(255)]
        public string description { get; set; }
        [Required]
        public bool Is_Available_To_Reserve { get; set; } = true; 
        
        public User User { get; set; }
        public Library library { get; set; }
        //public ICollection<Library> library { get; set; }
    }
}
