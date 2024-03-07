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
    /// Factory class for Section
    /// </summary>
    /// <remarks>
    /// This class is a factory for the Section class. It is used
    /// to create Section objects from data read from a database.
    /// </remarks>
    public class SectionFactory : Base.FactoryBase
    {
        #region Create methods

        /// <summary>
        /// Creates a new Section object from a data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <returns>The newly created Section object.</returns>
        public async Task<Section> CreateFromReaderAsync(MySqlDataReader dataReader)
        {
            // Get the data from the data reader
            int id = dataReader.GetInt32("Id");
            bool isActif = dataReader.GetBoolean("IsActive");
            decimal sectionMultiplier = dataReader.GetDecimal("SectionMultiplier");
            string sectionName = dataReader.GetString("SectionName");
            string sectionStatus = dataReader.GetString("SectionStatus");
            int auditoriumId = dataReader.GetInt32("AuditoriumId");

            // Get the enum values
            SectionStatus status = (SectionStatus)Enum.Parse(typeof(SectionStatus), sectionStatus);
            SectionName name = (SectionName)Enum.Parse(typeof(SectionName), sectionName);

            // Get the Auditorium with the given id
            Auditorium auditorium = await new AuditoriumFactory().GetByIdAsync(auditoriumId);
            if (auditorium is null)
            {
                throw new Exception("The auditorium with the given id does not exist.");
            }

            // Create the Section object
            Section section = new Section(id, isActif, name, status, sectionMultiplier, auditorium);

            return section;
        }

        /// <summary>
        /// Creates a new Section object from a row in a data table.
        /// </summary>
        /// <param name="row">The row in the data table.</param>
        /// <returns>The newly created Section object.</returns>
        public async Task<Section> CreateFromRowAsync(DataRow row)
        {
            // Get the data from the row
            int id = row.Field<int>("Id");
            bool isActif = row.Field<bool>("IsActive");
            decimal sectionMultiplier = row.Field<decimal>("SectionMultiplier");
            string sectionName = row.Field<string>("SectionName")
                ?? throw new Exception("The section name is null.");
            string sectionStatus = row.Field<string>("SectionStatus")
                ?? throw new Exception("The section status is null.");
            int auditoriumId = row.Field<int>("AuditoriumId");

            // Get the enum values
            SectionStatus status = (SectionStatus)Enum.Parse(typeof(SectionStatus), sectionStatus);
            SectionName name = (SectionName)Enum.Parse(typeof(SectionName), sectionName);

            // Get the Auditorium with the given id
            Auditorium auditorium = await new AuditoriumFactory().GetByIdAsync(auditoriumId);
            if (auditorium is null)
            {
                throw new Exception("The auditorium with the given id does not exist.");
            }

            // Create the Section object
            Section section = new Section(id, isActif, name, status, sectionMultiplier, auditorium);

            return section;
        }
        #endregion


        #region Factory methods

        /// <summary>
        /// Gets all the sections from the database for a given auditorium Id.
        /// </summary>
        /// <param name="auditorium">The auditorium.</param>
        /// <returns>A list of all the sections for the given auditorium.</returns>
        public async Task<List<Section>> GetAllByAuditoriumAsync(int auditoriumId)
        {
            try
            {
                // Get the auditorium with the given id
                Auditorium auditorium = await new AuditoriumFactory().GetByIdAsync(auditoriumId);
                if (auditorium is null)
                {
                    throw new Exception("The auditorium with the given id does not exist.");
                }

                // Get all the sections for the given auditorium
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM Section WHERE AuditoriumId = @auditoriumId",
                    new MySqlParameter("@auditoriumId", auditoriumId)
                    )
                )
                {
                    List<Section> sections = new List<Section>();
                    foreach (DataRow row in result.Rows)
                    {
                        Section section = await CreateFromRowAsync(row);
                        sections.Add(section);
                    }
                    return sections;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in SectionFactory.GetAllByAuditoriumAsync: " + ex.Message);
            }
        }

        /// <summary>
        /// Gets the section with the given id
        /// </summary>
        /// <param name="id">The id of the section.</param>
        /// <param name="auditoriumId">The id of the auditorium.</param>
        /// <returns>The section with the given id</returns>
        /// <exception cref="Exception">The section with the given id does not exist.</exception>
        /// <exception cref="Exception">The auditorium with the given id does not exist.</exception>
        public async Task<Section> GetByIdAsync(int id)
        {
            try
            {
                // Get the section with the given id AND the given auditorium id
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM Section WHERE Id = @id",
                    new MySqlParameter("@id", id)
                    )
                )
                {
                    if (result.Rows.Count == 0)
                    {
                        throw new Exception("The section with the given id AND the given auditorium id does not exist.");
                    }
                    return await CreateFromRowAsync(result.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in SectionFactory.GetByIdAndAuditoriumAsync: " + ex.Message);
            }
        }

        #endregion

        #region methods



        #endregion
    }
}
