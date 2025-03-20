using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using PropertyChanged;

namespace POS_For_Small_Shop.Utils
{
    [AddINotifyPropertyChangedInterface]
    public sealed class FullObservableCollection<T> : ObservableCollection<T>
        where T : INotifyPropertyChanged
    {
        public bool IsReadOnly { get; set; }

        private int _updateCount;
        public int UpdateCount
        {
            get => _updateCount;
            set => _updateCount = value;
        }

        public FullObservableCollection() : base()
        {
            InitializeEventHandlers();
        }

        public FullObservableCollection(IEnumerable<T> items) : base(items)
        {
            InitializeEventHandlers();
            SubscribeToPropertyChanged(items);
        }

        private void InitializeEventHandlers()
        {
            CollectionChanged += FullObservableCollectionCollectionChanged;
        }

        private void FullObservableCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                SubscribeToPropertyChanged(e.NewItems);

            if (e.OldItems != null)
                UnsubscribeFromPropertyChanged(e.OldItems);

            UpdateCount++;
        }

        private void SubscribeToPropertyChanged(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                if (item != null)
                    item.PropertyChanged += ItemPropertyChanged;
            }
        }

        private void SubscribeToPropertyChanged(IList items)
        {
            foreach (var item in items)
            {
                if (item is INotifyPropertyChanged notifyItem)
                    notifyItem.PropertyChanged += ItemPropertyChanged;
            }
        }

        private void UnsubscribeFromPropertyChanged(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                if (item != null)
                    item.PropertyChanged -= ItemPropertyChanged;
            }
        }

        private void UnsubscribeFromPropertyChanged(IList items)
        {
            foreach (var item in items)
            {
                if (item is INotifyPropertyChanged notifyItem)
                    notifyItem.PropertyChanged -= ItemPropertyChanged;
            }
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            int index = IndexOf((T)sender);
            if (index != -1)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Replace, sender, sender, index
                ));
                UpdateCount++;
            }
        }

        public void ToggleReadOnly()
        {
            IsReadOnly = !IsReadOnly;
        }
    }
}