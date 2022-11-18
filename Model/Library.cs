using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_System.Model
{
    public class Library
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(26)]
        public string Name { get; set; }

        [Required, MaxLength(250)]
        public string Description { get; set; }
        //[ForeignKey(nameof(Books))]
        //public Guid ?Book_Id { get; set; }
         
        public Book Books { get; set; }
        public ICollection<Library_Book> library_Books { get; set; }


    }
}
