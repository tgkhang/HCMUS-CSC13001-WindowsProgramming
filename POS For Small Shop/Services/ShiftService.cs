using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_For_Small_Shop.Data.Models;

namespace POS_For_Small_Shop.Services
{
    public interface IShiftService
    {
        Shift CurrentShift { get; }
        void SetCurrentShift(Shift shift);
        event EventHandler ShiftUpdated;
        void UpdateShift(Shift updatedShift);
    }

    public class ShiftService : IShiftService
    {
        private Shift _currentShift;

        public Shift CurrentShift => _currentShift;

        public event EventHandler ShiftUpdated;

        public void SetCurrentShift(Shift shift)
        {
            _currentShift = shift;
            ShiftUpdated?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateShift(Shift updatedShift)
        {
            _currentShift = updatedShift;
            ShiftUpdated?.Invoke(this, EventArgs.Empty);
        }
    }

}
