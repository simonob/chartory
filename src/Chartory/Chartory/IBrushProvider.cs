using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Chartory
{
    public interface IBrushProvider
    {
        Brush GetBrush(int itemIndex, Double itemValue, Double angle, object boundItem);
    }
}
