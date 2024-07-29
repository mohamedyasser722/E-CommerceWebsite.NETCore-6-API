using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Product_Specs
{
    public class ProductWithFilterationForCountAsync : BaseSpecifications<Product>
    {
        public ProductWithFilterationForCountAsync(ProductSpecParams productParams)
        {
            Criteria = x =>
                  (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
                  (!productParams.BrandId.HasValue || x.BrandId == productParams.BrandId) &&
                  (!productParams.CategoryId.HasValue || x.CategoryId == productParams.CategoryId);

            
        }
        
    }
    
    
}
