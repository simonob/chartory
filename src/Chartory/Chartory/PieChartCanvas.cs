using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
using System.Reflection;
using Chartory.BrushProviders;
using Chartory;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Chartory
{
    public class PieChartCanvas : Canvas
    {
        ObservableCollection<PieDataItem> Items { get; set; }

        internal EventHandler<ItemTappedEventArgs> ItemTapped;

        internal PieChartControl ParentPieChartControl { get; set; }

        public PieChartCanvas()
        {
            Items = new ObservableCollection<PieDataItem>();
            this.Loaded += OnLoaded;
            this.SizeChanged += PieChartCanvas_SizeChanged;
        }

        void PieChartCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            createPieSegments();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            createPieSegments();
        }

        internal void RedrawPie()
        {
            createPieSegments();
        }

        private void createPieSegments()
        {
            foreach (var item in this.Children)
            {
                if (item is PieSegment)
                {
                    var pieSegment = item as PieSegment;

                    if (pieSegment.DataItem is INotifyPropertyChanged)
                        ((INotifyPropertyChanged)pieSegment.DataItem).PropertyChanged -= PieChartCanvas_PropertyChanged;

                }
            }

            this.Children.Clear();
            Items.Clear();

            if (ParentPieChartControl.ItemsSource is IEnumerable)
            {
                var fillColourProvider = ParentPieChartControl.FillBrushProvider;
                if (fillColourProvider == null)
                    fillColourProvider = new DefaultBrushProvider();

                var items = (IEnumerable)ParentPieChartControl.ItemsSource;

                Double lastAngle = 0;
                Double total = 0;

                foreach (var obj in items)
                {
                    var value = GetValuePropertyValue(obj);
                    total += Math.Round(value, 2);
                }

                var centerPoint = new Point(this.ActualWidth / 2, this.ActualHeight / 2);
                var outerRadius = ParentPieChartControl.OuterRadius;

                if (outerRadius == 0)
                {
                    if (this.ActualWidth > this.ActualHeight)
                        outerRadius = this.ActualHeight / 2;
                    else
                        outerRadius = this.ActualWidth / 2;
                }

                var innerRadius = ParentPieChartControl.InnerRadius;
                
                int i = 0;
                foreach (var obj in items)
                {
                    var value = Math.Round(GetValuePropertyValue(obj), 2);
                    var text = GetDisplayTextPropertyValue(obj);

                    if (value > 0.0)
                    {
                        var percentage = value / total;
                        var angle = percentage * 360;
                        var brush = fillColourProvider.GetBrush(i, value, angle, obj);

                        PieDataItem item = new PieDataItem()
                        {
                            Value = value,
                            Percentage = percentage,
                            Text = text,
                            DataItem = obj,
                            FillBrush = brush
                        };

                        PieSegment p = new PieSegment()
                        {
                            CenterPoint = centerPoint,
                            Radius = outerRadius,
                            StartAngle = lastAngle,
                            EndAngle = lastAngle + angle,
                            Stroke = ParentPieChartControl.LineBrush,
                            Fill = brush,
                            InnerRadius = innerRadius,
                            StrokeThickness = 0,
                            ExplosionRadius = 15,
                            OffsetRadius = 0,
                            Value = value,
                            Percentage = percentage,
                            DataItem = obj,
                            Text = text

                        };

                        p.Tapped += pieSegment_Tapped;

                        Items.Add(item);
                        this.Children.Add(p);

                        if (obj is INotifyPropertyChanged)
                            ((INotifyPropertyChanged)obj).PropertyChanged += PieChartCanvas_PropertyChanged;
                        
                        lastAngle += angle;
                        i++;
                    }

                }
            }

            ParentPieChartControl.setLegendItems(Items);
        }

        void PieChartCanvas_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ParentPieChartControl.DisplayMember || e.PropertyName == ParentPieChartControl.ValueMember)
                this.createPieSegments();
        }

        void pieSegment_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var pieSegment = sender as PieSegment;
            ParentPieChartControl.SelectedDataItem = pieSegment.DataItem;

            pieSegment.IsExploaded = !pieSegment.IsExploaded;
            if (pieSegment.IsExploaded)
                pieSegment.PopOut();
            else
                pieSegment.PopIn();

            raiseItemTappedEvent(pieSegment);
        }

        void raiseItemTappedEvent(PieSegment item)
        {
            var tmpEventHandler = ItemTapped;
            if (tmpEventHandler != null)
                ItemTapped(this, new ItemTappedEventArgs(item.DataItem));
        }
        
        private Double GetValuePropertyValue(object item)
        {
            if (item == null)
                return 0.0;

            var valueMember = ParentPieChartControl.ValueMember;
            try
            {
                if (string.IsNullOrEmpty(valueMember))
                    return Convert.ToDouble(item);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException("Unable to cast object of type {0} to Double. Use the 'ValueMember' property to set a Path to a property of type Double.", ex);
            }

            var valueMemberPropInfo = item.GetType().GetTypeInfo().GetDeclaredProperty(valueMember);
            var obj = valueMemberPropInfo.GetValue(item);

            return Convert.ToDouble(obj);
        }

        private string GetDisplayTextPropertyValue(object item)
        {
            if (item == null)
                return "";

            var displayMember = ParentPieChartControl.DisplayMember;

            if (string.IsNullOrEmpty(displayMember))
                return item.ToString();

            var displayMemberPropInfo = item.GetType().GetTypeInfo().GetDeclaredProperty(displayMember);

            if (displayMemberPropInfo == null)
                return "";

            var obj = displayMemberPropInfo.GetValue(item);

            if (obj is string)
                return (string)obj;
            else
                return obj.ToString();
        }

    }
}
