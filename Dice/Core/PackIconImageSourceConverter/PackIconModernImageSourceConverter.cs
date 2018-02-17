using System.Windows.Media;
using MahApps.Metro.IconPacks;

namespace Dice.Core.PackIconImageSourceConverter
{
    /// <summary>
    /// </summary>
    public class PackIconModernImageSourceConverter : PackIconImageSourceConverterBase<PackIconModernKind>
    {
        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="foregroundBrush"></param>
        /// <param name="penThickness"></param>
        /// <returns></returns>
        protected override ImageSource CreateImageSource(object value, Brush foregroundBrush, double penThickness)
        {
            var packIcon = new PackIconModern
                           {
                               Kind = (PackIconModernKind) value
                           };

            var geometryDrawing = new GeometryDrawing
                                  {
                                      Geometry = Geometry.Parse(packIcon.Data),
                                      Brush = foregroundBrush,
                                      Pen = new Pen(foregroundBrush, penThickness)
                                  };

            var drawingGroup = new DrawingGroup
                               {
                                   Children =
                                   {
                                       geometryDrawing
                                   }
                               };

            return new DrawingImage
                   {
                       Drawing = drawingGroup
                   };
        }
    }
}