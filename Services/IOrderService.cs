using System.Collections.Generic;
using bookstore.Models;
using Orchard;
using System.Linq;

namespace bookstore.Services {
    public interface IOrderService : IDependency {
        /// <summary>
        /// Creates a new order based on the specified ShoppingCartItems
        /// </summary>
        OrderRecord CreateOrder(int customerId, IEnumerable<ShoppingCartItem> items);

        /// <summary>
        /// Gets a list of ProductParts from the specified list of OrderDetails. Useful if you need to use the product as a ProductPart (instead of just having access to the ProductRecord instance).
        /// </summary>
        IEnumerable<BookPart> GetBooks(IEnumerable<OrderDetailRecord> orderDetails);

        OrderRecord GetOrderByNumber(string orderNumber);
        void UpdateOrderStatus(OrderRecord order, string status);
        IEnumerable<OrderRecord> GetOrders(int customerId);
        IQueryable<OrderRecord> GetOrders();
        OrderRecord GetOrder(int id);
    }
}