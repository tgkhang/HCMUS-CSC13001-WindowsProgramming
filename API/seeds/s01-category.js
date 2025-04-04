/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('category').del()
  await knex('category').insert([
    {name: 'Beverages', description: 'Drinks and refreshments'},
    {name: 'Appetizers', description: 'Starters and small plates'},
    {name: 'Main Courses', description: 'Primary entrées and dishes'},
    {name: 'Desserts', description: 'Sweet treats and desserts'},
    {name: 'Sides', description: 'Side dishes and accompaniments'},
    {name: 'Specials', description: 'Chef specials and seasonal items'},
    {name: 'Breakfast', description: 'Morning meal options'},
  ]);
};