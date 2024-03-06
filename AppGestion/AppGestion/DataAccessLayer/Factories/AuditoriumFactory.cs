using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Text;
using System.Threading.Tasks;
using SeatSwiftDLL;
using System.Data;
using AppGestion.Tools;

namespace AppGestion.DataAccessLayer.Factories
{
    /// <summary>
    /// Factory class for Auditorium
    /// </summary>
    /// <remarks>
    /// This class is a factory for the Auditorium class. It is used
    /// to create Auditorium objects from data read from a database.
    /// </remarks>
    public class AuditoriumFactory : Base.FactoryBase
    {
        #region Create methods

        /// <summary>
        /// Method to create a Auditorium object from a database record
        /// </summary>
        /// <param name="dataReader">The SqlDataReader object that contains the data to be used to create the Auditorium object</param>
        /// <returns>The Auditorium object created from the data</returns>
        public async Task<Auditorium> CreateFromReaderAsync(MySqlDataReader dataReader)
        {
            // Read the data from the data reader
            int id = dataReader.GetInt32("Id");
            bool isActive = dataReader.GetBoolean("IsActive");
            string name = dataReader.GetString("AuditoriumName");

            // Create the auditorium
            Auditorium auditorium = new Auditorium(id, isActive, name);

            // Return the auditorium
            return auditorium;
        }

        /// <summary>
        /// Method to create an auditorium object from a row in a DataTable
        /// </summary>
        /// <param name="row">The DataRow object that contains the data to be used to create the auditorium object</param>
        /// <returns>The auditorium object created from the data</returns>
        public async Task<Auditorium> CreateFromRowAsync(DataRow row)
        {
            // Read the data from the data row
            int id = row.Field<int>("Id");
            bool isActive = row.Field<bool>("IsActive");
            string name = row.Field<string>("AuditoriumName")??
            throw new ArgumentNullException("The name of the auditorium cannot be null");

            // Create the auditorium
            Auditorium auditorium = new Auditorium(id, isActive, name);

            // Return the auditorium
            return auditorium;
        }

        #endregion


        #region Factory methods

        /// <summary>
        /// Get all the active auditoriums from the database
        /// </summary>
        /// <returns>A list of all the active auditoriums</returns>
        public async Task<List<Auditorium>> GetAllActiveAsync()
        {
            try
            {
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM Auditorium WHERE IsActive = 1")
                )
                {
                    // Create a list to hold the auditoriums
                    List<Auditorium> auditoriums = new List<Auditorium>();

                    // Loop through the rows in the result
                    foreach (DataRow row in result.Rows)
                    {
                        // Create an auditorium from the row and add it to the list
                        auditoriums.Add(await CreateFromRowAsync(row));
                    }

                    // Return the list of auditoriums
                    return auditoriums;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error in AuditoriumFactory.GetAllActiveAsync: " + ex.Message);
            }
        }

        /// <summary>
        /// Get all the auditoriums from the database
        /// </summary>
        /// <returns>A list of all the auditoriums</returns>
        public async Task<List<Auditorium>> GetAllAsync()
        {
            try
            {
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM Auditorium")
                )
                {
                    // Create a list to hold the auditoriums
                    List<Auditorium> auditoriums = new List<Auditorium>();

                    // Loop through the rows in the result
                    foreach (DataRow row in result.Rows)
                    {
                        // Create an auditorium from the row and add it to the list
                        auditoriums.Add(await CreateFromRowAsync(row));
                    }

                    // Return the list of auditoriums
                    return auditoriums;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error in AuditoriumFactory.GetAllAsync: " + ex.Message);
            }
        }

        /// <summary>
        /// Get an auditorium by its id
        /// </summary>
        /// <param name="id">The id of the auditorium to get</param>
        /// <returns>The auditorium with the given id</returns>
        /// <exception cref="System.ArgumentException">Thrown when the id is less than 1</exception>
        /// <exception cref="System.Exception">Thrown when an error occurs</exception>
        public async Task<Auditorium> GetByIdAsync(int id)
        {
            try
            {
                if (id < 1)
                {
                    throw new ArgumentException("The id must be greater than 0");
                }

                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM Auditorium WHERE Id = @Id)",
                    new MySqlParameter("@Id", id)
                    )   
                )
                {
                    // Check if the result has any rows
                    if (result.Rows.Count == 0)
                    {
                        throw new Exception("No auditorium found with the id " + id);
                    }

                    // Create an auditorium from the row and return it
                    return await CreateFromRowAsync(result.Rows[0]);
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error in AuditoriumFactory.GetByIdAsync: " + ex.Message);
            }
        }

        /// <summary>
        /// Get an auditorium by its name
        /// </summary>
        /// <param name="name">The name of the auditorium to get</param>
        /// <returns>The auditorium with the given name</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the name is null</exception>
        /// <exception cref="System.Exception">Thrown when an error occurs</exception>
        public async Task<Auditorium> GetByNameAsync(string name)
        {
            try
            {
                if (name == null)
                {
                    throw new ArgumentNullException("The name of the auditorium cannot be null");
                }

                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM Auditorium WHERE AuditoriumName = @Name)",
                    new MySqlParameter("@Name", name)
                    )
                )
                {
                    // Check if the result has any rows
                    if (result.Rows.Count == 0)
                    {
                        throw new Exception("No auditorium found with the name " + name);
                    }

                    // Create an auditorium from the row and return it
                    return await CreateFromRowAsync(result.Rows[0]);
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error in AuditoriumFactory.GetByNameAsync: " + ex.Message);
            }
        }

        /// <summary>
        /// Create a new auditorium in the database
        /// </summary>
        /// <param name="auditorium">The auditorium to create</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the auditorium is null</exception>
        /// <exception cref="System.Exception">Thrown when an error occurs</exception>
        public async Task CreateAsync(Auditorium auditorium)
        {
            try
            {
                if (auditorium == null)
                {
                    throw new ArgumentNullException("The auditorium cannot be null");
                }

                await DataBaseTool.ExecuteNonQueryAsync
                (this.ConnectionString,
                "INSERT INTO Auditorium (AuditoriumName, IsActive) VALUES (@Name, @IsActive)",
                new MySqlParameter("@Name", auditorium.Name),
                new MySqlParameter("@IsActive", auditorium.IsActive)
                );
            }
            catch(Exception ex)
            {
                throw new Exception("Error in AuditoriumFactory.CreateAsync: " + ex.Message);
            }
        }

        /// <summary>
        /// Update an auditorium in the database
        /// </summary>
        /// <param name="auditorium">The auditorium to update</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the auditorium is null</exception>
        /// <exception cref="System.Exception">Thrown when an error occurs</exception>
        public async Task UpdateAsync(Auditorium auditorium)
        {
            try
            {
                await DataBaseTool.ExecuteNonQueryAsync
                (this.ConnectionString,
                "UPDATE Auditorium SET AuditoriumName = @Name, IsActive = @IsActive WHERE Id = @Id",
                new MySqlParameter("@Name", auditorium.Name),
                new MySqlParameter("@IsActive", auditorium.IsActive),
                new MySqlParameter("@Id", auditorium.Id)
                );
            }
            catch(Exception ex)
            {
                throw new Exception("Error in AuditoriumFactory.UpdateAsync: " + ex.Message);
            }
        }

        /// <summary>
        /// Set an auditorium as inactive in the database
        /// </summary>
        /// <param name="auditorium">The auditorium to set as inactive</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the auditorium is null</exception>
        /// <exception cref="System.Exception">Thrown when an error occurs</exception>
        public async Task SetInactiveAsync(Auditorium auditorium)
        {
            try
            {
                await DataBaseTool.ExecuteNonQueryAsync
                (this.ConnectionString,
                "UPDATE Auditorium SET IsActive = 0 WHERE Id = @Id",
                new MySqlParameter("@Id", auditorium.Id)
                );
            }
            catch(Exception ex)
            {
                throw new Exception("Error in AuditoriumFactory.SetInactiveAsync: " + ex.Message);
            }
        }

        #endregion


        #region methods

        /// <summary>
        /// Method to check if an auditorium exists in the database by its name
        /// </summary>
        /// <param name="name">The name of the auditorium to check</param>
        /// <returns>True if the auditorium exists, false if it does not</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the name is null</exception>
        /// <exception cref="System.Exception">Thrown when an error occurs</exception>
        public async Task<bool> ExistsByNameAsync(string name)
        {
            try
            {
                if (name == null)
                {
                    throw new ArgumentNullException("The name of the auditorium cannot be null");
                }

                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM Auditorium WHERE AuditoriumName = @Name)",
                    new MySqlParameter("@Name", name)
                    )
                )
                {
                    // Return true if the result has any rows, false if it does not
                    return result.Rows.Count > 0;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error in AuditoriumFactory.ExistsByNameAsync: " + ex.Message);
            }
        }

        #endregion
    }
}
