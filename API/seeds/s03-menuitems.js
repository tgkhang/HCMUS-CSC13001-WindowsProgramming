/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('menu_items').del();
  
  // Inserts seed entries
  await knex('menu_items').insert([
    { 
      menu_item_id: 1, 
      name: 'Espresso', 
      category_id: 1, 
      selling_price: 3.50, 
      image_path: '/images/espresso.jpg' 
    },
    { 
      menu_item_id: 2, 
      name: 'Cappuccino', 
      category_id: 1, 
      selling_price: 4.75, 
      image_path: '/images/cappuccino.jpg' 
    },
    { 
      menu_item_id: 3, 
      name: 'Latte', 
      category_id: 1, 
      selling_price: 4.50, 
      image_path: '/images/latte.jpg' 
    },
    { 
      menu_item_id: 4, 
      name: 'Earl Grey Tea', 
      category_id: 2, 
      selling_price: 3.25, 
      image_path: '/images/earlgrey.jpg' 
    },
    { 
      menu_item_id: 5, 
      name: 'Croissant', 
      category_id: 3, 
      selling_price: 3.75, 
      image_path: '/images/croissant.jpg' 
    },
    { 
      menu_item_id: 6, 
      name: 'Turkey & Cheese Sandwich', 
      category_id: 4, 
      selling_price: 7.50, 
      image_path: '/images/turkeysandwich.jpg' 
    },
    { 
      menu_item_id: 7, 
      name: 'Mocha', 
      category_id: 1, 
      selling_price: 5.25, 
      image_path: '/images/mocha.jpg' 
    },
    { 
      menu_item_id: 8, 
      name: 'Vanilla Latte', 
      category_id: 1, 
      selling_price: 5.00, 
      image_path: '/images/vanillalatte.jpg' 
    },
    { 
      menu_item_id: 9, 
      name: 'Pumpkin Spice Latte', 
      category_id: 7, 
      selling_price: 5.75, 
      image_path: '/images/pumpkinlatte.jpg' 
    },
    { 
      menu_item_id: 10, 
      name: 'Iced Coffee', 
      category_id: 1, 
      selling_price: 4.25, 
      image_path: '/images/icedcoffee.jpg' 
    }
  ]);
};