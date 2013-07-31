using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Spocieties
{
    public class BhvrSeq : ObservableCollection<Behavior>, INotifyPropertyChanged
    {
        private int _currentIndex;
        public int CurrentIndex { get { return _currentIndex; } set { if (_currentIndex != value) { _currentIndex = value; RaisePropertyChanged("CurrentIndex"); } } }

        private Inventory _inventory;
        public Inventory Inventory { get { return _inventory; } set { if (_inventory != value) { _inventory = value; RaisePropertyChanged("Inventory"); } } }

        private bool _isChosen;
        public bool IsChosen { get { return _isChosen; } set { if (_isChosen != value) { _isChosen = value; RaisePropertyChanged("IsChosen"); } } }

        public BhvrSeq()
        {
            Inventory = new Inventory();
            IsChosen = false;
        }

        public BhvrSeq(Behavior b)
        {
            Inventory = new Inventory();
            this.Add(b);
            this.Inventory.InvAppBhvr(b);
            IsChosen = false;
        }

        public BhvrSeq(Behavior b, Inventory i)
        {
            this.Inventory = i;
            this.Add(b);
            this.Inventory.InvAppBhvr(b);
            IsChosen = false;
        }

        public BhvrSeq(ObservableCollection<Behavior> lb)
        {
            foreach (Behavior b in lb)
            {
                this.Add(b);
            }
            IsChosen = false;
        }

        public BhvrSeq(BhvrSeq bs)
        {
            foreach (Behavior b in bs)
            {
                this.Add(b);
                this.Inventory = new Inventory(bs.Inventory);
            }
            IsChosen = false;
        }

        public BhvrSeq(BhvrSeq bs, Behavior a)
        {
            this.Inventory = new Inventory(bs.Inventory);
            foreach (Behavior b in bs)
            {
                this.Add(b);
            }
            this.BsAdd(a);
            IsChosen = false;
        }

        public void BsAdd(Behavior b)
        {
            this.Inventory.InvAppBhvr(b);
            this.Add(b);
        }

        //public bool IsEquivalent(BhvrSeq bs)
        //{
        //    int _counter = 0;
        //    if (this.Count != bs.Count)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        foreach (Behavior b in this)
        //        {
        //            for (int i = 0; i <= bs.Count; i++)
        //            {
        //                if (b == bs[i])
        //                {
        //                    _counter++;
        //                }
        //            }
        //        }
        //    }
        //}

        public List<Behavior> NextPossBhvrs(List<Behavior> BList)
        {
            List<Behavior> swapBL = new List<Behavior>();

            foreach (Behavior b in BList)
            {
                if (this.Inventory.InvHasAmt(b.Inputs))
                {
                    if (!(this.Contains(b) && (b.Repeatable == false)))  //if BS doesnt contain the Bhvr and it is not repeatable
                    {
                        swapBL.Add(b);
                    }
                }
            }
            return swapBL;
        }

        public new event PropertyChangedEventHandler PropertyChanged;

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
