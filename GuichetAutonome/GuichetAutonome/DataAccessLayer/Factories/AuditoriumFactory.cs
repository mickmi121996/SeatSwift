﻿using GuichetAutonome.Tools;
using MySql.Data.MySqlClient;
using SeatSwiftDLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GuichetAutonome.DataAccessLayer.Factories
{
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
            int numberOfRows = dataReader.GetInt32("NumberOfRows");
            int numberOfColumns = dataReader.GetInt32("NumberOfColumns");

            // Create the auditorium
            Auditorium auditorium = new Auditorium(
                id,
                isActive,
                name,
                numberOfRows,
                numberOfColumns
            );

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
            string name =
                row.Field<string>("AuditoriumName")
                ?? throw new ArgumentNullException("The name of the auditorium cannot be null");
            int numberOfRows = row.Field<int>("NumberOfRows");
            int numberOfColumns = row.Field<int>("NumberOfColumns");

            // Create the auditorium
            Auditorium auditorium = new Auditorium(
                id,
                isActive,
                name,
                numberOfRows,
                numberOfColumns
            );

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
                    DataTable result = await DataBaseTools.GetDataTableFromQueryAsync(
                        this.ConnectionString,
                        "SELECT * FROM Auditorium WHERE IsActive = 1"
                    )
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
            catch (Exception ex)
            {
                throw new Exception("Error in AuditoriumFactory.GetAllActiveAsync: " + ex.Message);
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
                    DataTable result = await DataBaseTools.GetDataTableFromQueryAsync(
                        this.ConnectionString,
                        "SELECT * FROM auditorium WHERE Id = @Id",
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
            catch (Exception ex)
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
                    DataTable result = await DataBaseTools.GetDataTableFromQueryAsync(
                        this.ConnectionString,
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
            catch (Exception ex)
            {
                throw new Exception("Error in AuditoriumFactory.GetByNameAsync: " + ex.Message);
            }
        }

        /// <summary>
        /// Get all the auditoriums of a show where there is at least one representation
        /// </summary>
        /// <param name="show">The show to get the auditoriums of</param>
        /// <returns>A list of all the auditoriums of the show</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the show is null</exception>
        /// <exception cref="System.Exception">Thrown when an error occurs</exception>
        public async Task<List<Auditorium>> GetByShowAsync(Show show)
        {
            try
            {
                if (show == null)
                {
                    throw new ArgumentNullException("The show cannot be null");
                }

                using (
                    DataTable result = await DataBaseTools.GetDataTableFromQueryAsync(
                        this.ConnectionString,
                        "SELECT * FROM Auditorium WHERE Id IN (SELECT DISTINCT AuditoriumId FROM Representation WHERE ShowId = @ShowId)",
                        new MySqlParameter("@ShowId", show.Id)
                    )
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
            catch (Exception ex)
            {
                throw new Exception("Error in AuditoriumFactory.GetByShowAsync: " + ex.Message);
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

                // Check if the auditorium already exists
                if (await ExistsByNameAsync(auditorium.Name))
                {
                    throw new Exception(
                        "An auditorium with the name " + auditorium.Name + " already exists"
                    );
                }

                await DataBaseTools.ExecuteNonQueryAsync(
                    this.ConnectionString,
                    "INSERT INTO Auditorium (AuditoriumName, IsActive, NumberOfRows, NumberOfColumns) VALUES (@Name, @IsActive, @NumberOfRows, @NumberOfColumns)",
                    new MySqlParameter("@Name", auditorium.Name),
                    new MySqlParameter("@IsActive", auditorium.IsActive),
                    new MySqlParameter("@NumberOfRows", auditorium.NumberOfRows),
                    new MySqlParameter("@NumberOfColumns", auditorium.NumberOfColumns)
                );
            }
            catch (Exception ex)
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
                await DataBaseTools.ExecuteNonQueryAsync(
                    this.ConnectionString,
                    "UPDATE Auditorium SET AuditoriumName = @Name, IsActive = @IsActive, NumberOfRows = @NumberOfRows, NumberOfColumns = @NumberOfColumns WHERE Id = @Id",
                    new MySqlParameter("@Name", auditorium.Name),
                    new MySqlParameter("@IsActive", auditorium.IsActive),
                    new MySqlParameter("@NumberOfRows", auditorium.NumberOfRows),
                    new MySqlParameter("@NumberOfColumns", auditorium.NumberOfColumns),
                    new MySqlParameter("@Id", auditorium.Id)
                );
            }
            catch (Exception ex)
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
                await DataBaseTools.ExecuteNonQueryAsync(
                    this.ConnectionString,
                    "UPDATE Auditorium SET IsActive = 0 WHERE Id = @Id",
                    new MySqlParameter("@Id", auditorium.Id)
                );
            }
            catch (Exception ex)
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
                    DataTable result = await DataBaseTools.GetDataTableFromQueryAsync(
                        this.ConnectionString,
                        "SELECT * FROM Auditorium WHERE AuditoriumName = @Name)",
                        new MySqlParameter("@Name", name)
                    )
                )
                {
                    // Return true if the result has any rows, false if it does not
                    return result.Rows.Count > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in AuditoriumFactory.ExistsByNameAsync: " + ex.Message);
            }
        }

        #endregion
    }
}
