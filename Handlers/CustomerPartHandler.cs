using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Users.Models;
using bookstore.Models;

namespace Skywalker.Webshop.Handlers
{
    public class CustomerPartHandler : ContentHandler {
        public CustomerPartHandler(IRepository<CustomerPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<UserPart>("Customer"));
        }
    }
}