using System.ComponentModel;
using System.Threading;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using System.Windows;


namespace Spocieties
{
    public class Agent : INotifyPropertyChanged
    {

        #region Properties
        private static string[] sampleNames = { "ADITI", "ADITYA", "AGNI", "ANANTA", "ANIL", "ANIRUDDHA", "ARJUNA", "ARUNA", "ARUNDHATI", "BALA", "BALADEVA", "BHARATA", "BHASKARA", "BRAHMA", "BRIJESHA", "CHANDRA", "DAMAYANTI", "DAMODARA", "DEVARAJA", "DEVI", "DILIPA", "DIPAKA", "DRAUPADI", "DRUPADA", "DURGA", "GANESHA", "GAURI", "GIRISHA", "GOPALA", "GOPINATHA", "GOTAMA", "GOVINDA", "HARI", "HARISHA", "INDIRA", "INDRA", "INDRAJIT", "INDRANI", "JAGANNATHA", "JAYA", "JAYANTI", "KALI", "KALYANI", "KAMA", "KAMALA", "KANTI", "KAPILA", "KARNA", "KRISHNA", "KUMARA", "KUMARI", "LAKSHMANA", "LAKSHMI", "LALITA", "MADHAVA", "MADHAVI", "MAHESHA", "MANI", "MANU", "MAYA", "MINA", "MOHANA", "MOHINI", "MUKESHA", "MURALI", "NALA", "NANDA", "NARAYANA", "PADMA", "PADMAVATI", "PANKAJA", "PARTHA", "PARVATI", "PITAMBARA", "PRABHU", "PRAMODA", "PRITHA", "PRIYA", "PURUSHOTTAMA", "RADHA", "RAGHU", "RAJANI", "RAMA", "RAMACHANDRA", "RAMESHA", "RATI", "RAVI", "REVA", "RUKMINI", "SACHIN", "SANDHYA", "SANJAYA", "SARASWATI", "SATI", "SAVITR", "SAVITRI", "SHAILAJA", "SHAKTI", "SHANKARA", "SHANTA", "SHANTANU", "SHIVA", "SHIVALI", "SHRI", "SHRIPATI", "SHYAMA", "SITA", "SRI", "SUMATI", "SUNDARA", "SUNITA", "SURESHA", "SURYA", "SUSHILA", "TARA", "UMA", "USHA", "USHAS", "VALLI", "VASANTA", "VASU", "VIDYA", "VIJAYA", "VIKRAMA", "VISHNU", "YAMA", "YAMI" };
        
        private MarkovNameGenerator NameGen;

        private string _name;
        public string Name { get { return _name; } set { if (_name != value) { _name = value; RaisePropertyChanged("Name"); } } }

        private Inventory _inventory;
        public Inventory Inventory { get { return _inventory; } set { if (_inventory != value) { _inventory = value; RaisePropertyChanged("Inventory"); } } }

        private ObservableCollection<Behavior> _offeredBhvrs;
        public ObservableCollection<Behavior> OfferedBhvrs { get { return _offeredBhvrs; } set { if (_offeredBhvrs != value) { _offeredBhvrs = value; RaisePropertyChanged("OfferedBhvrs"); } } }

        private BhvrSeqColl _bhvrSeqColl;
        public BhvrSeqColl BhvrSeqColl { get { return _bhvrSeqColl; } set { if (_bhvrSeqColl != value) { _bhvrSeqColl = value; RaisePropertyChanged("BhvrSeqColl"); } } }

        private ObservableCollection<Asset> _endGoals;
        public ObservableCollection<Asset> EndGoals { get { return _endGoals; } set { if (_endGoals != value) { _endGoals = value; RaisePropertyChanged("EndGoals"); } } }

        private BhvrSeq _chosenBhvrSeq;
        public BhvrSeq ChosenBhvrSeq { get { return _chosenBhvrSeq; } set { if (_chosenBhvrSeq != value) { _chosenBhvrSeq = value; RaisePropertyChanged("ChosenBhvrSeq"); } } }

        #endregion

        public Agent()
        {
            NameGen = new MarkovNameGenerator(sampleNames, 1, 4);
            Name = NameGen.NextName;
            //Sleep not necessary for demo app, only one agent being created at a time
            //Thread.Sleep(15); //Sleep or else NameGen returns duplicate names
            Inventory = new Inventory();
            OfferedBhvrs = new ObservableCollection<Behavior>();
            BhvrSeqColl = new BhvrSeqColl();
            EndGoals = new ObservableCollection<Asset>();
            ChosenBhvrSeq = new BhvrSeq();
        }

        public void TimeStep(ObservableDictionary<string, Organization> orgs)
        {
            Inventory.GetAsset("Labor Hours").Amount = 16;

            GenBhvrSeqs(orgs);
            BhvrSeqColl.RemoveEqualInventories();
            AssignBhvrSeqNumbers();
            SelectBhvrSeq();
            ExecuteBhvrs(orgs);
        }


        public bool SurvivalCheck()
        {
            if (Inventory.InvHasAmt(EndGoals))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        #region                            Behavior Selection Algorithm

        public void GenBhvrSeqs(ObservableDictionary<string, Organization> orgs)
        {
            BhvrSeqColl = new BhvrSeqColl();
            List<Behavior> bList = new List<Behavior>();
            foreach (Organization o in orgs.Values)
            {
                //Distance not used in demo app
                //double dist = Math.Sqrt(Math.Pow((o.X - X), 2) + Math.Pow((o.Y - Y), 2));
                //if (dist <= MaxTravelDist)
                //{
                    foreach (Behavior b in o.OfferedBhvrs)
                    {
                        if (b.Available == true)
                        {
                            bList.Add(b);
                        }
                    }
                //}
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
                    BhvrSeqColl.Add(bs);
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
            foreach (BhvrSeq bs in BhvrSeqColl)
            {
                if (!bs.Inventory.InvHasAmt(EndGoals))
                {
                    _BSC.Add(bs);
                }
            }

            foreach (BhvrSeq bs in _BSC)
            {
                BhvrSeqColl.Remove(bs);
            }
            #endregion
        }

        private void AddandExport(BhvrSeq bs, ObservableDictionary<string, Organization> orgs)
        {
            BhvrSeqColl BSC = new BhvrSeqColl();
            List<Behavior> bList = new List<Behavior>();

            foreach (Organization o in orgs.Values)
            {
                //Distance not used in demo app
                //double dist = Math.Sqrt(Math.Pow((o.X - X), 2) + Math.Pow((o.Y - Y), 2));
                //if (dist <= MaxTravelDist)
                //{
                    foreach (Behavior b in o.OfferedBhvrs)
                    {
                        if (b.Available == true)
                        {
                            bList.Add(b);
                        }
                    }
                //}
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
                BhvrSeqColl.Add(_bs);
            }

            if (bs.Count > 1)
            {
                BhvrSeqColl.Add(bs);
            }

            foreach (BhvrSeq _bs in BSC)
            {
                AddandExport(_bs, orgs);
            }
        }

        private void AssignBhvrSeqNumbers()
        {
            #region ///////////////////// Assign Numbers to the BhvrSeqs
            foreach (BhvrSeq bs in BhvrSeqColl)
            {
                bs.CurrentIndex = BhvrSeqColl.IndexOf(bs) + 1;
            }

            #endregion
        }

        public void SelectBhvrSeq()
        {
            if (BhvrSeqColl == null || this.BhvrSeqColl.Count == 0)
            {
                return;
            }

            double storedTotalAmt = 0;

            foreach (BhvrSeq bs in BhvrSeqColl)
            {

                double totalAmt = bs.Inventory.SumOfAssetAmts();

                if (totalAmt > storedTotalAmt) /// if the BhvrSeq Inventory has a higher total of all assets....
                {
                    ChosenBhvrSeq = bs;
                    //_selectedBhvrSeqIndex = BhvrSeqColl.IndexOf(bs); /// then store its index
                    storedTotalAmt = totalAmt;
                }
            }
            ChosenBhvrSeq.IsChosen = true;
        }

        public void ExecuteBhvrs(ObservableDictionary<string, Organization> orgs)
        {
            //Here the Agent attempts to execute the behaviors in his/her chosen sequence of behaviors
            //If a behavior cannot be executed, the Agent comes up with a new sequence based on his/her
            // Inventory after the last successful behavior.
            bool done = false;

            while (done == false)
            {
                GenBhvrSeqs(orgs);
                if (BhvrSeqColl.Count == 0)
                {
                    return;
                }

                BhvrSeqColl.RemoveEqualInventories();
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


        //public void SelectandApplyBhvrSeq()
        //{
        //    if (BhvrSeqColl == null || this.BhvrSeqColl.Count == 0)
        //    {
        //        return;
        //    }

        //    _selectedBhvrSeqIndex = 0;
        //    int storedTotalAmt = 0;

        //    foreach (BhvrSeq bs in BhvrSeqColl)
        //    {
        //        int totalAmt = 0;
        //        foreach (Asset aa in bs.Inventory) /// sum all the Asset amounts
        //        {
        //            totalAmt += aa.Amount;
        //        }

        //        if (totalAmt > storedTotalAmt) /// if the BhvrSeq Inventory has a higher total of all assets....
        //        {
        //            _selectedBhvrSeqIndex = BhvrSeqColl.IndexOf(bs); /// then store its index
        //            storedTotalAmt = totalAmt;
        //        }
        //    }

        //    //Inventory = BhvrSeqColl[_selectedBhvrSeqIndex].Inventory;
        //    ChosenBhvrSeq = BhvrSeqColl[_selectedBhvrSeqIndex];
        //}

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
