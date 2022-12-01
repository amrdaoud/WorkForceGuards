using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.Bindings
{
    public class FilterModel
    {
        public string SearchQuery { get; set; } = "";
        public List<Filter> Filters { get; set; } = new List<Filter>();
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = int.MaxValue;
        public string Sort { get; set; } //key
        public string Order { get; set; }//asc desc
    }
}
