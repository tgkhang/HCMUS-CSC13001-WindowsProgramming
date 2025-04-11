/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.seed = async function (knex) {
  // Deletes ALL existing entries
  await knex("category").del();
  await knex("category").insert([
    { name: "Hot Coffee", description: "Freshly brewed hot coffee options" },
    { name: "Cold Coffee", description: "Iced and cold brew coffee drinks" },
    {
      name: "Specialty Drinks",
      description: "Signature coffee shop creations",
    },
    { name: "Tea", description: "Hot and cold tea selections" },
    { name: "Pastries", description: "Fresh-baked goods and treats" },
    { name: "Sandwiches", description: "Light meals and sandwiches" },
    { name: "Breakfast Items", description: "Morning food options" },
  ]);
};
