/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('Shifts').del();
  
  // Inserts seed entries
  await knex('Shifts').insert([
    { 
      ShiftID: 1, 
      StartTime: new Date('2025-03-25 07:00:00'), 
      EndTime: new Date('2025-03-25 15:00:00'), 
      OpeningCash: 200.00, 
      TotalSales: 1250.75, 
      TotalOrders: 42, 
      Status: 'Closed' 
    },
    { 
      ShiftID: 2, 
      StartTime: new Date('2025-03-25 15:00:00'), 
      EndTime: new Date('2025-03-25 23:00:00'), 
      OpeningCash: 250.00, 
      TotalSales: 1045.50, 
      TotalOrders: 35, 
      Status: 'Closed' 
    },
    { 
      ShiftID: 3, 
      StartTime: new Date('2025-03-26 07:00:00'), 
      EndTime: new Date('2025-03-26 15:00:00'), 
      OpeningCash: 200.00, 
      TotalSales: 1378.25, 
      TotalOrders: 45, 
      Status: 'Closed' 
    },
    { 
      ShiftID: 4, 
      StartTime: new Date('2025-03-26 15:00:00'), 
      EndTime: new Date('2025-03-26 23:00:00'), 
      OpeningCash: 250.00, 
      TotalSales: 1122.75, 
      TotalOrders: 38, 
      Status: 'Closed' 
    },
    { 
      ShiftID: 5, 
      StartTime: new Date('2025-03-27 07:00:00'), 
      EndTime: new Date('2025-03-27 15:00:00'), 
      OpeningCash: 200.00, 
      TotalSales: 1195.50, 
      TotalOrders: 40, 
      Status: 'Closed' 
    },
    { 
      ShiftID: 6, 
      StartTime: new Date('2025-03-27 15:00:00'), 
      EndTime: null, 
      OpeningCash: 250.00, 
      TotalSales: 520.25, 
      TotalOrders: 18, 
      Status: 'Open' 
    }
  ]);
};