/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('Transactions').del();
  
  // Inserts seed entries
  await knex('Transactions').insert([
    { 
      TransactionID: 1, 
      OrderID: 1, 
      AmountPaid: 13.75, 
      PaymentMethod: 'Card' 
    },
    { 
      TransactionID: 2, 
      OrderID: 2, 
      AmountPaid: 10.00, 
      PaymentMethod: 'Cash' 
    },
    { 
      TransactionID: 3, 
      OrderID: 3, 
      AmountPaid: 17.55, 
      PaymentMethod: 'Card' 
    },
    { 
      TransactionID: 4, 
      OrderID: 4, 
      AmountPaid: 5.00, 
      PaymentMethod: 'Cash' 
    },
    { 
      TransactionID: 5, 
      OrderID: 5, 
      AmountPaid: 12.25, 
      PaymentMethod: 'E-wallet' 
    },
    { 
      TransactionID: 6, 
      OrderID: 6, 
      AmountPaid: 16.50, 
      PaymentMethod: 'Card' 
    },
    { 
      TransactionID: 7, 
      OrderID: 7, 
      AmountPaid: 21.37, 
      PaymentMethod: 'Card' 
    },
    { 
      TransactionID: 8, 
      OrderID: 8, 
      AmountPaid: 10.00, 
      PaymentMethod: 'Cash' 
    },
    { 
      TransactionID: 9, 
      OrderID: 9, 
      AmountPaid: 14.00, 
      PaymentMethod: 'Card' 
    },
    { 
      TransactionID: 10, 
      OrderID: 10, 
      AmountPaid: 12.00, 
      PaymentMethod: 'Cash' 
    }
  ]);
};