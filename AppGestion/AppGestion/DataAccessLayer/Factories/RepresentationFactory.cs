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
        /// Created a reprensentation object from a DataRow 
        /// </summary>
        /// <param name="dataRow">The DataRow</param>
        /// <returns>The Representation object</returns>
        public async Task<Representation> CreateFromDataRowAsync(DataRow dataRow)
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



        #endregion
    }
}
