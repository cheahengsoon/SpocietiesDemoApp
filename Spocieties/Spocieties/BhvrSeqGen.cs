using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Spocieties
{
    public class BhvrSeqGen
    {
        private BhvrSeqColl _bhvrSeqCollOut;
        public BhvrSeqColl BhvrSeqCollOut { get { return _bhvrSeqCollOut; } set { if (_bhvrSeqCollOut != value) { _bhvrSeqCollOut = value; } } }

        //private Asset _endGoal;
        //public Asset EndGoal { get; set; }

        public BhvrSeqGen()
        {
            BhvrSeqCollOut = new BhvrSeqColl();
        }

        public void GenBhvrSeqs(Asset endGoal, ObservableCollection<Behavior> bList, Inventory _inventory)
        {
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
                    _bhvrSeqCollOut.Add(bs);
                }
            }
            #endregion

            #region/////////////////////  Recursively add Behaviors to BhvrSeqs
            foreach (BhvrSeq bs in _BSC) 
            {
                //AddandExport(bs, bList, endGoal);
            }

            _BSC.Clear();
            #endregion

            #region/////////////////////  Remove BhvrSeqs that do not result in endgoal acquisition
            foreach (BhvrSeq bs in _bhvrSeqCollOut)      
            {
                if (!bs.Inventory.InvHasAmt(endGoal))
                {
                    _BSC.Add(bs);
                }
            }
            foreach (BhvrSeq bs in _BSC)
            {
                _bhvrSeqCollOut.Remove(bs);
            }
            #endregion

        }

        //private void AddandExport(BhvrSeq bs, ObservableCollection<Behavior> bList, Asset endgoal)
        //{
        //    BhvrSeqColl BSC = new BhvrSeqColl();
            
        //    ObservableCollection<Behavior> NextPossibles = bs.NextPossBhvrs(bList);

        //    if (NextPossibles.Count == 0 || bs.Inventory.InvHasAmt(endgoal)) // if there are no possible next bhvrs or if endgoal is satisfied
        //    {
        //        return;
        //    }

        //    foreach(Behavior b in NextPossibles) //// Create a new BhvrSeq for each possible next bhvr
        //    {
        //        BhvrSeq _bs = new BhvrSeq(bs);
        //        _bs.BsAdd(b);
        //        BSC.Add(_bs);
        //        BhvrSeqCollOut.Add(_bs);
        //    }

        //    //if (bs.Count > 1)
        //    //{
        //    //    BhvrSeqCollOut.Add(bs);
        //    //}

        //    foreach (BhvrSeq _bs in BSC)
        //    {
        //        AddandExport(_bs, bList, endgoal);
        //    }

        //}

        private static ObservableCollection<Behavior> NextPossBhvrs(BhvrSeq BS, ObservableCollection<Behavior> BList)
        {
            ObservableCollection<Behavior> swapBL = new ObservableCollection<Behavior>();

            if (BList != null)
            {
                foreach (Behavior b in BList)
                {
                    if (BS.Inventory.InvHasAmt(b.Inputs))
                    {
                        if (!(BS.Contains(b) && (b.Repeatable == false)))  //if BS doesnt contain the Bhvr and it is not repeatable
                        {
                            swapBL.Add(b);
                        }
                    }
                }
            }
            return swapBL;
        }


        public static BhvrSeqColl GenTest1(ObservableCollection<Behavior> bList)
        {
            BhvrSeqColl bsc = new BhvrSeqColl();
            BhvrSeq bs = new BhvrSeq();
            bs.Add(bList[0]);
            bs.Add(bList[1]);
            bsc.Add(bs);
            return bsc;
        }

        //public static BhvrSeqColl GenTest2(BhvrSeq bs, ObservableCollection<Behavior> bList)
        //{

        //}


        public static BhvrSeqColl OldGenBhvrSeq(Asset endGoal, ObservableCollection<Behavior> bList, Inventory _inventory)
        {
            BhvrSeqColl swapBSC = new BhvrSeqColl();
            BhvrSeqColl removeBSC = new BhvrSeqColl();
            BhvrSeqColl BSC = new BhvrSeqColl();

            #region Seed BSC with BhvrSeqs containing Bhvrs possible with current inventory
            foreach (Behavior b in bList)
            {
                if (_inventory.InvHasAmt(b.Inputs))
                {
                    BSC.Add(new BhvrSeq(b, _inventory));
                }
            }
            #endregion

            //bool changed = true;

            //while (changed == true) //as long as additions were made to the BSC on the last iteration
            //{
            //    changed = false;

            //    foreach (BhvrSeq bs in BSC)
            //    {
            //        if (bs.Inventory.InvHasAmt(endGoal)) //if the current BS results in endGoal attainment
            //        {
            //            continue;
            //        }

            //        ObservableCollection<Behavior> BL = NextPossBhvrs(bs, bList); //determine what trades are possible given current inventory

            //        foreach (Behavior b in BL)
            //        {
            //            changed = true;
            //            swapBSC.Add(new BhvrSeq(bs, b));

            //            if (b == BL.Last() && !bs.Inventory.InvHasAmt(endGoal))
            //            {
            //                removeBSC.Add(bs);
            //            }
            //        }
            //    }

            //    if (changed == true)
            //    {
            //        foreach (BhvrSeq rbs in removeBSC)
            //        {
            //            BSC.Remove(rbs);
            //        }
            //        foreach (BhvrSeq bs in swapBSC)
            //        {
            //            BSC.Add(bs);
            //        }
            //        swapBSC.Clear();
            //    }
            //}

            //#region Remove BhvrSeqs that do not result in acquisition of endGoal

            //foreach (BhvrSeq bs in BSC)
            //{
            //    if (!bs.Inventory.InvHasAmt(endGoal))
            //    {
            //        swapBSC.Add(bs);
            //    }
            //}
            //foreach (BhvrSeq bs in swapBSC)
            //{
            //    BSC.Remove(bs);
            //}
            //#endregion

            return BSC;
        }
        
        public static BhvrSeqColl GenNoQuantities(Asset endGoal, ObservableCollection<Behavior> BList, Inventory inv)
        {
            ObservableCollection<BhvrSeq> removeBSC = new ObservableCollection<BhvrSeq>();
            BhvrSeqColl BSC = new BhvrSeqColl();

            #region Seed BSC with BhvrSeqs containing Bhvrs possible with current inventory.
            foreach (Behavior b in BList)
            {
                if (inv.InvHas(b.Inputs))
                {
                    BSC.Add(new BhvrSeq(b, inv));
                }
            }
            #endregion

            BhvrSeqColl swapBSC = new BhvrSeqColl();

            bool changed = true;

            while (changed == true)
            {
                changed = false;

                foreach (BhvrSeq bs in BSC)
                {
                    if (bs.Inventory.InvHas(endGoal))
                    {
                        continue;
                    }

                    ObservableCollection<Behavior> BL = NextPossNoQuantities(bs, BList);

                    foreach (Behavior b in BL)
                    {
                        changed = true;
                        swapBSC.Add(new BhvrSeq(bs, b));
                        removeBSC.Add(bs);
                    }
                }
                if (changed == true)
                {
                    foreach (BhvrSeq rbs in removeBSC)
                    {
                        BSC.Remove(rbs);
                    }
                    foreach (BhvrSeq bs in swapBSC)
                    {
                        BSC.Add(bs);
                    }
                    swapBSC.Clear();
                }
            }

            #region Remove BhvrSeqs that do not result in acquisition of endGoal
            /*List<BhvrSeq> BSL = new List<BhvrSeq>();

            foreach (BhvrSeq bs in BSC)
            {
                if (!bs.Inventory.InvHas(endGoal))
                {
                    BSL.Add(bs);
                }
            }
            foreach (BhvrSeq bs in BSL)
            {
                BSC.Remove(bs);
            }*/
            #endregion

            return BSC;
        }

        private static ObservableCollection<Behavior> NextPossNoQuantities(BhvrSeq BS, ObservableCollection<Behavior> BL)
        {
            ObservableCollection<Behavior> swapBL = new ObservableCollection<Behavior>();

            if (BL != null)
            {
                foreach (Behavior b in BL)
                {
                    if (BS.Inventory.InvHas(b.Inputs))
                    {
                        if (!(BS.Contains(b) ^ b.Repeatable == false))  //if BS doesnt contain the Bhvr and it is not repeatable
                        {
                            swapBL.Add(b);
                        }
                    }
                }
            }
            return swapBL;
        }

        public static BhvrSeqColl RemovePermutations(BhvrSeqColl BSC)
        {
            ObservableCollection<BhvrSeq> BSL = new ObservableCollection<BhvrSeq>();

            foreach (BhvrSeq bs in BSC)
            {
                for (int i = BSC.Count - 1; i >= BSC.Count / 2; i--)
                {
                    if (UnorderedEqual(bs, BSC[i]))
                    {
                        BSL.Add(BSC[i]);
                    }
                }
            }
            foreach (BhvrSeq bs in BSL)
            {
                BSC.Remove(bs);
            }
            return BSC;
        }

        private static bool UnorderedEqual<BhvrSeq>(ICollection<BhvrSeq> a, ICollection<BhvrSeq> b)
        {
            // 1
            // Require that the counts are equal
            if (a.Count != b.Count)
            {
                return false;
            }

            if (a == b)
            {
                return false;
            }

            // 2
            // Initialize new Dictionary of the type
            Dictionary<BhvrSeq, int> d = new Dictionary<BhvrSeq, int>();
            // 3
            // Add each key's frequency from collection A to the Dictionary
            foreach (BhvrSeq item in a)
            {
                int c;
                if (d.TryGetValue(item, out c))
                {
                    d[item] = c + 1;
                }
                else
                {
                    d.Add(item, 1);
                }
            }
            // 4
            // Add each key's frequency from collection B to the Dictionary
            // Return early if we detect a mismatch
            foreach (BhvrSeq item in b)
            {
                int c;
                if (d.TryGetValue(item, out c))
                {
                    if (c == 0)
                    {
                        return false;
                    }
                    else
                    {
                        d[item] = c - 1;
                    }
                }
                else
                {
                    // Not in dictionary
                    return false;
                }
            }
            // 5
            // Verify that all frequencies are zero
            foreach (int v in d.Values)
            {
                if (v != 0)
                {
                    return false;
                }
            }
            // 6
            // We know the collections are equal
            return true;
        }
    }
}
