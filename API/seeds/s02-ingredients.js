/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('ingredient').del();
  
  // First get the category IDs
  const categories = await knex('category').select('category_id', 'name');
  const categoryMap = {};
  categories.forEach(category => {
    categoryMap[category.name] = category.category_id;
  });

  await knex('ingredient').insert([
    {
      ingredient_name: 'Coffee Beans', 
      category_id: categoryMap['Hot Coffee'], 
      stock: 10.0, 
      unit: 'kg', 
      purchase_price: 15.0, 
      supplier: 'Global Coffee Suppliers', 
      expiry_date: new Date(2025, 6, 15)
    },
    {
      ingredient_name: 'Milk', 
      category_id: categoryMap['Cold Coffee'], 
      stock: 20.0, 
      unit: 'L', 
      purchase_price: 1.5, 
      supplier: 'Local Dairy', 
      expiry_date: new Date(2023, 11, 10)
    },
    {
      ingredient_name: 'Croissant', 
      category_id: categoryMap['Pastries'], 
      stock: 30.0, 
      unit: 'pcs', 
      purchase_price: 0.8, 
      supplier: 'Bakery Delight', 
      expiry_date: new Date(2023, 11, 5)
    },
    {
      ingredient_name: 'Lettuce', 
      category_id: categoryMap['Sandwiches'], 
      stock: 5.0, 
      unit: 'kg', 
      purchase_price: 2.5, 
      supplier: 'Fresh Greens', 
      expiry_date: new Date(2023, 11, 3)
    },
    {
      ingredient_name: 'Black Tea Leaves', 
      category_id: categoryMap['Tea'], 
      stock: 6.0, 
      unit: 'kg', 
      purchase_price: 4.0, 
      supplier: 'Eastern Teas', 
      expiry_date: new Date(2024, 5, 20)
    },
    {
      ingredient_name: 'Cheese Slices', 
      category_id: categoryMap['Sandwiches'], 
      stock: 50.0, 
      unit: 'pcs', 
      purchase_price: 0.4, 
      supplier: 'Dairy Best', 
      expiry_date: new Date(2023, 12, 15)
    },
    {
      ingredient_name: 'Espresso Syrup', 
      category_id: categoryMap['Specialty Drinks'], 
      stock: 2.0, 
      unit: 'L', 
      purchase_price: 10.0, 
      supplier: 'Flavor Co.', 
      expiry_date: new Date(2024, 2, 1)
    },
    {
      ingredient_name: 'Eggs', 
      category_id: categoryMap['Breakfast Items'], 
      stock: 60.0, 
      unit: 'pcs', 
      purchase_price: 0.2, 
      supplier: 'Happy Hens', 
      expiry_date: new Date(2023, 11, 20)
    }
  ]);
};
