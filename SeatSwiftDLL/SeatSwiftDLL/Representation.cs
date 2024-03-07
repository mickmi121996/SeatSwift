using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatSwiftDLL
{
    /// <summary>
    /// The representation class
    /// </summary>
    /// <remarks>
    /// This class is used to store the representation data
    /// </remarks>
    public class Representation : ICloneable
    {
        # region Properties

        /// <summary>
        /// The Id of the representation in the database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// If the representation is active in the database
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The date of the representation
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The show of the representation
        /// </summary>
        public Show Show { get; set; }

        /// <summary>
        /// The Auditorium of the representation
        /// </summary>
        public Auditorium Auditorium { get; set; }

        # endregion


        #region Constructor

        /// <summary>
        /// The default constructor
        /// </summary>
        public Representation()
        {
            Id = default;
            IsActive = true;
            Date = DateTime.Now;
            Show = new Show();
            Auditorium = new Auditorium();
        }

        /// <summary>
        /// The constructor with parameters
        /// </summary>
        /// <param name="id">The Id of the representation in the database</param>
        /// <param name="isActive">If the representation is active in the database</param>
        /// <param name="date">The date of the representation</param>
        /// <param name="show">The show of the representation</param>
        /// <param name="auditorium">The auditorium of the representation</param>
        public Representation(int id, bool isActive, DateTime date, Show show, Auditorium auditorium)
        {
            this.Id = id;
            this.IsActive = isActive;
            this.Date = date;
            this.Show = show;
            this.Auditorium = auditorium;
        }

        #endregion


        # region Interface methods

        public object Clone()
        {
            throw new NotImplementedException();
        }

        # endregion
    }
}
