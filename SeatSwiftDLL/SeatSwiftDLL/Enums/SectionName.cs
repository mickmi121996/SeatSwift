using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatSwiftDLL.Enums
{
    /// <summary>
    /// The name of a section in an auditorium
    /// </summary>
    /// <remarks>
    /// Balcony: The section is a balcony
    /// Parterre: The section is a parterre
    /// Loge: The section is a loge
    /// </remarks>
    public enum SectionName
    {
        Balcon = 0,
        Parterre = 1,
        Loge = 2,
    }
}
