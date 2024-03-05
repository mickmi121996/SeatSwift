using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatSwiftDLL
{
    /// <summary>
    /// The section class
    /// </summary>
    /// <remarks>
    /// This class is used to store the section data
    /// </remarks>
    public class Section : ICloneable
    {
        # region Properties

        /// <summary>
        /// The Id of the section in the database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// If the section is active in the database
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The section name
        /// </summary>
        public SectionName Name { get; set; }

        /// <summary>
        /// The section status
        /// </summary>
        public SectionStatus SectionStatus { get; set; }

        /// <summary>
        /// The auditorium of the section
        /// </summary>
        public Auditorium Auditorium { get; set; }

        /// <summary>
        /// The section multiplier
        /// </summary>
        public double Multiplier { get; set; }

        # endregion


        #region Constructor

        /// <summary>
        /// The default constructor
        /// </summary>
        public Section()
        {
            Id = default;
            IsActive = true;
            Name = string.empty;
            SectionStatus = default;
            Multiplier = default;
            Auditorium = new Auditorium();
        }

        /// <summary>
        /// The constructor with parameters
        /// </summary>
        /// <param name="id">The Id of the section in the database</param>
        /// <param name="isActive">If the section is active in the database</param>
        /// <param name="name">The section name</param>
        /// <param name="sectionStatus">The section status</param>
        /// <param name="multiplier">The section multiplier</param>
        public Section(int id, bool isActive, SectionName name, SectionStatus sectionStatus, double multiplier, Auditorium auditorium)
        {
            this.Id = id;
            this.IsActive = isActive;
            this.Name = name;
            this.SectionStatus = sectionStatus;
            this.Multiplier = multiplier;
            this.Auditorium = auditorium;
        }

        #endregion


        # region Interface methods

        public object Clone()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
