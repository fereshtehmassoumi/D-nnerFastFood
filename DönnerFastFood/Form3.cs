using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.IO;
using System.Diagnostics;


namespace DönnerFastFood
{
    public partial class Form3 : Form
    {
        public string orderText;
        public double totalPrice;
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            label1.Text = "Zu bezahlen : " + totalPrice.ToString() + "€";
            CreateAndOpenPdf();
        }
        private void CreateAndOpenPdf()
        {
            string filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "Rechnung.pdf"
            );

            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header().Text("Dönner Fast Food - Rechnung")
                        .FontSize(18).SemiBold();

                    page.Content().Column(col =>
                    {
                        col.Item().Text("Ihre Bestellung:")
                            .FontSize(14).SemiBold();

                        col.Item().Text(orderText);

                        col.Item().PaddingTop(10);

                        col.Item().Text("Gesamt: " + totalPrice + " €")
                            .FontSize(16).Bold();
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text("Danke für Ihren Einkauf!");
                });
            })
            .GeneratePdf(filePath);

            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
