using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Spocieties
{
    public class Technology : INotifyPropertyChanged
    {
        private string _name;
        public string Name { get { return _name; } set { if (_name != value) { _name = value; RaisePropertyChanged("Name"); } } }

        private Inventory _inputs;
        public Inventory Inputs { get { return _inputs; } set { if (_inputs != value) { _inputs = value; RaisePropertyChanged("Inputs"); } } }

        private Inventory _outputs;
        public Inventory Outputs { get { return _outputs; } set { if (_outputs != value) { _outputs = value; RaisePropertyChanged("Outputs"); } } }

        private double _multiplier;
        public double Multiplier { get { if (Inputs.Count > 1) { return Outputs[0].Amount / Inputs[1].Amount; } else { return Outputs[0].Amount / Inputs[0].Amount; } } set { if (_multiplier != value) { _multiplier = value; RaisePropertyChanged("Multiplier"); } } }

        private bool _available;
        public bool Available { get { return _available; } set { if (_available != value) { _available = value; RaisePropertyChanged("Available"); } } }

        public Technology()
        {
            Inputs = new Inventory();
            Outputs = new Inventory();
        }

        public Technology(Inventory inputs, Inventory outputs)
        {
            Inputs = inputs;
            Outputs = outputs;
        }

        //public Inventory Implement(Inventory i)
        //{
        //    i.InvAdd(Outputs);
        //    i.InvSubtract(Inputs);
        //    return i;
        //}

        public void GetName()
        {
            try
            {
                this.Name = Inputs.First().Amount.ToString() + " " + Inputs.First().CommodityType.Name + " for " + Outputs.First().Amount.ToString() + " " + Outputs.First().CommodityType.Name;
            }
            catch
            {
                return;
            }
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
