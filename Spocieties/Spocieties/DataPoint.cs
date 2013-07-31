using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Spocieties
{
    public class DataPoint : INotifyPropertyChanged
    {
        private double _price;
        public double Price { get { return _price; } set { if (_price != value) { _price = value; RaisePropertyChanged("Price"); } } }

        private double _qty;
        public double Qty { get { return _qty; } set { if (_qty != value) { _qty = value; RaisePropertyChanged("Qty"); } } }

        public DataPoint()
        {
            Price = 0;
            Qty = 0;
        }

        public DataPoint(double price, double qty)
        {
            Price = 0;
            Qty = 0;
            Price = price;
            Qty = qty;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
