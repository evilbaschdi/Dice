using System.Windows.Media;
using MahApps.Metro.IconPacks;

namespace Dice.Core.PackIconImageSourceConverter
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public class PackIconFontAwesomeImageSourceConverter : PackIconImageSourceConverterBase<PackIconFontAwesomeKind>
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
            var packIcon = new PackIconFontAwesome
                           {
                               Kind = (PackIconFontAwesomeKind) value
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
                                   },
                                   Transform = new ScaleTransform(1, -1)
                               };

            return new DrawingImage
                   {
                       Drawing = drawingGroup
                   };
        }
    }
}