using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Spocieties
{
    public class DemandModelColl : ObservableCollection<DemandModel>, INotifyPropertyChanged
    {
        public DemandModelColl()
        {

        }


        public bool Contains(CommodityType c)
        {
            foreach (DemandModel d in this)
            {
                if (d.CommodityType == c)
                {
                    return true;
                }
            }
            return false;
        }

        #region GetDemandModel Methods

        public DemandModel GetDemandModel(string s)
        {
            foreach (DemandModel d in this)
            {
                if (d.CommodityType.Name == s)
                {
                    return d;
                }
            }
            return null;
        }

        public DemandModel GetDemandModel(CommodityType c)
        {
            foreach (DemandModel d in this)
            {
                if (d.CommodityType.Name == c.Name)
                {
                    return d;
                }
            }
            return null;
        }

        public DemandModel GetDemandModel(Asset a)
        {
            foreach (DemandModel d in this)
            {
                if (d.CommodityType == a.CommodityType)
                {
                    return d;
                }
            }
            return null;
        }

        #endregion


        public void Populate(ObservableCollection<Behavior> bhvrs)
        {
            foreach (Behavior b in bhvrs)
            {
                if (b.Buying == true) //Money is in the Outputs
                {
                    if (!this.Contains(b.Inputs[0].CommodityType))
                    {
                        DemandModel DM = new DemandModel(b.Inputs[0].CommodityType);
                        double unitPrice = b.Outputs.GetAsset("Money").Amount / b.Inputs[0].Amount;
                        DM.Add(unitPrice, 0);
                        this.Add(DM);
                    }
                    else //if the DemandModelColl has an entry for the CommodityType, but not the unitPrice
                    {
                        DemandModel DM = GetDemandModel(b.Inputs[0].CommodityType);
                        double unitPrice = b.Outputs.GetAsset("Money").Amount / b.Inputs[0].Amount;
                        if (!DM.ContainsPrice(unitPrice))
                        {
                            DM.Add(unitPrice, 0);
                        }
                    }
                }

                else if (b.Buying == false) //Money is in the Inputs
                {
                    if (!this.Contains(b.Outputs[0].CommodityType))
                    {
                        DemandModel DM = new DemandModel(b.Outputs[0].CommodityType);
                        double unitPrice = b.Inputs.GetAsset("Money").Amount / b.Outputs[0].Amount;
                        DM.Add(unitPrice, 0);
                        this.Add(DM);
                    }
                    else //if the DemandModelColl has an entry for the CommodityType, but not the unitPrice
                    {
                        DemandModel DM = GetDemandModel(b.Outputs[0].CommodityType);
                        double unitPrice = b.Inputs.GetAsset("Money").Amount / b.Outputs[0].Amount;
                        if (!DM.ContainsPrice(unitPrice))
                        {
                            DM.Add(unitPrice, 0);
                        }
                    }
                }

                else if (b.Buying == null)
                {
                    continue;
                }
            }

        }

        public void Update(Behavior b)
        {
            if (b.Buying == true) //Money is in the Outputs
            {
                DemandModel dm = GetDemandModel(b.Inputs[0]);
                
                if (dm == null)
                {
                    Add(new DemandModel(b.Outputs[0].CommodityType));
                    dm = GetDemandModel(b.Outputs[0]);
                }

                double unitPrice = b.Outputs.GetAsset("Money").Amount / b.Inputs[0].Amount;

                if (dm.ContainsPrice(unitPrice))
                {
                    dm.GetPriceDataPoint(unitPrice).Qty += b.Inputs[0].Amount;
                }
                else
                {
                    dm.Add(unitPrice, b.Inputs[0].Amount);
                }
            }

            else if (b.Buying == false) //Money is in the Inputs
            {
                DemandModel dm = GetDemandModel(b.Outputs[0]);
                
                if (dm == null)
                {
                    Add(new DemandModel(b.Outputs[0].CommodityType));
                    dm = GetDemandModel(b.Outputs[0]);
                }

                double unitPrice = b.Inputs.GetAsset("Money").Amount / b.Outputs[0].Amount;

                if (!dm.ContainsPrice(unitPrice))
                {
                    dm.Add(unitPrice, b.Outputs[0].Amount);
                }
                else
                {
                    dm.GetPriceDataPoint(unitPrice).Qty += b.Outputs[0].Amount;
                }
            }

            else if (b.Buying == null)
            {
                return;
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
