using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Security;
using Orchard.Themes;
using bookstore.Models;
using bookstore.Services;

namespace bookstore.Controllers
{
    public class OrderController : Controller
    {
        private readonly dynamic _shapeFactory;
        private readonly IOrderService _orderService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IShoppingCart _shoppingCart;
        private readonly ICustomerService _customerService;
        private readonly Localizer _t;

        public OrderController(
            IShapeFactory shapeFactory,
            IOrderService orderService,
            IAuthenticationService authenticationService,
            IShoppingCart shoppingCart,
            ICustomerService customerService
            )
        {
            _shapeFactory = shapeFactory;
            _orderService = orderService;
            _authenticationService = authenticationService;
            _shoppingCart = shoppingCart;
            _customerService = customerService;
            _t = NullLocalizer.Instance;
        }

        [Themed, HttpPost]
        public ActionResult Create()
        {

            var user = _authenticationService.GetAuthenticatedUser();

            if (user == null)
                throw new OrchardSecurityException(_t("Login required"));

            var customer = user.ContentItem.As<CustomerPart>();

            if (customer == null)
                throw new InvalidOperationException("The current user is not a customer");

            var order = _orderService.CreateOrder(customer.Id, _shoppingCart.Items);


            // If we got here, no PSP handled the PaymentRequest event, so we'll just display the order.
            var shape = _shapeFactory.Order_Created(
                Order: order,
                Books: _orderService.GetBooks(order.Details).ToArray(),
                Customer: customer,
                BillingAddress: (dynamic)_customerService.GetAddress(user.Id)
            );
            _shoppingCart.Clear();
            return new ShapeResult(this, shape);
        }

    }
}