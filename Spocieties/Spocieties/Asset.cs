using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Spocieties
{
    public class Asset : INotifyPropertyChanged
    {
        private CommodityType _commodityType;
        public CommodityType CommodityType { get { return _commodityType; } set { if (_commodityType != value) { _commodityType = value; RaisePropertyChanged("CommodityType"); } } }

        private double _amount;
        public double Amount { get { return _amount; } set { if (_amount != value) { _amount = Math.Round(value, 2); RaisePropertyChanged("Amount"); } } }

        public string Name { get { return CommodityType.Name + "  Qty: " + Amount; }}

        public Asset(CommodityType ct, double a)
        {
            CommodityType = ct;
            Amount = Math.Round(a, 2);
        }

        public Asset(Asset a)
        {
            CommodityType = a.CommodityType;
            Amount = Math.Round(a.Amount, 2);
        }

        public Asset()
        {
            Amount = 0;
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
