using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Shared
{
    public class DataTableReturn
    {
        public int total { get; set; }
        public int pagesCount { get; set; }
        public dynamic list { get; set; }
    }

}
