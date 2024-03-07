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
    /// Factory class for SectionRow
    /// </summary>
    /// <remarks>
    /// This class is a factory for the SectionRow class. It is used
    /// to create SectionRow objects from data read from a database.
    /// </remarks>
    public class SectionRowFactory : Base.FactoryBase
    {
        #region Create methods

        /// <summary>
        /// Creates a new SectionRow object from a data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <returns>The newly created SectionRow object.</returns>
        public async Task<SectionRow> CreateFromReaderAsync(MySqlDataReader dataReader)
        {
            // Get the data from the data reader
            int id = dataReader.GetInt32("Id");
            string rowName = dataReader.GetString("RowName");
            string rowStatus = dataReader.GetString("RowStatus");
            bool isActif = dataReader.GetBoolean("IsActive");
            int sectionId = dataReader.GetInt32("SectionId");

            // Get the enum values
            RowStatus status = (RowStatus)Enum.Parse(typeof(RowStatus), rowStatus);

            // Get the Section with the given id
            Section section = await new SectionFactory().GetByIdAsync(sectionId);
            if (section is null)
            {
                throw new Exception("The section with the given id does not exist.");
            }

            // Create the SectionRow object
            SectionRow sectionRow = new SectionRow(id, isActif, rowName, section, status);

            return sectionRow;
        }

        /// <summary>
        /// Creates a new SectionRow object from a row in a data table.
        /// </summary>
        /// <param name="row">The row in the data table.</param>
        /// <returns>The newly created SectionRow object.</returns>
        public async Task<SectionRow> CreateFromRow(DataRow row)
        {
            // Get the data from the row
            int id = row.Field<int>("Id");
            string rowName = row.Field<string>("RowName")
                ?? throw new Exception("The row name cannot be null.");
            string rowStatus = row.Field<string>("RowStatus")
                ?? throw new Exception("The row status cannot be null.");
            bool isActif = row.Field<bool>("IsActive");
            int sectionId = row.Field<int>("SectionId");

            // Get the enum values
            RowStatus status = (RowStatus)Enum.Parse(typeof(RowStatus), rowStatus);

            // Get the Section with the given id
            Section section = await new SectionFactory().GetByIdAsync(sectionId);
            if (section is null)
            {
                throw new Exception("The section with the given id does not exist.");
            }

            // Create the SectionRow object
            SectionRow sectionRow = new SectionRow(id, isActif, rowName, section, status);

            return sectionRow;
        }

        #endregion


        #region Factory methods

        /// <summary>
        /// Gets all the SectionRow objects for a given section id.
        /// </summary>
        /// <param name="sectionId">The id of the section.</param>
        /// <returns>A list of SectionRow objects.</returns>
        public async Task<List<SectionRow>> GetAllBySectionIdAsync(int sectionId)
        {
            try
            {
                // Get the section in the database
                Section section = await new SectionFactory().GetByIdAsync(sectionId);
                if (section is null)
                {
                    throw new Exception("The section with the given id does not exist.");
                }

                // Get all the SectionRow objects for the given section
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync(
                        this.ConnectionString,
                        "SELECT * FROM SectionRow WHERE SectionId = @sectionId",
                        new MySqlParameter("@sectionId", sectionId))
                )
                {
                    // Create a list of SectionRow objects
                    List<SectionRow> sectionRows = new List<SectionRow>();
                    foreach (DataRow row in result.Rows)
                    {
                        sectionRows.Add(await CreateFromRow(row));
                    }

                    return sectionRows;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the section rows.", ex);
            }
        }

        /// <summary>
        /// Gets the SectionRow with the given id.
        /// </summary>
        /// <param name="id">The id of the SectionRow.</param>
        /// <returns>The SectionRow object.</returns>
        /// <exception cref="Exception">If the SectionRow with the given id does not exist.</exception>
        /// <exception cref="Exception">If an error occurred while getting the SectionRow.</exception>
        public async Task<SectionRow> GetByIdAsync(int id)
        {
            try
            {
                // Get the SectionRow with the given id
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync(
                        this.ConnectionString,
                        "SELECT * FROM SectionRow WHERE Id = @id",
                        new MySqlParameter("@id", id))
                )
                {
                    if (result.Rows.Count == 0)
                    {
                        throw new Exception("The section row with the given id does not exist.");
                    }

                    return await CreateFromRow(result.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the section row.", ex);
            }
        }

        #endregion
    }
}
