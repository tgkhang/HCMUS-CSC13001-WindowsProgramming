exports.up = async function(knex) {
  // category
  await knex.raw(`
    CREATE TABLE category(  
      category_id int NOT NULL PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
      name text NOT NULL,
      description text
    );
    COMMENT ON TABLE category IS 'Product categories';
    COMMENT ON COLUMN category.category_id IS 'Primary key for category';
    COMMENT ON COLUMN category.name IS 'Category name';
    COMMENT ON COLUMN category.description IS 'Category description';
  `);

  // ingredient
  await knex.raw(`
    CREATE TABLE ingredient(  
      ingredient_id int NOT NULL PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
      ingredient_name text NOT NULL,
      category_id int REFERENCES category(category_id) ON DELETE CASCADE,
      stock float NOT NULL,
      unit text NOT NULL,
      purchase_price float NOT NULL,
      supplier text NOT NULL,
      expiry_date date
    );
    COMMENT ON TABLE ingredient IS 'Inventory ingredients';
    COMMENT ON COLUMN ingredient.ingredient_id IS 'Primary key for ingredient';
    COMMENT ON COLUMN ingredient.ingredient_name IS 'Name of the ingredient';
    COMMENT ON COLUMN ingredient.category_id IS 'Foreign key to category';
    COMMENT ON COLUMN ingredient.stock IS 'Current stock quantity';
    COMMENT ON COLUMN ingredient.unit IS 'Unit of measurement (kg, L, pcs)';
    COMMENT ON COLUMN ingredient.purchase_price IS 'Cost per unit';
    COMMENT ON COLUMN ingredient.supplier IS 'Supplier name';
    COMMENT ON COLUMN ingredient.expiry_date IS 'Expiration date';
  `);

  // menu_item
  await knex.raw(`
    CREATE TABLE menu_item(  
      menu_item_id int NOT NULL PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
      name text NOT NULL,
      category_id int REFERENCES category(category_id) ON DELETE CASCADE,
      selling_price float NOT NULL,
      image_path text NOT NULL
    );
    COMMENT ON TABLE menu_item IS 'Menu items for sale';
    COMMENT ON COLUMN menu_item.menu_item_id IS 'Primary key for menu item';
    COMMENT ON COLUMN menu_item.name IS 'Menu item name';
    COMMENT ON COLUMN menu_item.category_id IS 'Foreign key to category';
    COMMENT ON COLUMN menu_item.selling_price IS 'Selling price';
    COMMENT ON COLUMN menu_item.image_path IS 'Path to the menu item image';
  `);

  // customer
  await knex.raw(`
    CREATE TABLE customer(  
      customer_id int NOT NULL PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
      name text NOT NULL,
      phone text NOT NULL,
      email text,
      address text,
      loyalty_points int NOT NULL DEFAULT 0
    );
    COMMENT ON TABLE customer IS 'Customer information';
    COMMENT ON COLUMN customer.customer_id IS 'Primary key for customer';
    COMMENT ON COLUMN customer.name IS 'Customer name';
    COMMENT ON COLUMN customer.phone IS 'Customer phone number';
    COMMENT ON COLUMN customer.email IS 'Customer email address';
    COMMENT ON COLUMN customer.address IS 'Customer address';
    COMMENT ON COLUMN customer.loyalty_points IS 'Accumulated loyalty points';
  `);

  // shift
  await knex.raw(`
    CREATE TABLE shift(  
      shift_id int NOT NULL PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
      start_time timestamp NOT NULL,
      end_time timestamp,
      opening_cash float NOT NULL DEFAULT 0,  
      closing_cash float NOT NULL DEFAULT 0,
      total_sales float NOT NULL DEFAULT 0,
      total_orders int NOT NULL DEFAULT 0,
      status text NOT NULL
    );
    COMMENT ON TABLE shift IS 'Work shifts';
    COMMENT ON COLUMN shift.shift_id IS 'Primary key for shift';
    COMMENT ON COLUMN shift.start_time IS 'Start time of shift';
    COMMENT ON COLUMN shift.end_time IS 'End time of shift';
    COMMENT ON COLUMN shift.opening_cash IS 'Cash at start of shift';
    COMMENT ON COLUMN shift.closing_cash IS 'Cash at end of shift';
    COMMENT ON COLUMN shift.total_sales IS 'Total sales during shift';
    COMMENT ON COLUMN shift.total_orders IS 'Number of orders during shift';
    COMMENT ON COLUMN shift.status IS 'Shift status (active, closed)';
  `);

  // order
  await knex.raw(`
    CREATE TABLE "order"(  
      order_id int NOT NULL PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
      customer_id int REFERENCES customer(customer_id) ON DELETE SET NULL,
      shift_id int REFERENCES shift(shift_id) ON DELETE CASCADE,
      total_amount float NOT NULL,
      discount float NOT NULL DEFAULT 0,
      final_amount float NOT NULL,
      payment_method text NOT NULL,
      status text NOT NULL
    );
    COMMENT ON TABLE "order" IS 'Customer orders';
    COMMENT ON COLUMN "order".order_id IS 'Primary key for order';
    COMMENT ON COLUMN "order".customer_id IS 'Foreign key to customer';
    COMMENT ON COLUMN "order".shift_id IS 'Foreign key to shift';
    COMMENT ON COLUMN "order".total_amount IS 'Total amount before discount';
    COMMENT ON COLUMN "order".discount IS 'Discount amount';
    COMMENT ON COLUMN "order".final_amount IS 'Final amount after discount';
    COMMENT ON COLUMN "order".payment_method IS 'Payment method (cash, credit_card)';
    COMMENT ON COLUMN "order".status IS 'Order status (completed, cancelled)';
  `);

  // order_detail
  await knex.raw(`
    CREATE TABLE order_detail(  
      order_detail_id int NOT NULL PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
      order_id int REFERENCES "order"(order_id) ON DELETE CASCADE,
      menu_item_id int REFERENCES menu_item(menu_item_id) ON DELETE CASCADE,
      quantity int NOT NULL,
      unit_price float NOT NULL,
      subtotal float NOT NULL
    );
    COMMENT ON TABLE order_detail IS 'Order line items';
    COMMENT ON COLUMN order_detail.order_detail_id IS 'Primary key for order detail';
    COMMENT ON COLUMN order_detail.order_id IS 'Foreign key to order';
    COMMENT ON COLUMN order_detail.menu_item_id IS 'Foreign key to menu item';
    COMMENT ON COLUMN order_detail.quantity IS 'Quantity ordered';
    COMMENT ON COLUMN order_detail.unit_price IS 'Price per unit';
    COMMENT ON COLUMN order_detail.subtotal IS 'Line item subtotal';
  `);

  // transaction
  await knex.raw(`
    CREATE TABLE transaction(  
      transaction_id int NOT NULL PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
      order_id int REFERENCES "order"(order_id) ON DELETE CASCADE,
      amount_paid float NOT NULL,
      payment_method text NOT NULL
    );
    COMMENT ON TABLE transaction IS 'Payment transactions';
    COMMENT ON COLUMN transaction.transaction_id IS 'Primary key for transaction';
    COMMENT ON COLUMN transaction.order_id IS 'Foreign key to order';
    COMMENT ON COLUMN transaction.amount_paid IS 'Amount paid';
    COMMENT ON COLUMN transaction.payment_method IS 'Payment method (cash, credit_card)';
  `);

// promotion table
await knex.raw(`
  CREATE TABLE promotion(  
    promo_id int NOT NULL PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    promo_name text NOT NULL CHECK (length(promo_name) <= 100),
    start_date date NOT NULL,
    end_date date NOT NULL,
    menu_item_ids int[] NOT NULL DEFAULT '{}'
  );
  COMMENT ON TABLE promotion IS 'Marketing promotions with associated menu items';
  COMMENT ON COLUMN promotion.promo_id IS 'Primary key for promotion';
  COMMENT ON COLUMN promotion.promo_name IS 'Promotion name (max 100 characters)';
  COMMENT ON COLUMN promotion.start_date IS 'Promotion start date';
  COMMENT ON COLUMN promotion.end_date IS 'Promotion end date';
  COMMENT ON COLUMN promotion.menu_item_ids IS 'Array of menu item IDs associated with this promotion';
`);

// promotion_details table
await knex.raw(`
  CREATE TABLE promotion_details(
    promo_details_id int NOT NULL PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    promo_id int NOT NULL,
    discount_type text NOT NULL CHECK (discount_type IN ('Percentage', 'FixedAmount')),
    discount_value float NOT NULL,
    description text,
    CONSTRAINT fk_promo
      FOREIGN KEY(promo_id) 
      REFERENCES promotion(promo_id)
      ON DELETE CASCADE
  );
  COMMENT ON TABLE promotion_details IS 'Details of marketing promotions';
  COMMENT ON COLUMN promotion_details.promo_details_id IS 'Primary key for promotion details';
  COMMENT ON COLUMN promotion_details.promo_id IS 'Foreign key referencing promotion';
  COMMENT ON COLUMN promotion_details.discount_type IS 'Discount type (Percentage or FixedAmount)';
  COMMENT ON COLUMN promotion_details.discount_value IS 'Discount amount or percentage';
  COMMENT ON COLUMN promotion_details.description IS 'Optional description of the promotion details';
`);

  // notification
  await knex.raw(`
    CREATE TABLE notification(  
      notification_id int NOT NULL PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
      message text NOT NULL,
      created_at timestamp NOT NULL,
      is_read boolean NOT NULL DEFAULT false
    );
    COMMENT ON TABLE notification IS 'System notifications';
    COMMENT ON COLUMN notification.notification_id IS 'Primary key for notification';
    COMMENT ON COLUMN notification.message IS 'Notification message';
    COMMENT ON COLUMN notification.created_at IS 'Creation timestamp';
    COMMENT ON COLUMN notification.is_read IS 'Read status flag';
  `);

  // shift_order
  await knex.raw(`
    CREATE TABLE shift_order(  
      shift_order_id int NOT NULL PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
      shift_id int REFERENCES shift(shift_id) ON DELETE CASCADE,
      order_id int REFERENCES "order"(order_id) ON DELETE CASCADE
    );
    COMMENT ON TABLE shift_order IS 'Orders in a shift';
    COMMENT ON COLUMN shift_order.shift_order_id IS 'Primary key for shift order';
    COMMENT ON COLUMN shift_order.shift_id IS 'Foreign key to shift';
    COMMENT ON COLUMN shift_order.order_id IS 'Foreign key to order';
  `);

  // cash_flow
  await knex.raw(`
    CREATE TABLE cash_flow(  
      cash_flow_id int NOT NULL PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
      shift_id int REFERENCES shift(shift_id) ON DELETE CASCADE,
      transaction_type text NOT NULL,
      amount float NOT NULL,
      timestamp timestamp NOT NULL
    );
    COMMENT ON TABLE cash_flow IS 'Cash transactions';
    COMMENT ON COLUMN cash_flow.cash_flow_id IS 'Primary key for cash flow';
    COMMENT ON COLUMN cash_flow.shift_id IS 'Foreign key to shift';
    COMMENT ON COLUMN cash_flow.transaction_type IS 'Transaction type (opening_balance, cash_payment, card_payment, supplies, refund, petty_cash)';
    COMMENT ON COLUMN cash_flow.amount IS 'Transaction amount';
    COMMENT ON COLUMN cash_flow.timestamp IS 'Transaction timestamp';
  `);
};

exports.down = async function(knex) {
  return knex.raw(`
    DROP TABLE IF EXISTS cash_flow;
    DROP TABLE IF EXISTS shift_order;
    DROP TABLE IF EXISTS notification;
    DROP TABLE IF EXISTS promotion;
    DROP TABLE IF EXISTS transaction;
    DROP TABLE IF EXISTS order_detail;
    DROP TABLE IF EXISTS "order";
    DROP TABLE IF EXISTS shift;
    DROP TABLE IF EXISTS customer;
    DROP TABLE IF EXISTS menu_item;
    DROP TABLE IF EXISTS ingredient;
    DROP TABLE IF EXISTS category;
  `);
};