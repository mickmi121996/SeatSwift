using SeatSwiftDLL.Enums;
using SeatSwiftDLL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Drawing;

namespace AppGestion.Converters
{
    public class SectionAndStatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // value est censé être un objet de type 'Seat'
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
                return Brushes.Gray; // OutOfService ou non défini
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

}
