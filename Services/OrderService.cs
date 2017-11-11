using System;
using System.Collections.Generic;
using System.Linq;
using Orchard.ContentManagement;
using Orchard.Data;
using bookstore.Models;

namespace bookstore.Services {
    public class OrderService : IOrderService {
        private readonly IDateTimeService _dateTimeService;
        private readonly IRepository<BookPartRecord> _bookRepository;
        private readonly IContentManager _contentManager;
        private readonly IRepository<OrderRecord> _orderRepository;
        private readonly IRepository<OrderDetailRecord> _orderDetailRepository;

        public OrderService(IDateTimeService dateTimeService, IRepository<BookPartRecord> bookRepository, IContentManager contentManager, IRepository<OrderRecord> orderRepository , IRepository<OrderDetailRecord> orderDetailRepository ) {
            _dateTimeService = dateTimeService;
            _bookRepository = bookRepository;
            _contentManager = contentManager;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public OrderRecord CreateOrder(int customerId, IEnumerable<ShoppingCartItem> items) {

            if(items == null)
                throw new ArgumentNullException("items");

            // Convert to an array to avoid re-running the enumerable
            var itemsArray = items.ToArray();

            if(!itemsArray.Any())
                throw new ArgumentException("Creating an order with 0 items is not supported", "items");

            var order = new OrderRecord {
                CreatedAt  = _dateTimeService.Now,
                CustomerId = customerId,
                Status     = "New",
                //SubTotal   = 0,
                //Vat        = 0
            };

            _orderRepository.Create(order);

            // Get all products in one shot, so we can add the product reference to each order detail
            var bookIds = itemsArray.Select(x => x.BookId).ToArray();
            var books = _bookRepository.Fetch(x => bookIds.Contains(x.Id)).ToArray();

            // Create an order detail for each item
            foreach (var item in itemsArray) {
                var book = books.Single(x => x.Id == item.BookId);
                
                var detail      = new OrderDetailRecord {
                    OrderRecord_Id     = order.Id,
                    BookId      = book.Id,
                    Quantity    = item.Quantity,
                    UnitPrice   = book.UnitPrice,
                    VatRate     = .19m
                };

                _orderDetailRepository.Create(detail);
                order.Details.Add(detail);
            }

            order.UpdateTotals();
            
            return order;
        }

        /// <summary>
        /// Gets a list of ProductParts from the specified list of OrderDetails. Useful if you need to use the product as a ProductPart (instead of just having access to the ProductRecord instance).
        /// </summary>
        public IEnumerable<BookPart> GetBooks(IEnumerable<OrderDetailRecord> orderDetails) {
            var bookIds = orderDetails.Select(x => x.BookId).ToArray();
            return _contentManager.GetMany<BookPart>(bookIds, VersionOptions.Latest, QueryHints.Empty);
        }

        public OrderRecord GetOrderByNumber(string orderNumber) {
            var orderId = int.Parse(orderNumber) - 1000;
            return _orderRepository.Get(orderId);
        }

        public void UpdateOrderStatus(OrderRecord order, string status)
        {
            order.Status = status;
            switch (status)
            {
                case "Completed":
                    order.CompletedAt = _dateTimeService.Now;
                    break;
                case "Cancelled":
                    order.CancelledAt = _dateTimeService.Now;
                    break;
            }
        }

        public IEnumerable<OrderRecord> GetOrders(int customerId)
        {
            return _orderRepository.Fetch(x => x.CustomerId == customerId);
        }

        public IQueryable<OrderRecord> GetOrders()
        {
            return _orderRepository.Table;
        }

        public OrderRecord GetOrder(int id)
        {
            return _orderRepository.Get(id);
        }
    }
}