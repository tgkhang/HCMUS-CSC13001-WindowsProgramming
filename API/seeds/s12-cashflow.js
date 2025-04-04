// s12-cash_flow.js
/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('cash_flow').del()
  await knex('cash_flow').insert([
    {
      shift_id: 1,
      transaction_type: 'opening_balance',
      amount: 500.0,
      timestamp: new Date(2025, 3, 27, 8, 0, 0)
    },
    {
      shift_id: 1,
      transaction_type: 'credit_card_payment',
      amount: 35.50,
      timestamp: new Date(2025, 3, 27, 9, 15, 0)
    },
    {
      shift_id: 1,
      transaction_type: 'cash_payment',
      amount: 38.48,
      timestamp: new Date(2025, 3, 27, 11, 30, 0)
    },
    {
      shift_id: 1,
      transaction_type: 'supplies',
      amount: -25.75,
      timestamp: new Date(2025, 3, 27, 14, 45, 0)
    },
    {
      shift_id: 2,
      transaction_type: 'opening_balance',
      amount: 500.0,
      timestamp: new Date(2025, 3, 28, 8, 0, 0)
    },
    {
      shift_id: 2,
      transaction_type: 'credit_card_payment',
      amount: 27.00,
      timestamp: new Date(2025, 3, 28, 10, 20, 0)
    },
    {
      shift_id: 2,
      transaction_type: 'credit_card_payment',
      amount: 47.93,
      timestamp: new Date(2025, 3, 28, 12, 15, 0)
    },
    {
      shift_id: 2,
      transaction_type: 'refund',
      amount: -15.00,
      timestamp: new Date(2025, 3, 28, 15, 30, 0)
    },
    {
      shift_id: 3,
      transaction_type: 'opening_balance',
      amount: 500.0,
      timestamp: new Date(2025, 3, 29, 8, 0, 0)
    },
    {
      shift_id: 3,
      transaction_type: 'cash_payment',
      amount: 18.50,
      timestamp: new Date(2025, 3, 29, 9, 45, 0)
    },
    {
      shift_id: 3,
      transaction_type: 'credit_card_payment',
      amount: 40.73,
      timestamp: new Date(2025, 3, 29, 13, 10, 0)
    },
    {
      shift_id: 3,
      transaction_type: 'petty_cash',
      amount: -12.50,
      timestamp: new Date(2025, 3, 29, 15, 0, 0)
    }
  ]);
};