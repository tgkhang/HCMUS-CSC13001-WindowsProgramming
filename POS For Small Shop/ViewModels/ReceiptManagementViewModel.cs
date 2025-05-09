﻿using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using POS_For_Small_Shop.Services;
using POS_For_Small_Shop.Data.Models;
using System.Windows.Input;
using System.Diagnostics;

namespace POS_For_Small_Shop.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class OrderDetailWithMenuItem
    {
        public OrderDetail OrderDetail { get; set; }
        public MenuItem MenuItem { get; set; }
    }


    [AddINotifyPropertyChangedInterface]
    public class ReceiptManagementViewModel : INotifyPropertyChanged
    {
        

        public event PropertyChangedEventHandler? PropertyChanged;
        private IDao _dao = Service.GetKeyedSingleton<IDao>();

        // Properties
        public ObservableCollection<Shift> FilteredShifts { get; set; } // Storage all Shift match the filter
        public ObservableCollection<Order> OrdersForShift { get; set; } // Storage the oder list of the selected shift
        public ObservableCollection<OrderDetail> DetailsOfSelectedOrder { get; set; } // Storage the order detail of the selected order
        public Order SelectedOrder { get; set; } // Storage the selected order
        public Customer? CustomerForSelectedOrder { get; set; } // Storage the customer of the selected order
        public ObservableCollection<OrderDetailWithMenuItem> OrderDetailsWithMenuItems { get; set; } // Storage the order detail with menu item of the selected order

        public string OrderStatus { get; set; } = "all"; // Storage the order status of the selected order

        // Command
        public ICommand GetOrderByShiftIDCommand { get; set; } // Command to get the order by shift ID
        public ICommand GetOrderByOrderIDCommand { get; set; } // Command to get the order by order ID
        public ICommand GetAllShiftsCommand { get; set; } // Command to get all shifts

        public ReceiptManagementViewModel()
        {
            GetAllShifts(); // Initialize the FilteredShifts with all shifts

            // Initialize the command
            GetOrderByShiftIDCommand = new RelayCommand<int>(GetOrderByShiftID);
            GetOrderByOrderIDCommand = new RelayCommand<int>(GetOrderByOrderID);
            GetAllShiftsCommand = new RelayCommand(_ => GetAllShifts());

        }

        /// <summary>
        /// Get all shifts and store them in FilteredShifts
        /// </summary>
        public void GetAllShifts()
        {
            FilteredShifts = new ObservableCollection<Shift>(_dao.Shifts.GetAll());
        }

        /// <summary>
        /// Get the order by shift ID and store it in OrdersForShift
        /// </summary>
        /// <param name="shiftID"></param>
        public void GetOrderByShiftID(int shiftID)
        {
            OrdersForShift = new ObservableCollection<Order>(_dao.Orders.getOrdersByShiftID(shiftID));

            // Filter the orders by status
            //Debug.WriteLine($"OrderStatus: {OrderStatus}");
            if (OrderStatus.ToLower() != "all")
            {
                OrdersForShift = new ObservableCollection<Order>(OrdersForShift.Where(order => order.Status.Equals(OrderStatus, StringComparison.OrdinalIgnoreCase)));
            }
        }

        /// <summary>
        /// Get the order detail by order ID and store it in SelectedOrder
        /// </summary>
        /// <param name="orderID"></param>
        public void GetOrderByOrderID(int orderID)
        {
            SelectedOrder = _dao.Orders.GetById(orderID);
            // Check if CustomerID is not null before calling GetById
            if (SelectedOrder.CustomerID.HasValue)
            {
                CustomerForSelectedOrder = _dao.Customers.GetById(SelectedOrder.CustomerID.Value);
            }
            else
            {
                CustomerForSelectedOrder = null; // Handle the case where CustomerID is null
            }

            // Get order details by order ID
            GetOrderDetailsByOrderID(orderID);
        }

        /// <summary>
        /// Get the order details by order ID and store it in DetailsOfSelectedOrder
        /// </summary>
        /// <param name="orderID"></param>
        public void GetOrderDetailsByOrderID(int orderID)
        {
            try
            {
                DetailsOfSelectedOrder = new ObservableCollection<OrderDetail>(
                _dao.OrderDetails.GetAll().Where(detail => detail.OrderID == orderID)
                );

                // Check if DetailsOfSelectedOrder is not null before proceeding
                OrderDetailsWithMenuItems ??= new ObservableCollection<OrderDetailWithMenuItem>();
                // Clear the existing items in OrderDetailsWithMenuItems    
                OrderDetailsWithMenuItems.Clear();

                // Populate OrderDetailsWithMenuItems
                foreach (var detail in DetailsOfSelectedOrder)
                {
                    var menuItem = _dao.MenuItems.GetById(detail.MenuItemID);
                    if (menuItem != null)
                    {
                        OrderDetailsWithMenuItems.Add(new OrderDetailWithMenuItem
                        {
                            OrderDetail = detail,
                            MenuItem = menuItem
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void GetShiftsWithFilters(DateTime? startDate, DateTime? endDate, string? status)
        {
            startDate = startDate ?? DateTime.MinValue;
            endDate = endDate ?? DateTime.MaxValue;
            status = status ?? "";

            FilteredShifts = new ObservableCollection<Shift>(
                _dao.Shifts.GetAll()
                .Where(shift => shift.StartTime >= startDate && shift.EndTime <= endDate)
                .Where(shift => string.IsNullOrEmpty(status) || status.Equals("all", StringComparison.OrdinalIgnoreCase) || shift.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
            );

        }

    }
}
