using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Shared
{
    public class PaginationInputs
    {
        [Min(1)]
        public int index { get; set; } = 1;
        [Min(1), Max(50)]
        public int size { get; set; } = 20;

    }
}
