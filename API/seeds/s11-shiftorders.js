/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('ShiftOrders').del();
  
  // Inserts seed entries
  await knex('ShiftOrders').insert([
    { 
      ShiftOrderID: 1, 
      ShiftID: 1, 
      OrderID: 1 
    },
    { 
      ShiftOrderID: 2, 
      ShiftID: 1, 
      OrderID: 2 
    },
    { 
      ShiftOrderID: 3, 
      ShiftID: 1, 
      OrderID: 3 
    },
    { 
      ShiftOrderID: 4, 
      ShiftID: 2, 
      OrderID: 4 
    },
    { 
      ShiftOrderID: 5, 
      ShiftID: 2, 
      OrderID: 5 
    },
    { 
      ShiftOrderID: 6, 
      ShiftID: 3, 
      OrderID: 6 
    },
    { 
      ShiftOrderID: 7, 
      ShiftID: 3, 
      OrderID: 7 
    },
    { 
      ShiftOrderID: 8, 
      ShiftID: 4, 
      OrderID: 8 
    },
    { 
      ShiftOrderID: 9, 
      ShiftID: 5, 
      OrderID: 9 
    },
    { 
      ShiftOrderID: 10, 
      ShiftID: 6, 
      OrderID: 10 
    }
  ]);
};