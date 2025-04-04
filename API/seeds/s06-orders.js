/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('orders').del();
  
  // Inserts seed entries
  await knex('orders').insert([
    { 
      order_id: 1, 
      customer_id: 1, 
      shift_id: 1, 
      total_amount: 13.75, 
      discount: 0.00, 
      final_amount: 13.75, 
      payment_method: 'Card', 
      status: 'Completed' 
    },
    { 
      order_id: 2, 
      customer_id: null, 
      shift_id: 1, 
      total_amount: 8.25, 
      discount: 0.00, 
      final_amount: 8.25, 
      payment_method: 'Cash', 
      status: 'Completed' 
    },
    { 
      order_id: 3, 
      customer_id: 2, 
      shift_id: 1, 
      total_amount: 19.50, 
      discount: 1.95, 
      final_amount: 17.55, 
      payment_method: 'Card', 
      status: 'Completed' 
    },
    { 
      order_id: 4, 
      customer_id: null, 
      shift_id: 2, 
      total_amount: 4.75, 
      discount: 0.00, 
      final_amount: 4.75, 
      payment_method: 'Cash', 
      status: 'Completed' 
    },
    { 
      order_id: 5, 
      customer_id: 3, 
      shift_id: 2, 
      total_amount: 12.25, 
      discount: 0.00, 
      final_amount: 12.25, 
      payment_method: 'E-wallet', 
      status: 'Completed' 
    },
    { 
      order_id: 6, 
      customer_id: null, 
      shift_id: 3, 
      total_amount: 16.50, 
      discount: 0.00, 
      final_amount: 16.50, 
      payment_method: 'Card', 
      status: 'Completed' 
    },
    { 
      order_id: 7, 
      customer_id: 4, 
      shift_id: 3, 
      total_amount: 23.75, 
      discount: 2.38, 
      final_amount: 21.37, 
      payment_method: 'Card', 
      status: 'Completed' 
    },
    { 
      order_id: 8, 
      customer_id: null, 
      shift_id: 4, 
      total_amount: 9.25, 
      discount: 0.00, 
      final_amount: 9.25, 
      payment_method: 'Cash', 
      status: 'Completed' 
    },
    { 
      order_id: 9, 
      customer_id: 5, 
      shift_id: 5, 
      total_amount: 14.00, 
      discount: 0.00, 
      final_amount: 14.00, 
      payment_method: 'Card', 
      status: 'Completed' 
    },
    { 
      order_id: 10, 
      customer_id: null, 
      shift_id: 6, 
      total_amount: 11.50, 
      discount: 0.00, 
      final_amount: 11.50, 
      payment_method: 'Cash', 
      status: 'Completed' 
    }
  ]);
};