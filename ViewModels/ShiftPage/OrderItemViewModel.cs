using System.ComponentModel;
using PropertyChanged;

namespace POS_For_Small_Shop.ViewModels.ShiftPage
{
    [AddINotifyPropertyChangedInterface]
    public class OrderItemViewModel : INotifyPropertyChanged
    {
        private int _quantity;

        public event PropertyChangedEventHandler PropertyChanged;

        public int MenuItemID { get; set; }
        public string Name { get; set; }
        public float UnitPrice { get; set; }

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                // Explicitly update Total when Quantity changes
                Total = _quantity * UnitPrice;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Quantity)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Total)));
            }
        }

        public float Total { get; set; }
    }
}

