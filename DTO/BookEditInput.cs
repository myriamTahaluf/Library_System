using Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class BookEditInput
    {
        public Guid Id { get; set; }
        public string Book_Name { get; set; }


        public Guid Library_Id { get; set; }

        public Book_Type_Enum Type { get; set; }

        public string description { get; set; }
    }
}
