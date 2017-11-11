using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bookstore.ViewModels
{
    public class AddressesViewModel : IValidatableObject {

        [UIHint("Address")]
        public AddressViewModel BillingAddress { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var address = BillingAddress;

            if (string.IsNullOrWhiteSpace(address.AddressLine1))
                yield return new ValidationResult("Addressline 1 is a required field", new[] { "BillingAddress.AddressLine1" });

            if (string.IsNullOrWhiteSpace(address.City))
                yield return new ValidationResult("City is a required field", new[] { "BillingAddress.City" });

            if (string.IsNullOrWhiteSpace(address.Country))
                yield return new ValidationResult("Country is a required field", new[] { "BillingAddress.Country" });
        }
    }
}