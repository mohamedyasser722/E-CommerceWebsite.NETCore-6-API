using Microsoft.Extensions.Configuration;
using Stripe;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.Order_Specs;

namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketService _basketService;

        public PaymentService(
            IConfiguration configuration,
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IBasketService basketService)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _basketService = basketService;
        }

        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket == null) return null;

            var shippingPrice = await _basketService.GetShippingPriceAsync(basket);
            var subtotalPrice = await _basketService.GetSubtotalPriceAsync(basket);

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
                 await CreatePaymentIntentAsync(basket, subtotalPrice, shippingPrice);
            else
                 await UpdatePaymentIntentAsync(basket, subtotalPrice, shippingPrice);
            

            var UpdatedBasket = await _basketRepository.UpdateBasketAsync(basket);
            return UpdatedBasket;
        }

        public async Task<Order> UpdatePaymentStatus(string paymentIntentId, bool flag)
        {
            
            var spec = new OrderWithPaymentIntentSpec(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);
            
            if(flag) 
                order.Status = OrderStatus.PaymentReceived;
            else
                order.Status = OrderStatus.PaymentFailed;

            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompleteAsync();

            return order;

        }

        private async Task<PaymentIntent> CreatePaymentIntentAsync(CustomerBasket basket, decimal subtotalPrice, decimal shippingPrice)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(subtotalPrice * 100 + shippingPrice * 100),
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" },
            };

            var requestOptions = new RequestOptions
            {
                IdempotencyKey = $"basket_{basket.Id}",
                StripeAccount = _configuration["StripeSettings:AccountID"]
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options, requestOptions);
            basket.PaymentIntentId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;

            return paymentIntent;
        }

        private async Task<PaymentIntent> UpdatePaymentIntentAsync(CustomerBasket basket, decimal subtotalPrice, decimal shippingPrice)
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = (long)(subtotalPrice * 100 + shippingPrice * 100)
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.UpdateAsync(basket.PaymentIntentId, options);
            basket.PaymentIntentId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;

            return paymentIntent;
        }
    }
}
