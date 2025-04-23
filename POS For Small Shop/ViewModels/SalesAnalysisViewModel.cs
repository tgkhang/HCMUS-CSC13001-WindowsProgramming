using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_For_Small_Shop.Services;
using POS_For_Small_Shop.Data.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace POS_For_Small_Shop.ViewModels
{
    public class TopSellingItem
    {
        public int Rank { get; set; }
        public int MenuItemID { get; set; }
        public string ImagePath { get; set; } = "";
        public string Name { get; set; }
        public int Quantity { get; set; }
        public float TotalSales { get; set; }
    }

    [AddINotifyPropertyChangedInterface]
    public class SalesAnalysisViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public IDao _dao = Service.GetKeyedSingleton<IDao>();

        // Properties
        public DateTime Today { get; set; } = DateTime.Now; // Storage the current date
        public DateTime YesterDate { get; set; } = DateTime.Now.AddDays(-1); // Storage the yesterday date

        public float IncomeToday { get; set; } // Storage the income today
        public int TotalOrderToday { get; set; } // Storage the total income this month

        public float IncomeDifferenceValueTodayAndYesterday { get; set; } // Storage the income difference value today and yesterday
        public float IncomeDifferencePercentTodayAndYesterday { get; set; } // Storage the income difference percent today and yesterday

        public int OrderDifferenceValueTodayAndYesterday { get; set; } // Storage the order difference value today and yesterday

        public MenuItem? TopSellingItemToday { get; set; } // Storage the top selling item today
        public int QuantityTopSellingItemToday { get; set; } // Storage the quantity of the top selling item today

        public float IncomeYesterday { get; set; } // Storage the income yesterday
        public int TotalOrderYesterday { get; set; } // Storage the total income this month

        public ObservableCollection<TopSellingItem> TopFiveSellingItems { get; set; } = new (); // Storage the top five selling items

        // Properties for the payment method pie chart
        public ObservableCollection<PieSeries<ObservableValue>> PaymentMethodSeries { get; set; } = new();
        public IEnumerable<ISeries> PaymentMethodChartSeries => PaymentMethodSeries;
        public Axis[] PaymentMethodAxes { get; set; } = Array.Empty<Axis>();

        // Properties for the sales analysis chart
        public ObservableCollection<ISeries> SalesSeries { get; set; } = new();
        public ObservableCollection<ICartesianAxis> SalesXAxes { get; set; } = new();
        public ObservableCollection<ICartesianAxis> SalesYAxes { get; set; } = new();

        // Properties for the Orders analysis chart
        public ObservableCollection<ISeries> OrdersSeries { get; set; } = new();
        public ObservableCollection<ICartesianAxis> OrdersXAxes { get; set; } = new();
        public ObservableCollection<ICartesianAxis> OrdersYAxes { get; set; } = new();

        public SalesAnalysisViewModel()
        {
            // Calculate the total sales for today
            CalculateTotalSalesForToday();
            GetTopFiveSellingItems();
            LoadPaymentMethodBreakdown();
            LoadSalesChartData("daily");
            LoadOrdersChartData("daily");
        }

        /// <summary>
        /// Calculate the total sales for today include (income, orders, top selling item) then storage them in the properties
        /// </summary>
        public void CalculateTotalSalesForToday()
        {
            DateTime today = DateTime.Today;
            DateTime yesterday = today.AddDays(-1);

            // Get shifts
            var shifts = _dao.Shifts.GetAll();

            var todayShifts = shifts.Where(s => s.StartTime.Date == today).ToList();
            var yesterdayShifts = shifts.Where(s => s.StartTime.Date == yesterday).ToList();

            // Income and orders
            IncomeToday = todayShifts.Sum(s => s.TotalSales);
            TotalOrderToday = todayShifts.Sum(s => s.TotalOrders);

            IncomeYesterday = yesterdayShifts.Sum(s => s.TotalSales);
            TotalOrderYesterday = yesterdayShifts.Sum(s => s.TotalOrders);

            // Calculate the income difference value and percent
            IncomeDifferenceValueTodayAndYesterday = IncomeToday - IncomeYesterday;
            IncomeDifferencePercentTodayAndYesterday = IncomeYesterday > 0
                ? (IncomeDifferenceValueTodayAndYesterday / IncomeYesterday) * 100
                : 100;

            // Calculate the order difference value
            OrderDifferenceValueTodayAndYesterday = TotalOrderToday - TotalOrderYesterday;

            // Get the top selling item today
            var orders = _dao.Orders.GetAll().Where(o => o.Status == "Completed")
                .Where(o => todayShifts.Select(s => s.ShiftID).Contains(o.ShiftID))
                .ToList();

            var orderIds = orders.Select(o => o.OrderID).ToList();

            var orderDetails = _dao.OrderDetails.GetAll()
                .Where(od => orderIds.Contains(od.OrderID))
                .GroupBy(od => od.MenuItemID)
                .Select(g => new
                {
                    MenuItemID = g.Key,
                    Quantity = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.Quantity)
                .FirstOrDefault();

            QuantityTopSellingItemToday = orderDetails != null ? orderDetails.Quantity : 0;

            if (orderDetails != null)
            {
                TopSellingItemToday = _dao.MenuItems.GetById(orderDetails.MenuItemID);
            }
        }

        /// <summary>
        /// Get the top five selling items from the order details and storage them in the TopFiveSellingItems property
        /// </summary>
        public void GetTopFiveSellingItems()
        {

            TopFiveSellingItems.Clear();

            var allOrderDetails = _dao.OrderDetails.GetAll();

            var topItems = allOrderDetails
                .GroupBy(od => od.MenuItemID)
                .Select(g => new
                {
                    MenuItemID = g.Key,
                    Quantity = g.Sum(x => x.Quantity),
                    TotalSales = g.Sum(x => x.Quantity * x.UnitPrice)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(5)
                .ToList();

            int rank = 1;
            foreach (var item in topItems)
            {
                var menuItem = _dao.MenuItems.GetById(item.MenuItemID);
                if (menuItem != null)
                {
                    TopFiveSellingItems.Add(new TopSellingItem
                    {
                        Rank = rank ++,
                        MenuItemID = menuItem.MenuItemID,
                        ImagePath = menuItem.ImagePath,
                        Name = menuItem.Name,
                        Quantity = item.Quantity,
                        TotalSales = item.TotalSales
                    });
                }
            }

        }

        /// <summary>
        /// Load the payment method breakdown from the orders and storage them in the PaymentMethodSeries property
        /// </summary>
        private void LoadPaymentMethodBreakdown()
        {
            PaymentMethodSeries.Clear();

            var allOrders = _dao.Orders.GetAll().Where(o => o.Status == "Completed").ToList();

            var groups = allOrders
                .GroupBy(o => o.PaymentMethod.ToLowerInvariant())
                .Select(g => new
                {
                    Method = g.Key,
                    Count = g.Count()
                })
                .ToList();

            int total = groups.Sum(g => g.Count);

            // Add "Others" if only one method exists
            if (groups.Count == 1)
            {
                groups.Add(new { Method = "others", Count = 0 });
            }

            foreach (var group in groups)
            {
                PaymentMethodSeries.Add(new PieSeries<ObservableValue>
                {
                    Values = new ObservableCollection<ObservableValue> { new ObservableValue(group.Count) },
                    Name = Capitalize(group.Method),
                    DataLabelsSize = 14,
                    DataLabelsPosition = PolarLabelsPosition.Middle,
                    DataLabelsFormatter = point =>
                    {
                        double percent = total == 0 ? 0 : ((point.Model?.Value ?? 0) / total);
                        return $"{percent:P0}";
                    }
                });
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PaymentMethodChartSeries)));
        }

        /// <summary>
        /// Capitalize the first letter of the string and make the rest lowercase
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string Capitalize(string input)
        {
            return string.IsNullOrWhiteSpace(input) ? "" :
                char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

        /// <summary>
        /// Load the sales chart data based on the selected mode (daily, monthly, yearly)
        /// </summary>
        /// <param name="mode"></param>
        public void LoadSalesChartData(string mode = "daily")
        {
            SalesSeries.Clear();
            SalesXAxes.Clear();
            SalesYAxes.Clear();

            var shifts = _dao.Shifts.GetAll();
            var today = DateTime.Today;

            switch (mode)
            {
                case "daily":
                    var startOfMonth = new DateTime(today.Year, today.Month, 1);
                    var dailyRange = Enumerable.Range(0, (today - startOfMonth).Days + 1)
                        .Select(offset => startOfMonth.AddDays(offset))
                        .ToList();

                    var dailySalesDict = shifts
                        .Where(s => s.StartTime >= startOfMonth && s.StartTime <= today)
                        .GroupBy(s => s.StartTime.Date)
                        .ToDictionary(g => g.Key, g => g.Sum(s => s.TotalSales));

                    var dailyLabels = new List<string>();
                    var dailyValues = new List<float>();

                    foreach (var date in dailyRange)
                    {
                        dailyLabels.Add(date.ToString("dd-MM"));
                        dailyValues.Add(dailySalesDict.TryGetValue(date, out var value) ? value : 0f);
                    }

                    SalesSeries.Add(new LineSeries<float>
                    {
                        Name = "Daily Sales",
                        Values = dailyValues,
                        Stroke = new SolidColorPaint(SKColors.Blue, 3),
                    });

                    SalesXAxes.Add(new Axis
                    {
                        Name = "Date",
                        Labels = dailyLabels.ToArray(),
                        LabelsRotation = 15,
                    });
                    break;

                case "monthly":
                    var monthsInYear = Enumerable.Range(1, today.Month);
                    var monthlySalesDict = shifts
                        .Where(s => s.StartTime.Year == today.Year)
                        .GroupBy(s => s.StartTime.Month)
                        .ToDictionary(g => g.Key, g => g.Sum(s => s.TotalSales));

                    var monthlyLabels = new List<string>();
                    var monthlyValues = new List<float>();

                    foreach (var month in monthsInYear)
                    {
                        monthlyLabels.Add(new DateTime(today.Year, month, 1).ToString("MMM"));
                        monthlyValues.Add(monthlySalesDict.TryGetValue(month, out var value) ? value : 0f);
                    }

                    SalesSeries.Add(new LineSeries<float>
                    {
                        Name = "Monthly Sales",
                        Values = monthlyValues,
                        Stroke = new SolidColorPaint(SKColors.Blue, 3),
                    });

                    SalesXAxes.Add(new Axis
                    {
                        Name = "Month",
                        Labels = monthlyLabels.ToArray(),
                        LabelsRotation = 15,
                    });
                    break;

                case "yearly":
                    var yearRange = shifts.Select(s => s.StartTime.Year).Distinct().OrderBy(y => y).ToList();
                    var currentYear = today.Year;
                    var startYear = currentYear - 4;
                    var fullYears = Enumerable.Range(startYear, 5);

                    var yearlySalesDict = shifts
                        .GroupBy(s => s.StartTime.Year)
                        .ToDictionary(g => g.Key, g => g.Sum(s => s.TotalSales));

                    var yearlyLabels = new List<string>();
                    var yearlyValues = new List<float>();

                    foreach (var year in fullYears)
                    {
                        yearlyLabels.Add(year.ToString());
                        yearlyValues.Add(yearlySalesDict.TryGetValue(year, out var value) ? value : 0f);
                    }

                    SalesSeries.Add(new LineSeries<float>
                    {
                        Name = "Yearly Sales",
                        Values = yearlyValues,
                        Stroke = new SolidColorPaint(SKColors.Blue, 3),
                    });

                    SalesXAxes.Add(new Axis
                    {
                        Name = "Year",
                        Labels = yearlyLabels.ToArray(),
                        LabelsRotation = 15,
                    });
                    break;
            }

            SalesYAxes.Add(new Axis
            {
                Name = "Sales (VND)",
                Labeler = value => $"{value:N0} ₫",
                MinLimit = 0
            });


            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SalesSeries)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SalesXAxes)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SalesYAxes)));
        }


        public void LoadOrdersChartData(string mode = "daily")
        {
            OrdersSeries.Clear();
            OrdersXAxes.Clear();
            OrdersYAxes.Clear();

            var shifts = _dao.Shifts.GetAll();
            var today = DateTime.Today;

            switch (mode)
            {
                case "daily":
                    var startOfMonth = new DateTime(today.Year, today.Month, 1);
                    var dailyRange = Enumerable.Range(0, (today - startOfMonth).Days + 1)
                        .Select(offset => startOfMonth.AddDays(offset))
                        .ToList();

                    var dailyOrdersDict = shifts
                        .Where(s => s.StartTime >= startOfMonth && s.StartTime <= today)
                        .GroupBy(s => s.StartTime.Date)
                        .ToDictionary(g => g.Key, g => g.Sum(s => s.TotalOrders));

                    var dailyLabels = new List<string>();
                    var dailyValues = new List<float>();

                    foreach (var date in dailyRange)
                    {
                        dailyLabels.Add(date.ToString("dd-MM"));
                        dailyValues.Add(dailyOrdersDict.TryGetValue(date, out var value) ? value : 0f);
                    }

                    OrdersSeries.Add(new LineSeries<float>
                    {
                        Name = "Daily Orders",
                        Values = dailyValues,
                        Stroke = new SolidColorPaint(SKColors.Blue, 3),
                    });

                    OrdersXAxes.Add(new Axis
                    {
                        Name = "Date",
                        Labels = dailyLabels.ToArray(),
                        LabelsRotation = 15,
                    });
                    break;

                case "monthly":
                    var monthsInYear = Enumerable.Range(1, today.Month);
                    var monthlyOrdersDict = shifts
                        .Where(s => s.StartTime.Year == today.Year)
                        .GroupBy(s => s.StartTime.Month)
                        .ToDictionary(g => g.Key, g => g.Sum(s => s.TotalOrders));

                    var monthlyLabels = new List<string>();
                    var monthlyValues = new List<float>();

                    foreach (var month in monthsInYear)
                    {
                        monthlyLabels.Add(new DateTime(today.Year, month, 1).ToString("MMM"));
                        monthlyValues.Add(monthlyOrdersDict.TryGetValue(month, out var value) ? value : 0f);
                    }

                    OrdersSeries.Add(new LineSeries<float>
                    {
                        Name = "Monthly Orders",
                        Values = monthlyValues,
                        Stroke = new SolidColorPaint(SKColors.Blue, 3),
                    });

                    OrdersXAxes.Add(new Axis
                    {
                        Name = "Month",
                        Labels = monthlyLabels.ToArray(),
                        LabelsRotation = 15,
                    });
                    break;

                case "yearly":
                    var currentYear = today.Year;
                    var startYear = currentYear - 4;
                    var fullYears = Enumerable.Range(startYear, 5);

                    var yearlyOrdersDict = shifts
                        .GroupBy(s => s.StartTime.Year)
                        .ToDictionary(g => g.Key, g => g.Sum(s => s.TotalOrders));

                    var yearlyLabels = new List<string>();
                    var yearlyValues = new List<float>();

                    foreach (var year in fullYears)
                    {
                        yearlyLabels.Add(year.ToString());
                        yearlyValues.Add(yearlyOrdersDict.TryGetValue(year, out var value) ? value : 0f);
                    }

                    OrdersSeries.Add(new LineSeries<float>
                    {
                        Name = "Yearly Orders",
                        Values = yearlyValues,
                        Stroke = new SolidColorPaint(SKColors.Blue, 3),
                    });

                    OrdersXAxes.Add(new Axis
                    {
                        Name = "Year",
                        Labels = yearlyLabels.ToArray(),
                        LabelsRotation = 15,
                    });
                    break;
            }

            OrdersYAxes.Add(new Axis
            {
                Name = "Total Orders (Order)",
                Labeler = value => value.ToString("N0"),
                MinLimit = 0
            });

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrdersSeries)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrdersXAxes)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrdersYAxes)));
        }

    }
}
