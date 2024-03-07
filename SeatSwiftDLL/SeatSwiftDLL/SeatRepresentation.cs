using SeatSwiftDLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatSwiftDLL
{
    /// <summary>
    /// The jonction class between the seat and the representation
    /// </summary>
    /// <remarks>
    /// This class is used to store the jonction data
    /// </remarks>
    public class SeatRepresentation
    {
        #region Properties

        /// <summary>
        /// The Id of the jonction in the database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The seat of the jonction
        /// </summary>
        public Seat Seat { get; set; }

        /// <summary>
        /// The representation of the jonction
        /// </summary>
        public Representation Representation { get; set; }

        /// <summary>
        /// The Seat status of the jonction
        /// </summary>
        public SeatStatus SeatStatus { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// The default constructor
        /// </summary>
        public SeatRepresentation()
        {
            Id = default;
            Seat = new Seat();
            Representation = new Representation();
            SeatStatus = default;
        }

        /// <summary>
        /// The constructor with parameters
        /// </summary>
        /// <param name="id">The Id of the jonction in the database</param>
        /// <param name="seat">The seat of the jonction</param>
        /// <param name="representation">The representation of the jonction</param>
        /// <param name="seatStatus">The Seat status of the jonction</param>
        public SeatRepresentation(int id, Seat seat, Representation representation, SeatStatus seatStatus)
        {
            this.Id = id;
            this.Seat = seat;
            this.Representation = representation;
            this.SeatStatus = seatStatus;
        }

        #endregion
    }
}
