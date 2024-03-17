namespace SeatSwiftDLL
{
    /// <summary>
    /// The auditorium class
    /// </summary>
    /// <remarks>
    /// This class is used to store the auditorium data
    /// </remarks>
    public class Auditorium : ICloneable
    {
        # region Properties

        /// <summary>
        /// The Id of the auditorium in the database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// If the auditorium is active in the database
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The auditorium name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The number of row
        /// </summary>
        public int NumberOfRows { get; set; }

        /// <summary>
        /// The number of columns
        /// </summary>
        public int NumberOfColumns { get; set; }

        # endregion


        #region Constructor

        /// <summary>
        /// The default constructor
        /// </summary>
        public Auditorium()
        {
            Id = default;
            IsActive = true;
            Name = string.Empty;
            NumberOfColumns = default;
            NumberOfRows = default;
        }

        /// <summary>
        /// The constructor with parameters
        /// </summary>
        /// <param name="id">The Id of the auditorium in the database</param>
        /// <param name="isActive">If the auditorium is active in the database</param>
        /// <param name="name">The auditorium name</param>
        /// <param name="rows">The number of row for the auditorium including 2 row per stairs and way</param>
        /// <param name="columns">The number of columns for the auditorium including 2 columns per stairs and way</param>
        public Auditorium(int id, bool isActive, string name, int rows, int columns)
        {
            this.Id = id;
            this.IsActive = isActive;
            this.Name = name;
            this.NumberOfRows = rows;
            this.NumberOfColumns = columns;
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
