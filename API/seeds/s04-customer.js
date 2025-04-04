/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('customer').del()
  
  await knex('customer').insert([
    {
      name: 'John Smith', 
      phone: '+1234567890', 
      email: 'john.smith@example.com', 
      address: '123 Main St, Anytown, USA', 
      loyalty_points: 150
    },
    {
      name: 'Jane Doe', 
      phone: '+1987654321', 
      email: 'jane.doe@example.com', 
      address: '456 Oak Ave, Somecity, USA', 
      loyalty_points: 200
    },
    {
      name: 'Robert Johnson', 
      phone: '+1567890123', 
      email: 'robert.j@example.com', 
      address: '789 Pine Blvd, Othertown, USA', 
      loyalty_points: 75
    },
    {
      name: 'Lisa Chen', 
      phone: '+1456789012', 
      email: 'lisa.chen@example.com', 
      address: '101 Cedar Ln, Newcity, USA', 
      loyalty_points: 300
    },
    {
      name: 'Michael Brown', 
      phone: '+1345678901', 
      email: 'mbrown@example.com', 
      address: '202 Maple Dr, Oldtown, USA', 
      loyalty_points: 50
    }
  ]);
};