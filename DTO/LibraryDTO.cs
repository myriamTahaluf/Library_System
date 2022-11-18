using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class LibraryDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        //[ForeignKey(nameof(Books))]
        //public Guid ?Book_Id { get; set; }

    }
}
