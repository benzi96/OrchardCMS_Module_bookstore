using Orchard.UI.Resources;

namespace bookstore
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder) {

            // Create and add a new manifest
            var manifest = builder.Add();

            // Define a "common" style sheet
            manifest.DefineStyle("bookstore.Common").SetUrl("common.css");

            // Define the "shoppingcart" style sheet
            manifest.DefineStyle("bookstore.ShoppingCart").SetUrl("shoppingcart.css").SetDependencies("bookstore.Common");

            // Define the "shoppingcartwidget" style sheet
            manifest.DefineStyle("bookstore.ShoppingCartWidget").SetUrl("shoppingcartwidget.css").SetDependencies("bookstore.Common");

            // Define the "shoppingcart" script and set a dependencies
            manifest.DefineScript("bookstore.ShoppingCart").SetUrl("shoppingcart.js").SetDependencies("jQuery");

            manifest.DefineStyle("bookstore.Checkout.Summary").SetUrl("checkout-summary.css").SetDependencies("bookstore.Common");
            manifest.DefineStyle("bookstore.Order").SetUrl("order.css").SetDependencies("bookstore.Common");
        }
    }
}