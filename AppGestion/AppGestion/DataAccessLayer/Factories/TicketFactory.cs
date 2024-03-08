using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeatSwiftDLL;
using SeatSwiftDLL.Enums;
using System.Data;
using AppGestion.Tools;

namespace AppGestion.DataAccessLayer.Factories
{
    /// <summary>
    /// Factory class for Ticket
    /// </summary>
    /// <remarks>
    /// This class is a factory for the Ticket class. It is used
    /// to create Ticket objects from data read from a database.
    /// </remarks>
    public class TicketFactory : Base.FactoryBase
    {
        #region Create methods

        /// <summary>
        /// Creates a new Ticket object from a data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <returns>The newly created Ticket object.</returns>
        public async Task<Ticket> CreateFromReadeAsync(MySqlDataReader dataReader)
        {
            int id = dataReader.GetInt32("Id");
            bool isActive = dataReader.GetBoolean("IsActive");
            string reservationNumber = dataReader.GetString("ReservationNumber");
            string ticketStatus = dataReader.GetString("TicketStatus");
            int seatId = dataReader.GetInt32("SeatId");
            int representationId = dataReader.GetInt32("RepresentationId");
            int orderId = dataReader.GetInt32("OrderId");

            // Convert the enum to a string
            TicketStatus ticketStatusEnum = (TicketStatus)Enum.Parse(typeof(TicketStatus), ticketStatus);

            // Get the seat from the database using the seatId
            Seat seat = await new SeatFactory().GetByIdAsync(seatId);
            if (seat == null)
            {
                throw new Exception("Seat cannot be null");
            }
            // Get the representation from the database using the representationId
            Representation representation = await new RepresentationFactory().GetByIdAsync(representationId);
            if (representation == null)
            {
                throw new Exception("Representation cannot be null");
            }

            // Get the order from the database using the orderId it can be null
            Order order = await new OrderFactory().GetByIdAsync(orderId);

            Ticket ticket = new Ticket(id, isActive, reservationNumber, ticketStatusEnum, representation, seat, order);

            return ticket;
        }

        /// <summary>
        /// Creates a new Ticket object from a data row
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <returns>The newly created Ticket object.</returns>
        public async Task<Ticket> CreateFromRowAsync(DataRow dataRow)
        {
            int id = dataRow.Field<int>("Id");
            bool isActive = dataRow.Field<bool>("IsActive");
            string reservationNumber = dataRow.Field<string>("ReservationNumber")
                ?? throw new Exception("ReservationNumber cannot be null");
            string ticketStatus = dataRow.Field<string>("TicketStatus")
                ?? throw new Exception("TicketStatus cannot be null");
            int seatId = dataRow.Field<int>("SeatId");
            int representationId = dataRow.Field<int>("RepresentationId");
            int orderId = dataRow.Field<int>("OrderId");

            // Convert the enum to a string
            TicketStatus ticketStatusEnum = (TicketStatus)Enum.Parse(typeof(TicketStatus), ticketStatus);

            // Get the seat from the database using the seatId
            Seat seat = await new SeatFactory().GetByIdAsync(seatId);
            if (seat == null)
            {
                throw new Exception("Seat cannot be null");
            }

            // Get the representation from the database using the representationId
            Representation representation = await new RepresentationFactory().GetByIdAsync(representationId);
            if (representation == null)
            {
                throw new Exception("Representation cannot be null");
            }

            // Get the order from the database using the orderId it can be null
            Order order = await new OrderFactory().GetByIdAsync(orderId);

            Ticket ticket = new Ticket(id, isActive, reservationNumber, ticketStatusEnum, representation, seat, order);

            return ticket;
        }

        #endregion


        #region Factory methods

        /// <summary>
        /// Get a Ticket object by its id.
        /// </summary>
        /// <param name="id">The id of the Ticket object to get.</param>
        /// <returns>The Ticket object with the given id, or null if no such object exists.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MySqlException">A MySQL exception was thrown.</exception>
        public async Task<Ticket> GetByIdAsync(int id)
        {
            try
            {
                // Get the ticket with the given id
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM ticket WHERE Id = @id;",
                    new MySqlParameter("@id", id)
                    )
                )
                {
                    // If no ticket is found, throw an exception
                    if (result.Rows.Count == 0)
                    {
                        throw new KeyNotFoundException("No ticket with the id " + id + " was found");
                    }

                    // Create the ticket object
                    return await CreateFromRowAsync(result.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the ticket with the given id", ex);
            }
        }

        /// <summary>
        /// Get all Ticket objects for a given representation.
        /// </summary>
        /// <param name="representation">The representation to get the tickets for.</param>
        /// <returns>A list of Ticket objects for the given representation.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MySqlException">A MySQL exception was thrown.</exception>
        public async Task<List<Ticket>> GetByRepresentationAsync(Representation representation)
        {
            try
            {
                // Get the tickets for the given representation
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM ticket WHERE RepresentationId = @representationId;",
                    new MySqlParameter("@representationId", representation.Id)
                    )
                )
                {
                    // Create the list of ticket objects
                    List<Ticket> tickets = new List<Ticket>();
                    foreach (DataRow row in result.Rows)
                    {
                        tickets.Add(await CreateFromRowAsync(row));
                    }
                    return tickets;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the tickets for the given representation", ex);
            }
        }

        /// <summary>
        /// Get all available Ticket objects for a given representation.
        /// </summary>
        /// <param name="representation">The representation to get the available tickets for.</param>
        /// <returns>A list of available Ticket objects for the given representation.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MySqlException">A MySQL exception was thrown.</exception>
        public async Task<List<Ticket>> GetAvailableByRepresentationAsync(Representation representation)
        {
            try
            {
                // Get the available tickets for the given representation
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM ticket WHERE RepresentationId = @representationId AND TicketStatus = @ticketStatus;",
                    new MySqlParameter("@representationId", representation.Id),
                    new MySqlParameter("@ticketStatus", TicketStatus.Available.ToString())
                    )
                )
                {
                    // Create the list of ticket objects
                    List<Ticket> tickets = new List<Ticket>();
                    foreach (DataRow row in result.Rows)
                    {
                        tickets.Add(await CreateFromRowAsync(row));
                    }
                    return tickets;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the available tickets for the given representation", ex);
            }
        }

        /// <summary>
        /// Asign a ticket to an order and change the status to Purchased.
        /// </summary>
        /// <param name="ticket">The ticket to asign to the order.</param>
        /// <param name="order">The order to asign the ticket to.</param>
        /// <returns>True if the ticket was asigned to the order, false otherwise.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MySqlException">A MySQL exception was thrown.</exception>
        public async Task<bool> AsignToOrderAsync(Ticket ticket, Order order)
        {
            try
            {
                // Asign the ticket to the order
                int rowsAffected = await DataBaseTool.ExecuteNonQueryAsync
                (this.ConnectionString,
                "UPDATE ticket SET TicketStatus = @ticketStatus, OrderId = @orderId WHERE Id = @id;",
                new MySqlParameter("@ticketStatus", TicketStatus.Purchased.ToString()),
                new MySqlParameter("@orderId", order.Id),
                new MySqlParameter("@id", ticket.Id)
                );

                // Return true if the ticket was asigned to the order
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while asigning the ticket to the order", ex);
            }
        }

        /// <summary>
        /// Get all Ticket objects for a given order.
        /// </summary>
        /// <param name="order">The order to get the tickets for.</param>
        /// <returns>A list of Ticket objects for the given order.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MySqlException">A MySQL exception was thrown.</exception>
        public async Task<List<Ticket>> GetByOrderAsync(Order order)
        {
            try
            {
                // Get the tickets for the given order
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM ticket WHERE OrderId = @orderId;",
                    new MySqlParameter("@orderId", order.Id)
                    )
                )
                {
                    // Create the list of ticket objects
                    List<Ticket> tickets = new List<Ticket>();
                    foreach (DataRow row in result.Rows)
                    {
                        tickets.Add(await CreateFromRowAsync(row));
                    }
                    return tickets;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the tickets for the given order", ex);
            }
        }

        /// <summary>
        /// set the ticket status to Reserved.
        /// </summary>
        /// <param name="ticket">The ticket to reserve.</param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MySqlException">A MySQL exception was thrown.</exception>
        public async Task ReserveAsync(Ticket ticket)
        {
            try
            {
                // Reserve the ticket
                await DataBaseTool.ExecuteNonQueryAsync
                (this.ConnectionString,
                "UPDATE ticket SET TicketStatus = @ticketStatus WHERE Id = @id;",
                new MySqlParameter("@ticketStatus", TicketStatus.Reserved.ToString()),
                new MySqlParameter("@id", ticket.Id)
                );
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while reserving the ticket", ex);
            }
        }

        /// <summary>
        /// Set the ticket status to Available.
        /// </summary>
        /// <param name="ticket">The ticket to make available.</param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MySqlException">A MySQL exception was thrown.</exception>
        public async Task MakeAvailableAsync(Ticket ticket)
        {
            try
            {
                // Make the ticket available
                await DataBaseTool.ExecuteNonQueryAsync
                (this.ConnectionString,
                "UPDATE ticket SET TicketStatus = @ticketStatus WHERE Id = @id;",
                new MySqlParameter("@ticketStatus", TicketStatus.Available.ToString()),
                new MySqlParameter("@id", ticket.Id)
                );
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while making the ticket available", ex);
            }
        }

        /// <summary>
        /// Set the ticket to inactive.
        /// </summary>
        /// <param name="ticket">The ticket to make inactive.</param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MySqlException">A MySQL exception was thrown.</exception>
        public async Task MakeInactiveAsync(Ticket ticket)
        {
            try
            {
                // Make the ticket inactive
                await DataBaseTool.ExecuteNonQueryAsync
                (this.ConnectionString,
                "UPDATE ticket SET IsActive = @isActive WHERE Id = @id;",
                new MySqlParameter("@isActive", false),
                new MySqlParameter("@id", ticket.Id)
                );
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while making the ticket inactive", ex);
            }
        }

        #endregion
    }
}
