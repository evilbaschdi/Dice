using System.Windows.Media;
using MahApps.Metro.IconPacks;

namespace Dice.Core.PackIconImageSourceConverter
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class PackIconMaterialLightImageSourceConverter : PackIconImageSourceConverterBase<PackIconMaterialLightKind>
    {
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="foregroundBrush"></param>
        /// <param name="penThickness"></param>
        /// <returns></returns>
        protected override ImageSource CreateImageSource(object value, Brush foregroundBrush, double penThickness)
        {
            var packIcon = new PackIconMaterialLight
                           {
                               Kind = (PackIconMaterialLightKind) value
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