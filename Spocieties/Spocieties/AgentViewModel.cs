using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spocieties
{
    class AgentViewModel : INotifyPropertyChanged
    {
        private Agent _agent;
        public Agent Agent { get { return _agent; } set { if (_agent != value) { _agent = value; RaisePropertyChanged("Agent"); } } }

        private ObservableCollection<Behavior> _bhvrs;
        public ObservableCollection<Behavior> Bhvrs { get { return _bhvrs; } set { if (_bhvrs != value) { _bhvrs = value; RaisePropertyChanged("Bhvrs"); } } }

        private CommTypesColl _commTypes;
        public CommTypesColl CommTypes { get { return _commTypes; } set { if (_commTypes != value) { _commTypes = value; RaisePropertyChanged("CommTypes"); } } }


        public AgentViewModel()
        {
            Agent = new Agent();
            Bhvrs = new ObservableCollection<Behavior>();
            CommTypes = new CommTypesColl();
            InitializeCollections();
            Agent.EndGoals.Add(new Asset(CommTypes[0], 2));
        }

        private void InitializeCollections()
        {
            //Add some basic CommodityTypes
            CommTypes.Add(new CommodityType("Money")); //0
            CommTypes.Add(new CommodityType("Time"));  //1
            CommTypes.Add(new CommodityType("Labor")); //2
            CommTypes.Add(new CommodityType("Trees")); //3
            CommTypes.Add(new CommodityType("Lumber")); //4
            CommTypes.Add(new CommodityType("Planks")); //5
            CommTypes.Add(new CommodityType("Shelter")); //6

            Agent.Inventory.InvAdd(CommTypes[2], 16);
            
            //Add a Work Behavior
            Behavior tempBhvr = new Behavior();
            tempBhvr.Inputs.Add(new Asset(CommTypes[2], 8));
            tempBhvr.Outputs.Add(new Asset(CommTypes[0], 24));
            tempBhvr.Name = "Employment";
            Bhvrs.Add(tempBhvr);

            //Add a Chop Trees Behavior
            tempBhvr = new Behavior();
            tempBhvr.Inputs.Add(new Asset(CommTypes[2], 4));
            tempBhvr.Outputs.Add(new Asset(CommTypes[3], 2));
            tempBhvr.Name = "Chop Trees";
            Bhvrs.Add(tempBhvr);

            //Add a Make Lumber Behavior
            tempBhvr = new Behavior();
            tempBhvr.Inputs.Add(new Asset(CommTypes[2], 4));
            tempBhvr.Inputs.Add(new Asset(CommTypes[3], 1));
            tempBhvr.Outputs.Add(new Asset(CommTypes[4], 4));
            tempBhvr.Name = "Make Lumber";
            Bhvrs.Add(tempBhvr);

            //Add a Sell Lumber Behavior
            tempBhvr = new Behavior();
            tempBhvr.Inputs.Add(new Asset(CommTypes[4], 1));
            tempBhvr.Outputs.Add(new Asset(CommTypes[0], 5));
            tempBhvr.Name = "Sell Lumber";
            Bhvrs.Add(tempBhvr);

            //Add a Beg Behavior
            tempBhvr = new Behavior();
            tempBhvr.Inputs.Add(new Asset(CommTypes[2], 4));
            tempBhvr.Outputs.Add(new Asset(CommTypes[0], 5));
            tempBhvr.Name = "Beg for Money";
            Bhvrs.Add(tempBhvr);

        }


        public event PropertyChangedEventHandler PropertyChanged;

        //event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        //{
        //    add { throw new NotImplementedException(); }
        //    remove { throw new NotImplementedException(); }
        //}

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
