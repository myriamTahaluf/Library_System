using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_System.Model
{
    public class Library_Book
    {
        [ForeignKey(nameof(Book))]
        public Guid Book_Id { get; set; }
        [ForeignKey(nameof(Library))]
        public Guid Library_Id { get; set; }

        public Library Library { get; set; }

        public Book Book { get; set; }

    }
}
