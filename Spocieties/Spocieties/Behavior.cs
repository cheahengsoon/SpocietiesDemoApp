using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Spocieties
{
    public class Behavior : INotifyPropertyChanged
    {
        #region Properties

        private string _name;
        public string Name { get { return _name; } set { if (_name != value) { _name = value; RaisePropertyChanged("Name"); } } }

        private Inventory _inputs;
        public Inventory Inputs { get { return _inputs; } set { if (_inputs != value) { _inputs = value; RaisePropertyChanged("Inputs"); } } }

        private Inventory _outputs;
        public Inventory Outputs { get { return _outputs; } set { if (_outputs != value) { _outputs = value; RaisePropertyChanged("Outputs"); } } }

        private Organization _owningOrg;
        public Organization OwningOrg { get { return _owningOrg; } set { if (_owningOrg != value) { _owningOrg = value; RaisePropertyChanged("OwningOrg"); } } }

        public bool? _buying;
        public bool? Buying
        {
            get
            {
                if (Inputs.InvHas("Money") && !Outputs.InvHas("Money"))
                {
                    return false;
                }
                else if (Outputs.InvHas("Money") && !Inputs.InvHas("Money"))
                {
                    return true;
                }
                else //if(!Inputs.InvHas("Money") && !Outputs.InvHas("Money"))
                {
                    return null;
                }
            }
        }
           

        private bool _repeatable;
        public bool Repeatable { get { return _repeatable; } set { if (_repeatable != value) { _repeatable = value; RaisePropertyChanged("Repeatable"); } } }

        private bool _mandated;
        public bool Mandated { get { return _mandated; } set { if (_mandated != value) { _mandated = value; RaisePropertyChanged("Mandated"); } } }

        private bool _available;
        public bool Available { get { return _available; } set { if (_available != value) { _available = value; RaisePropertyChanged("Available"); } } }


        #endregion


        #region Constructors
        public Behavior(string n, Inventory ip, Inventory op, bool rpt, bool mdt)
        {
            Name = n;
            Inputs = ip;           // Good object representing cost
            Outputs = op;             // Good object representing payoff
            Repeatable = rpt;
            Mandated = mdt;
            Available = true;
            
        }

        public Behavior(Inventory ip, Inventory op)
        {
            Inputs = ip;           // Good object representing cost
            Outputs = op;             // Good object representing payoff
            Repeatable = false;
            Mandated = false;
            Available = true;
        }

        public Behavior()
        {
            Inputs = new Inventory();
            Outputs = new Inventory();
            Repeatable = false;
            Mandated = false;
            Available = true;
        }

        public Behavior(Behavior b)
        {
            Inputs = b.Inputs;
            Outputs = b.Outputs;
            Name = b.Name;
            Repeatable = b.Repeatable;
            Mandated = b.Mandated;
            Available = b.Available;
        }
        #endregion

        public bool BhvrOutputHas(Asset a)
        {
            foreach (Asset aa in this.Outputs)
            {
                if (a.CommodityType.Name == aa.CommodityType.Name && a.Amount <= aa.Amount)
                {
                    return true;
                }
            }
            return false;
        }

        public bool BhvrOutputHas(CommodityType ct)
        {
            foreach (Asset aa in this.Outputs)
            {
                if (ct.Name == aa.CommodityType.Name)
                {
                    return true;
                }
            }
            return false;
        }

        
        public void GetName()
        {
            if (Name == null)
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
