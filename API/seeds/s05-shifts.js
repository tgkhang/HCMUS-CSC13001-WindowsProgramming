// s05-shift.js
/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('shift').del()
  
  // Create a few shifts for the past week
  const today = new Date();
  
  await knex('shift').insert([
    {
      start_time: new Date(today.getFullYear(), today.getMonth(), today.getDate() - 7, 8, 0, 0), 
      end_time: new Date(today.getFullYear(), today.getMonth(), today.getDate() - 7, 16, 0, 0), 
      opening_cash: 500.0, 
      closing_cash: 1750.75, 
      total_sales: 1250.75, 
      total_orders: 42, 
      status: 'closed'
    },
    {
      start_time: new Date(today.getFullYear(), today.getMonth(), today.getDate() - 6, 8, 0, 0), 
      end_time: new Date(today.getFullYear(), today.getMonth(), today.getDate() - 6, 16, 0, 0), 
      opening_cash: 500.0, 
      closing_cash: 1875.50,
      total_sales: 1375.50, 
      total_orders: 45, 
      status: 'closed'
    },
    {
      start_time: new Date(today.getFullYear(), today.getMonth(), today.getDate() - 5, 8, 0, 0), 
      end_time: new Date(today.getFullYear(), today.getMonth(), today.getDate() - 5, 16, 0, 0), 
      opening_cash: 500.0, 
      closing_cash: 1680.25,
      total_sales: 1180.25, 
      total_orders: 38, 
      status: 'closed'
    },
    {
      start_time: new Date(today.getFullYear(), today.getMonth(), today.getDate() - 4, 8, 0, 0), 
      end_time: new Date(today.getFullYear(), today.getMonth(), today.getDate() - 4, 16, 0, 0), 
      opening_cash: 500.0, 
      closing_cash: 1920.00,
      total_sales: 1420.00, 
      total_orders: 47, 
      status: 'closed'
    },
    {
      start_time: new Date(today.getFullYear(), today.getMonth(), today.getDate() - 3, 8, 0, 0), 
      end_time: new Date(today.getFullYear(), today.getMonth(), today.getDate() - 3, 16, 0, 0), 
      opening_cash: 500.0, 
      closing_cash: 1830.50,
      total_sales: 1330.50, 
      total_orders: 44, 
      status: 'closed'
    },
    {
      start_time: new Date(today.getFullYear(), today.getMonth(), today.getDate(), 8, 0, 0), 
      end_time: null, 
      opening_cash: 500.0, 
      closing_cash: 500.0,
      total_sales: 0, 
      total_orders: 0, 
      status: 'active'
    }
  ]);
};