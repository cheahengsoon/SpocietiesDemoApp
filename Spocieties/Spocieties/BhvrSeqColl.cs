using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Spocieties
{
    public class BhvrSeqColl : ObservableCollection<BhvrSeq>, INotifyPropertyChanged
    {
        public BhvrSeqColl()
        {
        }

        public BhvrSeqColl(BhvrSeqColl bsc)
        {
            foreach (BhvrSeq bs in bsc)
            {
                this.Add(new BhvrSeq(bs));
            }
        }

        public void RemoveEqualInventories()
        {
            if (this.Count <= 1)
            {
                return;
            }

            List<BhvrSeq> RemoveBS = new List<BhvrSeq>();

            for (int i = 0; i <= this.Count-1; i++)
            {
                for (int n = 0; n <= this.Count-1; n++)
                {
                    if (i >= n)
                    {
                        continue;
                    }
                    else if (this[i].Inventory.HasEqualAssets(this[n].Inventory))
                    {
                            RemoveBS.Add(CompareSimilarBhvrSeqs(this[i], this[n])); 
                    }
                }
            }
            

            foreach (BhvrSeq bs in RemoveBS)
            {
                this.Remove(bs);
            }
        }

        private BhvrSeq CompareSimilarBhvrSeqs(BhvrSeq bs1, BhvrSeq bs2)
        {
            Random rnd = new Random();
            int j = rnd.Next(2);
            if (j == 0)
            {
                return bs1;
            }
            else //if (j == 1)
            {
                return bs2;
            }
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
