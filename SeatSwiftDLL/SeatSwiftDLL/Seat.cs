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
        /// The number of the seat
        /// </summary>
        public int SeatNumber { get; set; }

        /// <summary>
        /// The status of the seat (OutOfService, InService)
        /// </summary>
        public SeatStatus Status { get; set; }

        /// <summary>
        /// The x coordinates of the seat
        /// </summary>
        public int XCoordinate { get; set; }

        /// <summary>
        /// The y coordinates of the seat
        /// </summary>
        public int YCoordinate { get; set; }

        /// <summary>
        /// The section name of the seat
        /// </summary>
        public SectionName SectionName { get; set; }

        /// <summary>
        /// The auditorium of the seat
        /// </summary>
        public Auditorium? Auditorium { get; set; }

        #endregion
        

        #region Constructor

        /// <summary>
        /// The default constructor
        /// </summary>
        public Seat()
        {
            Id = default;
            SeatNumber = default;
            Status = default;
            Auditorium = null;
            SectionName = default;
            XCoordinate = default;
            YCoordinate = default;
        }

        /// <summary>
        /// The parameterized constructor
        /// </summary>
        /// <param name="id">The Id of the seat in the database</param>
        /// <param name="seatNumber">The number of the seat</param>
        /// <param name="auditorium">The auditorium of the seat</param>
        /// <param name="sectionName">The section name of the seat</param>
        /// <param name="xCoordinate">The x coordinates of the seat in the auditorium</param>
        /// <param name="yCoordinate">The y coordinates of the seat in the auditorium</param>
        public Seat(int id, int seatNumber, SeatStatus status,Auditorium auditorium, SectionName sectionName, int xCoordinate, int yCoordinate)
        {
            if(auditorium == null) {throw new ArgumentNullException(nameof(auditorium));}
                
            this.Id = id;
            this.SeatNumber = seatNumber;
            this.Status = status;
            this.Auditorium = auditorium;
            this.SectionName = sectionName;
            this.XCoordinate = xCoordinate;
            this.YCoordinate = yCoordinate;
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
