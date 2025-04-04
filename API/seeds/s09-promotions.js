// s09-promotion.js
/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('promotion').del()
  
  const today = new Date();
  
  await knex('promotion').insert([
    {
      promo_name: 'Summer Special', 
      discount_type: 'percentage', 
      discount_value: 15.0, 
      start_date: new Date(today.getFullYear(), 5, 1), // June 1st
      end_date: new Date(today.getFullYear(), 7, 31)  // August 31st
    },
    {
      promo_name: 'Happy Hour', 
      discount_type: 'percentage', 
      discount_value: 20.0, 
      start_date: new Date(today.getFullYear(), today.getMonth(), 1), 
      end_date: new Date(today.getFullYear(), today.getMonth() + 6, 0)
    },
    {
      promo_name: 'Lunch Special', 
      discount_type: 'fixed', 
      discount_value: 5.0, 
      start_date: new Date(today.getFullYear(), today.getMonth(), 1), 
      end_date: new Date(today.getFullYear(), today.getMonth() + 3, 0)
    },
    {
      promo_name: 'Holiday Promo', 
      discount_type: 'percentage', 
      discount_value: 25.0, 
      start_date: new Date(today.getFullYear(), 11, 1),  // December 1st
      end_date: new Date(today.getFullYear(), 11, 31)   // December 31st
    }
  ]);
};