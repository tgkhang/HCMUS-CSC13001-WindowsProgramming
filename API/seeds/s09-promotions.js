/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('Promotions').del();
  
  // Inserts seed entries
  await knex('Promotions').insert([
    { 
      PromoID: 1, 
      PromoName: 'Spring Special', 
      DiscountType: 'Percentage', 
      DiscountValue: 10.00, 
      StartDate: new Date('2025-03-01'), 
      EndDate: new Date('2025-04-15') 
    },
    { 
      PromoID: 2, 
      PromoName: 'Loyalty Reward', 
      DiscountType: 'Fixed', 
      DiscountValue: 5.00, 
      StartDate: new Date('2025-01-01'), 
      EndDate: new Date('2025-12-31') 
    },
    { 
      PromoID: 3, 
      PromoName: 'Happy Hour', 
      DiscountType: 'Percentage', 
      DiscountValue: 15.00, 
      StartDate: new Date('2025-03-01'), 
      EndDate: new Date('2025-06-30') 
    },
    { 
      PromoID: 4, 
      PromoName: 'Student Discount', 
      DiscountType: 'Percentage', 
      DiscountValue: 8.00, 
      StartDate: new Date('2025-01-01'), 
      EndDate: new Date('2025-12-31') 
    },
    { 
      PromoID: 5, 
      PromoName: 'Buy One Get One Free', 
      DiscountType: 'Special', 
      DiscountValue: 100.00, 
      StartDate: new Date('2025-04-01'), 
      EndDate: new Date('2025-04-30') 
    }
  ]);
};