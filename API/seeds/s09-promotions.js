// s09-promotion.js
/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Clear existing entries (delete from promotion_details first due to CASCADE)
  await knex('promotion_details').del();
  await knex('promotion').del();

  const today = new Date();

  // Step 1: Insert into promotion table and get promo_ids
  const promotionIds = await knex('promotion')
    .insert([
      {
        promo_name: 'Summer Special',
        start_date: new Date(today.getFullYear(), 5, 1), // June 1st
        end_date: new Date(today.getFullYear(), 7, 31)   // August 31st
      },
      {
        promo_name: 'Happy Hour',
        start_date: new Date(today.getFullYear(), today.getMonth(), 1),
        end_date: new Date(today.getFullYear(), today.getMonth() + 6, 0)
      },
      {
        promo_name: 'Lunch Special',
        start_date: new Date(today.getFullYear(), today.getMonth(), 1),
        end_date: new Date(today.getFullYear(), today.getMonth() + 3, 0)
      },
      {
        promo_name: 'Holiday Promo',
        start_date: new Date(today.getFullYear(), 11, 1),  // December 1st
        end_date: new Date(today.getFullYear(), 11, 31)   // December 31st
      }
    ])
    .returning('promo_id');

  // Step 2: Insert into promotion_details using the returned promo_ids
  await knex('promotion_details').insert([
    {
      promo_id: promotionIds[0].promo_id, // Summer Special
      discount_type: 'Percentage',
      discount_value: 15.0,
      description: '15% off during summer months'
    },
    {
      promo_id: promotionIds[1].promo_id, // Happy Hour
      discount_type: 'Percentage',
      discount_value: 20.0,
      description: '20% off during happy hour periods'
    },
    {
      promo_id: promotionIds[2].promo_id, // Lunch Special
      discount_type: 'FixedAmount',
      discount_value: 5.0,
      description: '$5 off lunch items'
    },
    {
      promo_id: promotionIds[3].promo_id, // Holiday Promo
      discount_type: 'Percentage',
      discount_value: 25.0,
      description: '25% off for holiday season'
    }
  ]);
};