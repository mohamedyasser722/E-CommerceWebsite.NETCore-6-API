using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Services.Contract
{
    public interface IBasketService
    {
        Task<decimal> GetShippingPriceAsync(CustomerBasket basket);
        Task<decimal> GetSubtotalPriceAsync(CustomerBasket basket);
    }
}
