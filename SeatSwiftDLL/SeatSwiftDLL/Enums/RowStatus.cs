using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatSwiftDLL.Enums
{
    /// <summary>
    /// The status of a row in an auditorium
    /// </summary>
    /// <remarks>
    /// Available: The row is available for seating
    /// OutOfService: The row is not available for seating
    /// </remarks>
    public enum RowStatus
    {
        Available = 0,
        OutOfService = 1,
    }
}
