using Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public  class BookInput
    {
        public string Book_Name { get; set; }


        public Guid Library_Id { get; set; }

        public Book_Type_Enum Type { get; set; }

        public string description { get; set; }
    }
}
