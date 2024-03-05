using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Text;
using System.Threading.Tasks;
using SeatSwiftDLL;
using System.Data;

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
        public Auditorium CreateFromRow(DataRow row)
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



        #endregion
    }
}
