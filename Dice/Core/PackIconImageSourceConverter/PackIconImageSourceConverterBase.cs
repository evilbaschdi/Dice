using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using MahApps.Metro.IconPacks;

namespace Dice.Core.PackIconImageSourceConverter
{
    /// <summary>
    ///     Converts a <see cref="PackIcon{TKind}" /> to an ImageSource.
    ///     Use the ConverterParameter to pass a Brush.
    /// </summary>
    public abstract class PackIconImageSourceConverterBase<TKind> : MarkupExtension, IValueConverter
    {
        /// <summary>
        ///     Gets or sets the thickness to draw the icon with.
        /// </summary>
        private double Thickness { get; set; } = 0.25;

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TKind))
            {
                return null;
            }

            var foregroundBrush = parameter as Brush ?? Brushes.Black;
            return CreateImageSource(value, foregroundBrush, Thickness);
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="foregroundBrush"></param>
        /// <param name="penThickness"></param>
        /// <returns></returns>
        protected abstract ImageSource CreateImageSource(object value, Brush foregroundBrush, double penThickness);
    }
}