using Orchard.ContentManagement;

namespace bookstore.Models
{
    public class BookPart : ContentPart<BookPartRecord>
    {
        public string Arthor
        {
            get { return Record.Arthor; }
            set { Record.Arthor = value; }
        }

        public string Language
        {
            get { return Record.Language; }
            set { Record.Language = value; }
        }

        public decimal UnitPrice
        {
            get { return Record.UnitPrice; }
            set { Record.UnitPrice = value; }
        }

        public string Sku
        {
            get { return Record.Sku; }
            set { Record.Sku = value; }
        }
    }
}