using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections;

namespace Spocieties
{
    public class DemandModel : INotifyPropertyChanged
    {
        private CommodityType _commodityType;
        public CommodityType CommodityType { get { return _commodityType; } set { if (_commodityType != value) { _commodityType = value; RaisePropertyChanged("CommodityType"); } } }

        private ObservableCollection<DataPoint> _data;
        public ObservableCollection<DataPoint> Data { get { return _data; } set { if (_data != value) { _data = value; RaisePropertyChanged("Data"); } } }

        private DataPoint _selectedDataPoint;
        public DataPoint SelectedDataPoint { get { return _selectedDataPoint; } set { if (_selectedDataPoint != value) { _selectedDataPoint = value; RaisePropertyChanged("SelectedDataPoint"); RaisePropertyChanged("SelectedDataPointNextPrice"); RaisePropertyChanged("SelectedDataPointPrevPrice"); RaisePropertyChanged("SelectedDataPointNextQty"); RaisePropertyChanged("SelectedDataPointPrevQty"); } } }

        private DataPoint _selectedDataPointNextPrice;
        public DataPoint SelectedDataPointNextPrice { get { if (_selectedDataPoint == null) { return GetNextHigherPriceDataPoint(Price); } else { return GetNextHigherPriceDataPoint(SelectedDataPoint); } } set { if (_selectedDataPointNextPrice != value) { _selectedDataPointNextPrice = value; RaisePropertyChanged("SelectedDataPointNextPrice"); } } }

        private DataPoint _selectedDataPointPrevPrice;
        public DataPoint SelectedDataPointPrevPrice { get { if (_selectedDataPoint == null) { return GetNextLowerPriceDataPoint(Price); } else { return GetNextLowerPriceDataPoint(SelectedDataPoint); } } set { if (_selectedDataPointPrevPrice != value) { _selectedDataPointPrevPrice = value; RaisePropertyChanged("SelectedDataPointPrevPrice"); } } }

        private DataPoint _selectedDataPointNextQty;
        public DataPoint SelectedDataPointNextQty { get { if (_selectedDataPoint == null) { return GetNextHigherQtyDataPoint(Qty); } else { return GetNextHigherQtyDataPoint(SelectedDataPoint); } } set { if (_selectedDataPointNextQty != value) { _selectedDataPointNextQty = value; RaisePropertyChanged("SelectedDataPointNextQty"); } } }

        private DataPoint _selectedDataPointPrevQty;
        public DataPoint SelectedDataPointPrevQty { get { if (_selectedDataPoint == null) { return GetNextLowerQtyDataPoint(Qty); } else { return GetNextLowerQtyDataPoint(SelectedDataPoint); } } set { if (_selectedDataPointPrevQty != value) { _selectedDataPointPrevQty = value; RaisePropertyChanged("SelectedDataPointPrevQty"); } } }


        private DataPoint _linearEstQty;
        public DataPoint LinearEstQty { get { return EstimateQty(Price); } set { if (_linearEstQty != value) { _linearEstQty = value; RaisePropertyChanged("LinearEstQty"); } } }

        private DataPoint _linearEstPrice;
        public DataPoint LinearEstPrice { get { return EstimatePrice(Qty); } set { if (_linearEstPrice != value) { _linearEstPrice = value; RaisePropertyChanged("LinearEstPrice"); } } }

        private double _price;
        public double Price { get { return _price; } set { if (_price != value) { _price = value; RaisePropertyChanged("Price"); RaiseEvents(); } } }

        private double _qty;
        public double Qty { get { return _qty; } set { if (_qty != value) { _qty = value; RaisePropertyChanged("Qty"); RaiseEvents(); } } }


        public DemandModel(CommodityType c)
        {
            CommodityType = c;
            Data = new ObservableCollection<DataPoint>();
            //<price, qty>
        }

        public DemandModel()
        {
            Data = new ObservableCollection<DataPoint>();
        }


        public void Add(double price, double qty)
        {
            price = Math.Round(price, 2);
            qty = Math.Round(qty, 2);
            if (ContainsPrice(price))
            {
                GetPriceDataPoint(price).Qty = qty;
            }
            else
            {
                Data.Add(new DataPoint(price, qty));
            }
        }

        public void Increment(double price, double qty)
        {
            price = Math.Round(price, 2);
            qty = Math.Round(qty, 4);
            if (!ContainsPrice(price))
            {
                this.Add(price, 0);
            }
            GetPriceDataPoint(price).Qty += qty;
        }

        public bool ContainsPrice(double price)
        {
            price = Math.Round(price, 2);
            foreach (DataPoint dp in Data)
            {
                if (Math.Round(dp.Price, 2) == price)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsQty(double qty)
        {
            qty = Math.Round(qty, 2);
            foreach (DataPoint dp in Data)
            {
                qty = Math.Round(qty, 4);
                if (Math.Round(dp.Qty, 4) == qty)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsDataPoint(DataPoint dp)
        {
            foreach (DataPoint dp1 in Data)
            {
                if (dp == dp1)
                {
                    return true;
                }
            }
            return false;
        }


        public DataPoint GetPriceDataPoint(double price)
        {
            price = Math.Round(price, 2);
            if (Data.Count == 0 || Data == null)
            {
                return null;
            }
            foreach (DataPoint dp in Data)
            {
                if (dp.Price == price)
                {
                    return dp;
                }
            }
            return null;
        }

        public DataPoint GetQtyDataPoint(double qty)
        {
            qty = Math.Round(qty, 2);
            if (Data.Count == 0 || Data == null)
            {
                return null;
            }
            foreach (DataPoint dp in Data)
            {
                if (dp.Qty == qty)
                {
                    return dp;
                }
            }
            return null;
        }


        public DataPoint GetMaxRevenueDataPoint()
        {
            DataPoint tempDP = null;
            if (Data != null && Data.Count > 0)
            {
                double storedtotal = 0;
                
                foreach (DataPoint dp in Data)
                {
                    if (dp.Qty * dp.Price > storedtotal)
                    {
                        storedtotal = dp.Qty * dp.Price;
                        tempDP = dp;
                    }
                }
            }
            return tempDP;
        }

        public DataPoint GetMaxQtyDataPoint()
        {
            if (Data.Count == 0 || Data == null)
            {
                return null;
            }
            if (Data.Count == 1)
            {
                return Data[0];
            }
            DataPoint tempDP = Data[0];
            foreach (DataPoint dp in Data)
            {
                if (dp.Qty > tempDP.Qty)
                {
                    tempDP = dp;
                }
            }
            return tempDP;
        }

        public DataPoint GetMaxPriceDataPoint()
        {
            if (Data.Count == 0 || Data == null)
            {
                return null;
            }
            if (Data.Count == 1)
            {
                return Data[0];
            }
            DataPoint tempDP = Data[0];
            foreach (DataPoint dp in Data)
            {
                if (dp.Price > tempDP.Price)
                {
                    tempDP = dp;
                }
            }
            return tempDP;
        }


        public DataPoint GetMinQtyDataPoint()
        {
            if (Data.Count == 0 || Data == null)
            {
                return null;
            }
            if (Data.Count == 1)
            {
                return Data[0];
            }

            DataPoint tempDP = Data[0];
            foreach (DataPoint dp in Data)
            {
                if (dp.Qty < tempDP.Qty)
                {
                    tempDP = dp;
                }
            }
            return tempDP;
        }

        public DataPoint GetMinPriceDataPoint()
        {
            if (Data.Count == 0 || Data == null)
            {
                return null;
            }
            if (Data.Count == 1)
            {
                return Data[0];
            }
            DataPoint tempDP = Data[0];
            foreach (DataPoint dp in Data)
            {
                if (dp.Price < tempDP.Price)
                {
                    tempDP = dp;
                }
            }
            return tempDP;
        }


        public DataPoint GetNextHigherQtyDataPoint(double qty)
        {
            qty = Math.Round(qty, 2);
            DemandModel tempDM = new DemandModel(this.CommodityType);
            #region Sort by Qty
            var tempList = new List<DataPoint>();

            foreach (DataPoint dp in Data)
            {
                tempList.Add(dp);
            }

            tempList.Sort(new DataPointQtyComparer()); //sorts by qty, lowest to highest, equal qties sort by price

            ObservableCollection<DataPoint> tempData = new ObservableCollection<DataPoint>();

            foreach (DataPoint dp in tempList)
            {
                tempData.Add(dp);
            }

            tempDM.Data = tempData;
            #endregion
            DataPoint tempDP = tempDM.GetQtyDataPoint(qty);


            int i = tempDM.Data.IndexOf(tempDP);

            if (tempDP == null)
            {
                tempDP = GetMaxQtyDataPoint();
                if (qty > tempDP.Qty)
                {
                    return null;
                }
                foreach (DataPoint dp in tempDM.Data)
                {
                    if (dp.Qty > qty && dp.Qty <= tempDP.Qty)
                    {
                        tempDP = dp;
                    }
                }
                i = tempDM.Data.IndexOf(tempDP);
            }
            if (i == tempDM.Data.Count - 1)
            {
                return tempDM.Data[i];
            }
            return tempDM.Data[i + 1];
        }

        public DataPoint GetNextHigherPriceDataPoint(double price)
        {
            price = Math.Round(price, 2);
            DemandModel tempDM = new DemandModel(this.CommodityType);

            #region Sort by Price
            var tempList = new List<DataPoint>();

            foreach (DataPoint dp in Data)
            {
                tempList.Add(dp);
            }

            tempList.Sort(new DataPointPriceComparer());

            ObservableCollection<DataPoint> tempData = new ObservableCollection<DataPoint>();

            foreach (DataPoint dp in tempList)
            {
                tempData.Add(dp);
            }

            tempDM.Data = tempData;
            #endregion

            DataPoint tempDP = tempDM.GetPriceDataPoint(price);

            int i = tempDM.Data.IndexOf(tempDP);

            if (i == -1)
            {
                tempDP = GetMaxPriceDataPoint();
                if (price > tempDP.Price)
                {
                    return null;
                }
                foreach (DataPoint dp in tempDM.Data)
                {
                    if (dp.Price > price && dp.Price <= tempDP.Price)
                    {
                        tempDP = dp;
                    }
                }
                i = tempDM.Data.IndexOf(tempDP) - 1;
            }
            if (i == tempDM.Data.Count - 1)
            {
                return tempDM.Data[i];//if the price is the max price in the DemandModel, then return the DataPoint of that price
            }
            return tempDM.Data[i + 1];
        }

        public DataPoint GetNextLowerQtyDataPoint(double qty)
        {
            qty = Math.Round(qty, 2);
            DemandModel tempDM = new DemandModel(this.CommodityType);

            #region Sort by Qty
            var tempList = new List<DataPoint>();

            foreach (DataPoint dp in Data)
            {
                tempList.Add(dp);
            }

            tempList.Sort(new DataPointQtyComparer()); //sorts by qty, lowest to highest, equal qties sort by price

            ObservableCollection<DataPoint> tempData = new ObservableCollection<DataPoint>();

            foreach (DataPoint dp in tempList)
            {
                tempData.Add(dp);
            }

            tempDM.Data = tempData;
            #endregion

            DataPoint tempDP = tempDM.GetQtyDataPoint(qty);

            int i = tempDM.Data.IndexOf(tempDP);
            if (i == -1)
            {
                tempDP = tempDM.GetMinQtyDataPoint();
                if (qty < tempDP.Qty)
                {
                    return null;
                }

                foreach (DataPoint dp in tempDM.Data)
                {
                    if (dp.Qty < qty && dp.Qty >= tempDP.Qty)
                    {
                        tempDP = dp;
                    }
                }
                i = tempDM.Data.IndexOf(tempDP) + 1;
            }
            if (i == 0)
            {
                return tempDM.Data[0];
            }

            return tempDM.Data[i - 1];
        }

        public DataPoint GetNextLowerPriceDataPoint(double price)
        {
            price = Math.Round(price, 2);
            DemandModel tempDM = new DemandModel(this.CommodityType);

            #region Sort by Price
            var tempList = new List<DataPoint>();

            foreach (DataPoint dp in Data)
            {
                tempList.Add(dp);
            }

            tempList.Sort(new DataPointPriceComparer());

            ObservableCollection<DataPoint> tempData = new ObservableCollection<DataPoint>();

            foreach (DataPoint dp in tempList)
            {
                tempData.Add(dp);
            }

            tempDM.Data = tempData;
            #endregion

            DataPoint tempDP = tempDM.GetPriceDataPoint(price);

            int i = tempDM.Data.IndexOf(tempDP);
            if (i == -1)
            {
                tempDP = tempDM.GetMinPriceDataPoint();
                if (price < tempDP.Price)
                {
                    return null;
                }

                foreach (DataPoint dp in tempDM.Data)
                {
                    if (dp.Price < price && dp.Price >= tempDP.Price)
                    {
                        tempDP = dp;
                    }
                }
                i = tempDM.Data.IndexOf(tempDP) + 1;
            }
            if (i == 0)
            {
                return tempDM.Data[0];
            }

            return tempDM.Data[i - 1];
        }


        public DataPoint GetNextHigherQtyDataPoint(DataPoint tempDP)
        {
            #region Sort by Qty
            DemandModel tempDM = new DemandModel(this.CommodityType);

            var tempList = new List<DataPoint>();

            foreach (DataPoint dp in Data)
            {
                tempList.Add(dp);
            }

            tempList.Sort(new DataPointQtyComparer()); //sorts by qty, lowest to highest, equal qties sort by price

            ObservableCollection<DataPoint> tempData = new ObservableCollection<DataPoint>();

            foreach (DataPoint dp in tempList)
            {
                tempData.Add(dp);
            }

            tempDM.Data = tempData;
            #endregion

            int i = tempDM.Data.IndexOf(tempDP);

            if (i + 1 == Data.Count)
            {
                return tempDP;
            }

            if (tempDM.ContainsDataPoint(tempDP))
            {
                return tempDM.Data[i + 1];
            }
            return null;
        }

        public DataPoint GetNextHigherPriceDataPoint(DataPoint tempDP)
        {
            DemandModel tempDM = new DemandModel(this.CommodityType);

            #region Sort by Price
            var tempList = new List<DataPoint>();

            foreach (DataPoint dp in Data)
            {
                tempList.Add(dp);
            }

            tempList.Sort(new DataPointPriceComparer());

            ObservableCollection<DataPoint> tempData = new ObservableCollection<DataPoint>();

            foreach (DataPoint dp in tempList)
            {
                tempData.Add(dp);
            }

            tempDM.Data = tempData;
            #endregion

            int i = tempDM.Data.IndexOf(tempDP);

            if (i + 1 == Data.Count)
            {
                return tempDP;
            }

            if (tempDM.ContainsDataPoint(tempDP))
            {
                return tempDM.Data[i + 1];
            }
            return null;
        }

        public DataPoint GetNextLowerQtyDataPoint(DataPoint tempDP)
        {
            DemandModel tempDM = new DemandModel(this.CommodityType);

            #region Sort by Qty
            var tempList = new List<DataPoint>();

            foreach (DataPoint dp in Data)
            {
                tempList.Add(dp);
            }

            tempList.Sort(new DataPointQtyComparer()); //sorts by qty, lowest to highest, equal qties sort by price

            ObservableCollection<DataPoint> tempData = new ObservableCollection<DataPoint>();

            foreach (DataPoint dp in tempList)
            {
                tempData.Add(dp);
            }

            tempDM.Data = tempData;
            #endregion

            int i = tempDM.Data.IndexOf(tempDP);

            if (i == 0)
            {
                return tempDP;
            }

            if (tempDM.ContainsDataPoint(tempDP))
            {
                return tempDM.Data[i - 1];
            }
            return null;
        }

        public DataPoint GetNextLowerPriceDataPoint(DataPoint tempDP)
        {
            DemandModel tempDM = new DemandModel(this.CommodityType);

            #region Sort by Price
            var tempList = new List<DataPoint>();

            foreach (DataPoint dp in Data)
            {
                tempList.Add(dp);
            }

            tempList.Sort(new DataPointPriceComparer());

            ObservableCollection<DataPoint> tempData = new ObservableCollection<DataPoint>();

            foreach (DataPoint dp in tempList)
            {
                tempData.Add(dp);
            }

            tempDM.Data = tempData;
            #endregion

            int i = tempDM.Data.IndexOf(tempDP);

            if (i == 0)
            {
                return tempDP;
            }

            if (tempDM.ContainsDataPoint(tempDP))
            {
                return tempDM.Data[i - 1];
            }
            return null;
        }


        public void SortbyPrice()
        {
            var tempList = new List<DataPoint>();

            foreach (DataPoint dp in Data)
            {
                tempList.Add(dp);
            }

            tempList.Sort(new DataPointPriceComparer());

            ObservableCollection<DataPoint> tempData = new ObservableCollection<DataPoint>();

            foreach (DataPoint dp in tempList)
            {
                tempData.Add(dp);
            }

            Data = tempData;
        }

        public void SortbyQty()
        {
            var tempList = new List<DataPoint>();

            foreach (DataPoint dp in Data)
            {
                tempList.Add(dp);
            }

            tempList.Sort(new DataPointQtyComparer()); //sorts by qty, lowest to highest, equal qties sort by price

            ObservableCollection<DataPoint> tempData = new ObservableCollection<DataPoint>();

            foreach (DataPoint dp in tempList)
            {
                tempData.Add(dp);
            }

            Data = tempData;
        }


        public DataPoint EstimatePrice(double qty)
        {
            qty = Math.Round(qty, 4);
            
            if (Data.Count < 1 || Data == null)
            {
                return null;
            }
            if (Data.Count == 1)
            {
                return Data[0];
            }

            if (this.ContainsQty(qty))
            {
                return GetQtyDataPoint(qty);
            }

            //else
            //{
            //    return new DataPoint(price, 420);
            //}

            #region Sort by Qty
            DemandModel tempDM = new DemandModel(this.CommodityType);

            var tempList = new List<DataPoint>();

            foreach (DataPoint dp in Data)
            {
                tempList.Add(dp);
            }

            tempList.Sort(new DataPointQtyComparer()); //sorts by qty, lowest to highest, equal qties sort by price

            ObservableCollection<DataPoint> tempData = new ObservableCollection<DataPoint>();

            foreach (DataPoint dp in tempList)
            {
                tempData.Add(dp);
            }

            tempDM.Data = tempData;
            #endregion

            double slope = 0;
            double x1 = 0;
            double y1 = 0;
            double x2 = 0;
            double y2 = 0;

            DataPoint tempDP = tempDM.GetNextLowerQtyDataPoint(qty);

            if (tempDP == null)//qty is outside the range, too low
            {
                return null;

            }
            else
            {
                x1 = tempDP.Qty;
                y1 = tempDP.Price;
            }

            tempDP = tempDM.GetNextHigherQtyDataPoint(qty);
            if (tempDP == null) //qty is outside the range, too high
            {
                return null;
            }
            else
            {
                x2 = tempDP.Qty;
                y2 = tempDP.Price;
            }

            slope = (y2 - y1) / (x2 - x1);

            ////y=mx+b
            ////y - b = mx
            ////-b = -1(mx - y)

            double b = y1 - slope * x1;

            double price = slope * qty + b;

            price = Math.Round(price, 2);

            if (price < 0)
            {
                return null;
            }
            else
            {
                return new DataPoint(price, qty);
            }
        }

        public DataPoint EstimateQty(double price)
        {
            price = Math.Round(price, 2);
            if (Data.Count < 1 || Data == null)
            {
                return null;
            }
            if (Data.Count == 1)
            {
                return Data[0];
            }

            if (this.ContainsPrice(price))
            {
                return GetPriceDataPoint(price);
            }

            //else
            //{
            //    return new DataPoint(price, 420);
            //}

            #region Sort by Price

            DemandModel tempDM = new DemandModel(this.CommodityType);

            var tempList = new List<DataPoint>();

            foreach (DataPoint dp in Data)
            {
                tempList.Add(dp);
            }

            tempList.Sort(new DataPointPriceComparer());

            ObservableCollection<DataPoint> tempData = new ObservableCollection<DataPoint>();

            foreach (DataPoint dp in tempList)
            {
                tempData.Add(dp);
            }

            tempDM.Data = tempData;
            #endregion

            double slope = 0;
            double x1 = 0;
            double y1 = 0;
            double x2 = 0;
            double y2 = 0;

            DataPoint tempDP = tempDM.GetNextLowerPriceDataPoint(price);

            if (tempDP == null)//qty is outside the range, too low
            {
                return null;
                
            }
            else 
            {
                x1 = tempDP.Price;
                y1 = tempDP.Qty;
            }

            tempDP = tempDM.GetNextHigherPriceDataPoint(price);
            if (tempDP == null) //qty is outside the range, too high
            {
                return null;  
            }
            else 
            {
                x2 = tempDP.Price;
                y2 = tempDP.Qty;
            }

            slope = (y2 - y1) / (x2 - x1);

            ////y=mx+b
            ////y - b = mx
            ////-b = -1(mx - y)

            double b = y1 - slope * x1;

            double qty = slope * price + b;

            qty = Math.Round(qty, 4);

            if (qty < 0)
            {
                return null;
            }
            else
            {
                return new DataPoint(price, qty);
            }
        }


        private void RaiseEvents()
        {
            RaisePropertyChanged("SelectedDataPoint"); 
            RaisePropertyChanged("SelectedDataPointNextPrice"); 
            RaisePropertyChanged("SelectedDataPointPrevPrice");
            RaisePropertyChanged("SelectedDataPointNextQty");
            RaisePropertyChanged("SelectedDataPointPrevQty");
            RaisePropertyChanged("LinearEstQty");
            RaisePropertyChanged("LinearEstPrice"); 
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
