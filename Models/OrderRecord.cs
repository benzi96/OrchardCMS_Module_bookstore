using Orchard.ContentManagement.Records;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace bookstore.Models
{
    public class OrderRecord
    {
        public virtual int Id { get; set; }
        public virtual int CustomerId { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual string Status { get; set; }
        public virtual decimal SubTotal { get; set; }
        public virtual decimal Vat { get; set; }
        public virtual IList<OrderDetailRecord> Details { get; private set; }
        public virtual DateTime? CompletedAt { get; set; }
        public virtual DateTime? CancelledAt { get; set; }

        public OrderRecord() {
            Details = new List<OrderDetailRecord>();
        }
        public decimal Total()
        {
            return SubTotal + Vat;
        }

        public string Number()
        {
            return (Id + 1000).ToString(CultureInfo.InvariantCulture);
        }

        public void UpdateTotals()
        {
            var subTotal = 0m;
            var vat = 0m;

            foreach (var detail in Details) {
                subTotal += detail.SubTotal();
                vat += detail.Vat();
            }

            SubTotal = subTotal;
            Vat = vat;
        }
    }
}
