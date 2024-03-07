﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeatSwiftDLL;
using System.Data;
using AppGestion.Tools;

namespace AppGestion.DataAccessLayer.Factories
{
    /// <summary>
    /// Factory class for Client
    /// </summary>
    /// <remarks>
    /// This class is a factory for the Client class. It is used
    /// to create Client objects from data read from a database.
    /// </remarks>
    public class ClientFactory : Base.FactoryBase
    {
        #region Create methods

        /// <summary>
        /// Method to create a Client object from a database record
        /// </summary>
        /// <param name="dataReader">The SqlDataReader object that contains the data to be used to create the Client object</param>
        /// <returns>The Client object created from the data</returns>
        public async Task<Client> CreateFromReaderAsync(MySqlDataReader dataReader)
        {
            // Read the data from the data reader
            int id = dataReader.GetInt32("Id");
            bool isActive = dataReader.GetBoolean("IsActive");
            string FirstName = dataReader.GetString("FirstName")
                ?? throw new ArgumentNullException("The first name of the client cannot be null");
            string LastName = dataReader.GetString("LastName")
                ?? throw new ArgumentNullException("The last name of the client cannot be null");
            string email = dataReader.GetString("Email")
                ?? throw new ArgumentNullException("The email of the client cannot be null");
            string phone = dataReader.GetString("Phone");
            string city = dataReader.GetString("City");

            // Create the client
            Client client = new Client(id, isActive, FirstName, LastName, email, phone, city);

            // Return the client
            return client;
        }

        /// <summary>
        /// Method to create a client object from a row in a DataTable
        /// </summary>
        /// <param name="row">The DataRow object that contains the data to be used to create the client object</param>
        /// <returns>The client object created from the data</returns>
        public async Task<Client> CreateFromRowAsync(DataRow row)
        {
            // Read the data from the data row
            int id = row.Field<int>("Id");
            bool isActive = row.Field<bool>("IsActive");
            string FirstName = row.Field<string>("FirstName")
                ?? throw new ArgumentNullException("The first name of the client cannot be null");
            string LastName = row.Field<string>("LastName")
                ?? throw new ArgumentNullException("The last name of the client cannot be null");
            string email = row.Field<string>("Email")
                ?? throw new ArgumentNullException("The email of the client cannot be null");
            string? phone = row.Field<string>("Phone");
            string? city = row.Field<string>("City");

            // Create the client
            Client client = new Client(id, isActive, FirstName, LastName, email, phone, city);

            // Return the client
            return client;
        }

        #endregion


        #region Factory methods

        /// <summary>
        /// Method to get a client by its ID
        /// </summary>
        /// <param name="id">The ID of the client to get</param>
        /// <returns>The client with the specified ID</returns>
        /// <exception cref="Exception">An error occurred while getting the client</exception>
        /// <exception cref="Exception">An error occurred while getting the client</exception>
        public async Task<Client> GetByIdAsync(int id)
        {
            try
            {
                // Get the client
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync(
                        this.ConnectionString,
                        "SELECT * FROM Client WHERE Id = @id",
                        new MySqlParameter("@id", id)
                    )
                )
                {
                    // Check if the result is not null
                    if (result != null)
                    {
                        // Check if the result has rows
                        if (result.Rows.Count > 0)
                        {
                            // Create the client from the row
                            return await CreateFromRowAsync(result.Rows[0]);
                        }
                        else
                        {
                            throw new Exception("An error occurred while getting the client");
                        }
                    }
                    else
                    {
                        throw new Exception("An error occurred while getting the client");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the client", ex);
            }
        }

        /// <summary>
        /// Method to get all active clients
        /// </summary>
        /// <returns>All active clients</returns>
        /// <exception cref="Exception">An error occurred while getting all active clients</exception>
        /// <exception cref="Exception">An error occurred while getting all active clients</exception>
        public async Task<List<Client>> GetAllActiveAsync()
        {
            try
            {
                // Get all active clients
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync(
                        this.ConnectionString,
                        "SELECT * FROM Client WHERE IsActive = 1"
                    )
                )
                {
                    // Check if the result is not null
                    if (result != null)
                    {
                        // Create a list of clients
                        List<Client> clients = new List<Client>();

                        // Loop through the rows
                        foreach (DataRow row in result.Rows)
                        {
                            // Create the client from the row
                            clients.Add(await CreateFromRowAsync(row));
                        }

                        // Return the list of clients
                        return clients;
                    }
                    else
                    {
                        throw new Exception("An error occurred while getting all active clients");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting all active clients", ex);
            }
        }

        /// <summary>
        /// Method to get all clients with at least one order
        /// </summary>
        /// <returns>All clients with at least one order</returns>
        /// <exception cref="Exception">An error occurred while getting the clients</exception>
        public async Task<List<Client>> GetAllClientsWithOrdersAsync()
        {
            try
            {
                // SQL query to get all clients with at least one order
                string query = @"
                    SELECT DISTINCT c.* 
                    FROM client c
                    JOIN orders o ON c.Id = o.ClientId
                    WHERE c.IsActive = 1
                ";

                using (DataTable result = await DataBaseTool.GetDataTableFromQueryAsync(this.ConnectionString, query))
                {
                    // Check if the result is not null
                    if (result != null && result.Rows.Count > 0)
                    {
                        // Create a list of clients
                        List<Client> clients = new List<Client>();

                        // Loop through the rows
                        foreach (DataRow row in result.Rows)
                        {
                            // Create the client from the row
                            clients.Add(await CreateFromRowAsync(row));
                        }

                        // Return the list of clients
                        return clients;
                    }
                    else
                    {
                        throw new Exception("No clients with orders were found.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the clients with orders", ex);
            }
        }


        /// <summary>
        /// Method to create a client
        /// </summary>
        /// <param name="client">The client to create</param>
        /// <exception cref="Exception">An error occurred while creating the client</exception>
        /// <exception cref="ArgumentNullException">The client already exists</exception>
        /// <exception cref="Exception">An error occurred while creating the client</exception>
        public async Task CreateAsync(Client client)
        {
            try
            {
                // Check if the client already exists
                if (await ClientExistsAsync(client.Email))
                {
                    throw new ArgumentNullException("The client already exists");
                }

                // Create the client
                await DataBaseTool.ExecuteNonQueryAsync(
                    this.ConnectionString,
                    "INSERT INTO Client (IsActive, FirstName, LastName, Email, Phone, City) VALUES (@isActive, @firstName, @lastName, @email, @phone, @city)",
                    new MySqlParameter("@isActive", client.IsActive),
                    new MySqlParameter("@firstName", client.FirstName),
                    new MySqlParameter("@lastName", client.LastName),
                    new MySqlParameter("@email", client.Email),
                    new MySqlParameter("@phone", client.PhoneNumber),
                    new MySqlParameter("@city", client.City)
                );
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the client", ex);
            }
        }

        /// <summary>
        /// Method to update a client
        /// </summary>
        /// <param name="client">The client to update</param>
        /// <exception cref="Exception">An error occurred while updating the client</exception>
        /// <exception cref="Exception">An error occurred while updating the client</exception>
        public async Task UpdateAsync(Client client)
        {
            try
            {
                // Update the client
                await DataBaseTool.ExecuteNonQueryAsync(
                    this.ConnectionString,
                    "UPDATE Client SET IsActive = @isActive, FirstName = @firstName, LastName = @lastName, Phone = @phone, City = @city WHERE Email = @email",
                    new MySqlParameter("@isActive", client.IsActive),
                    new MySqlParameter("@firstName", client.FirstName),
                    new MySqlParameter("@lastName", client.LastName),
                    new MySqlParameter("@phone", client.PhoneNumber),
                    new MySqlParameter("@city", client.City),
                    new MySqlParameter("@email", client.Email)
                );
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the client", ex);
            }
        }

        /// <summary>
        /// Make the client inactive
        /// </summary>
        /// <param name="client">The client to make inactive</param>
        /// <exception cref="Exception">An error occurred while making the client inactive</exception>
        public async Task SetAsInactiveAsync(Client client)
        {
            try
            {
                // Make the client inactive
                await DataBaseTool.ExecuteNonQueryAsync(
                    this.ConnectionString,
                    "UPDATE Client SET IsActive = 0 WHERE Id = @Id",
                    new MySqlParameter("@Id", client.Id)
                );
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while making the client inactive", ex);
            }
        }

        /// <summary>
        /// Get the count of active clients
        /// </summary>
        /// <returns>The count of active clients</returns>
        /// <exception cref="Exception">An error occurred while getting the count of active clients</exception>
        public async Task<int> CountActiveAsync()
        {
            try
            {
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync(
                        this.ConnectionString,
                        "SELECT COUNT(*) FROM Client WHERE IsActive = 1"
                    )
                )
                {
                    // Check if the result is not null
                    if (result != null)
                    {
                        // Return the count of active clients
                        return result.Rows[0].Field<int>(0);
                    }
                    else
                    {
                        throw new Exception("An error occurred while getting the count of active clients");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the count of active clients", ex);
            }
        }

        #endregion


        #region methods

        /// <summary>
        /// Check if a client exists by its email
        /// </summary>
        /// <param name="email">The email of the client to check</param>
        /// <returns>True if the client exists, false otherwise</returns>
        /// <exception cref="Exception">An error occurred while checking if the client exists</exception>
        public async Task<bool> ClientExistsAsync(string email)
        {
            try
            {
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync(
                        this.ConnectionString,
                        "SELECT * FROM Client WHERE Email = @email",
                        new MySqlParameter("@email", email)
                    )
                )
                {
                    // Check if the result is not null
                    if (result != null)
                    {
                        // Return true if the result has rows, false otherwise
                        return result.Rows.Count > 0;
                    }
                    else
                    {
                        throw new Exception("An error occurred while checking if the client exists");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while checking if the client exists", ex);
            }
        }

        #endregion
    }
}
