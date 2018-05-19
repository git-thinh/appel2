using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace appel
{
   public class oMediaSearchLocalResult
    {
        public int TotalItem { set; get; } = 0;
        public int CountResult { set; get; } = 0;
        public int PageNumber { set; get; } = 1;
        public int PageSize { set; get; } = 6;
        public List<long> MediaIds = new List<long>();
    }
}
