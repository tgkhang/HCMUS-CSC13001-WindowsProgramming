
// s02-ingredient.js
/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('ingredient').del()
  
  // First get the category IDs
  const categories = await knex('category').select('category_id', 'name');
  const categoryMap = {};
  categories.forEach(category => {
    categoryMap[category.name] = category.category_id;
  });
  
  await knex('ingredient').insert([
    {
      ingredient_name: 'Coffee Beans', 
      category_id: categoryMap['Beverages'], 
      stock: 10.0, 
      unit: 'kg', 
      purchase_price: 15.0, 
      supplier: 'Global Coffee Suppliers', 
      expiry_date: new Date(2025, 6, 15)
    },
    {
      ingredient_name: 'Milk', 
      category_id: categoryMap['Beverages'], 
      stock: 20.0, 
      unit: 'L', 
      purchase_price: 1.5, 
      supplier: 'Local Dairy', 
      expiry_date: new Date(2023, 11, 10)
    },
    {
      ingredient_name: 'Tomatoes', 
      category_id: categoryMap['Appetizers'], 
      stock: 5.0, 
      unit: 'kg', 
      purchase_price: 3.0, 
      supplier: 'Fresh Farms', 
      expiry_date: new Date(2023, 11, 5)
    },
    {
      ingredient_name: 'Chicken Breast', 
      category_id: categoryMap['Main Courses'], 
      stock: 8.0, 
      unit: 'kg', 
      purchase_price: 9.0, 
      supplier: 'Quality Meats', 
      expiry_date: new Date(2023, 11, 3)
    },
    {
      ingredient_name: 'Sugar', 
      category_id: categoryMap['Desserts'], 
      stock: 15.0, 
      unit: 'kg', 
      purchase_price: 2.0, 
      supplier: 'Sweet Supplies', 
      expiry_date: new Date(2024, 5, 20)
    },
    {
      ingredient_name: 'Potatoes', 
      category_id: categoryMap['Sides'], 
      stock: 25.0, 
      unit: 'kg', 
      purchase_price: 1.0, 
      supplier: 'Farm Fresh', 
      expiry_date: new Date(2023, 12, 15)
    },
    {
      ingredient_name: 'Salmon', 
      category_id: categoryMap['Specials'], 
      stock: 4.0, 
      unit: 'kg', 
      purchase_price: 20.0, 
      supplier: 'Ocean Catch', 
      expiry_date: new Date(2023, 11, 2)
    },
    {
      ingredient_name: 'Eggs', 
      category_id: categoryMap['Breakfast'], 
      stock: 60.0, 
      unit: 'pcs', 
      purchase_price: 0.2, 
      supplier: 'Happy Hens', 
      expiry_date: new Date(2023, 11, 20)
    }
  ]);
};