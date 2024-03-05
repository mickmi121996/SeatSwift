using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatSwiftDLL.Enums
{
    /// <summary>
    /// The type of a show
    /// </summary>
    /// <remarks>
    /// Movie: The show is a movie
    /// Theater: The show is a theater performance
    /// Musical Comedy: The show is a musical comedy
    /// Concert: The show is a concert
    /// Humor: The show is a humor performance
    /// Dance: The show is a dance performance
    /// Conference: The show is a conference
    /// Variety: The show is a variety performance
    /// </remarks>
    public enum ShowType
    {
        Movie = 0,
        Theater = 1,
        MusicalComedy = 2,
        Concert = 3,
        Humor = 4,
        Dance = 5,
        Conference = 6,
        Variety = 7,
    }
}
