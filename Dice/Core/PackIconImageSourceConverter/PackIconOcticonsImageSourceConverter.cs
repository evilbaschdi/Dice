using System.Windows.Media;
using Dice.Core.PackIconImageSourceConverter;
using MahApps.Metro.IconPacks;

namespace Dice.Core
{
    /// <summary>
    /// </summary>
    public class PackIconOcticonsImageSourceConverter : PackIconImageSourceConverterBase<PackIconOcticonsKind>
    {
        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="foregroundBrush"></param>
        /// <param name="penThickness"></param>
        /// <returns></returns>
        protected override ImageSource CreateImageSource(object value, Brush foregroundBrush, double penThickness)
        {
            var packIcon = new PackIconOcticons
                           {
                               Kind = (PackIconOcticonsKind) value
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