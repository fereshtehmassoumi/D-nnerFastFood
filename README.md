# Dönner Fast Food - WinForms Mini ERP

A desktop fast-food ordering application built with **C# WinForms** and **SQLite**, extended toward a **Mini ERP** concept.

This project started as a simple ordering system and is being developed into a small restaurant management solution that covers:

- customer orders
- invoice preview
- PDF generation
- payment workflow
- stock management
- recipe-based inventory reduction
- reporting

---

## 🚀 Project Idea

The application simulates a small fast-food restaurant system where the user can:

- choose food and drinks
- select quantities
- calculate the total price
- preview the order
- generate a PDF invoice
- manage product stock through a mini ERP logic

Example:

If a customer orders **Pizza**, the system can reduce the required raw materials from inventory automatically, such as:

- 150g cheese
- 1 dough
- 100g tomato

This makes the project more than a simple POS interface — it becomes the foundation of a **Mini ERP for restaurant operations**.

---

## 🧩 Main Features

### Ordering System
- select products using checkboxes
- choose quantities using comboboxes
- calculate line totals and grand total
- display the full order in a RichTextBox

### Payment Workflow
- open a payment form after ordering
- show total amount to be paid
- support different payment methods such as:
  - card
  - cash

### PDF Invoice
- create a real invoice PDF using **QuestPDF**
- show ordered items and total price
- automatically open the generated PDF

### Database Integration
- uses **SQLite**
- stores products and stock data
- stores order information
- supports future ERP expansion

### Mini ERP Logic
- define products
- define inventory items
- define recipes for products
- automatically reduce stock when an item is ordered

### Reporting
- current stock report
- low-stock tracking
- future extension for sales reporting

---

## 🏗 Technologies Used

- **C#**
- **WinForms**
- **SQLite**
- **QuestPDF**
- **.NET**

---

## 🧠 ERP Concept in This Project

This project is being developed as a **Mini ERP prototype** for a fast-food business.

The ERP structure includes:

### 1. Products
Finished products sold to customers, for example:
- Pizza
- Hamburger
- Dönner Yofka
- Pommes
- Cola

### 2. Inventory Items
Raw materials stored in stock, for example:
- Cheese
- Dough
- Tomato
- Bread
- Meat
- Potatoes
- Oil

### 3. Recipes
Each product can be connected to ingredients.

Example:

**Pizza**
- Cheese → 150g
- Dough → 1 piece
- Tomato → 100g

### 4. Orders
Customer orders are stored with date and total amount.

### 5. Order Items
Each order contains individual ordered products with quantity and unit price.

### ERP Goal
When a product is sold, the system should:
1. save the order
2. save the ordered items
3. read the recipe
4. reduce the correct amount of raw materials from inventory

---

## 📌 Current Implementation Status

### Implemented
- WinForms UI
- product selection
- quantity selection
- order summary
- total price calculation
- payment form
- PDF invoice generation
- SQLite database setup
- initial stock and product seeding
- rounded form UI styling

### In Progress
- recipe table
- automatic ingredient reduction
- stock report form
- order history
- management panel

### Planned
- low-stock warning
- restocking form
- admin dashboard
- sales statistics
- printable receipt history

---

## 🖥 Application Workflow

1. User selects products and quantities
2. Order summary is generated
3. Total amount is calculated
4. Payment preview opens
5. PDF invoice is created
6. Inventory is updated based on ordered items
7. Reports can be generated for management

---

## 📷 UI Sections

The project currently contains multiple forms:

### Form1
Main ordering screen:
- food items
- quantity selection
- order summary
- total calculation

### Form2 / Form3
Used for:
- payment
- invoice preview
- PDF workflow

---

## 💾 Database Design

### Products
Stores customer-facing products and sale prices

### InventoryItems
Stores ingredients/raw materials and stock values

### Recipes
Defines how much inventory is needed per product

### Orders
Stores order headers

### OrderItems
Stores products inside each order

---

## ▶️ How to Run

1. Open the project in **Visual Studio**
2. Restore NuGet packages
3. Make sure these packages are installed:
   - `Microsoft.Data.Sqlite`
   - `QuestPDF`
4. Run the project with **F5**

---

## 📄 PDF Output

The system can generate an invoice PDF that includes:
- ordered items
- prices
- final total

The PDF is opened automatically after creation.

---

## 🎯 Learning Goals of This Project

This project was created to practice and improve skills in:

- C# WinForms development
- event handling
- object-oriented design
- UI logic
- database integration
- PDF generation
- ERP-style business logic
- stock and order management

---

## 📈 Why This Project Is Important

This is not only a beginner food-ordering app.  
It is also a practical step toward building a real business application.

It shows knowledge of:
- user interface design
- customer order processing
- database structure
- invoice generation
- inventory logic
- scalable architecture

---

## 🔮 Future Improvements

- full inventory dashboard
- CRUD forms for stock management
- recipe editor
- daily sales report
- user roles (admin/cashier)
- export reports to PDF or Excel
- multilingual UI

---

## 👩‍💻 Author

**Fereshteh Masoumi**

This project is part of my learning journey in C#, WinForms, and software development for real-world business scenarios.

---
