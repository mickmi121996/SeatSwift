using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatSwiftDLL
{
    /// <summary>
    /// The section row class
    /// </summary>
    /// <remarks>
    /// This class is used to store the section row data
    /// </remarks>
    public class SectionRow : ICloneable
    {
        # region Properties

        /// <summary>
        /// The Id of the section row in the database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// If the section row is active in the database
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The row name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The section of the row
        /// </summary>
        public Section Section { get; set; }

        /// <summary>
        /// The row status
        /// </summary>
        public RowStatus RowStatus { get; set; }

        # endregion


        # region Constructor

        /// <summary>
        /// The default constructor
        /// </summary>
        public SectionRow()
        {
            Id = default;
            IsActive = true;
            Name = string.Empty;
            Section = new Section();
            RowStatus = default;
        }

        /// <summary>
        /// The constructor with parameters
        /// </summary>
        /// <param name="id">The Id of the section row in the database</param>
        /// <param name="isActive">If the section row is active in the database</param>
        /// <param name="name">The row name</param>
        /// <param name="section">The section of the row</param>
        /// <param name="rowStatus">The row status</param>
        public SectionRow(int id, bool isActive, string name, Section section, RowStatus rowStatus)
        {
            this.Id = id;
            this.IsActive = isActive;
            this.Name = name;
            this.Section = section;
            this.RowStatus = rowStatus;
        }

        # endregion


        # region Interface methods

        public object Clone()
        {
            throw new NotImplementedException();
        }

        # endregion
    }
}
