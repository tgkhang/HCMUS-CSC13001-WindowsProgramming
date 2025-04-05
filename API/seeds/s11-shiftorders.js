// s11-shift_order.js
/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('shift_order').del()
  await knex('shift_order').insert([
    {
      shift_id: 1,
      order_id: 1
    },
    {
      shift_id: 1,
      order_id: 2
    },
    {
      shift_id: 2,
      order_id: 3
    },
    {
      shift_id: 2,
      order_id: 4
    },
    {
      shift_id: 3,
      order_id: 5
    },
    {
      shift_id: 3,
      order_id: 6
    }
  ]);
};
