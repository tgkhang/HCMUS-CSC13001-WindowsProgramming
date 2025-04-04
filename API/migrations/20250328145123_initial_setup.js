exports.up = function (knex) {
  return (
    knex.schema
      // Categories
      .createTable("Categories", (table) => {
        table.increments("CategoryID").primary();
        table.text("Name").notNullable();
        table.text("Description").nullable();
      })

      // Ingredients
      .createTable("Ingredients", (table) => {
        table.increments("IngredientID").primary();
        table.text("IngredientName").notNullable();
        table
          .integer("CategoryID")
          .unsigned()
          .references("CategoryID")
          .inTable("Categories")
          .onDelete("CASCADE"); // Ensure that if a category is deleted, the ingredients are also deleted
        table.float("Stock").notNullable();
        table.text("Unit").notNullable();
        table.float("PurchasePrice").notNullable();
        table.text("Supplier").notNullable();
        table.date("ExpiryDate").nullable();
      })

      // MenuItems
      .createTable("MenuItems", (table) => {
        table.increments("MenuItemID").primary();
        table.text("Name").notNullable();
        table
          .integer("CategoryID")
          .unsigned()
          .references("CategoryID")
          .inTable("Categories")
          .onDelete("CASCADE"); // Ensure that if a category is deleted, the menu items are also deleted
        table.float("SellingPrice").notNullable();
        table.text("ImagePath").notNullable();
      })

      // Customer
      .createTable("Customer", (table) => {
        table.increments("CustomerID").primary();
        table.text("Name").notNullable();
        table.text("Phone").notNullable();
        table.text("Email").nullable();
        table.text("Address").nullable();
        table.integer("LoyaltyPoints").defaultTo(0).notNullable();
      })

      // Shifts
      .createTable("Shifts", (table) => {
        table.increments("ShiftID").primary();
        table.datetime("StartTime").notNullable();
        table.datetime("EndTime").nullable();
        table.float("OpeningCash").notNullable();
        table.float("TotalSales").defaultTo(0).notNullable();
        table.integer("TotalOrders").defaultTo(0).notNullable();
        table.text("Status").notNullable();
      })

      // Orders
      .createTable("Orders", (table) => {
        table.increments("OrderID").primary();
        table
          .integer("CustomerID")
          .unsigned()
          .references("CustomerID")
          .inTable("Customer")
          .onDelete("SET NULL"); // Allow deleting customers by setting CustomerID to NULL
        table
          .integer("ShiftID")
          .unsigned()
          .references("ShiftID")
          .inTable("Shifts")
          .onDelete("CASCADE"); // Ensure that if a shift is deleted, its orders are also deleted
        table.float("TotalAmount").notNullable();
        table.float("Discount").defaultTo(0).notNullable();
        table.float("FinalAmount").notNullable();
        table.text("PaymentMethod").notNullable();
        table.text("Status").notNullable();
      })

      // OrdersDetails
      .createTable("OrdersDetails", (table) => {
        table.increments("OrderDetailID").primary();
        table
          .integer("OrderID")
          .unsigned()
          .references("OrderID")
          .inTable("Orders")
          .onDelete("CASCADE"); // Ensure that if an order is deleted, its details are also deleted
        table
          .integer("MenuItemID")
          .unsigned()
          .references("MenuItemID")
          .inTable("MenuItems")
          .onDelete("CASCADE"); // Ensure that if a menu item is deleted, related order details are also deleted
        table.integer("Quantity").notNullable();
        table.float("UnitPrice").notNullable();
        table.float("Subtotal").notNullable();
      })

      // Transactions
      .createTable("Transactions", (table) => {
        table.increments("TransactionID").primary();
        table
          .integer("OrderID")
          .unsigned()
          .references("OrderID")
          .inTable("Orders")
          .onDelete("CASCADE"); // Ensure that if an order is deleted, its transactions are also deleted
        table.float("AmountPaid").notNullable();
        table.text("PaymentMethod").notNullable();
      })

      // Promotions
      .createTable("Promotions", (table) => {
        table.increments("PromoID").primary();
        table.text("PromoName").notNullable();
        table.text("DiscountType").notNullable();
        table.float("DiscountValue").notNullable();
        table.date("StartDate").notNullable();
        table.date("EndDate").notNullable();
      })

      // Notifications
      .createTable("Notifications", (table) => {
        table.increments("NotificationID").primary();
        table.text("Message").notNullable();
        table.datetime("CreatedAt").notNullable();
        table.boolean("IsRead").defaultTo(false).notNullable();
      })

      // ShiftOrders
      .createTable("ShiftOrders", (table) => {
        table.increments("ShiftOrderID").primary();
        table
          .integer("ShiftID")
          .unsigned()
          .references("ShiftID")
          .inTable("Shifts")
          .onDelete("CASCADE"); // Ensure that if a shift is deleted, its shift orders are also deleted
        table
          .integer("OrderID")
          .unsigned()
          .references("OrderID")
          .inTable("Orders")
          .onDelete("CASCADE"); // Ensure that if an order is deleted, its shift orders are also deleted
      })

      // CashFlow
      .createTable("CashFlow", (table) => {
        table.increments("CashFlowID").primary();
        table
          .integer("ShiftID")
          .unsigned()
          .references("ShiftID")
          .inTable("Shifts")
          .onDelete("CASCADE"); // Ensure that if a shift is deleted, its cash flow records are also deleted
        table.text("TransactionType").notNullable();
        table.float("Amount").notNullable();
        table.datetime("Timestamp").notNullable();
      })
  );
};

exports.down = function (knex) {
  return knex.schema
    .dropTableIfExists("CashFlow")
    .dropTableIfExists("ShiftOrders")
    .dropTableIfExists("Notifications")
    .dropTableIfExists("Promotions")
    .dropTableIfExists("Transactions")
    .dropTableIfExists("OrdersDetails")
    .dropTableIfExists("Orders")
    .dropTableIfExists("Shifts")
    .dropTableIfExists("Customer")
    .dropTableIfExists("MenuItems")
    .dropTableIfExists("Ingredients")
    .dropTableIfExists("Categories");
};
