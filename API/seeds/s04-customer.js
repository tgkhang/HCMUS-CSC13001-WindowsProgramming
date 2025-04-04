/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('customers').del();
  
  // Inserts seed entries
  await knex('customers').insert([
    { 
      customer_id: 1, 
      name: 'John Smith', 
      phone: '555-123-4567', 
      email: 'john.smith@example.com', 
      address: '123 Main St, Anytown', 
      loyalty_points: 150 
    },
    { 
      customer_id: 2, 
      name: 'Emily Johnson', 
      phone: '555-234-5678', 
      email: 'emily.j@example.com', 
      address: '456 Oak Ave, Somewhere', 
      loyalty_points: 320 
    },
    { 
      customer_id: 3, 
      name: 'Michael Brown', 
      phone: '555-345-6789', 
      email: 'mbrown@example.com', 
      address: null, 
      loyalty_points: 75 
    },
    { 
      customer_id: 4, 
      name: 'Sarah Davis', 
      phone: '555-456-7890', 
      email: 'sarah.d@example.com', 
      address: '789 Pine St, Elsewhere', 
      loyalty_points: 220 
    },
    { 
      customer_id: 5, 
      name: 'Alex Rodriguez', 
      phone: '555-567-8901', 
      email: null, 
      address: null, 
      loyalty_points: 45 
    },
    { 
      customer_id: 6, 
      name: 'Jennifer Wilson', 
      phone: '555-678-9012', 
      email: 'jwilson@example.com', 
      address: '101 Maple Dr, Anytown', 
      loyalty_points: 175 
    },
    { 
      customer_id: 7, 
      name: 'David Lee', 
      phone: '555-789-0123', 
      email: 'dlee@example.com', 
      address: null, 
      loyalty_points: 90 
    }
  ]);
};