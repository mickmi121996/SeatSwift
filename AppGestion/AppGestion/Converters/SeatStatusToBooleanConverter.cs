using SeatSwiftDLL.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AppGestion.Converters
{
    [ValueConversion(typeof(SeatStatus), typeof(bool))]
    public class SeatStatusToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Assurez-vous que la valeur est de type SeatStatus
            if (value is SeatStatus status)
            {
                // SeatStatus.InService équivaut à true
                return status == SeatStatus.InService;
            }
            return false;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            // Convertit le booléen de retour en SeatStatus
            if (value is bool inService)
            {
                return inService ? SeatStatus.InService : SeatStatus.OutOfService;
            }
            return SeatStatus.OutOfService;
        }
    }
}
