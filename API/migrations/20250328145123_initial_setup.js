exports.up = function (knex) {
  return (
    knex.schema
      // categories
      .createTable("categories", (table) => {
        table.increments("category_id").primary();
        table.text("name").notNullable();
        table.text("description").nullable();
      })

      // ingredients
      .createTable("ingredients", (table) => {
        table.increments("ingredient_id").primary();
        table.text("ingredient_name").notNullable();
        table
          .integer("category_id")
          .unsigned()
          .references("category_id")
          .inTable("categories")
          .onDelete("CASCADE");
        table.float("stock").notNullable();
        table.text("unit").notNullable();
        table.float("purchase_price").notNullable();
        table.text("supplier").notNullable();
        table.date("expiry_date").nullable();
      })

      // menu_items
      .createTable("menu_items", (table) => {
        table.increments("menu_item_id").primary();
        table.text("name").notNullable();
        table
          .integer("category_id")
          .unsigned()
          .references("category_id")
          .inTable("categories")
          .onDelete("CASCADE");
        table.float("selling_price").notNullable();
        table.text("image_path").notNullable();
      })

      // customers
      .createTable("customers", (table) => {
        table.increments("customer_id").primary();
        table.text("name").notNullable();
        table.text("phone").notNullable();
        table.text("email").nullable();
        table.text("address").nullable();
        table.integer("loyalty_points").defaultTo(0).notNullable();
      })

      // shifts
      .createTable("shifts", (table) => {
        table.increments("shift_id").primary();
        table.datetime("start_time").notNullable();
        table.datetime("end_time").nullable();
        table.float("opening_cash").notNullable();
        table.float("total_sales").defaultTo(0).notNullable();
        table.integer("total_orders").defaultTo(0).notNullable();
        table.text("status").notNullable();
      })

      // orders
      .createTable("orders", (table) => {
        table.increments("order_id").primary();
        table
          .integer("customer_id")
          .unsigned()
          .references("customer_id")
          .inTable("customers")
          .onDelete("SET NULL");
        table
          .integer("shift_id")
          .unsigned()
          .references("shift_id")
          .inTable("shifts")
          .onDelete("CASCADE");
        table.float("total_amount").notNullable();
        table.float("discount").defaultTo(0).notNullable();
        table.float("final_amount").notNullable();
        table.text("payment_method").notNullable();
        table.text("status").notNullable();
      })

      // order_details
      .createTable("order_details", (table) => {
        table.increments("order_detail_id").primary();
        table
          .integer("order_id")
          .unsigned()
          .references("order_id")
          .inTable("orders")
          .onDelete("CASCADE");
        table
          .integer("menu_item_id")
          .unsigned()
          .references("menu_item_id")
          .inTable("menu_items")
          .onDelete("CASCADE");
        table.integer("quantity").notNullable();
        table.float("unit_price").notNullable();
        table.float("subtotal").notNullable();
      })

      // transactions
      .createTable("transactions", (table) => {
        table.increments("transaction_id").primary();
        table
          .integer("order_id")
          .unsigned()
          .references("order_id")
          .inTable("orders")
          .onDelete("CASCADE");
        table.float("amount_paid").notNullable();
        table.text("payment_method").notNullable();
      })

      // promotions
      .createTable("promotions", (table) => {
        table.increments("promo_id").primary();
        table.text("promo_name").notNullable();
        table.text("discount_type").notNullable();
        table.float("discount_value").notNullable();
        table.date("start_date").notNullable();
        table.date("end_date").notNullable();
      })

      // notifications
      .createTable("notifications", (table) => {
        table.increments("notification_id").primary();
        table.text("message").notNullable();
        table.datetime("created_at").notNullable();
        table.boolean("is_read").defaultTo(false).notNullable();
      })

      // shift_orders
      .createTable("shift_orders", (table) => {
        table.increments("shift_order_id").primary();
        table
          .integer("shift_id")
          .unsigned()
          .references("shift_id")
          .inTable("shifts")
          .onDelete("CASCADE");
        table
          .integer("order_id")
          .unsigned()
          .references("order_id")
          .inTable("orders")
          .onDelete("CASCADE");
      })

      // cash_flow
      .createTable("cash_flow", (table) => {
        table.increments("cash_flow_id").primary();
        table
          .integer("shift_id")
          .unsigned()
          .references("shift_id")
          .inTable("shifts")
          .onDelete("CASCADE");
        table.text("transaction_type").notNullable();
        table.float("amount").notNullable();
        table.datetime("timestamp").notNullable();
      })
  );
};

exports.down = function (knex) {
  return knex.schema
    .dropTableIfExists("cash_flow")
    .dropTableIfExists("shift_orders")
    .dropTableIfExists("notifications")
    .dropTableIfExists("promotions")
    .dropTableIfExists("transactions")
    .dropTableIfExists("order_details")
    .dropTableIfExists("orders")
    .dropTableIfExists("shifts")
    .dropTableIfExists("customers")
    .dropTableIfExists("menu_items")
    .dropTableIfExists("ingredients")
    .dropTableIfExists("categories");
};