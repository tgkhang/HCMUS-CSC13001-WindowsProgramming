/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('Notifications').del();
  
  // Inserts seed entries
  await knex('Notifications').insert([
    { 
      NotificationID: 1, 
      Message: 'Low stock alert: Espresso Beans below 30kg', 
      CreatedAt: new Date('2025-03-24 09:15:00'), 
      IsRead: true 
    },
    { 
      NotificationID: 2, 
      Message: 'Shift #1 closed successfully', 
      CreatedAt: new Date('2025-03-25 15:05:00'), 
      IsRead: true 
    },
    { 
      NotificationID: 3, 
      Message: 'New promotion added: Spring Special', 
      CreatedAt: new Date('2025-03-01 08:30:00'), 
      IsRead: true 
    },
    { 
      NotificationID: 4, 
      Message: 'Approaching expiry: Whole Milk (04/15/2025)', 
      CreatedAt: new Date('2025-03-25 10:00:00'), 
      IsRead: false 
    },
    { 
      NotificationID: 5, 
      Message: 'Daily sales report available', 
      CreatedAt: new Date('2025-03-26 23:30:00'), 
      IsRead: false 
    },
    { 
      NotificationID: 6, 
      Message: 'System update scheduled for March 30, 2025', 
      CreatedAt: new Date('2025-03-27 11:45:00'), 
      IsRead: false 
    },
    { 
      NotificationID: 7, 
      Message: 'New customer registered: David Lee', 
      CreatedAt: new Date('2025-03-26 14:20:00'), 
      IsRead: true 
    }
  ]);
};