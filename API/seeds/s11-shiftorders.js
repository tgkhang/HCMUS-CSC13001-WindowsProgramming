/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('shift_orders').del();
  
  // Inserts seed entries
  await knex('shift_orders').insert([
    { 
      shift_order_id: 1, 
      shift_id: 1, 
      order_id: 1 
    },
    { 
      shift_order_id: 2, 
      shift_id: 1, 
      order_id: 2 
    },
    { 
      shift_order_id: 3, 
      shift_id: 1, 
      order_id: 3 
    },
    { 
      shift_order_id: 4, 
      shift_id: 2, 
      order_id: 4 
    },
    { 
      shift_order_id: 5, 
      shift_id: 2, 
      order_id: 5 
    },
    { 
      shift_order_id: 6, 
      shift_id: 3, 
      order_id: 6 
    },
    { 
      shift_order_id: 7, 
      shift_id: 3, 
      order_id: 7 
    },
    { 
      shift_order_id: 8, 
      shift_id: 4, 
      order_id: 8 
    },
    { 
      shift_order_id: 9, 
      shift_id: 5, 
      order_id: 9 
    },
    { 
      shift_order_id: 10, 
      shift_id: 6, 
      order_id: 10 
    }
  ]);
};