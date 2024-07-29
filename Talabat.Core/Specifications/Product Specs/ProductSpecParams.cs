using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.Product_Specs
{   
    public class ProductSpecParams
    {
        public string? sort { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        
        private string? _search;
        public string? Search
        {
            get
            { return _search;}
            set
            { _search = value.ToLower();}
        }



        const int maxPageSize = 10;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 5;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        // has next page , has previous page

    }
}
