using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuichetAutonome.Tools
{
    public class DataBaseTools
    {
        #region Properties

        /// <summary>
        /// The connection string to the database.
        /// </summary>
        public string ConnectionString { get; set; }

        #endregion


        #region Constructor

        /// <summary>
        /// Parameterized constructor for the <see cref="DataBaseTools"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        public DataBaseTools(string connectionString)
        {
            // Sets the connection string.
            this.ConnectionString = connectionString;
        }

        #endregion


        #region Methods

        #region Static methods

        /// <summary>
        /// This method is used to get a result table from a query.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="parameters">The parameters of the query.</param>
        /// <returns>The result <see cref="DataTable"/>.</returns>
        public static async Task<DataTable> GetDataTableFromQueryAsync(
            string connectionString,
            string query,
            params MySqlParameter[] parameters
        )
        {
            // Creates the result dataTable.
            DataTable result = new DataTable();

            // Creates the connection to the database.
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // Opens the connection.
                await connection.OpenAsync();

                // Creates the command.
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Adds the parameters.
                    command.Parameters.AddRange(parameters);

                    // Executes the command.
                    using (DbDataReader reader = await command.ExecuteReaderAsync())
                    {
                        // Fills the result dataTable.
                        result.Load(reader);
                    }
                }
            }

            // Returns the result dataTable.
            return result;
        }

        /// <summary>
        /// This method is used to execute a query that doesn't yield any result.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="parameters">The parameters of the query.</param>
        /// <returns>The number of rows affected.</returns>
        public static async Task<int> ExecuteNonQueryAsync(
            string connectionString,
            string query,
            params MySqlParameter[] parameters
        )
        {
            // Creates the connection to the database.
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // Opens the connection.
                await connection.OpenAsync();

                // Creates the command.
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Adds the parameters.
                    command.Parameters.AddRange(parameters);

                    // Executes the command.
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        #endregion

        #region Instance methods

        /// <summary>
        /// This method is used to get a result table from a query.
        /// </summary>
        /// <param name="query">The query to execute.</param>
        /// <param name="parameters">The parameters of the query.</param>
        /// <returns>The result <see cref="DataTable"/>.</returns>
        public async Task<DataTable> GetDataTableFromQueryAsync(
            string query,
            params MySqlParameter[] parameters
        )
        {
            // Creates the result dataTable.
            DataTable result = new DataTable();

            // Creates the connection to the database.
            using (MySqlConnection connection = new MySqlConnection(this.ConnectionString))
            {
                // Opens the connection.
                await connection.OpenAsync();

                // Creates the command.
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Adds the parameters.
                    command.Parameters.AddRange(parameters);

                    // Executes the command.
                    using (DbDataReader reader = await command.ExecuteReaderAsync())
                    {
                        // Fills the result dataTable.
                        result.Load(reader);
                    }
                }
            }

            // Returns the result dataTable.
            return result;
        }

        /// <summary>
        /// This method is used to execute a query that doesn't yield any result.
        /// </summary>
        /// <param name="query">The query to execute.</param>
        /// <param name="parameters">The parameters of the query.</param>
        /// <returns>The number of rows affected.</returns>
        public async Task<int> ExecuteNonQueryAsync(
            string query,
            params MySqlParameter[] parameters
        )
        {
            // Creates the connection to the database.
            using (MySqlConnection connection = new MySqlConnection(this.ConnectionString))
            {
                // Opens the connection.
                await connection.OpenAsync();

                // Creates the command.
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Adds the parameters.
                    command.Parameters.AddRange(parameters);

                    // Executes the command.
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        #endregion

        #endregion
    }
}
