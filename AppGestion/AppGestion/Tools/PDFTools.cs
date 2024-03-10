using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using AppGestion.Classes;
using System.IO;
using System;
using System.Collections.Generic;

namespace AppGestion.Tools
{
    public class PDFTools
    {
        #region Sales Report
        public static void CreateSalesReportPdf(string filePath, List<SellLine> sellLines)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Header().Element(ComposeHeader);
                    page.Content().Element(content => ComposeContent(content, sellLines));
                    page.Footer().AlignCenter().Text(x => 
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            });

            document.GeneratePdf(filePath);
        }

        private static void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Text("Sales Report").FontSize(20).Bold();
            });
        }


        private static void ComposeContent(IContainer container, List<SellLine> sellLines)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Text("Date");
                    header.Cell().Text("Show Name");
                    header.Cell().Text("Tickets Sold");
                    header.Cell().Text("Amount Before Taxes");
                    header.Cell().Text("TPS");
                    header.Cell().Text("TVQ");
                    header.Cell().Text("Total After Taxes");
                });

                foreach (var sellLine in sellLines)
                {
                    table.Cell().Text(sellLine.RepresentationDate.ToString("g"));
                    table.Cell().Text(sellLine.ShowName);
                    table.Cell().Text(sellLine.TicketsSold.ToString());
                    table.Cell().Text($"${sellLine.TotalAmountBeforeTaxe:F2}");
                    table.Cell().Text($"${sellLine.TPS:F2}");
                    table.Cell().Text($"${sellLine.TVQ:F2}");
                    table.Cell().Text($"${sellLine.TotalAmountAfterTaxe:F2}");
                }
            });
        }
        #endregion


        #region Transaction Report

        public static void CreateTransactionsReportPdf(string filePath, List<TransactionLine> transactionLines)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Header().Element(ComposeTransactionsHeader);
                    page.Content().Element(content => ComposeTransactionsContent(content, transactionLines));
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            });

            document.GeneratePdf(filePath);
        }

        private static void ComposeTransactionsHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Text("Transactions Report").FontSize(20).Bold();
            });
        }

        private static void ComposeTransactionsContent(IContainer container, List<TransactionLine> transactionLines)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Text("Order Number");
                    header.Cell().Text("Transaction Date");
                    header.Cell().Text("Client Email");
                    header.Cell().Text("Tickets Sold");
                    header.Cell().Text("Amount Before Taxes");
                    header.Cell().Text("TPS");
                    header.Cell().Text("TVQ");
                    header.Cell().Text("Total After Taxes");
                });

                foreach (var line in transactionLines)
                {
                    table.Cell().Text(line.OrderNumber);
                    table.Cell().Text(line.TransactionDate.ToString("U"));
                    table.Cell().Text(line.ClientEmail);
                    table.Cell().Text(line.TicketsSold.ToString());
                    table.Cell().Text($"${line.TotalAmountBeforeTaxe:F2}");
                    table.Cell().Text($"${line.TPS:F2}");
                    table.Cell().Text($"${line.TVQ:F2}");
                    table.Cell().Text($"${line.TotalAmountAfterTaxe:F2}");
                }
            });
        }

        #endregion
    }
}
