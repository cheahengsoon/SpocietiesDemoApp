using System.ComponentModel;

namespace Spocieties
{
    public class CommodityType : INotifyPropertyChanged
    {
        private string _name;
        public string Name { get { return _name; } set { if (_name != value) { _name = value; RaisePropertyChanged("Name"); } } }

        private double? _mktPrice;
        public double? MktPrice { get { return _mktPrice; } set { if (_mktPrice != value) { _mktPrice = value; RaisePropertyChanged("MktPrice"); } } }

        public CommodityType()
        {
        }

        public CommodityType(string n)
        {
            Name = n;
        }

        public CommodityType(CommodityType ct)
        {
            this.Name = ct.Name;
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
