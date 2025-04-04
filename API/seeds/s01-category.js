/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.seed = async function (knex) {
  // Clear all tables that have dependencies in the right order
  // With the CASCADE delete constraints in place, we only need to delete from parent tables
  // and their child records will be automatically deleted
  
  // First, delete from shift_orders as it depends on both shifts and orders
  await knex("shift_orders").del();
  
  // Delete from tables that depend on orders
  await knex("order_details").del();
  await knex("transactions").del();
  
  // Delete from orders
  await knex("orders").del();
  
  // Delete from tables that depend on shifts
  await knex("cash_flow").del();
  
  // Delete from shifts
  await knex("shifts").del();
  
  // Delete from tables that depend on categories
  await knex("menu_items").del();
  await knex("ingredients").del();
  
  // Delete from customers and other independent tables
  await knex("customers").del();
  await knex("notifications").del();
  await knex("promotions").del();

  // Finally delete from categories
  await knex("categories").del();

  // Inserts seed entries
  await knex("categories").insert([
    {
      category_id: 1,
      name: "Coffee",
      description: "Various coffee beverages",
    },
    {
      category_id: 2,
      name: "Tea",
      description: "Various tea options",
    },
    {
      category_id: 3,
      name: "Pastries",
      description: "Fresh baked pastries and treats",
    },
    {
      category_id: 4,
      name: "Sandwiches",
      description: "Freshly made sandwiches",
    },
    {
      category_id: 5,
      name: "Milk & Alternatives",
      description: "Dairy milk and non-dairy alternatives",
    },
    {
      category_id: 6,
      name: "Syrups & Flavorings",
      description: "Sweeteners and flavor enhancers",
    },
    {
      category_id: 7,
      name: "Seasonal Beverages",
      description: "Limited time special drinks",
    },
  ]);
};