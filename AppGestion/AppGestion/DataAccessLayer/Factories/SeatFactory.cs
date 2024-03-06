using MySql.Data.MySqlClient;
using SeatSwiftDLL;
using System;
using System.Collections.Generic;
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
        /// Create a new Seat object from a data reader
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <returns>A new Seat object</returns>
        public static Seat Create(MySqlDataReader dataReader)
        {
            // Get the data from the data reader
            int id = dataReader.GetInt32("Id");

        #endregion


        #region Factory methods



        #endregion
    }
}
