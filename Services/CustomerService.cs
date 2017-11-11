using System.Linq;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Security;
using Orchard.Services;
using Orchard.Users.Models;
using bookstore.Models;
using System.Collections.Generic;

namespace bookstore.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IMembershipService _membershipService;
        private readonly IClock _clock;

        public CustomerService(IOrchardServices orchardServices, IMembershipService membershipService, IClock clock)
        {
            _orchardServices = orchardServices;
            _membershipService = membershipService;
            _clock = clock;
        }

        public CustomerPart CreateCustomer(string email, string password)
        {
            // New up a new content item of type "Customer"
            var customer = _orchardServices.ContentManager.New("Customer");

            // Cast the customer to a UserPart
            var userPart = customer.As<UserPart>();

            // Cast the customer to a CustomerPart
            var customerPart = customer.As<CustomerPart>();

            

            // Set some properties of the customer content item (via UserPart and CustomerPart)
            userPart.UserName                  = email;
            userPart.Email                     = email;
            userPart.NormalizedUserName        = email.ToLowerInvariant();
            userPart.Record.HashAlgorithm      = "SHA1";
            userPart.Record.RegistrationStatus = UserStatus.Approved;
            userPart.Record.EmailStatus        = UserStatus.Approved;

            // Use IClock to get the current date instead of using DateTime.Now (see http://skywalkersoftwaredevelopment.net/orchard-development/api/iclock)
            customerPart.CreatedUtc = _clock.UtcNow;

            // Use Ochard's MembershipService to set the password of our new user
            _membershipService.SetPassword(userPart, password);

            // Store the new user into the database
            _orchardServices.ContentManager.Create(customer);

            return customerPart;
        }

        public AddressPart GetAddress(int customerId) {
            return _orchardServices.ContentManager.Query<AddressPart, AddressPartRecord>().Where(x => x.CustomerId == customerId).List().FirstOrDefault();
        }

        public AddressPart CreateAddress(int customerId) {
            return _orchardServices.ContentManager.Create<AddressPart>("Address", x => {
                x.CustomerId = customerId;
            });
        }
        public IContentQuery<CustomerPart> GetCustomers()
        {
            return _orchardServices.ContentManager.Query<CustomerPart, CustomerPartRecord>();
        }

        public IEnumerable<AddressPart> GetAddresses(int customerId)
        {
            return _orchardServices.ContentManager.Query<AddressPart, AddressPartRecord>().Where(x => x.CustomerId == customerId).List();
        }
    }
}