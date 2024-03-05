using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatSwiftDLL.Enums
{
    /// <summary>
    /// The status of a section in an auditorium
    /// </summary>
    /// <remarks>
    /// Available: The section is available for seating
    /// OutOfService: The section is not available for seating
    /// </remarks>
    public enum SectionStatus
    {
        Available = 0,
        OutOfService = 1,
    }
}
