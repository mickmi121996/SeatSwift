using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatSwiftDLL.Enums
{
    /// <summary>
    /// The status of a representation
    /// </summary>
    /// <remarks>
    /// Available: The representation is available for viewing
    /// Complete: The representation is complete
    /// Cancelled: The representation is cancelled
    /// </remarks>
    public enum RepresentationStatus
    {
        Available = 0,
        Complete = 1,
        Cancelled = 2,
    }
}
