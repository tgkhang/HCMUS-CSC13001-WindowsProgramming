/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex.raw('DELETE FROM "order"');
  
  // Get customer and shift IDs
  const customers = await knex('customer').select('customer_id');
  const shifts = await knex('shift').select('shift_id', 'status');
  
  // Only create orders for closed shifts
  const closedShifts = shifts.filter(shift => shift.status === 'closed');
  
  // Create some orders
  const orders = [];
  
  for (const shift of closedShifts) {
    // Create random number of orders for each shift (5-10)
    const numOrders = 5 + Math.floor(Math.random() * 6);
    
    for (let i = 0; i < numOrders; i++) {
      // Randomly select a customer
      const customer = customers[Math.floor(Math.random() * customers.length)];
      
      // Generate random order details
      const totalAmount = 25 + Math.random() * 75;
      const discount = Math.random() < 0.3 ? totalAmount * 0.1 : 0; // 30% chance of 10% discount
      const finalAmount = totalAmount - discount;
      
      orders.push({
        customer_id: customer.customer_id,
        shift_id: shift.shift_id,
        total_amount: parseFloat(totalAmount.toFixed(2)),
        discount: parseFloat(discount.toFixed(2)),
        final_amount: parseFloat(finalAmount.toFixed(2)),
        payment_method: Math.random() < 0.7 ? 'credit_card' : 'cash',
        status: 'completed'
      });
    }
  }
  
  if (orders.length > 0) {
    await knex.raw(`
      INSERT INTO "order" (customer_id, shift_id, total_amount, discount, final_amount, payment_method, status)
      VALUES ${orders.map(order => `(
        ${order.customer_id}, 
        ${order.shift_id}, 
        ${order.total_amount}, 
        ${order.discount}, 
        ${order.final_amount}, 
        '${order.payment_method}', 
        '${order.status}'
      )`).join(', ')}
    `);
  }
};