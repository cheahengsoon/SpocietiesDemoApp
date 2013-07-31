using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spocieties
{
    class Factory : INotifyPropertyChanged
    {
        private string _name;
        public string Name { get { return _name; } set { if (_name != value) { _name = value; RaisePropertyChanged("Name"); } } }

        private Technology _tech;
        public Technology Tech { get { return _tech; } set { if (_tech != value) { _tech = value; RaisePropertyChanged("Tech"); } } }


        public Factory()
        {
            

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
