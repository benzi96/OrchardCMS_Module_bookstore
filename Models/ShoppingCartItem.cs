using System;

namespace bookstore.Models
{
    [Serializable]
    public sealed class ShoppingCartItem {
        public int BookId { get; private set; }

        private int _quantity;
        public int Quantity {
            get { return _quantity; }
            set {
                if (value < 0)
                    throw new IndexOutOfRangeException();

                _quantity = value;
            }
        }

        public ShoppingCartItem() {
        }

        public ShoppingCartItem(int bookId, int quantity = 1) {
            BookId = bookId;
            Quantity = quantity;
        }
    }

}