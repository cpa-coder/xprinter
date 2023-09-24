using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace FastReceipt.Demo.Server;

public class Receipt : IDocument
{
    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.MarginHorizontal(20);
            page.MarginVertical(5);
            page.ContinuousSize(76, Unit.Millimetre);
            page.DefaultTextStyle(s => s.FontSize(9));

            page.Header().Column(col =>
            {
                col.Item().ShowOnce().Column(c =>
                {
                    c.Item().AlignCenter().Text("DJFM INC.").FontSize(10);
                    c.Item().AlignCenter().Text("Lasersita Casitas & Water Spa Beach Resort Mansalay");
                    c.Item().AlignCenter().Text("TIN: 000-000-000-000");
                    c.Item().PaddingVertical(10).Text("SALES ORDER").FontSize(10);

                    c.Item().Text("CASHIER: ADMIN");
                    c.Item().Text("ORDER NO: 0001");
                    c.Item().Text($"{DateTime.Now:MM/dd/yyyy HH:mm:ss}");
                    c.Item().PaddingTop(5).LineHorizontal(1);
                });
            });
            page.Content().PaddingVertical(5).Column(col =>
            {
                col.Item().Row(r =>
                {
                    r.RelativeItem().Text("Apple");
                    r.AutoItem().AlignRight().AlignBottom().Text($"{20d:N2}");
                });
                col.Item().Row(r =>
                {
                    r.RelativeItem().Text("Grapes");
                    r.AutoItem().AlignRight().AlignBottom().Text($"{30d:N2}");
                });
                col.Item().Row(r =>
                {
                    r.RelativeItem().Text("Bananas with something special and colors");
                    r.AutoItem().AlignRight().AlignBottom().Text($"{40d:N2}");
                });
                col.Item().Row(r =>
                {
                    r.RelativeItem().Text("Oranges");
                    r.AutoItem().AlignRight().AlignBottom().Text($"{40d:N2}");
                });
                col.Item().PaddingTop(10).Row(r =>
                {
                    r.RelativeItem().Text("Tax excluded");
                    r.AutoItem().AlignRight().AlignBottom().Text($"{20d:N2}");
                });
                col.Item().Row(r =>
                {
                    r.RelativeItem().Text("Tax 5%");
                    r.AutoItem().AlignRight().AlignBottom().Text($"{10d:N2}");
                });
                col.Item().LineHorizontal(1);
                col.Item().Row(r =>
                {
                    r.RelativeItem().Text("Total").FontSize(11).Bold();
                    r.AutoItem().AlignRight().AlignBottom().AlignBottom().Text($"{210d:N2}").FontSize(11).Bold();
                });
                col.Item().PaddingBottom(2).Row(r => r.RelativeItem().BorderBottom(1));
                col.Item().PaddingBottom(1).Row(r => r.RelativeItem().BorderBottom(1));
                col.Item().PaddingTop(10).Row(r =>
                {
                    r.AutoItem().Text("Customer:");
                    r.RelativeItem().BorderBottom(1);
                });
                col.Item().Row(r =>
                {
                    r.RelativeItem().Text("Customers payment");
                    r.AutoItem().AlignRight().AlignBottom().Text($"{250:N2}");
                });
                col.Item().Row(r =>
                {
                    r.RelativeItem().Text("Change").FontSize(11);
                    r.AutoItem().AlignRight().AlignBottom().Text($"{10d:N2}").FontSize(11);
                });
            });
            page.Footer().Column(col =>
            {
                col.Item().Text("POS Vendor").FontSize(10).Bold();
                col.Item().Text("POS Maker of the Year");
                col.Item().Text("Barangay, Municipality, Province, Philippines 0000").FontSize(8);
                col.Item().Text("NON-VAT REG TIN: 000-000-000-001");
                col.Item().PaddingTop(20).AlignCenter().Text("THIS DOCUMENT IS NOT VALID FOR CLAIMING INPUT TAX");
            });
        });
    }
}