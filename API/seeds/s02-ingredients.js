/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('Ingredients').del();
  
  // Inserts seed entries
  await knex('Ingredients').insert([
    { 
      IngredientID: 1, 
      IngredientName: 'Espresso Beans', 
      CategoryID: 1, 
      Stock: 25.0, 
      Unit: 'kg', 
      PurchasePrice: 15.50, 
      Supplier: 'Premium Coffee Co.', 
      ExpiryDate: new Date('2025-06-30')
    },
    { 
      IngredientID: 2, 
      IngredientName: 'Whole Milk', 
      CategoryID: 5, 
      Stock: 50.0, 
      Unit: 'liter', 
      PurchasePrice: 1.20, 
      Supplier: 'Local Dairy', 
      ExpiryDate: new Date('2025-04-15')
    },
    { 
      IngredientID: 3, 
      IngredientName: 'Almond Milk', 
      CategoryID: 5, 
      Stock: 30.0, 
      Unit: 'liter', 
      PurchasePrice: 2.50, 
      Supplier: 'Plant Beverage Inc.', 
      ExpiryDate: new Date('2025-05-20')
    },
    { 
      IngredientID: 4, 
      IngredientName: 'Vanilla Syrup', 
      CategoryID: 6, 
      Stock: 10.0, 
      Unit: 'liter', 
      PurchasePrice: 8.75, 
      Supplier: 'Flavor Systems', 
      ExpiryDate: new Date('2025-08-15')
    },
    { 
      IngredientID: 5, 
      IngredientName: 'Earl Grey Tea', 
      CategoryID: 2, 
      Stock: 5.0, 
      Unit: 'kg', 
      PurchasePrice: 25.00, 
      Supplier: 'Luxury Teas', 
      ExpiryDate: new Date('2025-07-10')
    },
    { 
      IngredientID: 6, 
      IngredientName: 'Croissant Pastry', 
      CategoryID: 3, 
      Stock: 40.0, 
      Unit: 'piece', 
      PurchasePrice: 1.25, 
      Supplier: 'Bakers Delight', 
      ExpiryDate: new Date('2025-04-05')
    },
    { 
      IngredientID: 7, 
      IngredientName: 'Chocolate Chips', 
      CategoryID: 6, 
      Stock: 8.0, 
      Unit: 'kg', 
      PurchasePrice: 12.00, 
      Supplier: 'Sweet Supplies', 
      ExpiryDate: new Date('2025-09-20')
    },
    { 
      IngredientID: 8, 
      IngredientName: 'Sliced Turkey', 
      CategoryID: 4, 
      Stock: 15.0, 
      Unit: 'kg', 
      PurchasePrice: 18.50, 
      Supplier: 'Fresh Farms', 
      ExpiryDate: new Date('2025-04-10')
    },
    { 
      IngredientID: 9, 
      IngredientName: 'Cinnamon Powder', 
      CategoryID: 6, 
      Stock: 3.0, 
      Unit: 'kg', 
      PurchasePrice: 15.00, 
      Supplier: 'Spice Traders', 
      ExpiryDate: new Date('2025-10-30')
    },
    { 
      IngredientID: 10, 
      IngredientName: 'Oat Milk', 
      CategoryID: 5, 
      Stock: 25.0, 
      Unit: 'liter', 
      PurchasePrice: 2.80, 
      Supplier: 'Plant Beverage Inc.', 
      ExpiryDate: new Date('2025-05-15')
    }
  ]);
};