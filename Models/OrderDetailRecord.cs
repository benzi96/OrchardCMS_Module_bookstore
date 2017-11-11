namespace bookstore.Models {
    public class OrderDetailRecord {
        public virtual int Id { get; set; }
        public virtual int OrderRecord_Id { get; set; }
        public virtual int BookId { get; set; }
        public virtual int Quantity { get; set; }
        public virtual decimal UnitPrice { get; set; }
        public virtual decimal VatRate { get; set; }

        public decimal UnitVat()
        {
            return UnitPrice * VatRate;
        }

        public decimal Vat()
        {
            return UnitVat() * Quantity;
        }

        public decimal SubTotal()
        {
            return UnitPrice * Quantity;
        }

        public decimal Total()
        {
            return SubTotal() + Vat();
        }
    }
}