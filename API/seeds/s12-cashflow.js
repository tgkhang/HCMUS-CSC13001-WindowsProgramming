/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('cash_flow').del();
  
  // Inserts seed entries
  await knex('cash_flow').insert([
    { 
      cash_flow_id: 1, 
      shift_id: 1, 
      transaction_type: 'Opening Cash', 
      amount: 200.00, 
      timestamp: new Date('2025-03-25 07:00:00') 
    },
    { 
      cash_flow_id: 2, 
      shift_id: 1, 
      transaction_type: 'Cash In', 
      amount: 8.25, 
      timestamp: new Date('2025-03-25 08:45:00') 
    },
    { 
      cash_flow_id: 3, 
      shift_id: 1, 
      transaction_type: 'Cash Out', 
      amount: 25.00, 
      timestamp: new Date('2025-03-25 12:30:00') 
    },
    { 
      cash_flow_id: 4, 
      shift_id: 2, 
      transaction_type: 'Opening Cash', 
      amount: 250.00, 
      timestamp: new Date('2025-03-25 15:00:00') 
    },
    { 
      cash_flow_id: 5, 
      shift_id: 2, 
      transaction_type: 'Cash In', 
      amount: 5.00, 
      timestamp: new Date('2025-03-25 16:20:00') 
    },
    { 
      cash_flow_id: 6, 
      shift_id: 3, 
      transaction_type: 'Opening Cash', 
      amount: 200.00, 
      timestamp: new Date('2025-03-26 07:00:00') 
    },
    { 
      cash_flow_id: 7, 
      shift_id: 4, 
      transaction_type: 'Opening Cash', 
      amount: 250.00, 
      timestamp: new Date('2025-03-26 15:00:00') 
    },
    { 
      cash_flow_id: 8, 
      shift_id: 4, 
      transaction_type: 'Cash In', 
      amount: 10.00, 
      timestamp: new Date('2025-03-26 17:30:00') 
    },
    { 
      cash_flow_id: 9, 
      shift_id: 5, 
      transaction_type: 'Opening Cash', 
      amount: 200.00, 
      timestamp: new Date('2025-03-27 07:00:00') 
    },
    { 
      cash_flow_id: 10, 
      shift_id: 5, 
      transaction_type: 'Cash Out', 
      amount: 50.00, 
      timestamp: new Date('2025-03-27 11:45:00') 
    },
    { 
      cash_flow_id: 11, 
      shift_id: 6, 
      transaction_type: 'Opening Cash', 
      amount: 250.00, 
      timestamp: new Date('2025-03-27 15:00:00') 
    },
    { 
      cash_flow_id: 12, 
      shift_id: 6, 
      transaction_type: 'Cash In', 
      amount: 12.00, 
      timestamp: new Date('2025-03-27 16:15:00') 
    }
  ]);
};