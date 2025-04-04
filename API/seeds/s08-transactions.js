// s08-transaction.js
/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('transaction').del()
  await knex('transaction').insert([
    {
      order_id: 1,
      amount_paid: 35.50,
      payment_method: 'credit_card'
    },
    {
      order_id: 2,
      amount_paid: 38.48,
      payment_method: 'cash'
    },
    {
      order_id: 3,
      amount_paid: 27.00,
      payment_method: 'credit_card'
    },
    {
      order_id: 4,
      amount_paid: 47.93,
      payment_method: 'credit_card'
    },
    {
      order_id: 5,
      amount_paid: 18.50,
      payment_method: 'cash'
    },
    {
      order_id: 6,
      amount_paid: 40.73,
      payment_method: 'credit_card'
    }
  ]);
};