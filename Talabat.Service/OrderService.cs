using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.Order_Specs;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int DeliveryMethodId, Address ShippingAddress)
        {
            // 1. Get the basket from the BasketRepository

            var basket = await _basketRepository.GetBasketAsync(basketId);

            // 2. Get the selected items at the basket from product repository

            var OrderItems = new List<OrderItem>();
            if (basket?.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if (product == null) continue;
                    var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productItemOrdered, item.Quantity, product.Price);    // take the price from the database cause the user can change the price!!
                    OrderItems.Add(orderItem);
                }
            }

            // 3. Calculate the subtotal

            var subtotal = OrderItems.Sum(item => item.Price * item.Quantity);

            // 4. Get a delivery method from the DeliveryMethod repository

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);

            // 5. Create an order

            // check if the order is already exist
            var spec = new OrderWithPaymentIntentSpec(basket.PaymentIntentId);
            var ExOrder = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);
            if (ExOrder is not null)        // it can't be an order still not created associated with a paymentIntentId
            {
                _unitOfWork.Repository<Order>().Delete(ExOrder);
                basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }



            var order = new Order(buyerEmail, ShippingAddress, deliveryMethod, OrderItems, subtotal,basket.PaymentIntentId);

            // 6. Save Order Locally
            await _unitOfWork.Repository<Order>().AddAsync(order);
            // 7. Save Order to the database 

            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;
            return order;
        }

        public async Task<Order?> GetOrderByIdForSpecificUserAsync(string buyerEmail, int orderId)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail, orderId);
            var order = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);
            return order;
           
        }
        // Note Get Orders from New to Old
        public async Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string buyerEmail)  
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
        }
    }
}
