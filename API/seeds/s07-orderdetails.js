// s07-order_detail.js
/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('order_detail').del()
  await knex('order_detail').insert([
    {
      order_id: 1,
      menu_item_id: 1,
      quantity: 2,
      unit_price: 3.5,
      subtotal: 7.0
    },
    {
      order_id: 1,
      menu_item_id: 4,
      quantity: 1,
      unit_price: 15.0,
      subtotal: 15.0
    },
    {
      order_id: 1,
      menu_item_id: 5,
      quantity: 1,
      unit_price: 6.0,
      subtotal: 6.0
    },
    {
      order_id: 1,
      menu_item_id: 6,
      quantity: 2,
      unit_price: 4.0,
      subtotal: 8.0
    },
    {
      order_id: 2,
      menu_item_id: 2,
      quantity: 1,
      unit_price: 4.5,
      subtotal: 4.5
    },
    {
      order_id: 2,
      menu_item_id: 3,
      quantity: 2,
      unit_price: 7.0,
      subtotal: 14.0
    },
    {
      order_id: 2,
      menu_item_id: 7,
      quantity: 1,
      unit_price: 22.0,
      subtotal: 22.0
    },
    {
      order_id: 3,
      menu_item_id: 8,
      quantity: 2,
      unit_price: 12.0,
      subtotal: 24.0
    },
    {
      order_id: 3,
      menu_item_id: 1,
      quantity: 1,
      unit_price: 3.5,
      subtotal: 3.5
    }
  ]);
};
