/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('ingredients').del();
  
  // Inserts seed entries
  await knex('ingredients').insert([
    { 
      ingredient_id: 1, 
      ingredient_name: 'Espresso Beans', 
      category_id: 1, 
      stock: 25.0, 
      unit: 'kg', 
      purchase_price: 15.50, 
      supplier: 'Premium Coffee Co.', 
      expiry_date: new Date('2025-06-30')
    },
    { 
      ingredient_id: 2, 
      ingredient_name: 'Whole Milk', 
      category_id: 5, 
      stock: 50.0, 
      unit: 'liter', 
      purchase_price: 1.20, 
      supplier: 'Local Dairy', 
      expiry_date: new Date('2025-04-15')
    },
    { 
      ingredient_id: 3, 
      ingredient_name: 'Almond Milk', 
      category_id: 5, 
      stock: 30.0, 
      unit: 'liter', 
      purchase_price: 2.50, 
      supplier: 'Plant Beverage Inc.', 
      expiry_date: new Date('2025-05-20')
    },
    { 
      ingredient_id: 4, 
      ingredient_name: 'Vanilla Syrup', 
      category_id: 6, 
      stock: 10.0, 
      unit: 'liter', 
      purchase_price: 8.75, 
      supplier: 'Flavor Systems', 
      expiry_date: new Date('2025-08-15')
    },
    { 
      ingredient_id: 5, 
      ingredient_name: 'Earl Grey Tea', 
      category_id: 2, 
      stock: 5.0, 
      unit: 'kg', 
      purchase_price: 25.00, 
      supplier: 'Luxury Teas', 
      expiry_date: new Date('2025-07-10')
    },
    { 
      ingredient_id: 6, 
      ingredient_name: 'Croissant Pastry', 
      category_id: 3, 
      stock: 40.0, 
      unit: 'piece', 
      purchase_price: 1.25, 
      supplier: 'Bakers Delight', 
      expiry_date: new Date('2025-04-05')
    },
    { 
      ingredient_id: 7, 
      ingredient_name: 'Chocolate Chips', 
      category_id: 6, 
      stock: 8.0, 
      unit: 'kg', 
      purchase_price: 12.00, 
      supplier: 'Sweet Supplies', 
      expiry_date: new Date('2025-09-20')
    },
    { 
      ingredient_id: 8, 
      ingredient_name: 'Sliced Turkey', 
      category_id: 4, 
      stock: 15.0, 
      unit: 'kg', 
      purchase_price: 18.50, 
      supplier: 'Fresh Farms', 
      expiry_date: new Date('2025-04-10')
    },
    { 
      ingredient_id: 9, 
      ingredient_name: 'Cinnamon Powder', 
      category_id: 6, 
      stock: 3.0, 
      unit: 'kg', 
      purchase_price: 15.00, 
      supplier: 'Spice Traders', 
      expiry_date: new Date('2025-10-30')
    },
    { 
      ingredient_id: 10, 
      ingredient_name: 'Oat Milk', 
      category_id: 5, 
      stock: 25.0, 
      unit: 'liter', 
      purchase_price: 2.80, 
      supplier: 'Plant Beverage Inc.', 
      expiry_date: new Date('2025-05-15')
    }
  ]);
};