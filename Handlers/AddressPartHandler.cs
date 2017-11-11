using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using bookstore.Models;

namespace bookstore.Handlers
{
    public class AddressPartHandler : ContentHandler {
        public AddressPartHandler(IRepository<AddressPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}