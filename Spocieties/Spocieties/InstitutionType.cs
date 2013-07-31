
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Spocieties
{
    public class InstitutionType : INotifyPropertyChanged
    {
        private string _name;
        public string Name { get { return _name; } set { _name = value; RaisePropertyChanged("Name"); } }

        public InstitutionType()
        {
        }

        public InstitutionType(string n)
        {
            Name = n;
        }

        public InstitutionType(InstitutionType i)
        {
            this.Name = i.Name;
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
