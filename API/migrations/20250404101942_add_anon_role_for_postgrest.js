/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    // First check if role exists to avoid errors
    const roleExists = await knex.raw(`
        SELECT 1 FROM pg_roles WHERE rolname = 'anon'
    `);
    
    if (roleExists.rows.length === 0) {
        await knex.raw(`
            CREATE ROLE anon NOLOGIN;
        `);
    }

    // Grant usage on schema
    await knex.raw(`
        GRANT USAGE ON SCHEMA public TO anon;
    `);
    
    // Grant select permissions on all tables in your schema
    await knex.raw(`
        GRANT SELECT ON categories TO anon;
        GRANT SELECT ON ingredients TO anon;
        GRANT SELECT ON menu_items TO anon;
        GRANT SELECT ON customers TO anon;
        GRANT SELECT ON shifts TO anon;
        GRANT SELECT ON orders TO anon;
        GRANT SELECT ON order_details TO anon;
        GRANT SELECT ON transactions TO anon;
        GRANT SELECT ON promotions TO anon;
        GRANT SELECT ON notifications TO anon;
        GRANT SELECT ON shift_orders TO anon;
        GRANT SELECT ON cash_flow TO anon;
    `);
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function(knex) {
    // Revoke permissions
    await knex.raw(`
        REVOKE SELECT ON categories, ingredients, menu_items, customers, 
        shifts, orders, order_details, transactions, promotions, 
        notifications, shift_orders, cash_flow FROM anon;
        REVOKE USAGE ON SCHEMA public FROM anon;
    `);
    
    // Check if role exists before dropping
    const roleExists = await knex.raw(`
        SELECT 1 FROM pg_roles WHERE rolname = 'anon'
    `);
    
    if (roleExists.rows.length > 0) {
        await knex.raw(`
            DROP ROLE anon;
        `);
    }
};