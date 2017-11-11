using System.ComponentModel.DataAnnotations;

namespace bookstore.ViewModels
{
    public class AddressViewModel {
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(256)]
        public string AddressLine1 { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string Country { get; set; }
    }
}