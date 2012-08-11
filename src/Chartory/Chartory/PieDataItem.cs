using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Chartory
{
    public class PieDataItem
    {
        public Double Value { get; set; }
        public Double Percentage { get; set; }
        public string Text { get; set; }
        public Brush FillBrush { get; set; }
        public object DataItem { get; set; }
    }
}
