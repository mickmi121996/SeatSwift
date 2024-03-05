using SeatSwiftDLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatSwiftDLL
{
    /// <summary>
    /// The seat class
    /// </summary>
    /// <remarks>
    /// This class is used to store the seat data
    /// </remarks>
    public class Seat : ICloneable
    {
        # region Properties

        /// <summary>
        /// The Id of the seat in the database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// If the seat is active in the database
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The row of the seat
        /// </summary>
        public SectionRow SectionRow { get; set; }

        /// <summary>
        /// The number of the seat
        /// </summary>
        public int SeatNumber { get; set; }

        /// <summary>
        /// The seat status
        /// </summary>
        public SeatStatus SeatStatus { get; set; }

        # endregion


        #region Constructor

        /// <summary>
        /// The default constructor
        /// </summary>
        public Seat()
        {
            Id = default;
            IsActive = true;
            SectionRow = new SectionRow();
            SeatNumber = default;
            SeatStatus = SeatStatus.Available;
        }

        /// <summary>
        /// The constructor with parameters
        /// </summary>
        /// <param name="id">The Id of the seat in the database</param>
        /// <param name="isActive">If the seat is active in the database</param>
        /// <param name="sectionRow">The row of the seat</param>
        /// <param name="seatNumber">The number of the seat</param>
        /// <param name="seatStatus">The status of the seat</param>
        public Seat(int id, bool isActive, SectionRow sectionRow, int seatNumber, SeatStatus seatStatus)
        {
            this.Id = id;
            this.IsActive = isActive;
            this.SectionRow = sectionRow;
            this.SeatNumber = seatNumber;
            this.SeatStatus = seatStatus;
        }

        #endregion


        # region Interface methods
        
        public object Clone()
        {
            throw new NotImplementedException();
        }

        # endregion
    }
}
