using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Spocieties
{
    class DataPointPriceComparer : IComparer<DataPoint>
    {
        public int Compare(DataPoint x, DataPoint y)
        {
            if (x.Price > y.Price)
            {
                return 1;
            }
            else if (x.Price < y.Price)
            {
                return -1;
            }
            else if (x.Price == y.Price)
            {
                if (x.Qty > y.Qty)
                {
                    return 1;
                }
                else if (x.Qty < y.Qty)
                {
                    return -1;
                }
            }

            {
                return 1;
            }
        }
    }

    class DataPointQtyComparer : IComparer<DataPoint>
    {
        public int Compare(DataPoint x, DataPoint y)
        {
            if (x.Qty > y.Qty)
            {
                return 1;
            }
            else if (x.Qty < y.Qty)
            {
                return -1;
            }
            else if (x.Qty == y.Qty)
            {
                if (x.Price < y.Price)
                {
                    return 1;
                }
                else if (x.Price > y.Price)
                {
                    return -1;
                }
            }

            {
                return 1;
            }
        }
    }
}
