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
    /// Factory class for Representation
    /// </summary>
    /// <remarks>
    /// This class is a factory for the Representation class. It is used
    /// to create Representation objects from data read from a database.
    /// </remarks>
    public class RepresentationFactory : Base.FactoryBase
    {
        #region Create methods

        /// <summary>
        /// Creates a Representation object from a data reader
        /// </summary>
        /// <param name="dataReader">The data reader</param>
        /// <returns>The Representation object</returns>
        public async Task<Representation> CreateFromDataReaderAsync(MySqlDataReader dataReader)
        {
            int id = dataReader.GetInt32("Id");
            DateTime date = dataReader.GetDateTime("Date");
            bool isActive = dataReader.GetBoolean("IsActive");
            string representationStatus = dataReader.GetString("RepresentationStatus");
            int showId = dataReader.GetInt32("ShowId");
            int auditoriumId = dataReader.GetInt32("AuditoriumId");

            // Convert the representation status string to an enum
            RepresentationStatus status = (RepresentationStatus)Enum.Parse(typeof(RepresentationStatus), representationStatus);

            // Get the show and auditorium objects using there Ids
            Show show = await new ShowFactory().GetByIdAsync(showId);
            Auditorium auditorium = await new AuditoriumFactory().GetByIdAsync(auditoriumId);

            // Create the representation object
            Representation representation = new Representation(id, isActive, date, status, show, auditorium);

            return representation;
        }

        /// <summary>
        /// Created a representation object from a DataRow 
        /// </summary>
        /// <param name="dataRow">The DataRow</param>
        /// <returns>The Representation object</returns>
        public async Task<Representation> CreateFromRowAsync(DataRow dataRow)
        {
            int id = dataRow.Field<int>("Id");
            DateTime date = dataRow.Field<DateTime>("Date");
            bool isActive = dataRow.Field<bool>("IsActive");
            string representationStatus = dataRow.Field<string>("RepresentationStatus")
                ?? throw new ArgumentNullException("The representation status is null");
            int showId = dataRow.Field<int>("ShowId");
            int auditoriumId = dataRow.Field<int>("AuditoriumId");
            
            // Convert the representation status string to an enum
            RepresentationStatus status = (RepresentationStatus)Enum.Parse(typeof(RepresentationStatus), representationStatus);

            // Get the show and auditorium objects using there Ids
            Show show = await new ShowFactory().GetByIdAsync(showId);
            Auditorium auditorium = await new AuditoriumFactory().GetByIdAsync(auditoriumId);

            // Create the representation object
            Representation representation = new Representation(id, isActive, date, status, show, auditorium);

            return representation;
        }  

        #endregion

        #region Factory methods

        /// <summary>
        /// Get the representation by its Id
        /// </summary>
        /// <param name="id">The Id of the representation</param>
        /// <returns>The representation object</returns>
        /// <exception cref="DataAccessLayerException">Thrown when an error occurs in the data access layer</exception>
        /// <exception cref="ArgumentException">Thrown when the Id is less than 1</exception>
        public async Task<Representation> GetByIdAsync(int id)
        {
            try
            {
                // Get the show with the given id
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM representation WHERE Id = @id;",
                    new MySqlParameter("@id", id)
                    )
                )
                {
                    // If no show is found, throw an exception
                    if (result.Rows.Count == 0)
                    {
                        throw new KeyNotFoundException("No representation with the id " + id + " was found");
                    }

                    // Create the Show object
                    return await CreateFromRowAsync(result.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the representation with the given id", ex);
            }
        }

        /// <summary>
        /// Get all the representations for a given show
        /// </summary>
        /// <param name="show">The show</param>
        /// <returns>The list of representations</returns>
        /// <exception cref="DataAccessLayerException">Thrown when an error occurs in the data access layer</exception>
        /// <exception cref="ArgumentNullException">Thrown when the show is null</exception>
        public async Task<List<Representation>> GetByShowAsync(Show show)
        {
            if (show == null)
            {
                throw new ArgumentNullException(nameof(show));
            }

            try
            {
                // Get the representations for the given show
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM representation WHERE ShowId = @showId;",
                    new MySqlParameter("@showId", show.Id)
                    )
                )
                {
                    // Create the list of representations
                    List<Representation> representations = new List<Representation>();

                    // Create the representations
                    foreach (DataRow row in result.Rows)
                    {
                        representations.Add(await CreateFromRowAsync(row));
                    }

                    // Return the list of representations ordered by date
                    return representations.OrderBy(r => r.Date).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the representations for the given show", ex);
            }
        }

        /// <summary>
        /// Get all in coming, active and Available representations for a given Show
        /// </summary>
        /// <param name="show">The show</param>
        /// <returns>The list of representations</returns>
        /// <exception cref="DataAccessLayerException">Thrown when an error occurs in the data access layer</exception>
        /// <exception cref="ArgumentNullException">Thrown when the show is null</exception>
        public async Task<List<Representation>> GetInComingActiveAvailableByShowAsync(Show show)
        {
            if (show == null)
            {
                throw new ArgumentNullException(nameof(show));
            }

            try
            {
                // Get the representations for the given show
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM representation WHERE ShowId = @showId AND Date >= @date AND IsActive = 1 AND RepresentationStatus = @status;",
                    new MySqlParameter("@showId", show.Id),
                    new MySqlParameter("@date", DateTime.Now),
                    new MySqlParameter("@status", RepresentationStatus.Available.ToString())
                    )
                )
                {
                    // Create the list of representations
                    List<Representation> representations = new List<Representation>();

                    // Create the representations
                    foreach (DataRow row in result.Rows)
                    {
                        representations.Add(await CreateFromRowAsync(row));
                    }

                    // Return the list of representations ordered by date
                    return representations.OrderBy(r => r.Date).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the representations for the given show", ex);
            }
        }

        /// <summary>
        /// Get all the representations for a given auditorium
        /// </summary>
        /// <param name="auditorium">The auditorium</param>
        /// <returns>The list of representations</returns>
        /// <exception cref="DataAccessLayerException">Thrown when an error occurs in the data access layer</exception>
        /// <exception cref="ArgumentNullException">Thrown when the auditorium is null</exception>
        public async Task<List<Representation>> GetByAuditoriumAsync(Auditorium auditorium)
        {
            if (auditorium == null)
            {
                throw new ArgumentNullException(nameof(auditorium));
            }

            try
            {
                // Get the representations for the given auditorium
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM representation WHERE AuditoriumId = @auditoriumId;",
                    new MySqlParameter("@auditoriumId", auditorium.Id)
                    )
                )
                {
                    // Create the list of representations
                    List<Representation> representations = new List<Representation>();

                    // Create the representations
                    foreach (DataRow row in result.Rows)
                    {
                        representations.Add(await CreateFromRowAsync(row));
                    }

                    // Return the list of representations ordered by date
                    return representations.OrderBy(r => r.Date).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the representations for the given auditorium", ex);
            }
        }

        /// <summary>
        /// Get all representations
        /// </summary>
        /// <returns>The list of representations</returns>
        /// <exception cref="DataAccessLayerException">Thrown when an error occurs in the data access layer</exception>
        /// <exception cref="ArgumentNullException">Thrown when the auditorium is null</exception>
        public async Task<List<Representation>> GetAllAsync()
        {
            try
            {
                // Get all the representations
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM representation;")
                )
                {
                    // Create the list of representations
                    List<Representation> representations = new List<Representation>();

                    // Create the representations
                    foreach (DataRow row in result.Rows)
                    {
                        representations.Add(await CreateFromRowAsync(row));
                    }

                    // Return the list of representations ordered by date
                    return representations.OrderBy(r => r.Date).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting all the representations", ex);
            }
        }

        /// <summary>
        /// Create a new representation
        /// </summary>
        /// <param name="representation">The representation to create</param>
        /// <exception cref="DataAccessLayerException">Thrown when an error occurs in the data access layer</exception>
        /// <exception cref="ArgumentNullException">Thrown when the representation is null</exception>
        public async Task CreateAsync(Representation representation)
        {
            if (representation == null)
            {
                throw new ArgumentNullException(nameof(representation));
            }

            // Check if the show and auditorium are not null
            if (representation.Show == null)
            {
                throw new ArgumentNullException(nameof(representation.Show));
            }
            if (representation.Auditorium == null)
            {
                throw new ArgumentNullException(nameof(representation.Auditorium));
            }

            try
            {
                // Create the representation
                await DataBaseTool.ExecuteNonQueryAsync
                (this.ConnectionString,
                "INSERT INTO representation (Date, IsActive, RepresentationStatus, ShowId, AuditoriumId) VALUES (@date, @isActive, @representationStatus, @showId, @auditoriumId);",
                new MySqlParameter("@date", representation.Date),
                new MySqlParameter("@isActive", representation.IsActive),
                new MySqlParameter("@representationStatus", representation.Status.ToString()),
                new MySqlParameter("@showId", representation.Show.Id),
                new MySqlParameter("@auditoriumId", representation.Auditorium.Id)
                );
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the representation", ex);
            }
        }

        /// <summary>
        /// Update a representation
        /// </summary>
        /// <param name="representation">The representation to update</param>
        /// <exception cref="DataAccessLayerException">Thrown when an error occurs in the data access layer</exception>
        /// <exception cref="ArgumentNullException">Thrown when the representation is null</exception>
        public async Task UpdateAsync(Representation representation)
        {
            if (representation == null)
            {
                throw new ArgumentNullException(nameof(representation));
            }

            // Check if the show and auditorium are not null
            if (representation.Show == null)
            {
                throw new ArgumentNullException(nameof(representation.Show));
            }
            if (representation.Auditorium == null)
            {
                throw new ArgumentNullException(nameof(representation.Auditorium));
            }

            try
            {
                // Update the representation
                await DataBaseTool.ExecuteNonQueryAsync
                (this.ConnectionString,
                "UPDATE representation SET Date = @date, IsActive = @isActive, RepresentationStatus = @representationStatus, ShowId = @showId, AuditoriumId = @auditoriumId WHERE Id = @id;",
                new MySqlParameter("@date", representation.Date),
                new MySqlParameter("@isActive", representation.IsActive),
                new MySqlParameter("@representationStatus", representation.Status.ToString()),
                new MySqlParameter("@showId", representation.Show.Id),
                new MySqlParameter("@auditoriumId", representation.Auditorium.Id),
                new MySqlParameter("@id", representation.Id)
                );
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the representation", ex);
            }
        }

        /// <summary>
        /// Set the representation as inactive
        /// </summary>
        /// <param name="representation">The representation to set as inactive</param>
        /// <exception cref="DataAccessLayerException">Thrown when an error occurs in the data access layer</exception>
        /// <exception cref="ArgumentNullException">Thrown when the representation is null</exception>
        public async Task SetAsInactiveAsync(Representation representation)
        {
            if (representation == null)
            {
                throw new ArgumentNullException(nameof(representation));
            }

            try
            {
                // Set the representation as inactive
                await DataBaseTool.ExecuteNonQueryAsync
                (this.ConnectionString,
                "UPDATE representation SET IsActive = 0 WHERE Id = @id;",
                new MySqlParameter("@id", representation.Id)
                );
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while setting the representation as inactive", ex);
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Check if the representation exists for a show , auditorium and date
        /// </summary>
        /// <param name="show">The show</param>
        /// <param name="auditorium">The auditorium</param>
        /// <param name="date">The date</param>
        /// <returns>True if the representation exists, false otherwise</returns>
        /// <exception cref="DataAccessLayerException">Thrown when an error occurs in the data access layer</exception>
        /// <exception cref="ArgumentNullException">Thrown when the show or auditorium is null</exception>
        public async Task<bool> ExistsAsync(Show show, Auditorium auditorium, DateTime date)
        {
            if (show == null)
            {
                throw new ArgumentNullException(nameof(show));
            }
            if (auditorium == null)
            {
                throw new ArgumentNullException(nameof(auditorium));
            }

            try
            {
                // Check if the representation exists
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM representation WHERE ShowId = @showId AND AuditoriumId = @auditoriumId AND Date = @date;",
                    new MySqlParameter("@showId", show.Id),
                    new MySqlParameter("@auditoriumId", auditorium.Id),
                    new MySqlParameter("@date", date)
                    )
                )
                {
                    return result.Rows.Count > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while checking if the representation exists", ex);
            }
        }

        #endregion
    }
}
