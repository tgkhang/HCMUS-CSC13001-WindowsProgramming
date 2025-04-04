/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('notifications').del();
  
  // Inserts seed entries
  await knex('notifications').insert([
    { 
      notification_id: 1, 
      message: 'Low stock alert: Espresso Beans below 30kg', 
      created_at: new Date('2025-03-24 09:15:00'), 
      is_read: true 
    },
    { 
      notification_id: 2, 
      message: 'Shift #1 closed successfully', 
      created_at: new Date('2025-03-25 15:05:00'), 
      is_read: true 
    },
    { 
      notification_id: 3, 
      message: 'New promotion added: Spring Special', 
      created_at: new Date('2025-03-01 08:30:00'), 
      is_read: true 
    },
    { 
      notification_id: 4, 
      message: 'Approaching expiry: Whole Milk (04/15/2025)', 
      created_at: new Date('2025-03-25 10:00:00'), 
      is_read: false 
    },
    { 
      notification_id: 5, 
      message: 'Daily sales report available', 
      created_at: new Date('2025-03-26 23:30:00'), 
      is_read: false 
    },
    { 
      notification_id: 6, 
      message: 'System update scheduled for March 30, 2025', 
      created_at: new Date('2025-03-27 11:45:00'), 
      is_read: false 
    },
    { 
      notification_id: 7, 
      message: 'New customer registered: David Lee', 
      created_at: new Date('2025-03-26 14:20:00'), 
      is_read: true 
    }
  ]);
};