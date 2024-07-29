using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Specs
{
    public class OrdersWithItemsAndOrderingSpecification : BaseSpecifications<Order>
    {
        public OrdersWithItemsAndOrderingSpecification(string buyerEmail) : base(o => o.BuyerEmail == buyerEmail)
        {
            AddIncludes();
            AddOrderByDescending();
        }
        public OrdersWithItemsAndOrderingSpecification(string buyerEmail ,int id) : base(o => o.Id == id && o.BuyerEmail == buyerEmail)
        {
            AddIncludes();
            AddOrderByDescending();
        }

      
        private void AddIncludes()
        {
            Includes.Add(o => o.Items);
            Includes.Add(o => o.DeliveryMethod);
        }
        
        private void AddOrderByDescending()
        {
            OrderByDescending = o => o.OrderDate;
        }
        

    }
}
