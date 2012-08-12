using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Chartory
{
    public sealed class LegendControl : Control
    {
        internal PieChartControl ParentPieChartControl { get; set; }

        public LegendControl()
        {
            this.DefaultStyleKey = typeof(LegendControl);
        }

        public ObservableCollection<PieDataItem> Items
        {
            get { return (ObservableCollection<PieDataItem>)GetValue(ItemsProperty); }
            set 
            { 
                SetValue(ItemsProperty, value); 
            }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<PieDataItem>), typeof(LegendControl), new PropertyMetadata(null));
    }
}
