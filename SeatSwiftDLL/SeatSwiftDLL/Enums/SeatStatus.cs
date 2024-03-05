using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatSwiftDLL.Enums
{
    /// <summary>
    /// The status of a seat in a row
    /// </summary>
    /// <remarks>
    /// Available: The seat is available for seating
    /// Reserved: The seat is reserved for seating
    /// Unavailable: The seat is not available for seating
    /// </remarks>
    public enum SeatStatus
    {
        Available = 0,
        Reserved = 1,
        Unavailable = 2,
    }
}
