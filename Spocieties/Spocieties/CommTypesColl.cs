using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Spocieties
{
    public class CommTypesColl : ObservableCollection<CommodityType>, INotifyPropertyChanged
    {
        public CommTypesColl()
        {
        }

        public CommodityType GetCommType(string s)
        {
            foreach (CommodityType c in this)
            {
                if (c.Name == s)
                {
                    return c;
                }
            }
            return null;
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
