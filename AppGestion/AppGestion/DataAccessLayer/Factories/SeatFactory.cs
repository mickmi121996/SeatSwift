using AppGestion.Tools;
using MySql.Data.MySqlClient;
using SeatSwiftDLL;
using SeatSwiftDLL.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGestion.DataAccessLayer.Factories
{
    /// <summary>
    /// Factory class for Seat
    /// </summary>
    /// <remarks>
    /// This class is a factory for the Seat class. It is used
    /// to create Seat objects from data read from a database.
    /// </remarks>
    public class SeatFactory : Base.FactoryBase
    {
        #region Create methods

                /// <summary>
        /// Create seat from data reader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <returns>seat</returns>
        public async Task<Seat> Create(MySqlDataReader dataReader)
        {
            int id = dataReader.GetInt32("Id");
            int auditoriumId = dataReader.GetInt32("AuditoriumId");
            int seatNumber = dataReader.GetInt32("SeatNumber");
            string seatStatus = dataReader.GetString("SeatStatus");
            string sectionName = dataReader.GetString("SectionName");
            string rowName = dataReader.GetString("RowName");
            int XCoordinate = dataReader.GetInt32("XCoordinate");
            int YCoordinate = dataReader.GetInt32("YCoordinate");

            // convert section name to enum
            SectionName section = (SectionName)Enum.Parse(typeof(SectionName), sectionName);
            SeatStatus status = (SeatStatus)Enum.Parse(typeof(SeatStatus), seatStatus);

            // Get the auditorium using the auditorium id
            Auditorium auditorium = await new AuditoriumFactory().GetByIdAsync(auditoriumId);

            // Create the order
            return new Seat(id, seatNumber, status,auditorium, section, rowName, XCoordinate, YCoordinate);
        }

        /// <summary>
        /// Create seat from a data row
        /// </summary>
        /// <param name="dataRow">Data row</param>
        /// <returns>seat</returns>
        public async Task<Seat> CreateFromRowAsync(DataRow dataRow)
        {
            int id = dataRow.Field<int>("Id");
            int auditoriumId = dataRow.Field<int>("AuditoriumId");
            int seatNumber = dataRow.Field<int>("SeatNumber");
            string sectionName = dataRow.Field<string>("SectionName")
                ?? throw new ArgumentNullException("The section name is null");
            string rowName = dataRow.Field<string>("RowName")
                ?? throw new ArgumentNullException("The row name is null");
            string seatStatus = dataRow.Field<string>("SeatStatus")
                ?? throw new ArgumentNullException("The seat status is null");
            int XCoordinate = dataRow.Field<int>("XCoordinate");
            int YCoordinate = dataRow.Field<int>("YCoordinate");

            // convert section name to enum
            SectionName section = (SectionName)Enum.Parse(typeof(SectionName), sectionName);
            SeatStatus status = (SeatStatus)Enum.Parse(typeof(SeatStatus), seatStatus);

            // Get the auditorium using the auditorium id
            Auditorium auditorium = await new AuditoriumFactory().GetByIdAsync(auditoriumId);

            // Create the order
            return new Seat(id, seatNumber, status, auditorium, section,rowName ,XCoordinate, YCoordinate);
        }

        /// <summary>
        /// Create seat from a data row withID
        /// </summary>
        /// <param name="dataRow">Data row</param>
        /// <returns>seat</returns>
        public async Task<Seat> CreateFromRowIDAsync(DataRow dataRow)
        {
            int id = dataRow.Field<int>("Id");
            int auditoriumId = dataRow.Field<int>("AuditoriumId");
            int seatNumber = dataRow.Field<int>("SeatNumber");
            string sectionName = dataRow.Field<string>("SectionName")
                ?? throw new ArgumentNullException("The section name is null");
            string rowName = dataRow.Field<string>("RowName")
                ?? throw new ArgumentNullException("The row name is null");
            string seatStatus = dataRow.Field<string>("SeatStatus")
                ?? throw new ArgumentNullException("The seat status is null");
            int XCoordinate = dataRow.Field<int>("XCoordinate");
            int YCoordinate = dataRow.Field<int>("YCoordinate");

            // convert section name to enum
            SectionName section = (SectionName)Enum.Parse(typeof(SectionName), sectionName);
            SeatStatus status = (SeatStatus)Enum.Parse(typeof(SeatStatus), seatStatus);

            // Create the order
            return new Seat(id, seatNumber, status, auditoriumId, section, rowName, XCoordinate, YCoordinate);
        }

        #endregion


        #region Factory methods

        /// <summary>
        /// Get all seats of an auditorium
        /// </summary>
        /// <param name="auditoriumId">The id of the auditorium</param>
        /// <returns>The list of seats</returns>    
        /// <exception cref="Exception">Throws an exception if the data could not be retrieved</exception>
        public async Task<List<Seat>> GetAllByAuditoriumIdAsync(int auditoriumId)
        {
            try
            {
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync(
                        this.ConnectionString,
                        "SELECT * FROM Seat WHERE AuditoriumId = @auditoriumId",
                        new MySqlParameter("@auditoriumId", auditoriumId)
                    )
                )
                {
                    List<Seat> seats = new List<Seat>();

                    foreach (DataRow row in result.Rows)
                    {
                        seats.Add(await CreateFromRowAsync(row));
                    }

                    return seats;
                }
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while getting the seats of the auditorium", e);
            }
        }

        /// <summary>
        /// Get all seats of an auditorium
        /// </summary>
        /// <param name="auditoriumId">The id of the auditorium</param>
        /// <returns>The list of seats</returns>    
        /// <exception cref="Exception">Throws an exception if the data could not be retrieved</exception>
        public async Task<List<Seat>> GetAllByAuditoriumIdSimpleAsync(int auditoriumId)
        {
            try
            {
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync(
                        this.ConnectionString,
                        "SELECT * FROM Seat WHERE AuditoriumId = @auditoriumId",
                        new MySqlParameter("@auditoriumId", auditoriumId)
                    )
                )
                {
                    List<Seat> seats = new List<Seat>();

                    foreach (DataRow row in result.Rows)
                    {
                        seats.Add(await CreateFromRowIDAsync(row));
                    }

                    return seats;
                }
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while getting the seats of the auditorium", e);
            }
        }


        /// <summary>
        /// Get a seat by its id
        /// </summary>
        /// <param name="id">The id of the seat</param>
        /// <returns>The seat</returns>
        /// <exception cref="Exception">Throws an exception if the data could not be retrieved</exception>
        /// <exception cref="Exception">Throws an exception if the seat does not exist</exception>
        public async Task<Seat> GetByIdAsync(int id)
        {
            try
            {
                // Get the seat
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync(
                        this.ConnectionString,
                        "SELECT * FROM Seat WHERE Id = @id",
                        new MySqlParameter("@id", id)
                    )
                )
                {
                    // Check if the seat exists
                    if (result.Rows.Count == 0)
                    {
                        throw new Exception("The seat does not exist");
                    }

                    return await CreateFromRowAsync(result.Rows[0]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while getting the seat", e);
            }
        }

        /// <summary>
        /// Create a seat
        /// </summary>
        /// <param name="seat">The seat to create</param>
        /// <exception cref="Exception">Throws an exception if the data could not be inserted</exception>
        /// <exception cref="Exception">Throws an exception if the seat already exists</exception>
        public async Task CreateAsync(Seat seat)
        {
            try
            {
                // Check if the seat already exists
                if (await GetByIdAsync(seat.Id) != null)
                {
                    throw new Exception("The seat already exists");
                }

                // Check if the auditorium is null
                if (seat.Auditorium == null)
                {
                    throw new Exception("The auditorium is null");
                }
                
                // Insert the seat
                await DataBaseTool.ExecuteNonQueryAsync(
                    this.ConnectionString,
                    "INSERT INTO Seat (AuditoriumId, SeatNumber, SeatStatus, SectionName, XCoordinate, YCoordinate) VALUES (@auditoriumId, @seatNumber, @seatStatus, @sectionName, @xCoordinate, @yCoordinate)",
                    new MySqlParameter("@auditoriumId", seat.Auditorium.Id),
                    new MySqlParameter("@seatNumber", seat.SeatNumber),
                    new MySqlParameter("@seatStatus", seat.Status.ToString()),
                    new MySqlParameter("@sectionName", seat.SectionName.ToString()),
                    new MySqlParameter("@xCoordinate", seat.XCoordinate),
                    new MySqlParameter("@yCoordinate", seat.YCoordinate)
                );
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while creating the seat", e);
            }
        }

        /// <summary>
        /// Update a seat the status of a seat
        /// </summary>
        /// <param name="seatId">The Id of the seat to update</param>
        /// <param name="status">The new status of the seat</param>
        /// <exception cref="Exception">Throws an exception if the data could not be updated</exception>
        /// <exception cref="Exception">Throws an exception if the seat does not exist</exception>  
        public async Task UpdateStatusAsync(int seatId, SeatStatus status)
        {
            try
            {
                // Check if the seat exists
                if (await GetByIdAsync(seatId) == null)
                {
                    throw new Exception("The seat does not exist");
                }

                // Update the status of the seat
                await DataBaseTool.ExecuteNonQueryAsync(
                    this.ConnectionString,
                    "UPDATE Seat SET SeatStatus = @status WHERE Id = @id",
                    new MySqlParameter("@status", status.ToString()),
                    new MySqlParameter("@id", seatId)
                );
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while updating the status of the seat", e);
            }
        }


        #endregion
    }
}
