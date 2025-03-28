/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('Customer').del();
  
  // Inserts seed entries
  await knex('Customer').insert([
    { 
      CustomerID: 1, 
      Name: 'John Smith', 
      Phone: '555-123-4567', 
      Email: 'john.smith@example.com', 
      Address: '123 Main St, Anytown', 
      LoyaltyPoints: 150 
    },
    { 
      CustomerID: 2, 
      Name: 'Emily Johnson', 
      Phone: '555-234-5678', 
      Email: 'emily.j@example.com', 
      Address: '456 Oak Ave, Somewhere', 
      LoyaltyPoints: 320 
    },
    { 
      CustomerID: 3, 
      Name: 'Michael Brown', 
      Phone: '555-345-6789', 
      Email: 'mbrown@example.com', 
      Address: null, 
      LoyaltyPoints: 75 
    },
    { 
      CustomerID: 4, 
      Name: 'Sarah Davis', 
      Phone: '555-456-7890', 
      Email: 'sarah.d@example.com', 
      Address: '789 Pine St, Elsewhere', 
      LoyaltyPoints: 220 
    },
    { 
      CustomerID: 5, 
      Name: 'Alex Rodriguez', 
      Phone: '555-567-8901', 
      Email: null, 
      Address: null, 
      LoyaltyPoints: 45 
    },
    { 
      CustomerID: 6, 
      Name: 'Jennifer Wilson', 
      Phone: '555-678-9012', 
      Email: 'jwilson@example.com', 
      Address: '101 Maple Dr, Anytown', 
      LoyaltyPoints: 175 
    },
    { 
      CustomerID: 7, 
      Name: 'David Lee', 
      Phone: '555-789-0123', 
      Email: 'dlee@example.com', 
      Address: null, 
      LoyaltyPoints: 90 
    }
  ]);
};