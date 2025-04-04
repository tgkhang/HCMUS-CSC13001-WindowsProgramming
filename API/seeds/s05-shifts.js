/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('shifts').del();
  
  // Inserts seed entries
  await knex('shifts').insert([
    { 
      shift_id: 1, 
      start_time: new Date('2025-03-25 07:00:00'), 
      end_time: new Date('2025-03-25 15:00:00'), 
      opening_cash: 200.00, 
      total_sales: 1250.75, 
      total_orders: 42, 
      status: 'Closed' 
    },
    { 
      shift_id: 2, 
      start_time: new Date('2025-03-25 15:00:00'), 
      end_time: new Date('2025-03-25 23:00:00'), 
      opening_cash: 250.00, 
      total_sales: 1045.50, 
      total_orders: 35, 
      status: 'Closed' 
    },
    { 
      shift_id: 3, 
      start_time: new Date('2025-03-26 07:00:00'), 
      end_time: new Date('2025-03-26 15:00:00'), 
      opening_cash: 200.00, 
      total_sales: 1378.25, 
      total_orders: 45, 
      status: 'Closed' 
    },
    { 
      shift_id: 4, 
      start_time: new Date('2025-03-26 15:00:00'), 
      end_time: new Date('2025-03-26 23:00:00'), 
      opening_cash: 250.00, 
      total_sales: 1122.75, 
      total_orders: 38, 
      status: 'Closed' 
    },
    { 
      shift_id: 5, 
      start_time: new Date('2025-03-27 07:00:00'), 
      end_time: new Date('2025-03-27 15:00:00'), 
      opening_cash: 200.00, 
      total_sales: 1195.50, 
      total_orders: 40, 
      status: 'Closed' 
    },
    { 
      shift_id: 6, 
      start_time: new Date('2025-03-27 15:00:00'), 
      end_time: null, 
      opening_cash: 250.00, 
      total_sales: 520.25, 
      total_orders: 18, 
      status: 'Open' 
    }
  ]);
};