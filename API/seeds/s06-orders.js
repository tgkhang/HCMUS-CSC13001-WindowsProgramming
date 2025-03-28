/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('Orders').del();
  
  // Inserts seed entries
  await knex('Orders').insert([
    { 
      OrderID: 1, 
      CustomerID: 1, 
      ShiftID: 1, 
      TotalAmount: 13.75, 
      Discount: 0.00, 
      FinalAmount: 13.75, 
      PaymentMethod: 'Card', 
      Status: 'Completed' 
    },
    { 
      OrderID: 2, 
      CustomerID: null, 
      ShiftID: 1, 
      TotalAmount: 8.25, 
      Discount: 0.00, 
      FinalAmount: 8.25, 
      PaymentMethod: 'Cash', 
      Status: 'Completed' 
    },
    { 
      OrderID: 3, 
      CustomerID: 2, 
      ShiftID: 1, 
      TotalAmount: 19.50, 
      Discount: 1.95, 
      FinalAmount: 17.55, 
      PaymentMethod: 'Card', 
      Status: 'Completed' 
    },
    { 
      OrderID: 4, 
      CustomerID: null, 
      ShiftID: 2, 
      TotalAmount: 4.75, 
      Discount: 0.00, 
      FinalAmount: 4.75, 
      PaymentMethod: 'Cash', 
      Status: 'Completed' 
    },
    { 
      OrderID: 5, 
      CustomerID: 3, 
      ShiftID: 2, 
      TotalAmount: 12.25, 
      Discount: 0.00, 
      FinalAmount: 12.25, 
      PaymentMethod: 'E-wallet', 
      Status: 'Completed' 
    },
    { 
      OrderID: 6, 
      CustomerID: null, 
      ShiftID: 3, 
      TotalAmount: 16.50, 
      Discount: 0.00, 
      FinalAmount: 16.50, 
      PaymentMethod: 'Card', 
      Status: 'Completed' 
    },
    { 
      OrderID: 7, 
      CustomerID: 4, 
      ShiftID: 3, 
      TotalAmount: 23.75, 
      Discount: 2.38, 
      FinalAmount: 21.37, 
      PaymentMethod: 'Card', 
      Status: 'Completed' 
    },
    { 
      OrderID: 8, 
      CustomerID: null, 
      ShiftID: 4, 
      TotalAmount: 9.25, 
      Discount: 0.00, 
      FinalAmount: 9.25, 
      PaymentMethod: 'Cash', 
      Status: 'Completed' 
    },
    { 
      OrderID: 9, 
      CustomerID: 5, 
      ShiftID: 5, 
      TotalAmount: 14.00, 
      Discount: 0.00, 
      FinalAmount: 14.00, 
      PaymentMethod: 'Card', 
      Status: 'Completed' 
    },
    { 
      OrderID: 10, 
      CustomerID: null, 
      ShiftID: 6, 
      TotalAmount: 11.50, 
      Discount: 0.00, 
      FinalAmount: 11.50, 
      PaymentMethod: 'Cash', 
      Status: 'Completed' 
    }
  ]);
};