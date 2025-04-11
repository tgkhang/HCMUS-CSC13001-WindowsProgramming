exports.seed = async function(knex) {
  await knex('menu_item').del();
  
  const categories = await knex('category').select('category_id', 'name');
  const categoryMap = {};
  categories.forEach(category => {
    categoryMap[category.name] = category.category_id;
  });

  const commonImagePath = '/Assets/StoreLogo.png';

  await knex('menu_item').insert([
    { name: 'Espresso', category_id: categoryMap['Hot Coffee'], selling_price: 35000, image_path: commonImagePath },
    { name: 'Americano', category_id: categoryMap['Hot Coffee'], selling_price: 40000, image_path: commonImagePath },
    { name: 'Cappuccino', category_id: categoryMap['Hot Coffee'], selling_price: 47500, image_path: commonImagePath },
    { name: 'Latte', category_id: categoryMap['Hot Coffee'], selling_price: 45000, image_path: commonImagePath },
    { name: 'Mocha', category_id: categoryMap['Hot Coffee'], selling_price: 50000, image_path: commonImagePath },

    { name: 'Iced Coffee', category_id: categoryMap['Cold Coffee'], selling_price: 42500, image_path: commonImagePath },
    { name: 'Cold Brew', category_id: categoryMap['Cold Coffee'], selling_price: 47500, image_path: commonImagePath },
    { name: 'Iced Latte', category_id: categoryMap['Cold Coffee'], selling_price: 50000, image_path: commonImagePath },
    { name: 'Frappuccino', category_id: categoryMap['Cold Coffee'], selling_price: 55000, image_path: commonImagePath },

    { name: 'Caramel Macchiato', category_id: categoryMap['Specialty Drinks'], selling_price: 55000, image_path: commonImagePath },
    { name: 'Vanilla Bean Latte', category_id: categoryMap['Specialty Drinks'], selling_price: 57500, image_path: commonImagePath },
    { name: 'Hazelnut Mocha', category_id: categoryMap['Specialty Drinks'], selling_price: 57500, image_path: commonImagePath },

    { name: 'Green Tea', category_id: categoryMap['Tea'], selling_price: 37500, image_path: commonImagePath },
    { name: 'Chai Latte', category_id: categoryMap['Tea'], selling_price: 45000, image_path: commonImagePath },
    { name: 'Iced Tea', category_id: categoryMap['Tea'], selling_price: 37500, image_path: commonImagePath },

    { name: 'Chocolate Croissant', category_id: categoryMap['Pastries'], selling_price: 37500, image_path: commonImagePath },
    { name: 'Blueberry Muffin', category_id: categoryMap['Pastries'], selling_price: 35000, image_path: commonImagePath },
    { name: 'Cinnamon Roll', category_id: categoryMap['Pastries'], selling_price: 42500, image_path: commonImagePath },

    { name: 'Turkey & Cheese', category_id: categoryMap['Sandwiches'], selling_price: 67500, image_path: commonImagePath },
    { name: 'Veggie Wrap', category_id: categoryMap['Sandwiches'], selling_price: 65000, image_path: commonImagePath },

    { name: 'Avocado Toast', category_id: categoryMap['Breakfast Items'], selling_price: 75000, image_path: commonImagePath },
    { name: 'Breakfast Sandwich', category_id: categoryMap['Breakfast Items'], selling_price: 62500, image_path: commonImagePath }
  ]);
};