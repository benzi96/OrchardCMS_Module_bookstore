using System.Collections.Generic;
using Orchard;
using bookstore.Models;

namespace bookstore.Services
{
    public interface IShoppingCart : IDependency {
        IEnumerable<ShoppingCartItem> Items { get; }
        void Add(int bookId, int quantity = 1);
        void Remove(int bookId);
        BookPart GetBook(int bookId);
        IEnumerable<BookQuantity> GetBooks();
        decimal Subtotal();
        decimal Vat();
        decimal Total();
        int ItemCount();
        void UpdateItems();
        void Clear();
        void AddRange(IEnumerable<ShoppingCartItem> items);
    }
}