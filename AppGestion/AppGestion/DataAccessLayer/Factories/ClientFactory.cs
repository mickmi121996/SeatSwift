using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeatSwiftDLL;
using System.Data;

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
        public Client CreateFromRow(DataRow row)
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



        #endregion
    }
}
