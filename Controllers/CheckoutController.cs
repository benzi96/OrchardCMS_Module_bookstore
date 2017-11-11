using System.Linq;
using System.Web.Mvc;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Security;
using Orchard.Themes;
using bookstore.Helpers;
using bookstore.Models;
using bookstore.Services;
using bookstore.ViewModels;

namespace bookstore.Controllers {
	public class CheckoutController : Controller {
		private readonly IAuthenticationService _authenticationService;
		private readonly IOrchardServices _services;
		private readonly ICustomerService _customerService;
		private readonly IMembershipService _membershipService;
		private readonly IShoppingCart _shoppingCart;
		private Localizer T { get; set; }

		public CheckoutController(IOrchardServices services, IAuthenticationService authenticationService, ICustomerService customerService, IMembershipService membershipService, IShoppingCart shoppingCart) {
			_authenticationService = authenticationService;
			_services = services;
			_customerService = customerService;
			_membershipService = membershipService;
			_shoppingCart = shoppingCart;
			T = NullLocalizer.Instance;
		}

		[Themed]
		public ActionResult SignupOrLogin() {

            if (_authenticationService.GetAuthenticatedUser() != null && _authenticationService.GetAuthenticatedUser().UserName == "admin")
                return Redirect("/OrchardLocal");
			if (_authenticationService.GetAuthenticatedUser() != null)
				return RedirectToAction("SelectAddress");

			return new ShapeResult(this, _services.New.Checkout_SignupOrLogin());
		}

		[Themed]
		public ActionResult Signup() {
			var shape = _services.New.Checkout_Signup();
			return new ShapeResult(this, shape);
		}

		[HttpPost]
		public ActionResult Signup(SignupViewModel signup) {
			if (!ModelState.IsValid)
				return new ShapeResult(this, _services.New.Checkout_Signup(Signup: signup));

			var customer       = _customerService.CreateCustomer(signup.Email, signup.Password);
			customer.FirstName = signup.FirstName;
			customer.LastName  = signup.LastName;
            customer.PhoneNumber = signup.PhoneNumber;

			_authenticationService.SignIn(customer.User, true);

			return RedirectToAction("SelectAddress");
		}

		[Themed]
		public ActionResult Login() {
			var shape = _services.New.Checkout_Login();
			return new ShapeResult(this, shape);
		}

		[Themed, HttpPost]
		public ActionResult Login(LoginViewModel login) {

			// Validate the specified credentials
			var user = _membershipService.ValidateUser(login.Email, login.Password);

			// If no user was found, add a model error
			if (user == null) {
				ModelState.AddModelError("Email", T("Incorrect username/password combination").ToString());
			}

			// If there are any model errors, redisplay the login form
			if (!ModelState.IsValid) {
				var shape = _services.New.Checkout_Login(Login: login);
				return new ShapeResult(this, shape);
			}

			// Create a forms ticket for the user
			_authenticationService.SignIn(user, login.CreatePersistentCookie);
            //_authenticationService.SignIn(user, true);

            // Redirect to the next step
            return RedirectToAction("SelectAddress");
		}

		[Themed]
		public ActionResult SelectAddress() {
			var currentUser = _authenticationService.GetAuthenticatedUser();

			if (currentUser == null)
				throw new OrchardSecurityException(T("Login required"));

			var customer = currentUser.ContentItem.As<CustomerPart>();
			var billingAddress = _customerService.GetAddress(customer.Id);

			var addressesViewModel = new AddressesViewModel {
				BillingAddress = MapAddress(billingAddress)
			};

			var shape = _services.New.Checkout_SelectAddress(Addresses: addressesViewModel);
			if (string.IsNullOrWhiteSpace(addressesViewModel.BillingAddress.Name))
				addressesViewModel.BillingAddress.Name = string.Format("{0} {1}", customer.FirstName, customer.LastName);
			return new ShapeResult(this, shape);
		}

		[Themed, HttpPost]
		public ActionResult SelectAddress(AddressesViewModel addresses) {
			var currentUser = _authenticationService.GetAuthenticatedUser();

			if (currentUser == null)
				throw new OrchardSecurityException(T("Login required"));

			if (!ModelState.IsValid) {
				return new ShapeResult(this, _services.New.Checkout_SelectAddress(Addresses: addresses));
			}

			var customer = currentUser.ContentItem.As<CustomerPart>();
			MapAddress(addresses.BillingAddress, customer);

			return RedirectToAction("Summary");
		}

		[Themed]
		public ActionResult Summary() {
			var user = _authenticationService.GetAuthenticatedUser();

			if (user == null)
				throw new OrchardSecurityException(T("Login required"));

			dynamic billingAddress = _customerService.GetAddress(user.Id);
			dynamic shoppingCartShape = _services.New.ShoppingCart();

			var query = _shoppingCart.GetBooks().Select(x => _services.New.ShoppingCartItem(
				Book: x.BookPart,
				Quantity: x.Quantity,
				Title: _services.ContentManager.GetItemMetadata(x.BookPart).DisplayText
			));

			shoppingCartShape.ShopItems = query.ToArray();
			shoppingCartShape.Total     = _shoppingCart.Total();
			shoppingCartShape.Subtotal  = _shoppingCart.Subtotal();
			shoppingCartShape.Vat       = _shoppingCart.Vat();

			return new ShapeResult(this, _services.New.Checkout_Summary(
				ShoppingCart: shoppingCartShape,
				BillingAddress: billingAddress
			));
		}

		private AddressViewModel MapAddress(AddressPart addressPart) {
			dynamic address = addressPart;
			var addressViewModel = new AddressViewModel();

			if (addressPart != null) {
				addressViewModel.Name = address.Name.Value;
				addressViewModel.AddressLine1 = address.AddressLine1.Value;
				addressViewModel.City = address.City.Value;
				addressViewModel.Country = address.Country.Value;
			}

			return addressViewModel;
		}

		private AddressPart MapAddress(AddressViewModel source, CustomerPart customerPart) {
			var addressPart = _customerService.GetAddress(customerPart.Id) ?? _customerService.CreateAddress(customerPart.Id);
			dynamic address = addressPart;

			address.Name.Value         = source.Name.TrimSafe();
			address.AddressLine1.Value = source.AddressLine1.TrimSafe();
			address.City.Value         = source.City.TrimSafe();
			address.Country.Value      = source.Country.TrimSafe();

			return addressPart;
		}
	}
}