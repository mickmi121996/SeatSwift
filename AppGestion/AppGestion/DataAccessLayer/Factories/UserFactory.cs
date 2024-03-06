﻿using MySql.Data.MySqlClient;
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
    /// Factory class for User
    /// </summary>
    /// <remarks>
    /// This class is a factory for the User class. It is used
    /// to create User objects from data read from a database.
    /// </remarks>
    public class UserFactory : Base.FactoryBase
    {
        #region Create methods

        /// <summary>
        /// Create a User object from a data reader
        /// </summary>
        /// <param name="reader">The data reader</param>
        /// <returns>The User object</returns>
        public async Task<User> CreateFromReaderAsync(MySqlDataReader reader)
        {
            // Read the data from the data reader
            int id = reader.GetInt32("Id");
            bool isActive = reader.GetBoolean("IsActive");
            string firstName = reader.GetString("FirstName")
                ?? throw new ArgumentNullException("The first name of the user cannot be null");
            string lastName = reader.GetString("LastName")
                ?? throw new ArgumentNullException("The last name of the user cannot be null");
            string employeeNumber = reader.GetString("EmployeeNumber")
                ?? throw new ArgumentNullException("The username of the user cannot be null");
            string Type = reader.GetString("Type")
                ?? throw new ArgumentNullException("The type of the user cannot be null");
            string email = reader.GetString("Email")
                ?? throw new ArgumentNullException("The email of the user cannot be null");
            string phone = reader.GetString("Phone")
                ?? throw new ArgumentNullException("The phone of the user cannot be null");

            //Get the enum value from the type string
            EmployeeType employeeType = (EmployeeType)Enum.Parse(typeof(EmployeeType), Type);

            // Create the user object
            User user = new User(id, isActive, firstName, lastName, employeeNumber, employeeType ,email, phone);

            // Return the user object
            return user;
        }

        /// <summary>
        /// Create a User object from a row in a data table
        /// </summary>
        /// <param name="row">The row in the data table</param>
        /// <returns>The User object</returns>
        public async Task<User> CreateFromRowAsync(DataRow row)
        {
            // Read the data from the data reader
            int id = row.Field<int>("Id");
            bool isActive = row.Field<bool>("IsActive");
            string firstName = row.Field<string>("FirstName")
                ?? throw new ArgumentNullException("The first name of the user cannot be null");
            string lastName = row.Field<string>("LastName")
                ?? throw new ArgumentNullException("The last name of the user cannot be null");
            string employeeNumber = row.Field<string>("EmployeeNumber")
                ?? throw new ArgumentNullException("The username of the user cannot be null");
            string Type = row.Field<string>("Type")
                ?? throw new ArgumentNullException("The type of the user cannot be null");
            string email = row.Field<string>("Email")
                ?? throw new ArgumentNullException("The email of the user cannot be null");
            string phone = row.Field<string>("Phone")
                ?? throw new ArgumentNullException("The phone of the user cannot be null");
            

            //Get the enum value from the type string
            EmployeeType employeeType = (EmployeeType)Enum.Parse(typeof(EmployeeType), Type);

            // Create the user object
            User user = new User(id, isActive, firstName, lastName, employeeNumber, employeeType, email, phone);

            // Return the user object
            return user;
        }

        #endregion


        #region Factory methods

        /// <summary>
        /// Get all active users from the database
        /// </summary>
        /// <returns>A Icollection of all active users</returns>
        public async Task<ICollection<User>> GetAllActiveAsync()
        {
            // Create a new list of users
            List<User> users = new List<User>();

            // Create a new command
            using (
                DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                (this.ConnectionString,
                "SELECT * FROM User WHERE IsActive = 1"
                )
            )
            {
                // Read the data from the data table
                foreach (DataRow row in result.Rows)
                {
                    // Create a new user object
                    users.Add(await CreateFromRowAsync(row));

                }
            }

            // Return the list of users ordered by EmployeeNumber
            return users.OrderBy(u => u.EmployeeNumber).ToList();
        }

        /// <summary>
        /// Get all users from the database
        /// </summary>
        /// <returns>A Icollection of all users</returns>
        public async Task<ICollection<User>> GetAllAsync()
        {
            // Create a new list of users
            List<User> users = new List<User>();

            // Create a new command
            using (
                DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                (this.ConnectionString, 
                "SELECT * FROM User"
                )
            )
            {
                // Read the data from the data table
                foreach (DataRow row in result.Rows)
                {
                    // Create a new user object
                    users.Add(await CreateFromRowAsync(row));
                }
            }

            // Return the list of users ordered by EmployeeNumber
            return users.OrderBy(u => u.EmployeeNumber).ToList();
        }

        /// <summary>
        /// Get a user by its employee number
        /// </summary>
        /// <param name="employeeNumber">The employee number of the user</param>
        /// <returns>The user object</returns>
        /// <exception cref="ArgumentNullException">If no user was found with the employee number</exception>
        public async Task<User> GetByEmployeeNumberAsync(string employeeNumber)
        {
            // Create a new command
            using (
                DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                (this.ConnectionString,
                "SELECT * FROM User WHERE EmployeeNumber = @EmployeeNumber where IsActive = 1", 
                new MySqlParameter("@EmployeeNumber", employeeNumber)
                )
            )
            {
                // Read the data from the data table
                if (result.Rows.Count == 1)
                {
                    // Create a new user object
                    User user = await CreateFromRowAsync(result.Rows[0]);

                    // Return the user object
                    return user;
                }
            }

            // If no user was found, return null
            throw new ArgumentNullException("No user was found with the employee number " + employeeNumber);
        }

        /// <summary>
        /// Get a user by its id
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <returns>The user object</returns>
        /// <exception cref="ArgumentNullException">If no user was found with the id</exception>
        public async Task<User> GetByIdAsync(int id)
        {
            // Create a new command
            using (
                DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                (this.ConnectionString,
                "SELECT * FROM User WHERE Id = @Id", 
                new MySqlParameter("@Id", id)
                )
            )
            {
                // Read the data from the data table
                if (result.Rows.Count == 1)
                {
                    // Create a new user object
                    User user = await CreateFromRowAsync(result.Rows[0]);

                    // Return the user object
                    return user;
                }
            }

            // If no user was found, return null
            throw new ArgumentNullException("No user was found with the id " + id);
        }

        /// <summary>
        /// Create a new user in the database
        /// </summary>
        /// <param name="user">The user object</param>
        public async Task CreateAsync(User user)
        {
            try
            {
                // Check if the user exists
                if(await IsEmployeeNumberOrEmailAlreadyUsedAsync(user.EmployeeNumber, user.Email))
                {
                    throw new ArgumentNullException("The employee number or the email is already used");
                }

                // Create a new command
                int result = await DataBaseTool.ExecuteNonQueryAsync(this.ConnectionString,
                    "INSERT INTO User (IsActive, FirstName, LastName, EmployeeNumber, Type, Email, Phone) VALUES (@IsActive, @FirstName, @LastName, @EmployeeNumber, @Type, @Email, @Phone)",
                    new MySqlParameter("@IsActive", user.IsActive),
                    new MySqlParameter("@FirstName", user.FirstName),
                    new MySqlParameter("@LastName", user.LastName),
                    new MySqlParameter("@EmployeeNumber", user.EmployeeNumber),
                    new MySqlParameter("@Type", user.Type.ToString()),
                    new MySqlParameter("@Email", user.Email),
                    new MySqlParameter("@Phone", user.PhoneNumber)
                );

                // Check if the user was created
                if (result != 1)
                {
                    throw new Exception("The user was not created");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while creating the user", ex);
            }
        }

        /// <summary>
        /// Update a user in the database
        /// </summary>
        /// <param name="user">The user object</param>
        /// <exception cref="ArgumentNullException">If no user was found with the employee number</exception>
        /// <exception cref="Exception">If an error occured while updating the user</exception>
        public async Task UpdateAsync(User user)
        {
            try
            {
                // Create a new command
                int result = await DataBaseTool.ExecuteNonQueryAsync(this.ConnectionString,
                    "UPDATE User SET IsActive = @IsActive, FirstName = @FirstName, LastName = @LastName, Type = @Type, Email = @Email, Phone = @Phone WHERE EmployeeNumber = @EmployeeNumber",
                    new MySqlParameter("@IsActive", user.IsActive),
                    new MySqlParameter("@FirstName", user.FirstName),
                    new MySqlParameter("@LastName", user.LastName),
                    new MySqlParameter("@Type", user.Type.ToString()),
                    new MySqlParameter("@Email", user.Email),
                    new MySqlParameter("@Phone", user.PhoneNumber),
                    new MySqlParameter("@EmployeeNumber", user.EmployeeNumber)
                );

                // Check if the user was updated
                if (result != 1)
                {
                    throw new Exception("The user was not updated");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while updating the user", ex);
            }
        }

        /// <summary>
        /// Set a user as inactive in the database
        /// </summary>
        /// <param name="employeeNumber">The employee number of the user</param>
        /// <exception cref="ArgumentNullException">If no user was found with the employee number</exception>
        /// <exception cref="Exception">If an error occured while setting the user as inactive</exception>
        public async Task SetAsInactiveAsync(string employeeNumber)
        {
            try
            {
                // Create a new command
                int result = await DataBaseTool.ExecuteNonQueryAsync(this.ConnectionString,
                    "UPDATE User SET IsActive = 0 WHERE EmployeeNumber = @EmployeeNumber",
                    new MySqlParameter("@EmployeeNumber", employeeNumber)
                );

                // Check if the user was set as inactive
                if (result != 1)
                {
                    throw new Exception("The user was not set as inactive");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while setting the user as inactive", ex);
            }
        }

        /// <summary>
        /// Count the number of active users in the database
        /// </summary>
        /// <returns>The number of active users</returns>
        /// <exception cref="Exception">If an error occured while counting the number of active users</exception>
        public async Task<int> CountActiveAsync()
        {
            try
            {
                // Get the total number of active users
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT COUNT(*) FROM User WHERE IsActive = 1"
                    )
                )
                {
                    // Return the number of active users
                    return result.Rows[0].Field<int>(0);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while counting the number of active users", ex);
            }
        }

        #endregion


        #region Methods

        /// <summary>
        /// Method to check the EmployeeNumber and the email of a user is already used
        /// </summary>
        /// <param name="employeeNumber">The employee number of the user</param>
        /// <param name="email">The email of the user</param>
        /// <returns>True if the employee number or the email is already used, false otherwise</returns>
        public async Task<bool> IsEmployeeNumberOrEmailAlreadyUsedAsync(string employeeNumber, string email)
        {
            try
            {
                // Get the total number of active users
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT COUNT(*) FROM User WHERE EmployeeNumber = @EmployeeNumber OR Email = @Email",
                    new MySqlParameter("@EmployeeNumber", employeeNumber),
                    new MySqlParameter("@Email", email)
                    )
                )
                {
                    // Return true if the employee number or the email is already used
                    return result.Rows[0].Field<int>(0) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while checking if the employee number or the email is already used", ex);
            }
        }

        #endregion
    }
}
