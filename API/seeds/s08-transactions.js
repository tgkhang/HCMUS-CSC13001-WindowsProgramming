/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('transactions').del();
  
  // Inserts seed entries
  await knex('transactions').insert([
    { 
      transaction_id: 1, 
      order_id: 1, 
      amount_paid: 13.75, 
      payment_method: 'Card' 
    },
    { 
      transaction_id: 2, 
      order_id: 2, 
      amount_paid: 10.00, 
      payment_method: 'Cash' 
    },
    { 
      transaction_id: 3, 
      order_id: 3, 
      amount_paid: 17.55, 
      payment_method: 'Card' 
    },
    { 
      transaction_id: 4, 
      order_id: 4, 
      amount_paid: 5.00, 
      payment_method: 'Cash' 
    },
    { 
      transaction_id: 5, 
      order_id: 5, 
      amount_paid: 12.25, 
      payment_method: 'E-wallet' 
    },
    { 
      transaction_id: 6, 
      order_id: 6, 
      amount_paid: 16.50, 
      payment_method: 'Card' 
    },
    { 
      transaction_id: 7, 
      order_id: 7, 
      amount_paid: 21.37, 
      payment_method: 'Card' 
    },
    { 
      transaction_id: 8, 
      order_id: 8, 
      amount_paid: 10.00, 
      payment_method: 'Cash' 
    },
    { 
      transaction_id: 9, 
      order_id: 9, 
      amount_paid: 14.00, 
      payment_method: 'Card' 
    },
    { 
      transaction_id: 10, 
      order_id: 10, 
      amount_paid: 12.00, 
      payment_method: 'Cash' 
    }
  ]);
};