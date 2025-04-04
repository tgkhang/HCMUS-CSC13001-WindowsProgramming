/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.seed = async function (knex) {
  // Clear all tables that have dependencies in the right order
  // With the CASCADE delete constraints in place, we only need to delete from parent tables
  // and their child records will be automatically deleted
  
  // First, delete from ShiftOrders as it depends on both Shifts and Orders
  await knex("ShiftOrders").del();
  
  // Delete from tables that depend on Orders
  await knex("OrdersDetails").del();
  await knex("Transactions").del();
  
  // Delete from Orders
  await knex("Orders").del();
  
  // Delete from tables that depend on Shifts
  await knex("CashFlow").del();
  
  // Delete from Shifts
  await knex("Shifts").del();
  
  // Delete from tables that depend on Categories
  await knex("MenuItems").del();
  await knex("Ingredients").del();
  
  // Delete from Customer and other independent tables
  await knex("Customer").del();
  await knex("Notifications").del();
  await knex("Promotions").del();

  // Finally delete from Categories
  await knex("Categories").del();

  // Inserts seed entries
  await knex("Categories").insert([
    {
      CategoryID: 1,
      Name: "Coffee",
      Description: "Various coffee beverages",
    },
    {
      CategoryID: 2,
      Name: "Tea",
      Description: "Various tea options",
    },
    {
      CategoryID: 3,
      Name: "Pastries",
      Description: "Fresh baked pastries and treats",
    },
    {
      CategoryID: 4,
      Name: "Sandwiches",
      Description: "Freshly made sandwiches",
    },
    {
      CategoryID: 5,
      Name: "Milk & Alternatives",
      Description: "Dairy milk and non-dairy alternatives",
    },
    {
      CategoryID: 6,
      Name: "Syrups & Flavorings",
      Description: "Sweeteners and flavor enhancers",
    },
    {
      CategoryID: 7,
      Name: "Seasonal Beverages",
      Description: "Limited time special drinks",
    },
  ]);
};