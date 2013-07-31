using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace Spocieties
{
    public class Inventory : ObservableCollection<Asset>, INotifyPropertyChanged
    {
        public Inventory()
        {
        }

        public Inventory(Inventory i)
        {
            foreach (Asset a in i)
            {
                this.Add(new Asset(a));
            }
        }

        public bool InvHas(CommodityType ct)
        {
            foreach (Asset b in this)
            {
                if (b.CommodityType == ct)
                {
                    return true;
                }
            }
            return false;
        }

        public bool InvHas(Asset a)
        {
            foreach (Asset b in this)
            {
                if (b.CommodityType == a.CommodityType && b.Amount > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool InvHas(string s)
        {
            foreach (Asset b in this)
            {
                if (b.CommodityType.Name == s && b.Amount > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool InvHas(ObservableCollection<Asset> a)
        {
            foreach (Asset aa in a)
            {
                if (!this.InvHas(aa))
                {
                    return false;
                }
            }
            return true;
        }

        public bool InvHasAmt(Asset a)
        {
            foreach (Asset b in this)
            {
                if (b.CommodityType == a.CommodityType && b.Amount >= a.Amount)
                {
                    return true;
                }
            }
            return false;
        }

        public bool InvHasAmt(ObservableCollection<Asset> a)
        {
            foreach (Asset aa in a)
            {
                if (!this.InvHasAmt(aa))
                {
                    return false;
                }
            }
            return true;
        }

        public bool InvHasAmt(CommodityType ct, double amt)
        {
            amt = Math.Round(amt, 4);
            foreach (Asset a in this)
            {
                if (a.CommodityType == ct && a.Amount >= amt)
                {
                    return true;
                }
            }
            return false;
        }

        public void InvAdd(Asset b)
        {
            foreach (Asset a in this)
            {
                if (a.CommodityType == b.CommodityType)
                {
                    a.Amount += b.Amount;
                    return;
                }
            }
            this.Add(new Asset(b));
        }

        public void InvAdd(CommodityType ct, double i)
        {
            i = Math.Round(i, 4);
            foreach (Asset a in this)
            {
                if (a.CommodityType.Name == ct.Name)
                {
                    a.Amount += i;
                    return;
                }
            }

            this.Add(new Asset(ct, i));
        }

        public void InvAdd(ObservableCollection<Asset> aa)
        {
            foreach (Asset a in aa)
            {
                if (!this.InvHas(a.CommodityType))
                {
                    this.InvAdd(a.CommodityType, 0);
                }
            }
            foreach (Asset a in aa)
            {
                foreach (Asset b in this)
                {
                    if (b.CommodityType == a.CommodityType)
                    {
                        b.Amount += a.Amount;
                        continue;
                    }
                }
            }
        }

        public void InvAdd(Inventory i)
        {
            foreach (Asset a in i)
            {
                if (!this.InvHas(a.CommodityType))
                {
                    this.InvAdd(a.CommodityType, 0);
                }
            }
            foreach (Asset a in i)
            {
                foreach (Asset b in this)
                {
                    if (b.CommodityType == a.CommodityType)
                    {
                        b.Amount += a.Amount;
                        continue;
                    }
                }
            }
        }

        public bool InvSubtract(Asset b)
        {
            foreach (Asset a in this)
            {
                if (a.CommodityType.Name == b.CommodityType.Name)
                {
                    if (a.Amount < b.Amount)
                    {
                        //MessageBox.Show("Insufficient Assets in Inventory!");
                        return false;
                    }
                    a.Amount -= b.Amount;
                    return true;
                }
            }
            if (!this.InvHas(b))
            {
                //MessageBox.Show("Asset Not Held in Inventory!");
                return false;
            }
            return false;
        }

        public bool InvSubtract(CommodityType ct, double i)
        {
            i = Math.Round(i, 4);
            if (!this.InvHasAmt(new Asset(ct, i)))
            {
                //MessageBox.Show("Asset Not Held in Inventory!");
                return false;
            }
            this.GetAsset(ct).Amount -= i;
            return true;
        }

        public bool InvSubtract(ObservableCollection<Asset> aa)
        {
            if (!this.InvHasAmt(aa))
            {
                //MessageBox.Show("Asset Not Held in Inventory!");
                return false;
            }

            List<Asset> swapAA = new List<Asset>();

            foreach (Asset a in aa)
            {
                this.GetAsset(a.CommodityType).Amount -= a.Amount;
            }
            return true;
        }

        public bool InvSubtract(Inventory i)
        {
            if (!this.InvHasAmt(i))
            {
                //MessageBox.Show("Asset Not Held in Inventory!");
                return false;
            }

            List<Asset> swapAA = new List<Asset>();

            foreach (Asset a in i)
            {
                this.GetAsset(a.CommodityType).Amount -= a.Amount;
            }
            return true;
        }

        public bool InvAppBhvr(Behavior b)
        {
            if (InvSubtract(b.Inputs))
            {
                InvAdd(b.Outputs);
                return true;
            }
            return false;
        }

        public bool InvAppOfferedBhvr(Behavior b)
        {
            if (InvSubtract(b.Outputs))
            {
                InvAdd(b.Inputs);
                return true;
            }
            return false;
        }

        public void InvAppBhvrSeq(BhvrSeq bs)
        {
            foreach (Behavior b in bs)
            {
                this.InvAppBhvr(b);
            }

        }

        public CommodityType GetCommType(string s)
        {
            foreach (Asset a in this)
            {
                if (a.CommodityType.Name == s)
                {
                    return a.CommodityType;
                }
            }
            return null;
        }

        public Asset GetAsset(string s)
        {
            foreach (Asset a in this)
            {
                if (a.CommodityType.Name == s)
                {
                    return a;
                }
            }
            return null;
        }

        public Asset GetAsset(CommodityType c)
        {
            foreach (Asset a in this)
            {
                if (a.CommodityType.Name == c.Name)
                {
                    return a;
                }
            }
            return null;
        }

        public void InvImplementTech(Technology t)
        {



        }

        public bool HasEqualAssets(Inventory ii)
        {
            Inventory i = new Inventory(this);
            //Inventory ii = new Inventory(inv);

            if (i.InvSubtract(ii) == false)
            {
                return false;
            }

            foreach (Asset a in i)
            {
                if (a.Amount > 0)
                {
                    return false;
                }
            }

            return true;
        }

        public double SumOfAssetAmts()
        {
            double total = 0;
            foreach (Asset a in this)
            {
                total += a.Amount;
            }

            return total;
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
