using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Chartory
{
    public sealed class PieChartControl : Control
    {

        #region Fields

        PieChartCanvas _canvas;
        LegendControl _legend;

        #endregion

        public PieChartControl()
        {
            this.DefaultStyleKey = typeof(PieChartControl);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_canvas != null)
                _canvas.ItemTapped -= PieChartCanvas_ItemTapped;

            _canvas = this.GetTemplateChild("PieChartControl_Canvas") as PieChartCanvas;
            if (_canvas != null)
            {
                _canvas.ItemTapped += PieChartCanvas_ItemTapped;
                _canvas.ParentPieChartControl = this;
            }

            _legend = this.GetTemplateChild("PieChartControl_Legend") as LegendControl;
            if(_legend != null)
                _legend.ParentPieChartControl = this;
        }

        void raiseItemTappedEvent(object dataItem)
        {
            var tmpEventHandler = ItemTapped;
            if (tmpEventHandler != null)
                ItemTapped(this, new ItemTappedEventArgs(dataItem));
        }

        void PieChartCanvas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            raiseItemTappedEvent(e.DataItem);
        }

        internal void setLegendItems(ObservableCollection<PieDataItem> items)
        {
            if (_legend != null)
                _legend.Items = items;
        }

        #region Properties

        public Brush LineBrush { get; set; }
        public Double OuterRadius { get; set; }
        public IBrushProvider FillBrushProvider { get; set; }
        public Double InnerRadius { get; set; }

        public object SelectedDataItem { get; set; }

        #endregion

        #region Events

        public event EventHandler<ItemTappedEventArgs> ItemTapped;

        #endregion

        #region Dependency Properties

        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(PieChartControl), new PropertyMetadata(null, OnItemsSourcePropertyChanged));

        private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (d as PieChartControl);
            if (ctrl._canvas != null)
                ctrl._canvas.RedrawPie();

            if (e.OldValue != null && e.OldValue is INotifyCollectionChanged)
                ((INotifyCollectionChanged)e.OldValue).CollectionChanged -= ctrl.PieChartControl_CollectionChanged;

            if (e.NewValue != null && e.NewValue is INotifyCollectionChanged)
                ((INotifyCollectionChanged)e.NewValue).CollectionChanged += ctrl.PieChartControl_CollectionChanged;
        }

        void PieChartControl_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_canvas != null)
                _canvas.RedrawPie();
        }

        public string ValueMember
        {
            get { return (string)GetValue(ValueMemberProperty); }
            set { SetValue(ValueMemberProperty, value); }
        }

        public static readonly DependencyProperty ValueMemberProperty =
            DependencyProperty.Register("ValueMember", typeof(string), typeof(PieChartControl), new PropertyMetadata(""));

        public string DisplayMember
        {
            get { return (string)GetValue(DisplayMemberProperty); }
            set { SetValue(DisplayMemberProperty, value); }
        }

        public static readonly DependencyProperty DisplayMemberProperty =
            DependencyProperty.Register("DisplayMember", typeof(string), typeof(PieChartControl), new PropertyMetadata(""));

        public Visibility LegendVisibility
        {
            get { return (Visibility)GetValue(LegendVisibilityProperty); }
            set { SetValue(LegendVisibilityProperty, value); }
        }

        public static readonly DependencyProperty LegendVisibilityProperty =
            DependencyProperty.Register("LegendVisibility", typeof(Visibility), typeof(PieChartControl), new PropertyMetadata(Visibility.Visible));

        #endregion

    }
}
