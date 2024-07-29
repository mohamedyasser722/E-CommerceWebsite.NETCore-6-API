using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using Talabat.Core;

namespace Talabat.Service
{
    public class BasketService : IBasketService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BasketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<decimal> GetShippingPriceAsync(CustomerBasket basket)
        {
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                return deliveryMethod.Cost;
            }
            return 0m;
        }

        public async Task<decimal> GetSubtotalPriceAsync(CustomerBasket basket)
        {
            decimal subtotalPrice = 0;
            if (basket.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var productItem = await _unitOfWork.Repository<Core.Entities.Product>().GetByIdAsync(item.Id);
                    if (productItem.Price != item.Price)
                        item.Price = productItem.Price;

                    subtotalPrice += productItem.Price * item.Quantity;
                }
            }
            return subtotalPrice;
        }
    }
}
