using bookstore.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookstore.Drivers
{
    public class BookPartDriver: ContentPartDriver<BookPart>
    {

        protected override string Prefix
        {
            get { return "Book"; }
        }

        protected override DriverResult Display(BookPart part, string displayType, dynamic shapeHelper)
        {
            if (displayType == "SummaryAdmin")
                return null;

            return Combined
                (
                    ContentShape("Parts_Book", () => shapeHelper.Parts_Book(
                        Arthor: part.Arthor,
                        Language: part.Language,
                        Price: part.UnitPrice,
                        Sku: part.Sku
                    )),
                    ContentShape("Parts_Book_AddButton", () => shapeHelper.Parts_Book_AddButton(

                        // Set a property on the shape called ProductId and set it to the content ID
                        BookId: part.Id
                    ))
                );
        }

        protected override DriverResult Editor(BookPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Book_Edit", () => shapeHelper
                .EditorTemplate(TemplateName: "Parts/Book", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(BookPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }

    }
}
