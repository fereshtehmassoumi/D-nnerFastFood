using System.Drawing.Drawing2D;
using Microsoft.Data.Sqlite;

namespace DönnerFastFood
{
    public partial class Form1 : Form
    {
        private readonly string dbPath =
    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fastfood.db");
        private double total = 0;
        public string ordertext = "";

        public Form1()
        {
            InitializeComponent();
            EnsureDatabase();
            SeedProducts();
            SeedInventory();
            SeedRecipes();
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
            SalePrice REAL NOT NULL
        );

        CREATE TABLE IF NOT EXISTS InventoryItems (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL UNIQUE,
            Unit TEXT NOT NULL,
            Stock REAL NOT NULL
        );

        CREATE TABLE IF NOT EXISTS Recipes (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            ProductId INTEGER NOT NULL,
            InventoryItemId INTEGER NOT NULL,
            QuantityNeeded REAL NOT NULL,
            FOREIGN KEY(ProductId) REFERENCES Products(Id),
            FOREIGN KEY(InventoryItemId) REFERENCES InventoryItems(Id)
        );

        CREATE TABLE IF NOT EXISTS Orders (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            OrderDate TEXT NOT NULL,
            Total REAL NOT NULL
        );

        CREATE TABLE IF NOT EXISTS OrderItems (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            OrderId INTEGER NOT NULL,
            ProductId INTEGER NOT NULL,
            Quantity INTEGER NOT NULL,
            UnitPrice REAL NOT NULL,
            FOREIGN KEY(OrderId) REFERENCES Orders(Id),
            FOREIGN KEY(ProductId) REFERENCES Products(Id)
        );
    ";
            cmd.ExecuteNonQuery();
        }

        private void SeedProducts()//Menue mit Preice
        {

            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
        INSERT OR IGNORE INTO Products (Name, SalePrice) VALUES ('Dönner Yofka', 10);
        INSERT OR IGNORE INTO Products (Name, SalePrice) VALUES ('Hamburger', 15);
        INSERT OR IGNORE INTO Products (Name, SalePrice) VALUES ('Pizza', 12);
        INSERT OR IGNORE INTO Products (Name, SalePrice) VALUES ('Sandwich', 8);
        INSERT OR IGNORE INTO Products (Name, SalePrice) VALUES ('Pommes', 7);
        INSERT OR IGNORE INTO Products (Name, SalePrice) VALUES ('Salat', 10);

        INSERT OR IGNORE INTO Products (Name, SalePrice) VALUES ('Cola', 3);
        INSERT OR IGNORE INTO Products (Name, SalePrice) VALUES ('Beer', 3.5);
        INSERT OR IGNORE INTO Products (Name, SalePrice) VALUES ('Saft', 5);

        INSERT OR IGNORE INTO Products (Name, SalePrice) VALUES ('Eis', 3.5);
        INSERT OR IGNORE INTO Products (Name, SalePrice) VALUES ('Kuchen', 4.5);
        INSERT OR IGNORE INTO Products (Name, SalePrice) VALUES ('Baqlava', 5.5);
    ";
            cmd.ExecuteNonQuery();
        }
        private void SeedInventory()//Inventar nach Gramm
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
INSERT OR IGNORE INTO InventoryItems(Name, Unit, Stock) VALUES ('Käse','g','5000');
INSERT OR IGNORE INTO InventoryItems(Name, Unit, Stock) VALUES ('FleiSch','g','10000');
INSERT OR IGNORE INTO InventoryItems(Name, Unit, Stock) VALUES ('Tomatenwerk','g','3000');
INSERT OR IGNORE INTO InventoryItems(Name, Unit, Stock) VALUES ('Mehl','g','10000');
INSERT OR IGNORE INTO InventoryItems(Name, Unit, Stock) VALUES ('Salami','g','5000');
INSERT OR IGNORE INTO InventoryItems(Name, Unit, Stock) VALUES ('Kartofel','g','10000');
INSERT OR IGNORE INTO InventoryItems(Name, Unit, Stock) VALUES ('Soße','g','1000');
INSERT OR IGNORE INTO InventoryItems(Name, Unit, Stock) VALUES ('Kopfsalat','g','1000');
INSERT OR IGNORE INTO InventoryItems(Name, Unit, Stock) VALUES ('Tomaten','g','5000');
INSERT OR IGNORE INTO InventoryItems(Name, Unit, Stock) VALUES ('Gurke','g','1000');
INSERT OR IGNORE INTO InventoryItems(Name, Unit, Stock) VALUES ('Zucker','g','5000');
INSERT OR IGNORE INTO InventoryItems(Name, Unit, Stock) VALUES ('Ice','g','2000');
";
            cmd.ExecuteNonQuery();
        }
        private void SeedRecipes() // Zutaten für jede Bestellung
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
        INSERT OR IGNORE INTO Recipes (ProductId, InventoryItemId, QuantityNeeded)
        VALUES (
            (SELECT Id FROM Products WHERE Name='Pizza'),
            (SELECT Id FROM InventoryItems WHERE Name='Käse'),
            200
        );

        INSERT OR IGNORE INTO Recipes (ProductId, InventoryItemId, QuantityNeeded)
        VALUES (
            (SELECT Id FROM Products WHERE Name='Pizza'),
            (SELECT Id FROM InventoryItems WHERE Name='Mehl'),
            400
        );

        INSERT OR IGNORE INTO Recipes (ProductId, InventoryItemId, QuantityNeeded)
        VALUES (
            (SELECT Id FROM Products WHERE Name='Pizza'),
            (SELECT Id FROM InventoryItems WHERE Name='Salami'),
            150
        );

        INSERT OR IGNORE INTO Recipes (ProductId, InventoryItemId, QuantityNeeded)
        VALUES (
            (SELECT Id FROM Products WHERE Name='Pizza'),
            (SELECT Id FROM InventoryItems WHERE Name='Tomatenwark'),
            50
        );

        INSERT OR IGNORE INTO Recipes (ProductId, InventoryItemId, QuantityNeeded)
        VALUES (
            (SELECT Id FROM Products WHERE Name='Dönner Yofka'),
            (SELECT Id FROM InventoryItems WHERE Name='Fleisch'),
            200
        );

        INSERT OR IGNORE INTO Recipes (ProductId, InventoryItemId, QuantityNeeded)
        VALUES (
            (SELECT Id FROM Products WHERE Name='Dönner Yofka'),
            (SELECT Id FROM InventoryItems WHERE Name='Tomaten'),
            100
        );

        INSERT OR IGNORE INTO Recipes (ProductId, InventoryItemId, QuantityNeeded)
        VALUES (
            (SELECT Id FROM Products WHERE Name='Dönner Yofka'),
            (SELECT Id FROM InventoryItems WHERE Name='Mehl'),
            200
        );

        INSERT OR IGNORE INTO Recipes (ProductId, InventoryItemId, QuantityNeeded)
        VALUES (
            (SELECT Id FROM Products WHERE Name='Dönner Yofka'),
            (SELECT Id FROM InventoryItems WHERE Name='Kopfsalat'),
            100
        );

        INSERT OR IGNORE INTO Recipes (ProductId, InventoryItemId, QuantityNeeded)
        VALUES (
            (SELECT Id FROM Products WHERE Name='Hamburger'),
            (SELECT Id FROM InventoryItems WHERE Name='Fleisch'),
            200
        );

        INSERT OR IGNORE INTO Recipes (ProductId, InventoryItemId, QuantityNeeded)
        VALUES (
            (SELECT Id FROM Products WHERE Name='Hamburger'),
            (SELECT Id FROM InventoryItems WHERE Name='Tomaten'),
            100
        );

        INSERT OR IGNORE INTO Recipes (ProductId, InventoryItemId, QuantityNeeded)
        VALUES (
            (SELECT Id FROM Products WHERE Name='Hamburger'),
            (SELECT Id FROM InventoryItems WHERE Name='Mehl'),
            300
        );

        INSERT OR IGNORE INTO Recipes (ProductId, InventoryItemId, QuantityNeeded)
        VALUES (
            (SELECT Id FROM Products WHERE Name='Hamburger'),
            (SELECT Id FROM InventoryItems WHERE Name='Gurke'),
            100
        );

        INSERT OR IGNORE INTO Recipes (ProductId, InventoryItemId, QuantityNeeded)
        VALUES (
            (SELECT Id FROM Products WHERE Name='Eis'),
            (SELECT Id FROM InventoryItems WHERE Name='Zucker'),
            50
        );

        INSERT OR IGNORE INTO Recipes (ProductId, InventoryItemId, QuantityNeeded)
        VALUES (
            (SELECT Id FROM Products WHERE Name='Eis'),
            (SELECT Id FROM InventoryItems WHERE Name='Ice'),
            100
        );
    ";
            cmd.ExecuteNonQuery();
        }


        private void ReduceInventory(string productName, int orderQty)//Reduzierende Materiale
        {
            /*using var connection = new SqliteConnection($"Datasource={dbPath}");
            connection.Open();
            var cmd = connection.CreateCommand();
            using var transaction=connection.BeginTransaction();
            var recipeCmd=connection.CreateCommand();
            recipeCmd.Transaction = transaction;
            recipeCmd.CommandText = @"";*/
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            using var transaction = connection.BeginTransaction();

            var recipeCmd = connection.CreateCommand();
            recipeCmd.Transaction = transaction;
            recipeCmd.CommandText = @"
        SELECT ii.Id, ii.Name, ii.Stock, r.QuantityNeeded
        FROM Recipes r
        JOIN Products p ON r.ProductId = p.Id
        JOIN InventoryItems ii ON r.InventoryItemId = ii.Id
        WHERE p.Name = $productName;
    ";
            recipeCmd.Parameters.AddWithValue("$productName", productName);

            using var reader = recipeCmd.ExecuteReader();

            var itemsToUpdate = new List<(int id, string name, double stock, double neededPerUnit)>();

            while (reader.Read())
            {
                itemsToUpdate.Add((
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetDouble(2),
                    reader.GetDouble(3)
                ));
            }

            reader.Close();

            foreach (var item in itemsToUpdate)
            {
                double totalNeeded = item.neededPerUnit * orderQty;

                if (item.stock < totalNeeded)
                    throw new Exception($"Nicht genug Bestand für {item.name}");
            }

            foreach (var item in itemsToUpdate)
            {
                double totalNeeded = item.neededPerUnit * orderQty;

                var updateCmd = connection.CreateCommand();
                updateCmd.Transaction = transaction;
                updateCmd.CommandText = @"
            UPDATE InventoryItems
            SET Stock = Stock - $needed
            WHERE Id = $id;
        ";
                updateCmd.Parameters.AddWithValue("$needed", totalNeeded);
                updateCmd.Parameters.AddWithValue("$id", item.id);
                updateCmd.ExecuteNonQuery();
            }

            transaction.Commit();
        }



        private string GetStockReport()//Bericht
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Name, Stock, Unit  FROM InventoryItems ORDER BY Name;";

            using var reader = cmd.ExecuteReader();

            string report = "Bestand:\n\n";
            while (reader.Read())
            {
                report += $"{reader.GetString(0)} : " +
                    $"{reader.GetDouble(1)}" +
                    reader.GetString(2) +
                    $"{Environment.NewLine}";
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
            total = 0;
            richTextBox1.AppendText("Sie bestellen:\n\n");
            richTextBox1.AppendText("Num\t" + "Produk\t\t" + "TotalPrice\n");

            if (checkBox1.Checked && comboBox1.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox1.Text);
                richTextBox1.AppendText(comboBox1.Text + "\t Dönner Yofka\t" + qty * 10 + "€\n");
                //SaveOrder("Dönner Yofka", qty, 10);
                ReduceInventory("Dönner Yofka", qty);
                double deTotal = qty * 10;
                total += deTotal;
            }
            if (checkBox2.Checked && comboBox2.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox2.Text);
                double deTotal = qty * 15;
                richTextBox1.AppendText(comboBox2.Text + "\t Hamburger\t" + deTotal + "€\n");
                //SaveOrder("Hamburger", qty, 15);
                ReduceInventory("Hamburger", qty);
                total += deTotal;
            }
            if (checkBox3.Checked && comboBox3.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox3.Text);
                double deTotal = qty * 12;
                richTextBox1.AppendText(comboBox3.Text + "\t Pizza\t" + deTotal + "€\n");
                ReduceInventory("Pizza", qty);
                //SaveOrder("Pizza", qty, 12);
                total += deTotal;

            }
            if (checkBox11.Checked && comboBox11.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox11.Text);
                double deTotal = qty * 8;
                richTextBox1.AppendText(comboBox11.Text + "\t Sandwich\t" + deTotal + "€\n");
                // SaveOrder("Sandwich", qty, 8);
                total += deTotal;
            }
            if (checkBox4.Checked && comboBox4.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox4.Text);
                double deTotal = qty * 7;
                richTextBox1.AppendText(comboBox4.Text + "\t Pommes\t" + deTotal + "€\n");
                //SaveOrder("Pommes", qty, 7);
                total += deTotal;

            }
            if (checkBox5.Checked && comboBox5.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox5.Text);
                double deTotal = qty * 10;
                richTextBox1.AppendText(comboBox5.Text + "\t Salat\t" + deTotal + "€\n");
                // SaveOrder("Salat", qty, 10);
                total += deTotal;
            }
            if (checkBox6.Checked && comboBox6.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox6.Text);
                double deTotal = qty * 3;
                richTextBox1.AppendText(comboBox6.Text + "\t Cola\t" + deTotal + "€\n");
                total += deTotal;
                // SaveOrder("Cola", qty, 3);
            }
            if (checkBox7.Checked && comboBox7.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox7.Text);
                double deTotal = qty * 3.5;
                richTextBox1.AppendText(comboBox7.Text + "\t Beer\t" + deTotal + "€\n");
                //SaveOrder("Beer", qty, 3.5);
                total += deTotal;

            }
            if (checkBox12.Checked && comboBox12.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox12.Text);
                double deTotal = qty * 5;
                richTextBox1.AppendText(comboBox12.Text + "\t Saft\t" + deTotal + "€\n");
                //SaveOrder("Saft", qty, 5);
                total += deTotal;
            }
            if (checkBox8.Checked && comboBox8.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox8.Text);
                double deTotal = qty * 3.5;
                richTextBox1.AppendText(comboBox8.Text + "\t Eis\t" + deTotal + "€\n");
                //SaveOrder("Eis", qty, 3.5);
                total += deTotal;
            }
            if (checkBox9.Checked && comboBox9.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox9.Text);
                double deTotal = qty * 4.5;
                richTextBox1.AppendText(comboBox9.Text + "\t Kuchen\t" + deTotal + "€\n");
                //SaveOrder("Kuchen", qty, 4.5);
                total += deTotal;
            }
            if (checkBox10.Checked && comboBox10.SelectedIndex != -1)
            {
                int qty = Convert.ToInt32(comboBox10.Text);
                double deTotal = qty * 5.5;
                richTextBox1.AppendText(comboBox10.Text + "\t Baqlava\t" + deTotal + "€\n");
                //SaveOrder("Baqlava", qty, 5.5);
                total += deTotal;

            }
            richTextBox1.Text += "Gesämtlich : " + total.ToString() + "€";


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();
            f.orderText = richTextBox1.Text;
            f.totalPrice = total;
            f.ShowDialog();
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.ShowDialog();

        }
    }
}
