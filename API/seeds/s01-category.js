/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('Categories').del();
  
  // Inserts seed entries
  await knex('Categories').insert([
    { 
      CategoryID: 1, 
      Name: 'Coffee', 
      Description: 'Various coffee beverages' 
    },
    { 
      CategoryID: 2, 
      Name: 'Tea', 
      Description: 'Various tea options' 
    },
    { 
      CategoryID: 3, 
      Name: 'Pastries', 
      Description: 'Fresh baked pastries and treats' 
    },
    { 
      CategoryID: 4, 
      Name: 'Sandwiches', 
      Description: 'Freshly made sandwiches' 
    },
    { 
      CategoryID: 5, 
      Name: 'Milk & Alternatives', 
      Description: 'Dairy milk and non-dairy alternatives' 
    },
    { 
      CategoryID: 6, 
      Name: 'Syrups & Flavorings', 
      Description: 'Sweeteners and flavor enhancers' 
    },
    { 
      CategoryID: 7, 
      Name: 'Seasonal Beverages', 
      Description: 'Limited time special drinks' 
    }
  ]);
};