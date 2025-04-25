exports.seed = async function(knex) {
  await knex('menu_item').del();
  
  const categories = await knex('category').select('category_id', 'name');
  const categoryMap = {};
  categories.forEach(category => {
    categoryMap[category.name] = category.category_id;
  });

  const commonImagePath = '/Assets/StoreLogo.png';

  await knex('menu_item').insert([
    { name: 'Espresso', category_id: categoryMap['Hot Coffee'], selling_price: 35000, image_path: '/Assets/MenuItems/espresso.jpg' },
    { name: 'Americano', category_id: categoryMap['Hot Coffee'], selling_price: 40000, image_path: '/Assets/MenuItems/americano.jpg' },
    { name: 'Cappuccino', category_id: categoryMap['Hot Coffee'], selling_price: 47500, image_path: '/Assets/MenuItems/Cappuccino.jpg' },
    { name: 'Latte', category_id: categoryMap['Hot Coffee'], selling_price: 45000, image_path: '/Assets/MenuItems/Latte.jpg' },
    { name: 'Mocha', category_id: categoryMap['Hot Coffee'], selling_price: 50000, image_path: '/Assets/MenuItems/Mocha.jpg' },

    { name: 'Iced Coffee', category_id: categoryMap['Cold Coffee'], selling_price: 42500, image_path: '/Assets/MenuItems/Iced Coffee.jpg' },
    { name: 'Cold Brew', category_id: categoryMap['Cold Coffee'], selling_price: 47500, image_path: '/Assets/MenuItems/Cold Brew.jpg' },
    { name: 'Iced Latte', category_id: categoryMap['Cold Coffee'], selling_price: 50000, image_path: '/Assets/MenuItems/Iced Latte.jpg' },
    { name: 'Frappuccino', category_id: categoryMap['Cold Coffee'], selling_price: 55000, image_path: '/Assets/MenuItems/Frappuccino.jpg' },

    { name: 'Caramel Macchiato', category_id: categoryMap['Specialty Drinks'], selling_price: 55000, image_path: '/Assets/MenuItems/Caramel Macchiato.jpg' },
    { name: 'Vanilla Bean Latte', category_id: categoryMap['Specialty Drinks'], selling_price: 57500, image_path: '/Assets/MenuItems/Vanilla Bean Latte.jpg' },
    { name: 'Hazelnut Mocha', category_id: categoryMap['Specialty Drinks'], selling_price: 57500, image_path: '/Assets/MenuItems/Hazelnut Mocha.jpg' },

    { name: 'Green Tea', category_id: categoryMap['Tea'], selling_price: 37500, image_path: '/Assets/MenuItems/Green Tea.jpg' },
    { name: 'Chai Latte', category_id: categoryMap['Tea'], selling_price: 45000, image_path: '/Assets/MenuItems/Chai Latte.jpg' },
    { name: 'Iced Tea', category_id: categoryMap['Tea'], selling_price: 37500, image_path: '/Assets/MenuItems/Iced Tea.jpg' },

    { name: 'Chocolate Croissant', category_id: categoryMap['Pastries'], selling_price: 37500, image_path: '/Assets/MenuItems/Chocolate Croissant.jpg' },
    { name: 'Blueberry Muffin', category_id: categoryMap['Pastries'], selling_price: 35000, image_path: '/Assets/MenuItems/Blueberry Muffin.jpg' },
    { name: 'Cinnamon Roll', category_id: categoryMap['Pastries'], selling_price: 42500, image_path: '/Assets/MenuItems/Cinnamon Roll.jpg' },

    { name: 'Turkey & Cheese', category_id: categoryMap['Sandwiches'], selling_price: 67500, image_path: '/Assets/MenuItems/Turkey & Cheese.jpg' },
    { name: 'Veggie Wrap', category_id: categoryMap['Sandwiches'], selling_price: 65000, image_path: '/Assets/MenuItems/Veggie Wrap.jpg' },

    { name: 'Avocado Toast', category_id: categoryMap['Breakfast Items'], selling_price: 75000, image_path: '/Assets/MenuItems/Avocado Toast.jpg' },
    { name: 'Breakfast Sandwich', category_id: categoryMap['Breakfast Items'], selling_price: 62500, image_path: '/Assets/MenuItems/Breakfast Sandwich.jpg' }
  ]);
};