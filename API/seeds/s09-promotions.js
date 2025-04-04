/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('promotions').del();
  
  // Inserts seed entries
  await knex('promotions').insert([
    { 
      promo_id: 1, 
      promo_name: 'Spring Special', 
      discount_type: 'Percentage', 
      discount_value: 10.00, 
      start_date: new Date('2025-03-01'), 
      end_date: new Date('2025-04-15') 
    },
    { 
      promo_id: 2, 
      promo_name: 'Loyalty Reward', 
      discount_type: 'Fixed', 
      discount_value: 5.00, 
      start_date: new Date('2025-01-01'), 
      end_date: new Date('2025-12-31') 
    },
    { 
      promo_id: 3, 
      promo_name: 'Happy Hour', 
      discount_type: 'Percentage', 
      discount_value: 15.00, 
      start_date: new Date('2025-03-01'), 
      end_date: new Date('2025-06-30') 
    },
    { 
      promo_id: 4, 
      promo_name: 'Student Discount', 
      discount_type: 'Percentage', 
      discount_value: 8.00, 
      start_date: new Date('2025-01-01'), 
      end_date: new Date('2025-12-31') 
    },
    { 
      promo_id: 5, 
      promo_name: 'Buy One Get One Free', 
      discount_type: 'Special', 
      discount_value: 100.00, 
      start_date: new Date('2025-04-01'), 
      end_date: new Date('2025-04-30') 
    }
  ]);
};