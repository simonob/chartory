using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Chartory
{
    public sealed class PieSegmentLabel : Control
    {
        public PieSegmentLabel()
        {
            this.DefaultStyleKey = typeof(PieSegmentLabel);
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(PieSegmentLabel),new PropertyMetadata(""));

        public Double Value
        {
            get { return (Double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(Double), typeof(PieSegmentLabel), new PropertyMetadata(0.0));

        public Double Percentage
        {
            get { return (Double)GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register("Percentage", typeof(Double), typeof(PieSegmentLabel), new PropertyMetadata(0.0));

        public object DataItem
        {
            get { return GetValue(DataItemProperty); }
            set { SetValue(DataItemProperty, value); }
        }

        public static readonly DependencyProperty DataItemProperty =
            DependencyProperty.Register("DataItem", typeof(object), typeof(PieSegmentLabel), new PropertyMetadata(0.0));

        public Brush Brush
        {
            get { return (Brush)GetValue(BrushProperty); }
            set { SetValue(BrushProperty, value); }
        }

        public static readonly DependencyProperty BrushProperty =
            DependencyProperty.Register("Brush", typeof(object), typeof(PieSegmentLabel), new PropertyMetadata(0.0));

    }
}
