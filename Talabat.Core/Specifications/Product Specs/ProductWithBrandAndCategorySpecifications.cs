    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Product_Specs
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {
        public ProductWithBrandAndCategorySpecifications()
        {
            AddIncludes();
        }
        // sortAsc, sortDesc
        public ProductWithBrandAndCategorySpecifications(string? sort)
        {
            AddIncludes();
            
            // switch on sort and set OrderBy

            switch (sort)
            {
                case "priceAsc":
                    OrderBy = x => x.Price;
                    break;
                case "priceDesc":
                    OrderByDescending = x => x.Price;
                    break;
                default:
                    OrderBy = x => x.Name;
                    break;
            }

        }

        public ProductWithBrandAndCategorySpecifications(string? sort, int? BrandId, int? CategoryId, string? search) : this(sort)
        {
            Criteria = x =>
                 (string.IsNullOrEmpty(search) || x.Name.ToLower().Contains(search)) &&
                 (!BrandId.HasValue || x.BrandId == BrandId) &&
                 (!CategoryId.HasValue || x.CategoryId == CategoryId);
        }

        public ProductWithBrandAndCategorySpecifications(ProductSpecParams specParams):this(specParams.sort, specParams.BrandId, specParams.CategoryId,specParams.Search)
        {
            Skip = (specParams.PageIndex - 1) * specParams.PageSize;
            Take = specParams.PageSize;
            isPaginationEnabled = true;
        }
        public ProductWithBrandAndCategorySpecifications(int id) : base(x => x.Id == id)
        {
            AddIncludes();
        }

        
        private void AddIncludes()
        {
            Includes.Add(x => x.Brand);
            Includes.Add(x => x.Category);
        }
        



    }
}
