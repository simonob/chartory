using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Chartory.BrushProviders
{
    public class DefaultBrushProvider : IBrushProvider
    {
        Brush[] _brushes;

        public DefaultBrushProvider()
        {
            _brushes = new[] 
            { 
                new SolidColorBrush(ColorHelper.FromArgb(255, 255, 171, 61)),
                new SolidColorBrush(ColorHelper.FromArgb(255, 242, 255, 61)),
                new SolidColorBrush(ColorHelper.FromArgb(255, 61, 255, 74)),
                new SolidColorBrush(ColorHelper.FromArgb(255, 255, 74, 61)),
                new SolidColorBrush(ColorHelper.FromArgb(255, 255, 61, 145)),
                new SolidColorBrush(ColorHelper.FromArgb(255, 0, 84, 194)),
                new SolidColorBrush(ColorHelper.FromArgb(255, 0, 111, 255)),
                new SolidColorBrush(ColorHelper.FromArgb(255, 61, 145, 255)),
                new SolidColorBrush(ColorHelper.FromArgb(255, 171, 61, 255)),
                new SolidColorBrush(ColorHelper.FromArgb(255, 145, 255, 61))
            };
        }

        public Windows.UI.Xaml.Media.Brush GetBrush(int itemIndex, double itemValue, double angle, object boundItem)
        {
            var len = _brushes.Length;
            var brushIndex = itemIndex % len;
            return _brushes[brushIndex];
        }

    }
}
