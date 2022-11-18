using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enum;

namespace DTO
{
    public class BookDTO
    {
        public Guid Id { get; set; }
        public string Book_Name { get; set; }


        public Guid Library_Id { get; set; }

        public Guid? Reserved_User_Id { get; set; }
        public Book_Type_Enum Type { get; set; }

        public string description { get; set; }
        public bool Is_Available_To_Reserve { get; set; } = true;

        public UserDTO User { get; set; }
        public LibraryDTO library { get; set; }
    }
}
