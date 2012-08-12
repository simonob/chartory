using Windows.Foundation;

namespace Chartory.Charting.Extensions
{
    public static class PointExtensions
    {
        public static Point Offset(this Point p, double x, double y)
        {
            return new Point(p.X + x, p.Y + y);
        }
    }
}
