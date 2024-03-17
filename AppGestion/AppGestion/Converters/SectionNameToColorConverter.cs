using SeatSwiftDLL.Enums;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;

namespace AppGestion.Converters
{
    public class SectionNameToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SectionName sectionName)
            {
                switch (sectionName)
                {
                    case SectionName.Parterre:
                        return Brushes.Green;
                    case SectionName.Balcon:
                        return Brushes.Purple;
                    case SectionName.Loge:
                        return Brushes.Blue;
                    default:
                        return Brushes.Gray;
                }
            }

            return Brushes.Gray;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            throw new NotImplementedException();
        }
    }
}
