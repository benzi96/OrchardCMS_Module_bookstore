using bookstore.Models;

namespace bookstore.ViewModels {
    public class EditOrderVM {

        public int Id { get; set; }
        public string Status { get; set; }
        public EditOrderVM() {
        }

        public EditOrderVM(OrderRecord order) {
            Id = order.Id;
            Status = order.Status;
        }
    }
}