using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatSwiftDLL
{
    /// <summary>
    /// The ticket class
    /// </summary>
    /// <remarks>
    /// This class is used to store the ticket data
    /// </remarks>
    public class Ticket : ICloneable
    {
        # region Properties

        /// <summary>
        /// The Id of the ticket in the database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// If the ticket is active in the database
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The ticket number
        /// </summary>
        public string TicketNumber { get; set; }

        /// <summary>
        /// The ticket status
        /// </summary>
        public TicketStatus TicketStatus { get; set; }

        /// <summary>
        /// The representation of the ticket
        /// </summary>
        public Representation Representation { get; set; }

        /// <summary>
        /// The seat of the ticket
        /// </summary>
        public Seat Seat { get; set; }

        # endregion


        # region Constructor

        /// <summary>
        /// The default constructor
        /// </summary>
        public Ticket()
        {
            Id = default;
            IsActive = true;
            TicketNumber = string.Empty;
            TicketStatus = default;
            Representation = new Representation();
            Seat = new Seat();
        }

        /// <summary>
        /// The constructor with parameters for the ticket
        /// </summary>
        /// <param name="id">The Id of the ticket in the database</param>
        /// <param name="isActive">If the ticket is active in the database</param>
        /// <param name="ticketNumber">The ticket number</param>
        /// <param name="ticketStatus">The ticket status</param>
        /// <param name="representation">The representation of the ticket</param>
        /// <param name="seat">The seat of the ticket</param>
        public Ticket(int id, bool isActive, string ticketNumber, TicketStatus ticketStatus, Representation representation, Seat seat)
        {
            this.Id = id;
            this.IsActive = isActive;
            this.TicketNumber = ticketNumber;
            this.TicketStatus = ticketStatus;
            this.Representation = representation;
            this.Seat = seat;
        }

        # endregion


        # region Interface methods
        
        public object Clone()
        {
            throw new NotImplementedException();
        }

        # endregion
    }
}
