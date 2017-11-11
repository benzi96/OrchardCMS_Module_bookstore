using Orchard.ContentManagement.Records;

namespace bookstore.Models
{
    public class AddressPartRecord : ContentPartRecord {
        public virtual int CustomerId { get; set; }
    }
}
