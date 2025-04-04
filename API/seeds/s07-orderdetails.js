/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('order_details').del();
  
  // Inserts seed entries
  await knex('order_details').insert([
    { 
      order_detail_id: 1, 
      order_id: 1, 
      menu_item_id: 3, 
      quantity: 2, 
      unit_price: 4.50, 
      subtotal: 9.00 
    },
    { 
      order_detail_id: 2, 
      order_id: 1, 
      menu_item_id: 5, 
      quantity: 1, 
      unit_price: 3.75, 
      subtotal: 3.75 
    },
    { 
      order_detail_id: 3, 
      order_id: 1, 
      menu_item_id: 4, 
      quantity: 1, 
      unit_price: 3.25, 
      subtotal: 3.25 
    },
    { 
      order_detail_id: 4, 
      order_id: 2, 
      menu_item_id: 1, 
      quantity: 1, 
      unit_price: 3.50, 
      subtotal: 3.50 
    },
    { 
      order_detail_id: 5, 
      order_id: 2, 
      menu_item_id: 5, 
      quantity: 1, 
      unit_price: 3.75, 
      subtotal: 3.75 
    },
    { 
      order_detail_id: 6, 
      order_id: 3, 
      menu_item_id: 6, 
      quantity: 2, 
      unit_price: 7.50, 
      subtotal: 15.00 
    },
    { 
      order_detail_id: 7, 
      order_id: 3, 
      menu_item_id: 10, 
      quantity: 1, 
      unit_price: 4.25, 
      subtotal: 4.25 
    },
    { 
      order_detail_id: 8, 
      order_id: 4, 
      menu_item_id: 2, 
      quantity: 1, 
      unit_price: 4.75, 
      subtotal: 4.75 
    },
    { 
      order_detail_id: 9, 
      order_id: 5, 
      menu_item_id: 8, 
      quantity: 1, 
      unit_price: 5.00, 
      subtotal: 5.00 
    },
    { 
      order_detail_id: 10, 
      order_id: 5, 
      menu_item_id: 9, 
      quantity: 1, 
      unit_price: 5.75, 
      subtotal: 5.75 
    },
    { 
      order_detail_id: 11, 
      order_id: 5, 
      menu_item_id: 5, 
      quantity: 1, 
      unit_price: 3.75, 
      subtotal: 3.75 
    },
    { 
      order_detail_id: 12, 
      order_id: 6, 
      menu_item_id: 7, 
      quantity: 2, 
      unit_price: 5.25, 
      subtotal: 10.50 
    },
    { 
      order_detail_id: 13, 
      order_id: 6, 
      menu_item_id: 6, 
      quantity: 1, 
      unit_price: 7.50, 
      subtotal: 7.50 
    },
    { 
      order_detail_id: 14, 
      order_id: 7, 
      menu_item_id: 3, 
      quantity: 2, 
      unit_price: 4.50, 
      subtotal: 9.00 
    },
    { 
      order_detail_id: 15, 
      order_id: 7, 
      menu_item_id: 6, 
      quantity: 1, 
      unit_price: 7.50, 
      subtotal: 7.50 
    },
    { 
      order_detail_id: 16, 
      order_id: 7, 
      menu_item_id: 9, 
      quantity: 1, 
      unit_price: 5.75, 
      subtotal: 5.75 
    },
    { 
      order_detail_id: 17, 
      order_id: 8, 
      menu_item_id: 10, 
      quantity: 1, 
      unit_price: 4.25, 
      subtotal: 4.25 
    },
    { 
      order_detail_id: 18, 
      order_id: 8, 
      menu_item_id: 5, 
      quantity: 1, 
      unit_price: 3.75, 
      subtotal: 3.75 
    },
    { 
      order_detail_id: 19, 
      order_id: 9, 
      menu_item_id: 3, 
      quantity: 1, 
      unit_price: 4.50, 
      subtotal: 4.50 
    },
    { 
      order_detail_id: 20, 
      order_id: 9, 
      menu_item_id: 8, 
      quantity: 1, 
      unit_price: 5.00, 
      subtotal: 5.00 
    },
    { 
      order_detail_id: 21, 
      order_id: 9, 
      menu_item_id: 4, 
      quantity: 1, 
      unit_price: 3.25, 
      subtotal: 3.25 
    },
    { 
      order_detail_id: 22, 
      order_id: 10, 
      menu_item_id: 7, 
      quantity: 1, 
      unit_price: 5.25, 
      subtotal: 5.25 
    },
    { 
      order_detail_id: 23, 
      order_id: 10, 
      menu_item_id: 5, 
      quantity: 2, 
      unit_price: 3.75, 
      subtotal: 7.50 
    }
  ]);
};