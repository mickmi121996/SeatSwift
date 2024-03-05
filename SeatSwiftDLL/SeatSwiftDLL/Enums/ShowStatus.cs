using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatSwiftDLL.Enums
{
    /// <summary>
    /// The status of a show
    /// </summary>
    /// <remarks>
    /// Available: The show is available for viewing
    /// Complete: The show is complete
    /// Cancelled: The show is cancelled
    /// </remarks>
    public enum ShowStatus
    {
        Available = 0,
        Complete = 1,
        Cancelled = 2,
    }
}
