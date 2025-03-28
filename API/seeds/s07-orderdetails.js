/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('OrdersDetails').del();
  
  // Inserts seed entries
  await knex('OrdersDetails').insert([
    { 
      OrderDetailID: 1, 
      OrderID: 1, 
      MenuItemID: 3, 
      Quantity: 2, 
      UnitPrice: 4.50, 
      Subtotal: 9.00 
    },
    { 
      OrderDetailID: 2, 
      OrderID: 1, 
      MenuItemID: 5, 
      Quantity: 1, 
      UnitPrice: 3.75, 
      Subtotal: 3.75 
    },
    { 
      OrderDetailID: 3, 
      OrderID: 1, 
      MenuItemID: 4, 
      Quantity: 1, 
      UnitPrice: 3.25, 
      Subtotal: 3.25 
    },
    { 
      OrderDetailID: 4, 
      OrderID: 2, 
      MenuItemID: 1, 
      Quantity: 1, 
      UnitPrice: 3.50, 
      Subtotal: 3.50 
    },
    { 
      OrderDetailID: 5, 
      OrderID: 2, 
      MenuItemID: 5, 
      Quantity: 1, 
      UnitPrice: 3.75, 
      Subtotal: 3.75 
    },
    { 
      OrderDetailID: 6, 
      OrderID: 3, 
      MenuItemID: 6, 
      Quantity: 2, 
      UnitPrice: 7.50, 
      Subtotal: 15.00 
    },
    { 
      OrderDetailID: 7, 
      OrderID: 3, 
      MenuItemID: 10, 
      Quantity: 1, 
      UnitPrice: 4.25, 
      Subtotal: 4.25 
    },
    { 
      OrderDetailID: 8, 
      OrderID: 4, 
      MenuItemID: 2, 
      Quantity: 1, 
      UnitPrice: 4.75, 
      Subtotal: 4.75 
    },
    { 
      OrderDetailID: 9, 
      OrderID: 5, 
      MenuItemID: 8, 
      Quantity: 1, 
      UnitPrice: 5.00, 
      Subtotal: 5.00 
    },
    { 
      OrderDetailID: 10, 
      OrderID: 5, 
      MenuItemID: 9, 
      Quantity: 1, 
      UnitPrice: 5.75, 
      Subtotal: 5.75 
    },
    { 
      OrderDetailID: 11, 
      OrderID: 5, 
      MenuItemID: 5, 
      Quantity: 1, 
      UnitPrice: 3.75, 
      Subtotal: 3.75 
    },
    { 
      OrderDetailID: 12, 
      OrderID: 6, 
      MenuItemID: 7, 
      Quantity: 2, 
      UnitPrice: 5.25, 
      Subtotal: 10.50 
    },
    { 
      OrderDetailID: 13, 
      OrderID: 6, 
      MenuItemID: 6, 
      Quantity: 1, 
      UnitPrice: 7.50, 
      Subtotal: 7.50 
    },
    { 
      OrderDetailID: 14, 
      OrderID: 7, 
      MenuItemID: 3, 
      Quantity: 2, 
      UnitPrice: 4.50, 
      Subtotal: 9.00 
    },
    { 
      OrderDetailID: 15, 
      OrderID: 7, 
      MenuItemID: 6, 
      Quantity: 1, 
      UnitPrice: 7.50, 
      Subtotal: 7.50 
    },
    { 
      OrderDetailID: 16, 
      OrderID: 7, 
      MenuItemID: 9, 
      Quantity: 1, 
      UnitPrice: 5.75, 
      Subtotal: 5.75 
    },
    { 
      OrderDetailID: 17, 
      OrderID: 8, 
      MenuItemID: 10, 
      Quantity: 1, 
      UnitPrice: 4.25, 
      Subtotal: 4.25 
    },
    { 
      OrderDetailID: 18, 
      OrderID: 8, 
      MenuItemID: 5, 
      Quantity: 1, 
      UnitPrice: 3.75, 
      Subtotal: 3.75 
    },
    { 
      OrderDetailID: 19, 
      OrderID: 9, 
      MenuItemID: 3, 
      Quantity: 1, 
      UnitPrice: 4.50, 
      Subtotal: 4.50 
    },
    { 
      OrderDetailID: 20, 
      OrderID: 9, 
      MenuItemID: 8, 
      Quantity: 1, 
      UnitPrice: 5.00, 
      Subtotal: 5.00 
    },
    { 
      OrderDetailID: 21, 
      OrderID: 9, 
      MenuItemID: 4, 
      Quantity: 1, 
      UnitPrice: 3.25, 
      Subtotal: 3.25 
    },
    { 
      OrderDetailID: 22, 
      OrderID: 10, 
      MenuItemID: 7, 
      Quantity: 1, 
      UnitPrice: 5.25, 
      Subtotal: 5.25 
    },
    { 
      OrderDetailID: 23, 
      OrderID: 10, 
      MenuItemID: 5, 
      Quantity: 2, 
      UnitPrice: 3.75, 
      Subtotal: 7.50 
    }
  ]);
};