using SeatSwiftDLL;
using SeatSwiftDLL.Enums;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;

namespace AppGestion.Converters
{
    public class SectionAndStatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var seat = (Seat)value;
            if (seat.Status == SeatStatus.InService)
            {
                switch (seat.SectionName)
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
            else
            {
                return Brushes.Gray;
            }
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            throw new NotSupportedException();
        }
    }
}
