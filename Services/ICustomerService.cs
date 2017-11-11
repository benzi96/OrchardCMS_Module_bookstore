using Orchard;
using bookstore.Models;
using Orchard.ContentManagement;
using System.Collections.Generic;

namespace bookstore.Services
{
    public interface ICustomerService : IDependency {
        CustomerPart CreateCustomer(string email, string password);
        AddressPart GetAddress(int customerId);
        AddressPart CreateAddress(int customerId);
        IContentQuery<CustomerPart> GetCustomers();
        IEnumerable<AddressPart> GetAddresses(int customerId);
    }
}