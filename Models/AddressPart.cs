using Orchard.ContentManagement;

namespace bookstore.Models
{
    public class AddressPart : ContentPart<AddressPartRecord> {
        
        public int CustomerId {
            get { return Record.CustomerId; }
            set { Record.CustomerId = value; }
        }
    }
}
