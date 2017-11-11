using Orchard.ContentManagement.Records;

namespace bookstore.Models
{
    public class BookPartRecord : ContentPartRecord
    {
        public virtual string Arthor { get; set; }
        public virtual string Language { get; set; }
        public virtual decimal UnitPrice { get; set; }
        public virtual string Sku { get; set; }
    }
}
