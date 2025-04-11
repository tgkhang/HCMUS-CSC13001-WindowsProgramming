exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('menu_item').del()
  
  // First get the category IDs
  const categories = await knex('category').select('category_id', 'name');
  const categoryMap = {};
  categories.forEach(category => {
    categoryMap[category.name] = category.category_id;
  });
  
  await knex('menu_item').insert([
    {
      name: 'Espresso', 
      category_id: categoryMap['Beverages'], 
      selling_price: 3.5, 
      image_path: '/images/espresso.jpg'
    },
    {
      name: 'Latte', 
      category_id: categoryMap['Beverages'], 
      selling_price: 4.5, 
      image_path: '/images/latte.jpg'
    },
    {
      name: 'Bruschetta', 
      category_id: categoryMap['Appetizers'], 
      selling_price: 7.0, 
      image_path: '/images/bruschetta.jpg'
    },
    {
      name: 'Grilled Chicken', 
      category_id: categoryMap['Main Courses'], 
      selling_price: 15.0, 
      image_path: '/images/grilled_chicken.jpg'
    },
    {
      name: 'Chocolate Cake', 
      category_id: categoryMap['Desserts'], 
      selling_price: 6.0, 
      image_path: '/images/chocolate_cake.jpg'
    },
    {
      name: 'French Fries', 
      category_id: categoryMap['Sides'], 
      selling_price: 4.0, 
      image_path: '/images/french_fries.jpg'
    },
    {
      name: 'Salmon Steak', 
      category_id: categoryMap['Specials'], 
      selling_price: 22.0, 
      image_path: '/images/salmon_steak.jpg'
    },
    {
      name: 'Breakfast Platter', 
      category_id: categoryMap['Breakfast'], 
      selling_price: 12.0, 
      image_path: '/images/breakfast_platter.jpg'
    }
  ]);
};