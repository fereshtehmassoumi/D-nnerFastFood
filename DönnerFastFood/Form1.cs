using System.Drawing.Drawing2D;
using Microsoft.Data.Sqlite;

namespace DönnerFastFood
{
    public partial class Form1 : Form
    {
        private readonly string dbPath =
    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fastfood.db");
        public Form1()
        {
            InitializeComponent();
            EnsureDatabase();
            SeedProducts();
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, 30, 30), 180, 90);
            path.AddArc(new Rectangle(this.Width - 30, 0, 30, 30), 270, 90);
            path.AddArc(new Rectangle(this.Width - 30, this.Height - 30, 30, 30), 0, 90);
            path.AddArc(new Rectangle(0, this.Height - 30, 30, 30), 90, 90);
            path.CloseFigure();

            this.Region = new Region(path);
        }
        private void EnsureDatabase()
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
        CREATE TABLE IF NOT EXISTS Products (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL UNIQUE,
            Price REAL NOT NULL,
            Stock INTEGER NOT NULL
        );

        CREATE TABLE IF NOT EXISTS OrderItems (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            ProductName TEXT NOT NULL,
            Quantity INTEGER NOT NULL,
            UnitPrice REAL NOT NULL,
            OrderDate TEXT NOT NULL
        );
    ";
            cmd.ExecuteNonQuery();
        }
        private void SeedProducts()
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
        INSERT OR IGNORE INTO Products (Name, Price, Stock) VALUES ('Dönner Yofka', 10, 50);
        INSERT OR IGNORE INTO Products (Name, Price, Stock) VALUES ('Hamburger', 15, 40);
        INSERT OR IGNORE INTO Products (Name, Price, Stock) VALUES ('Pizza', 12, 30);
        INSERT OR IGNORE INTO Products (Name, Price, Stock) VALUES ('Sandwich', 8, 35);
        INSERT OR IGNORE INTO Products (Name, Price, Stock) VALUES ('Pommes', 7, 60);
        INSERT OR IGNORE INTO Products (Name, Price, Stock) VALUES ('Salat', 10, 20);

        INSERT OR IGNORE INTO Products (Name, Price, Stock) VALUES ('Cola', 3, 80);
        INSERT OR IGNORE INTO Products (Name, Price, Stock) VALUES ('Beer', 3.5, 50);
        INSERT OR IGNORE INTO Products (Name, Price, Stock) VALUES ('Saft', 5, 40);

        INSERT OR IGNORE INTO Products (Name, Price, Stock) VALUES ('Eis', 3.5, 25);
        INSERT OR IGNORE INTO Products (Name, Price, Stock) VALUES ('Kuchen', 4.5, 20);
        INSERT OR IGNORE INTO Products (Name, Price, Stock) VALUES ('Baqlava', 5.5, 30);
    ";
            cmd.ExecuteNonQuery();
        }
        private void SaveOrder(string productName, int quantity, double unitPrice)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            using var transaction = connection.BeginTransaction();

            var stockCmd = connection.CreateCommand();
            stockCmd.Transaction = transaction;
            stockCmd.CommandText = "SELECT Stock FROM Products WHERE Name = $name;";
            stockCmd.Parameters.AddWithValue("$name", productName);

            var result = stockCmd.ExecuteScalar();
            if (result == null)
                throw new Exception("Produkt nicht gefunden.");

            int currentStock = Convert.ToInt32(result);

            if (currentStock < quantity)
                throw new Exception($"Nicht genug Bestand für {productName}.");

            var insertCmd = connection.CreateCommand();
            insertCmd.Transaction = transaction;
            insertCmd.CommandText = @"
        INSERT INTO OrderItems (ProductName, Quantity, UnitPrice, OrderDate)
        VALUES ($name, $qty, $price, $date);
    ";
            insertCmd.Parameters.AddWithValue("$name", productName);
            insertCmd.Parameters.AddWithValue("$qty", quantity);
            insertCmd.Parameters.AddWithValue("$price", unitPrice);
            insertCmd.Parameters.AddWithValue("$date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            insertCmd.ExecuteNonQuery();

            var updateCmd = connection.CreateCommand();
            updateCmd.Transaction = transaction;
            updateCmd.CommandText = @"
        UPDATE Products
        SET Stock = Stock - $qty
        WHERE Name = $name;
    ";
            updateCmd.Parameters.AddWithValue("$qty", quantity);
            updateCmd.Parameters.AddWithValue("$name", productName);
            updateCmd.ExecuteNonQuery();

            transaction.Commit();
        }
        private string GetStockReport()
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Name, Stock FROM Products ORDER BY Name;";

            using var reader = cmd.ExecuteReader();

            string report = "Bestand:\n\n";
            while (reader.Read())
            {
                report += $"{reader.GetString(0)} : {reader.GetInt32(1)}{Environment.NewLine}";
            }

            return report;
        }
        private void label5_Click(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void pictureBox12_Click(object sender, EventArgs e)
        {

        }
        void ClearControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is CheckBox)
                    ((CheckBox)c).Checked = false;

                if (c is ComboBox)
                    ((ComboBox)c).SelectedIndex = -1;

                if (c.HasChildren)
                    ClearControls(c);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ClearControls(this);
            richTextBox1.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.AppendText("Sie bestellen:\n\n");
            richTextBox1.AppendText("Num\t"+ "Produk\t\t" + "TotalPrice\n" );

            if (checkBox1.Checked && comboBox1.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox1.Text);
                richTextBox1.AppendText(comboBox1.Text + "\t Dönner Yofka\t"+qty*10 +"€\n");
                SaveOrder("Dönner Yofka", qty, 10);
            }
            if (checkBox2.Checked && comboBox2.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox2.Text);
                richTextBox1.AppendText(comboBox2.Text + "\t Hamburger\t" + qty * 15 + "€\n");
                SaveOrder("Hamburger", qty, 15);
            }
            if (checkBox3.Checked && comboBox3.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox3.Text);
                richTextBox1.AppendText(comboBox3.Text + "\t Pizza\t" + qty * 12 + "€\n");

            }
            if (checkBox11.Checked && comboBox11.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox11.Text);
                richTextBox1.AppendText(comboBox11.Text + "\t Sandwich\t" + qty * 8 + "€\n");
                SaveOrder("Sandwich", qty, 8);
            }
            if (checkBox4.Checked && comboBox4.SelectedIndex != -1)
            {  int qty = Convert.ToInt32(comboBox4.Text);
                richTextBox1.AppendText(comboBox4.Text + "\t Pommes\t" + qty * 7 + "€\n");
                SaveOrder("Pommes", qty, 7);

            }
            if (checkBox5.Checked && comboBox5.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox5.Text);
                richTextBox1.AppendText(comboBox5.Text + "\t Salat\t" + qty * 10 + "€\n");
            }
            if (checkBox6.Checked && comboBox6.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox6.Text);
                richTextBox1.AppendText(comboBox6.Text + "\t Cola\t" + qty * 3 + "€\n");
                SaveOrder("Cola", qty, 3);
            }
            if (checkBox7.Checked && comboBox7.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox7.Text);
                richTextBox1.AppendText(comboBox7.Text + "\t Beer\t" + qty * 3.5 + "€\n");
                SaveOrder("Beer", qty, 3.5);

            }
            if (checkBox12.Checked && comboBox12.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox12.Text);
                richTextBox1.AppendText(comboBox12.Text + "\t Saft\t" + qty * 5 + "€\n");
                SaveOrder("Saft", qty, 5);
            }
            if (checkBox8.Checked && comboBox8.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox8.Text);
                richTextBox1.AppendText(comboBox8.Text + "\t Eis\t" + qty * 3.5 + "€\n");
                SaveOrder("Eis", qty, 3.5);
            }
            if (checkBox9.Checked && comboBox9.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox9.Text);
                richTextBox1.AppendText(comboBox9.Text + "\t Kuchen\t" + qty * 4.5 + "€\n");
                SaveOrder("Kuchen", qty, 4.5);
            }
            if (checkBox10.Checked && comboBox10.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox10.Text);
                richTextBox1.AppendText(comboBox10.Text + "\t Baqlava\t" + qty * 5.5 + "€\n");
                SaveOrder("Baqlava", qty, 5.5);

            }


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Form2().ShowDialog();
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
