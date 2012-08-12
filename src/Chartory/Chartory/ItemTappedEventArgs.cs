using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chartory
{
    public class ItemTappedEventArgs : EventArgs
    {
        public ItemTappedEventArgs(object dataItem)
        {
            this.DataItem = dataItem;
        }

        public object DataItem { get; set; }
    }
}
