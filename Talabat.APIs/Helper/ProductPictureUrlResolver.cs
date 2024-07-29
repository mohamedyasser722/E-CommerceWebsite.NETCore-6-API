using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helper
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductDto, string>  
    {
       
        private readonly IConfiguration _configuration;
        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ProductPictureUrlResolver()
        {
        }

        public  string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if(! string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.PictureUrl}";
            }
            return string.Empty;

        }
    }
}
