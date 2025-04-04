// s10-notification.js
/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('notification').del()
  
  const today = new Date();
  
  await knex('notification').insert([
    {
      message: 'Low stock alert: Coffee Beans (2kg remaining)',
      created_at: new Date(today.getFullYear(), today.getMonth(), today.getDate() - 2, 10, 15, 0),
      is_read: true
    },
    {
      message: 'New promotion "Summer Special" is now active',
      created_at: new Date(today.getFullYear(), today.getMonth(), today.getDate() - 5, 9, 0, 0),
      is_read: true
    },
    {
      message: 'Daily sales target reached!',
      created_at: new Date(today.getFullYear(), today.getMonth(), today.getDate() - 1, 18, 30, 0),
      is_read: false
    },
    {
      message: 'System maintenance scheduled for tomorrow at 23:00',
      created_at: new Date(today.getFullYear(), today.getMonth(), today.getDate(), 14, 45, 0),
      is_read: false
    },
    {
      message: 'New menu items added to Beverages category',
      created_at: new Date(today.getFullYear(), today.getMonth(), today.getDate() - 3, 11, 20, 0),
      is_read: true
    }
  ]);
};