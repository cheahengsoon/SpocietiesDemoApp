using System.ComponentModel;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace Spocieties
{
    public class Organization : INotifyPropertyChanged
    {
        readonly double DefaultWage = .6;
        private const int displayNameMaxChars = 6; //DisplayName is truncated to 6 characters, which is the maximum that will fit into the Control which represents the Org on the view Canvas (i.e., the ViewModel is sort-of accomodating the view here.) 
        private const int stdShift = 16;
        readonly int maxDepth = 7; //The maximum number of times that AddandExport can be called recursively, to prevent supder long lags between Time Steps


        #region Basic Properties: Name, Location, InstType, etc.

        private string _name;
        public string Name { get { return _name; } set { if (_name != value) { _name = value; RaisePropertyChanged("Name"); } } }

        public string DisplayName { get { if (Name != null) { return Name.Length <= displayNameMaxChars ? Name : Name.Substring(0, displayNameMaxChars); } else { return null; } } }

        private InstitutionType _instType;
        public InstitutionType InstType { get { return _instType; } set { if (_instType != value) { _instType = value; RaisePropertyChanged("InstType"); } } }

        private double _x;
        public double X { get { return (_x); } set { if (_x != value) { _x = value; RaisePropertyChanged("X"); } } }
        private double _y;
        public double Y { get { return (_y); } set { if (_y != value) { _y = value; RaisePropertyChanged("Y"); } } }

        public double DispX { get { return (_x - 20); } } ////Offsets Hardcoded to make Symbol appear centered on Location
        public double DispY { get { return (_y - 24); } }

        private int _depth;
        #endregion

        #region Collections

        private DemandModelColl _demandModels;
        public DemandModelColl DemandModels { get { return _demandModels; } set { if (_demandModels != value) { _demandModels = value; RaisePropertyChanged("DemandModels"); } } }

        private DemandModel _selectedDemandModel;
        public DemandModel SelectedDemandModel { get { return _selectedDemandModel; } set { if (_selectedDemandModel != value) { _selectedDemandModel = value; RaisePropertyChanged("SelectedDemandModel"); } } }

        private Inventory _endGoals;
        public Inventory EndGoals { get { return _endGoals; } set { if (_endGoals != value) { _endGoals = value; RaisePropertyChanged("EndGoals"); } } }

        private Inventory _inventory;
        public Inventory Inventory { get { return _inventory; } set { if (_inventory != value) { _inventory = value; RaisePropertyChanged("Inventory"); } } }

        private ObservableCollection<Behavior> _offeredBhvrs;
        public ObservableCollection<Behavior> OfferedBhvrs { get { return _offeredBhvrs; } set { if (_offeredBhvrs != value) { _offeredBhvrs = value; RaisePropertyChanged("OfferedBhvrs"); } } }

        private BhvrSeqColl _bhvrSeqs;
        public BhvrSeqColl BhvrSeqs { get { return _bhvrSeqs; } set { if (_bhvrSeqs != value) { _bhvrSeqs = value; RaisePropertyChanged("BhvrSeqs"); } } }

        private BhvrSeq _chosenBhvrSeq;
        public BhvrSeq ChosenBhvrSeq { get { return _chosenBhvrSeq; } set { if (_chosenBhvrSeq != value) { _chosenBhvrSeq = value; RaisePropertyChanged("ChosenBhvrSeq"); } } }

        //private ObservableCollection<Technology> _techs;
        //public ObservableCollection<Technology> Techs { get { return _techs; } set { if (_techs != value) { _techs = value; RaisePropertyChanged("Techs"); } } }

        #endregion

        #region Production Information
        private CommodityType _productionOutputCommType;
        public CommodityType ProductionOutputCommType { get { return _productionOutputCommType; } set { if (_productionOutputCommType != value) { _productionOutputCommType = value; RaisePropertyChanged("ProductionOutputCommType"); } } }

        private double? _expPrice;
        public double? ExpPrice { get { return _expPrice; } set { if (_expPrice != value) { _expPrice = Math.Round((double)value, 2); RaisePropertyChanged("ExpPrice"); } } }
        private double? _qtyExpToSell;
        public double? QtyExpToSell { get { return _qtyExpToSell; } set { if (_qtyExpToSell != value) { _qtyExpToSell = value; RaisePropertyChanged("QtyExpToSell"); } } }
        private double? _qtyMCEqualsPrice;
        public double? QtyMCEqualsPrice { get { return _qtyMCEqualsPrice; } set { if (_qtyMCEqualsPrice != value) { _qtyMCEqualsPrice = value; RaisePropertyChanged("QtyMCEqualsPrice"); } } }
        private double? _totalCost;
        public double? TotalCost { get { return _totalCost; } set { if (_totalCost != value) { _totalCost = Math.Round((double)value, 2); RaisePropertyChanged("TotalCost"); } } }

        //private double? _avgTotalCost;
        //public double? AvgTotalCost { get { if (_amttoProduce == null || _amttoProduce == 0 || _totalCost == null) { return null; } return Math.Round((double)(_totalCost / _amttoProduce), 2); } set { if (_avgTotalCost != value) { _avgTotalCost = value; RaisePropertyChanged("AvgTotalCost"); } } }

        private double? _marginalCost;
        public double? MarginalCost { get { return _marginalCost; } set { if (_marginalCost != value) { _marginalCost = Math.Round((double)value, 2); RaisePropertyChanged("MarginalCost"); } } }

        private double? _expPriceOfLabor;
        public double? ExpPriceOfLabor { get { return _expPriceOfLabor; } set { if (_expPriceOfLabor != value) { _expPriceOfLabor = Math.Round((double)value, 2); RaisePropertyChanged("ExpPriceOfLabor"); } } }
        private double? _expPriceOfInputs;
        public double? ExpPriceOfInputs { get { return _expPriceOfInputs; } set { if (_expPriceOfInputs != value) { _expPriceOfInputs = Math.Round((double)value, 2); RaisePropertyChanged("ExpPriceOfInputs"); } } }
        private double? _expQtyOfLabor;
        public double? ExpQtyOfLabor { get { return _expQtyOfLabor; } set { if (_expQtyOfLabor != value) { _expQtyOfLabor = value; RaisePropertyChanged("ExpQtyOfLabor"); } } }
        private double? _expQtyOfInputs;
        public double? ExpQtyOfInputs { get { return _expQtyOfInputs; } set { if (_expQtyOfInputs != value) { _expQtyOfInputs = value; RaisePropertyChanged("ExpQtyOfInputs"); } } }
        private double? _amttoProduce;
        public double? AmttoProduce { get { return _amttoProduce; } set { if (_amttoProduce != value) { _amttoProduce = value; RaisePropertyChanged("AmttoProduce"); RaisePropertyChanged("AvgTotalCost"); } } }

        #endregion

        private bool EndGoalsSatisfied { get { if (Inventory.InvHasAmt(EndGoals)) { return true; } else return false; } }

        private Inventory _endGoalsNotSatisfied;
        public Inventory EndGoalsNotSatisfied { get { _endGoalsNotSatisfied = new Inventory(); foreach (Asset a in EndGoals) { if (!Inventory.InvHasAmt(a)) { _endGoalsNotSatisfied.InvAdd(a); } } return _endGoalsNotSatisfied; } }

        private Technology _tech;
        public Technology Tech { get { return _tech; } set { if (_tech != value) { _tech = value; RaisePropertyChanged("Tech"); } } }

        #region Constructors

        public Organization()
        {
            Name = null;
            Inventory = new Inventory();
            OfferedBhvrs = new ObservableCollection<Behavior>();
            //Techs = new ObservableCollection<Technology>();
            DemandModels = new DemandModelColl();
            EndGoals = new Inventory();
            //this.Fluff(); ////Add a basic Behavior
        }

        public Organization(string s, InstitutionType it)
        {
            Name = s;
            InstType = it;
            Inventory = new Inventory();
            OfferedBhvrs = new ObservableCollection<Behavior>();
            //Techs = new ObservableCollection<Technology>();
            DemandModels = new DemandModelColl();
            EndGoals = new Inventory();
        }

        public Organization(string s, InstitutionType it, Inventory i)
        {
            Name = s;
            InstType = it;
            Inventory = new Inventory(i);
            OfferedBhvrs = new ObservableCollection<Behavior>();
            //Techs = new ObservableCollection<Technology>();
            DemandModels = new DemandModelColl();
            EndGoals = new Inventory();
        }

        #endregion

        #region Methods

        public void TimeStep(ObservableCollection<Behavior> bhvrs, ObservableDictionary<string, Organization> orgs)
        {
            if (Inventory.InvHas("Labor Hours"))
            {
                Inventory.GetAsset("Labor Hours").Amount = 0;
            }

            if (Tech != null && Tech.Inputs != null && Tech.Outputs != null)
            {
                GenOfferedBhvrs(bhvrs);
                if (AmttoProduce != null)
                {
                    AcquireInputsOnMarket(orgs);
                    Produce();
                }
            }
            CheckBhvrsForAvailability();
            ResetDemandModels(bhvrs);
        }

        public void CheckBhvrsForAvailability() //Set Bhvrs to unavailable if inventory is insufficient to meet obligations
        {
            foreach (Behavior b in OfferedBhvrs)
            {
                //Make sure Inventory is sufficient to meet obligations for behaviors, set to unavailable if not
                if (!Inventory.InvHasAmt(b.Outputs))
                {
                    b.Available = false;
                }
                else if (Inventory.InvHasAmt(b.Outputs))
                {
                    b.Available = true;
                }

                //Check to see if EndGoals are satisfied and set Available to false if Input not needed
                if (Tech != null)
                {
                    foreach (Asset a in b.Inputs)
                    {
                        if (a.CommodityType.Name == "Money")//Money is always welcome
                        {
                            continue;
                        }
                        if (!EndGoalsNotSatisfied.InvHas(a))//the asset is not needed to fulfill endgoals
                        {
                            b.Available = false;
                        }
                        else
                        {
                            b.Available = true;
                            continue;
                        }
                    }
                }
            }
        }


        #region DemandModel Methods

        public void ResetDemandModels(ObservableCollection<Behavior> bhvrs)
        {
            DemandModels.Populate(OfferedBhvrs);

            if ((DemandModels == null || DemandModels.Count == 0) || (OfferedBhvrs == null || OfferedBhvrs.Count == 0))
            {
                return;
            }

            #region Set quantities to 0 for Bhvrs that are available

            foreach (Behavior b in OfferedBhvrs)
            {
                if (b.Available == true)
                {
                    if (b.Buying == true)
                    {
                        double price = b.Outputs.GetAsset("Money").Amount;
                        double qty = b.Inputs[0].Amount;
                        price /= qty;
                        DemandModel dm = DemandModels.GetDemandModel(b.Inputs[0].CommodityType);
                        if (dm.ContainsPrice(price))
                        {
                            dm.GetPriceDataPoint(price).Qty = 0;
                        }
                    }

                    else if (b.Buying == false)
                    {
                        double price = b.Inputs.GetAsset("Money").Amount;
                        double qty = b.Outputs[0].Amount;
                        price /= qty;
                        DemandModel dm = DemandModels.GetDemandModel(b.Outputs[0].CommodityType);
                        if (dm.ContainsPrice(price))
                        {
                            dm.GetPriceDataPoint(price).Qty = 0;
                        }
                    }

                    else if (b.Buying == null)
                    {
                        continue;
                    }
                }
            }
            #endregion
        }

        private void UpdateDemandModel(Behavior b)
        {
            if (DemandModels.Count == 0 || b.Buying == null)
            {
                return;
            }

            if (b.Buying == true)
            {
                //Get per unit price
                double price = b.Outputs.GetAsset("Money").Amount;
                double qty = b.Inputs[0].Amount;
                price /= qty;
                //Increment the qty in the DemandModel entry for the given price
                DemandModels.GetDemandModel(b.Inputs[0].CommodityType).Increment(price, qty);
            }

            else if (b.Buying == false)
            {
                //Get per unit price
                double price = b.Inputs.GetAsset("Money").Amount;
                double qty = b.Outputs[0].Amount;
                price /= qty;
                //Increment the qty in the DemandModel entry for the given price
                DemandModels.GetDemandModel(b.Outputs[0].CommodityType).Increment(price, qty);
            }
        }

        private double? GetAvgMktPrice(ObservableCollection<Behavior> bhvrs, CommodityType commtype)
        {
            double i = 0;
            //double n = 0;
            double counter = 0;

            foreach (Behavior b in bhvrs)
            {
                if (b.Outputs.InvHas(commtype) && b.Inputs.InvHas("Money"))
                {
                    i += b.Inputs.GetAsset("Money").Amount / b.Outputs.GetAsset(commtype).Amount;
                    //n += b.Outputs.GetAsset(commtype).Amount;
                    counter++;
                }

                else if (b.Inputs.InvHas(commtype) && b.Outputs.InvHas("Money"))
                {
                    i += b.Outputs.GetAsset("Money").Amount / b.Inputs.GetAsset(commtype).Amount;
                    //n += b.Inputs.GetAsset(commtype).Amount;
                    counter++;
                }
            }

            if (counter == 0)
            {
                return null;
            }
            else
            {
                return i / counter;
            }
        }

        public bool Try(Agent a, Behavior b)
        {
            DemandModels.Update(b);

            RaisePropertyChanged("DemandModels");

            CheckBhvrsForAvailability();

            if (b.Available == false)
            {
                return false;
            }

            else
            {
                a.Inventory.InvAppBhvr(b);
                Inventory.InvAppOfferedBhvr(b);
                return true;
            }
        }

        public bool Try(Organization o, Behavior b)
        {
            DemandModels.Update(b);

            RaisePropertyChanged("DemandModels");

            CheckBhvrsForAvailability();

            if (b.Available == false)
            {
                return false;
            }

            else
            {
                o.Inventory.InvAppBhvr(b);
                Inventory.InvAppOfferedBhvr(b);
                return true;
            }
        }

        #endregion


        #region Production Methods

        #region Get Prices and Quantities
        private double? GetExpPurchasePrice(ObservableCollection<Behavior> bhvrs, CommodityType ct)
        {
            return GetAvgMktPrice(bhvrs, ct);
        }

        private double? GetExpSellPrice(ObservableCollection<Behavior> bhvrs, CommodityType ct)
        {
            //First, check the demand model
            DemandModel localDemandModel = DemandModels.GetDemandModel(ct);

            if (localDemandModel != null && localDemandModel.Data.Count > 0)
            {
                DataPoint tempDP = localDemandModel.GetMaxRevenueDataPoint();
                if (tempDP != null && tempDP.Qty > 0)
                {
                    return tempDP.Price;
                }
            }
            //Otherwise, return the avg market price
            return ct.MktPrice;
        }

        private double? GetExpOffertoPurchasePrice(ObservableCollection<Behavior> bhvrs, CommodityType ct, double qty)
        {
            DemandModel tempDM = DemandModels.GetDemandModel(ct);
            if (tempDM != null)
            {
                DataPoint tempDP = tempDM.EstimatePrice(qty);
                if (tempDP != null)
                {
                    return tempDP.Price;
                }
            }
            return ct.MktPrice;
        }

        private double GetLaborHoursReqd(Technology tech, double qty)
        {
            return Math.Pow((qty / (tech.Inputs[1].Amount * tech.Multiplier)), 2) * tech.Inputs[0].Amount;
        }

        private double? GetWageforQty(double qty, ObservableCollection<Behavior> bhvrs)
        {
            DemandModel laborDemandModel = DemandModels.GetDemandModel("Labor Hours");
            double? wage = null;
            if (laborDemandModel == null)
            {
                wage = GetAvgMktPrice(bhvrs, laborDemandModel.CommodityType);
            }
            else
            {
                DataPoint dp = laborDemandModel.EstimateQty(qty);
                if (dp == null)
                {
                    return null;
                }
                else
                {
                    wage = laborDemandModel.EstimatePrice(qty).Price;
                }
            }
            return wage;
        }

        private double GetRawMaterialsReqd(double qty)
        {
            return qty / Tech.Multiplier;
        }

        private double GetCostofProduction(double wage, double laborHours, double outputQty, double multiplier, double rawMaterialCost)
        {
            return wage * laborHours + outputQty / multiplier * rawMaterialCost;
        }

        private double? GetTotalCost(ObservableCollection<Behavior> bhvrs, double qty)
        {
            #region Calculate required labor hours and corresponding wage reqd to attract sufficient labor
            ExpQtyOfLabor = GetLaborHoursReqd(Tech, qty);

            ExpPriceOfLabor = GetExpOffertoPurchasePrice(bhvrs, Tech.Inputs[0].CommodityType, (double)ExpQtyOfLabor);
            //if Wage == null, use a default wage
            if (ExpPriceOfLabor == null)
            {
                ExpPriceOfLabor = DefaultWage;
            }
            #endregion

            #region Calculate required quantity of raw inputs and the expected purchase price
            ExpQtyOfInputs = GetRawMaterialsReqd(qty);

            if (Inventory.InvHas(Tech.Inputs[1].CommodityType))
            {
                ExpQtyOfInputs -= Inventory.GetAsset(Tech.Inputs[1].CommodityType).Amount;
                if (ExpQtyOfInputs < 0)
                {
                    ExpQtyOfInputs = 0;
                }
            }

            ExpPriceOfInputs = GetExpPurchasePrice(bhvrs, Tech.Inputs[1].CommodityType);

            if (ExpPriceOfInputs == null)
            {
                return null;
            }
            #endregion

            //TODO: double check that Total Costs are accurate
            return GetCostofProduction((double)ExpPriceOfLabor, (double)ExpQtyOfLabor, (double)qty, Tech.Multiplier, (double)ExpPriceOfInputs);
        }

        private void GetQtytoProduce(ObservableCollection<Behavior> bhvrs)
        {
            if (Tech == null)
            {
                return;
            }

            if (Tech.Inputs[1].CommodityType.MktPrice == null)// if the raw material input is not expected to be available for purchase, i.e. none are currently being sold
            {
                AmttoProduce = null;
                return;
            }

            #region Estimate the price at which the output will sell
            ProductionOutputCommType = Tech.Outputs[0].CommodityType;

            ExpPrice = GetExpSellPrice(bhvrs, ProductionOutputCommType);

            //if price == null, use tech Default output qty
            if (ExpPrice == null)
            {
                AmttoProduce = Tech.Outputs[0].Amount;
            }
            #endregion

            #region Estimate the quantity demanded at that price
            DemandModel localDemandModel = DemandModels.GetDemandModel(ProductionOutputCommType);
            if (localDemandModel != null)
            {
                DataPoint tempDP = localDemandModel.EstimateQty((double)ExpPrice);
                if (tempDP != null)
                {
                    QtyExpToSell = tempDP.Qty;
                }
                else
                {
                    AmttoProduce = Tech.Outputs[0].Amount;
                }
            }
            else
            {
                AmttoProduce = Tech.Outputs[0].Amount;
            }
            #endregion

            AmttoProduce = QtyExpToSell;

            #region Find the total cost of producing that quantity

            double? TC1 = GetTotalCost(bhvrs, (double)AmttoProduce);

            double? TC2 = GetTotalCost(bhvrs, (double)AmttoProduce - 1);

            TotalCost = TC1;
            MarginalCost = TC1 - TC2;
            #endregion

            if (MarginalCost > ExpPrice)
            {
                AmttoProduce -= 1;
                while (MarginalCost > ExpPrice)
                {
                    TC1 = TC2;
                    AmttoProduce -= 1;
                    TC2 = GetTotalCost(bhvrs, (double)AmttoProduce);
                    MarginalCost = TC1 - TC2;
                    TotalCost = TC1;
                }
                AmttoProduce += 1;
            }
            GetTotalCost(bhvrs, (double)AmttoProduce);
        }

        #endregion

        private void Produce()
        {
            if (ExpQtyOfLabor == null || ExpQtyOfInputs == null)
            {
                ExpQtyOfLabor = Tech.Inputs[0].Amount;
                Asset _tempAsset = Inventory.GetAsset(Tech.Inputs[1].CommodityType);
                if (_tempAsset != null)
                {
                    ExpQtyOfInputs = Tech.Inputs[1].Amount - _tempAsset.Amount;
                }
                else if (_tempAsset == null)
                {
                    ExpQtyOfInputs = Tech.Inputs[1].Amount;
                }
            }
            if (Inventory.InvSubtract(Tech.Inputs[0].CommodityType, (double)ExpQtyOfLabor) == true && Inventory.InvSubtract(Tech.Inputs[1].CommodityType, (double)ExpQtyOfInputs) == true)
            {
                Inventory.InvAdd(new Asset(Tech.Outputs[0].CommodityType, (double)AmttoProduce));
                return;
            }
        }

        #endregion


        #region Trading and Offered Bhvr Methods

        private void PopulateEndGoals()
        {
            EndGoals.Clear();
            if (ExpQtyOfLabor != null && ExpQtyOfLabor > 0)
            {
                EndGoals.InvAdd(new Asset(Tech.Inputs[0].CommodityType, (double)ExpQtyOfLabor));
            }
            if (ExpQtyOfInputs != null && ExpQtyOfInputs > 0)
            {
                EndGoals.InvAdd(new Asset(Tech.Inputs[1].CommodityType, (double)ExpQtyOfInputs));
            }
        }

        private void AcquireInputsOnMarket(ObservableDictionary<string, Organization> orgs)
        {
            ExecuteBhvrs(orgs);
        }

        public void GenOfferedBhvrs(ObservableCollection<Behavior> bhvrs)
        {
            GetQtytoProduce(bhvrs);
            if (AmttoProduce == null) // Raw Material inputs are not available
            {
                return; //Produce nothing
            }

            PopulateEndGoals();

            //OfferedBhvrs.Clear();

            //#region Add the "Work" behavior
            //Behavior _tempBhvr = new Behavior();
            //_tempBhvr.Inputs.Add(new Asset(Tech.Inputs[0].CommodityType, stdShift));

            //if (ExpPriceOfLabor == null)
            //{
            //    ExpPriceOfLabor = DefaultWage;
            //}
            //_tempBhvr.Outputs.Add(new Asset(Inventory.GetAsset("Money").CommodityType, (double)ExpPriceOfLabor * stdShift));
            //_tempBhvr.GetName();
            //OfferedBhvrs.Add(_tempBhvr);
            //#endregion



            //#region Add the "Sell Product" behavior
            //_tempBhvr = new Behavior();
            //if (ExpPrice == null)
            //{
            //    ExpPrice = MarginalCost;
            //}

            //_tempBhvr.Inputs.Add(new Asset(Inventory.GetAsset("Money").CommodityType, (double)ExpPrice));

            //_tempBhvr.Outputs.Add(new Asset(Tech.Outputs[0].CommodityType, 1));
            //_tempBhvr.GetName();
            //OfferedBhvrs.Add(_tempBhvr);
            //#endregion
        }

        #endregion


        #region BhvrSeq Methods
        private void GenBhvrSeqs(ObservableDictionary<string, Organization> orgs)
        {
            if (EndGoals == null || EndGoals.Count == 0)
            {
                return;
            }
            _depth = 0;
            BhvrSeqs = new BhvrSeqColl();
            List<Behavior> bList = new List<Behavior>();
            foreach (Organization o in orgs.Values)
            {
                foreach (Behavior b in o.OfferedBhvrs)
                {
                    if (b.Available == true)
                    {
                        bList.Add(b);
                    }
                }
            }

            BhvrSeqColl _BSC = new BhvrSeqColl();

            #region Seed BSC with BhvrSeqs containing Bhvrs possible with current inventory
            foreach (Behavior b in bList)
            {
                if (_inventory.InvHasAmt(b.Inputs))
                {
                    BhvrSeq bs = new BhvrSeq();
                    bs.Inventory = new Inventory(_inventory);
                    bs.BsAdd(b);
                    _BSC.Add(bs);
                    BhvrSeqs.Add(bs);
                }
            }
            #endregion

            #region/////////////////////  Recursively add Behaviors to BhvrSeqs
            foreach (BhvrSeq bs in _BSC)
            {
                AddandExport(bs, orgs);
            }

            _BSC.Clear();
            #endregion

            #region/////////////////////  Remove BhvrSeqs that do not result in endgoal acquisition
            foreach (BhvrSeq bs in BhvrSeqs)
            {
                if (!bs.Inventory.InvHasAmt(EndGoals))
                {
                    _BSC.Add(bs);
                }
            }

            foreach (BhvrSeq bs in _BSC)
            {
                BhvrSeqs.Remove(bs);
            }
            #endregion
        }

        private void AddandExport(BhvrSeq bs, ObservableDictionary<string, Organization> orgs)
        {
            _depth++;
            if (_depth >= maxDepth)
            {
                return;
            }
            BhvrSeqColl BSC = new BhvrSeqColl();
            List<Behavior> bList = new List<Behavior>();

            foreach (Organization o in orgs.Values)
            {
                foreach (Behavior b in o.OfferedBhvrs)
                {
                    if (b.Available == true)
                    {
                        bList.Add(b);
                    }
                }
            }

            List<Behavior> NextPossibles = bs.NextPossBhvrs(bList);

            if (NextPossibles.Count == 0 || bs.Inventory.InvHasAmt(EndGoals)) // if there are no possible next bhvrs or if endgoal is satisfied
            {
                return;
            }

            foreach (Behavior b in NextPossibles) //// Create a new BhvrSeq for each possible next bhvr
            {
                BhvrSeq _bs = new BhvrSeq(bs);
                _bs.BsAdd(b);
                BSC.Add(_bs);
                BhvrSeqs.Add(_bs);
            }

            if (bs.Count > 1)
            {
                BhvrSeqs.Add(bs);
            }

            foreach (BhvrSeq _bs in BSC)
            {
                AddandExport(_bs, orgs);
            }
        }

        private void AssignBhvrSeqNumbers()
        {
            if (BhvrSeqs == null || this.BhvrSeqs.Count == 0)
            {
                return;
            }

            #region ///////////////////// Assign Numbers to the BhvrSeqs
            foreach (BhvrSeq bs in BhvrSeqs)
            {
                bs.CurrentIndex = BhvrSeqs.IndexOf(bs) + 1;
            }

            #endregion
        }

        private void SelectBhvrSeq()
        {
            if (BhvrSeqs == null || this.BhvrSeqs.Count == 0)
            {
                return;
            }

            double storedTotalAmt = 0;

            foreach (BhvrSeq bs in BhvrSeqs)
            {

                double totalAmt = bs.Inventory.SumOfAssetAmts();

                if (totalAmt > storedTotalAmt) /// if the BhvrSeq Inventory has a higher total of all assets....
                {
                    ChosenBhvrSeq = bs;
                    //_selectedBhvrSeqIndex = BhvrSeqs.IndexOf(bs); /// then store its index
                    storedTotalAmt = totalAmt;
                }
            }
            ChosenBhvrSeq.IsChosen = true;
        }

        private void ExecuteBhvrs(ObservableDictionary<string, Organization> orgs)
        {
            if (BhvrSeqs == null || this.BhvrSeqs.Count == 0)
            {
                return;
            }

            bool done = false;

            while (done == false)
            {
                GenBhvrSeqs(orgs);
                if (BhvrSeqs.Count == 0)
                {
                    return;
                }

                if (BhvrSeqs.Count == 1)
                {
                    ChosenBhvrSeq = BhvrSeqs[0];
                }

                BhvrSeqs.RemoveEqualInventories();
                AssignBhvrSeqNumbers();
                SelectBhvrSeq();

                if (ChosenBhvrSeq.Count == 0 || ChosenBhvrSeq == null)
                {
                    return;
                }

                foreach (Behavior b in ChosenBhvrSeq)
                {
                    if (b.OwningOrg == null)
                    {
                        Inventory.InvAppBhvr(b);
                    }

                    else if (b.OwningOrg.Try(this, b) == false)
                    {
                        done = false;
                        break;
                    }
                    done = true;
                }
            }
        }

        #endregion

        #endregion


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
