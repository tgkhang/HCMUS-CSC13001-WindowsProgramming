/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('CashFlow').del();
  
  // Inserts seed entries
  await knex('CashFlow').insert([
    { 
      CashFlowID: 1, 
      ShiftID: 1, 
      TransactionType: 'Opening Cash', 
      Amount: 200.00, 
      Timestamp: new Date('2025-03-25 07:00:00') 
    },
    { 
      CashFlowID: 2, 
      ShiftID: 1, 
      TransactionType: 'Cash In', 
      Amount: 8.25, 
      Timestamp: new Date('2025-03-25 08:45:00') 
    },
    { 
      CashFlowID: 3, 
      ShiftID: 1, 
      TransactionType: 'Cash Out', 
      Amount: 25.00, 
      Timestamp: new Date('2025-03-25 12:30:00') 
    },
    { 
      CashFlowID: 4, 
      ShiftID: 2, 
      TransactionType: 'Opening Cash', 
      Amount: 250.00, 
      Timestamp: new Date('2025-03-25 15:00:00') 
    },
    { 
      CashFlowID: 5, 
      ShiftID: 2, 
      TransactionType: 'Cash In', 
      Amount: 5.00, 
      Timestamp: new Date('2025-03-25 16:20:00') 
    },
    { 
      CashFlowID: 6, 
      ShiftID: 3, 
      TransactionType: 'Opening Cash', 
      Amount: 200.00, 
      Timestamp: new Date('2025-03-26 07:00:00') 
    },
    { 
      CashFlowID: 7, 
      ShiftID: 4, 
      TransactionType: 'Opening Cash', 
      Amount: 250.00, 
      Timestamp: new Date('2025-03-26 15:00:00') 
    },
    { 
      CashFlowID: 8, 
      ShiftID: 4, 
      TransactionType: 'Cash In', 
      Amount: 10.00, 
      Timestamp: new Date('2025-03-26 17:30:00') 
    },
    { 
      CashFlowID: 9, 
      ShiftID: 5, 
      TransactionType: 'Opening Cash', 
      Amount: 200.00, 
      Timestamp: new Date('2025-03-27 07:00:00') 
    },
    { 
      CashFlowID: 10, 
      ShiftID: 5, 
      TransactionType: 'Cash Out', 
      Amount: 50.00, 
      Timestamp: new Date('2025-03-27 11:45:00') 
    },
    { 
      CashFlowID: 11, 
      ShiftID: 6, 
      TransactionType: 'Opening Cash', 
      Amount: 250.00, 
      Timestamp: new Date('2025-03-27 15:00:00') 
    },
    { 
      CashFlowID: 12, 
      ShiftID: 6, 
      TransactionType: 'Cash In', 
      Amount: 12.00, 
      Timestamp: new Date('2025-03-27 16:15:00') 
    }
  ]);
};