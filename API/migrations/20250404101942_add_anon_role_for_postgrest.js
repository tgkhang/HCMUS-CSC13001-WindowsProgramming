/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    await knex.raw(`
        create role anon nologin;
        grant usage on schema public to anon;
        grant select on public.category to anon;
        grant select on public.ingredient to anon;
        grant select on public.menu_item to anon;
        grant select on public.customer to anon;
        grant select on public.shift to anon;
        grant select on public."order" to anon;
        grant select on public.order_detail to anon;
        grant select on public.transaction to anon;
        grant select on public.promotion to anon;
        grant select on public.notification to anon;
        grant select on public.shift_order to anon;
        grant select on public.cash_flow to anon;
    `);
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function(knex) {
    await knex.raw(`
        drop role anon;
    `);
};