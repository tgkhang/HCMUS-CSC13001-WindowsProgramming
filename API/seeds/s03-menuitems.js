/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('MenuItems').del();
  
  // Inserts seed entries
  await knex('MenuItems').insert([
    { 
      MenuItemID: 1, 
      Name: 'Espresso', 
      CategoryID: 1, 
      SellingPrice: 3.50, 
      ImagePath: '/images/espresso.jpg' 
    },
    { 
      MenuItemID: 2, 
      Name: 'Cappuccino', 
      CategoryID: 1, 
      SellingPrice: 4.75, 
      ImagePath: '/images/cappuccino.jpg' 
    },
    { 
      MenuItemID: 3, 
      Name: 'Latte', 
      CategoryID: 1, 
      SellingPrice: 4.50, 
      ImagePath: '/images/latte.jpg' 
    },
    { 
      MenuItemID: 4, 
      Name: 'Earl Grey Tea', 
      CategoryID: 2, 
      SellingPrice: 3.25, 
      ImagePath: '/images/earlgrey.jpg' 
    },
    { 
      MenuItemID: 5, 
      Name: 'Croissant', 
      CategoryID: 3, 
      SellingPrice: 3.75, 
      ImagePath: '/images/croissant.jpg' 
    },
    { 
      MenuItemID: 6, 
      Name: 'Turkey & Cheese Sandwich', 
      CategoryID: 4, 
      SellingPrice: 7.50, 
      ImagePath: '/images/turkeysandwich.jpg' 
    },
    { 
      MenuItemID: 7, 
      Name: 'Mocha', 
      CategoryID: 1, 
      SellingPrice: 5.25, 
      ImagePath: '/images/mocha.jpg' 
    },
    { 
      MenuItemID: 8, 
      Name: 'Vanilla Latte', 
      CategoryID: 1, 
      SellingPrice: 5.00, 
      ImagePath: '/images/vanillalatte.jpg' 
    },
    { 
      MenuItemID: 9, 
      Name: 'Pumpkin Spice Latte', 
      CategoryID: 7, 
      SellingPrice: 5.75, 
      ImagePath: '/images/pumpkinlatte.jpg' 
    },
    { 
      MenuItemID: 10, 
      Name: 'Iced Coffee', 
      CategoryID: 1, 
      SellingPrice: 4.25, 
      ImagePath: '/images/icedcoffee.jpg' 
    }
  ]);
};