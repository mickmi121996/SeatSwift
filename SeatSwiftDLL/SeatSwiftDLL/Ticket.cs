﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeatSwiftDLL.Enums;

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
        public string ReservationNumber { get; set; }

        /// <summary>
        /// The ticket status
        /// </summary>
        public TicketStatus TicketStatus { get; set; }

        /// <summary>
        /// The representation of the ticket
        /// </summary>
        public Representation? Representation { get; set; }

        /// <summary>
        /// The seat of the ticket
        /// </summary>
        public Seat? Seat { get; set; }

        /// <summary>
        /// The order that purchased the ticket. Nullable if the ticket is not associated with an order.
        /// </summary>
        public Order? Order { get; set; }

        #endregion


        #region Constructor

        /// <summary>
        /// The default constructor
        /// </summary>
        public Ticket()
        {
            // Assignation des valeurs par défaut appropriées.
            Id = default;
            IsActive = true;
            ReservationNumber = string.Empty;
            TicketStatus = default;
            Representation = null;
            Seat = null;
            Order = null;
        }

        /// <summary>
        /// The constructor with parameters for the ticket
        /// </summary>
        /// <param name="id">The Id of the ticket in the database</param>
        /// <param name="isActive">If the ticket is active in the database</param>
        /// <param name="reservationNumber">The ticket number</param>
        /// <param name="ticketStatus">The ticket status</param>
        /// <param name="representation">The representation of the ticket. Must not be null.</param>
        /// <param name="seat">The seat of the ticket. Must not be null.</param>
        /// <param name="order">The order that purchased the ticket. Nullable if the ticket is not associated with an order.</param>
        public Ticket(int id, bool isActive, string reservationNumber, TicketStatus ticketStatus, Representation representation, Seat seat, Order? order)
        {
            if (representation == null) throw new ArgumentNullException(nameof(representation));
            if (seat == null) throw new ArgumentNullException(nameof(seat));

            this.Id = id;
            this.IsActive = isActive;
            this.ReservationNumber = reservationNumber;
            this.TicketStatus = ticketStatus;
            this.Representation = representation;
            this.Seat = seat;
            this.Order = order;
        }


        #endregion


        #region Interface methods

        public object Clone()
        {
            throw new NotImplementedException();
        }

        # endregion
    }
}
