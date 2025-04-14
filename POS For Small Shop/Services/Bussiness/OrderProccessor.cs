using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.ViewModels.ShiftPage;

namespace POS_For_Small_Shop.Services.Business
{
    /// <summary>
    /// Handles business logic for calculating totals, discounts, and tax for an order.
    /// Applies item-specific promotions if provided.
    /// </summary>
    public class OrderProcessor
    {
        private const float TaxRate = 0.08f; // 8% sales tax

        /// <summary>
        /// Represents a breakdown of the financial summary of an order.
        /// </summary>
        public class OrderSummary
        {
            public float Subtotal { get; set; }  // Total before discounts/tax
            public float Tax { get; set; }       // Calculated tax on post-discount amount
            public float Discount { get; set; }  // Total discount applied from promotion
            public float Total { get; set; }     // Final total to charge
        }

        /// <summary>
        /// Calculates subtotal, discount, tax, and total for a list of order items.
        /// Applies a promotion if it targets any items in the order.
        /// </summary>
        /// <param name="items">List of order items in the cart</param>
        /// <param name="promotion">Optional promotion to apply</param>
        /// <returns>Calculated OrderSummary with detailed breakdown</returns>
        public OrderSummary CalculateTotal(List<OrderItemViewModel> items, Promotion? promotion = null)
        {
            // Calculate full subtotal (no discounts yet)
            float subtotal = items.Sum(item => item.UnitPrice * item.Quantity);
            float discount = 0;

            // If promotion exists and has valid details, apply it only to matching items
            if (promotion != null && promotion.Details != null)
            {
                // Find items eligible for the promotion
                var applicableItems = items.Where(i => promotion.ItemIDs.Contains(i.MenuItemID)).ToList();
                float applicableSubtotal = applicableItems.Sum(i => i.UnitPrice * i.Quantity);

                // Calculate discount amount based on type
                discount = promotion.Details.DiscountType switch
                {
                    DiscountType.Percentage => applicableSubtotal * (promotion.Details.DiscountValue / 100f),
                    DiscountType.FixedAmount => applicableItems.Sum(item => promotion.Details.DiscountValue * item.Quantity),
                    _ => 0f
                };

                // Prevent over-discounting (e.g., fixed amount > eligible subtotal)
                discount = Math.Min(discount, applicableSubtotal);
            }

            // Apply tax on the discounted subtotal
            float tax = (subtotal - discount) * TaxRate;

            // Final total to charge
            float total = subtotal + tax - discount;

            return new OrderSummary
            {
                Subtotal = subtotal,
                Tax = tax,
                Discount = discount,
                Total = total
            };
        }
    }
}

