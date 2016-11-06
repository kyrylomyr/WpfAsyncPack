using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfAsyncPack.Converters
{
    /// <summary>
    /// Converts a <see cref="bool"/> value to <see cref="Visibility"/> using the specified mapping of <see cref="Visibility"/> values.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the <see cref="Visibility"/> value that is used when the <see cref="bool"/> value is <c>true</c>.
        /// </summary>
        /// <value>
        /// The <see cref="Visibility"/> value that is used when the <see cref="bool"/> value is <c>true</c>.
        /// </value>
        public Visibility TrueValue { get; set; } = Visibility.Visible;

        /// <summary>
        /// Gets or sets the <see cref="Visibility"/> value that is used when the <see cref="bool"/> value is <c>false</c>.
        /// </summary>
        /// <value>
        /// The <see cref="Visibility"/> value that is used when the <see cref="bool"/> value is <c>false</c>.
        /// </value>
        public Visibility FalseValue { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="bool"/> value is inverted before converting to <see cref="Visibility"/>.
        /// </summary>
        /// <value>
        /// <c>true</c> if the <see cref="bool"/> value is inverted; otherwise, <c>false</c>.
        /// </value>
        public bool Invert { get; set; }

        /// <summary>
        /// Converts a <see cref="bool"/> value to <see cref="Visibility"/>.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted <see cref="Visibility"/> value.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = (bool)value;
            return Invert ? (boolValue ? FalseValue : TrueValue) : (boolValue ? TrueValue : FalseValue);
        }

        /// <summary>
        /// The conversion back is not supported and throws an <see cref="NotImplementedException"/> when invoked.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}