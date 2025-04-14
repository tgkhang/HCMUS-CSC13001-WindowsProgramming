using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;
using PropertyChanged;

namespace POS_For_Small_Shop.ViewModels.ShiftPage
{
    [AddINotifyPropertyChangedInterface]
    public class PaymentViewModel //: INotifyPropertyChanged
    {
        private IDao _dao;
        private IShiftService _shiftService;

        // Store information
        public string StoreName { get; set; } = "KSHOP";
        public string StoreAddress { get; set; } = "227 Nguyen Van Cu, p4, Q5, TPHCM";
        public string StorePhone { get; set; } = "0123456789";

        // Order information
        public int OrderNumber { get; set; }
        public string OrderDateTime { get; set; }
        public string CustomerName { get; set; }
        public bool HasCustomer => !string.IsNullOrEmpty(CustomerName);

        // Order items
        public ObservableCollection<OrderItemViewModel> OrderItems { get; set; }

        // Order summary
        public float Subtotal { get; set; }
        public float Tax { get; set; }
        public float Discount { get; set; }
        public bool HasDiscount => Discount > 0;
        public float Total { get; set; }

        // Payment result
        public bool IsPaid { get; set; }
        public string PaymentMethod { get; set; }

        //public event PropertyChangedEventHandler? PropertyChanged;
        //protected void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        public PaymentViewModel()
        {
            _dao = Service.GetKeyedSingleton<IDao>();
            _shiftService = Service.GetKeyedSingleton<IShiftService>();
            OrderItems = new ObservableCollection<OrderItemViewModel>();
            OrderDateTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
        }

        public PaymentViewModel(NewOrderViewModel orderViewModel) : this() // call the default constructor
        {
            // Copy data 
            OrderNumber = orderViewModel.OrderNumber;
            if (orderViewModel.SelectedCustomer != null)
            {
                CustomerName = orderViewModel.SelectedCustomer.Name;
            }
            foreach (var item in orderViewModel.OrderItems)
            {
                OrderItems.Add(new OrderItemViewModel
                {
                    MenuItemID = item.MenuItemID,
                    Name = item.Name,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    Total = item.Total
                });
            }

            // Copy order summary
            Subtotal = orderViewModel.Subtotal;
            Tax = orderViewModel.Tax;
            Discount = orderViewModel.Discount;
            Total = orderViewModel.Total;
        }

        // Method to finalize the payment
        public void FinalizePayment(string paymentMethod)
        {
            PaymentMethod = paymentMethod;
            IsPaid = true;
            //OnPropertyChanged(nameof(IsPaid));
            //OnPropertyChanged(nameof(PaymentMethod));
        }

        // Method to process the cash payment
        public bool ProcessCashPayment()
        {
            try
            {
                // Get the current active shift
                Shift currentShift = _shiftService.CurrentShift;

                // Create a new Order object
                var order = new Order
                {
                    OrderID = OrderNumber,
                    CustomerID = !string.IsNullOrEmpty(CustomerName) ? GetCustomerIdByName(CustomerName) : null,
                    ShiftID = currentShift.ShiftID,
                    TotalAmount = Subtotal,
                    Discount = Discount,
                    FinalAmount = Total,
                    PaymentMethod = "Cash",
                    Status = "Completed"
                };

                // Save order to database
                bool success = _dao.Orders.Update(OrderNumber, order);
                if (success)
                {
                    // Create a transaction record
                    var transaction = new Transaction
                    {
                        OrderID = order.OrderID,
                        AmountPaid = Total,
                        PaymentMethod = order.PaymentMethod
                    };

                    _dao.Transactions.Insert(transaction);

                    // Update shift statistics
                    currentShift.TotalSales += Total;
                    currentShift.TotalOrders += 1;
                    _dao.Shifts.Update(currentShift.ShiftID, currentShift);

                    // Update shift service
                    _shiftService.UpdateShift(currentShift);

                    // Set as paid
                    FinalizePayment("Cash");

                    return true;
                }
            }
            catch (Exception)
            {
                // Log error or handle exception
            }

            return false;
        }

        // Method to process the QR code payment
        public bool ProcessQRCodePayment()
        {
            try
            {
                // Get the current active shift
                Shift currentShift = _shiftService.CurrentShift;
                var order = new Order
                {
                    OrderID = OrderNumber,
                    CustomerID = !string.IsNullOrEmpty(CustomerName) ? GetCustomerIdByName(CustomerName) : null,
                    ShiftID = currentShift.ShiftID,
                    TotalAmount = Subtotal,
                    Discount = Discount,
                    FinalAmount = Total,
                    PaymentMethod = "QR Code",
                    Status = "Completed"
                };

                // Save order to database
                bool success = _dao.Orders.Update(OrderNumber, order);
                if (success)
                {
                    var transaction = new Transaction
                    {
                        OrderID = order.OrderID,
                        AmountPaid = Total,
                        PaymentMethod = order.PaymentMethod
                    };

                    _dao.Transactions.Insert(transaction);

                    // Update shift statistics
                    currentShift.TotalSales += Total;
                    currentShift.TotalOrders += 1;
                    _dao.Shifts.Update(currentShift.ShiftID, currentShift);

                    _shiftService.UpdateShift(currentShift);
                    FinalizePayment("QR Code");

                    return true;
                }
            }
            catch (Exception)
            {
                // Log error or handle exception
            }

            return false;
        }
        private int? GetCustomerIdByName(string name)
        {
            try
            {
                var customer = _dao.Customers.GetAll().FirstOrDefault(c => c.Name == name);
                return customer?.CustomerID;
            }
            catch
            {
                return null;
            }
        }
    }
}

