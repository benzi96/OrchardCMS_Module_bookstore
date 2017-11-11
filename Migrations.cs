using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using System;

namespace bookstore {
    public class Migrations : DataMigrationImpl {

        public int Create() {
            SchemaBuilder.CreateTable("BookPartRecord", table => table

                // The following method will create an "Id" column for us and set it is the primary key for the table
                .ContentPartRecord()
                .Column<string>("Arthor", column => column.WithLength(50))
                .Column<string>("Language", column => column.WithLength(50))
                // Create a column named "UnitPrice" of type "decimal"
                .Column<decimal>("UnitPrice")

                // Create the "Sku" column and specify a maximum length of 50 characters
                .Column<string>("Sku", column => column.WithLength(50))
                );

            return 1;
        }

        public int UpdateFrom1() {
            // Create (or alter) a part called "ProductPart" and configure it to be "attachable".
            ContentDefinitionManager.AlterPartDefinition("BookPart", part => part
                .Attachable());

            return 2;
        }

        public int UpdateFrom2()
        {
            ContentDefinitionManager.AlterTypeDefinition("Book",
                cfg => cfg
                    .WithPart("TitlePart")
                    .WithPart("AutoroutePart")
                    .WithPart("BodyPart")
                    .WithPart("CommentsPart")
                    .WithPart("BookPart")
                    .WithPart("TagsPart")
                );

            return 3;
        }

        public int UpdateFrom3()
        {
            ContentDefinitionManager.AlterTypeDefinition("Book",
                cfg => cfg
                    .Creatable()
                    .Draftable()
                    .Listable()
                    .Securable()
                );

            return 4;
        }

        public int UpdateFrom4()
        {
            ContentDefinitionManager.AlterTypeDefinition("Book",
                cfg => cfg
                    .WithPart("CommonPart")
                );

            return 5;
        }

        public int UpdateFrom5()
        {

            // Define a new content type called "ShoppingCartWidget"
            ContentDefinitionManager.AlterTypeDefinition("ShoppingCartWidget", type => type

                // Attach the "ShoppingCartWidgetPart"
                .WithPart("ShoppingCartWidgetPart")

                // In order to turn this content type into a widget, it needs the WidgetPart
                .WithPart("WidgetPart")

                .WithPart("CommonPart")
                // It also needs a setting called "Stereotype" to be set to "Widget"
                .WithSetting("Stereotype", "Widget")
                );

            return 6;
        }

        public int UpdateFrom6()
        {
            SchemaBuilder.CreateTable("CustomerPartRecord", table => table
                .ContentPartRecord()
                .Column<string>("FirstName", c => c.WithLength(50))
                .Column<string>("LastName", c => c.WithLength(50))
                .Column<string>("PhoneNumber", c => c.WithLength(20))
                .Column<DateTime>("CreatedUtc")
                );

            SchemaBuilder.CreateTable("AddressPartRecord", table => table
                .ContentPartRecord()
                .Column<int>("CustomerId")
                );

            ContentDefinitionManager.AlterPartDefinition("CustomerPart", part => part
                .Attachable(false)
                );

            ContentDefinitionManager.AlterTypeDefinition("Customer", type => type
                .WithPart("CustomerPart")
                .WithPart("UserPart")
                );

            ContentDefinitionManager.AlterPartDefinition("AddressPart", part => part
                .Attachable(false)
                .WithField("Name", f => f.OfType("TextField"))
                .WithField("AddressLine1", f => f.OfType("TextField"))
                .WithField("City", f => f.OfType("TextField"))
                .WithField("Country", f => f.OfType("TextField"))
                );

            ContentDefinitionManager.AlterTypeDefinition("Address", type => type
                .WithPart("CommonPart")
                .WithPart("AddressPart")
                );

            return 7;
        }

        public int UpdateFrom7()
        {
            SchemaBuilder.CreateTable("OrderRecord", t => t
                .Column<int>("Id", c => c.PrimaryKey().Identity())
                .Column<int>("CustomerId", c => c.NotNull())
                .Column<DateTime>("CreatedAt", c => c.NotNull())
                .Column<string>("Status", c => c.WithLength(50).NotNull())
                .Column<decimal>("SubTotal", c => c.NotNull())
                .Column<decimal>("Vat", c => c.NotNull())
                .Column<DateTime>("CompletedAt", c => c.Nullable())
                .Column<DateTime>("CancelledAt", c => c.Nullable())
                );

            SchemaBuilder.CreateTable("OrderDetailRecord", t => t
                .Column<int>("Id", c => c.PrimaryKey().Identity())
                .Column<int>("OrderRecord_Id", c => c.NotNull())
                .Column<int>("BookId", c => c.NotNull())
                .Column<int>("Quantity", c => c.NotNull())
                .Column<decimal>("UnitPrice", c => c.NotNull())
                .Column<decimal>("VatRate", c => c.NotNull())
                );

            SchemaBuilder.CreateForeignKey("Order_Customer", "OrderRecord", new[] { "CustomerId" }, "CustomerPartRecord", new[] { "Id" });
            SchemaBuilder.CreateForeignKey("OrderDetail_Order", "OrderDetailRecord", new[] { "OrderRecord_Id" }, "OrderRecord", new[] { "Id" });
            SchemaBuilder.CreateForeignKey("OrderDetail_Book", "OrderDetailRecord", new[] { "BookId" }, "BookPartRecord", new[] { "Id" });

            return 8;
        }
    }
}