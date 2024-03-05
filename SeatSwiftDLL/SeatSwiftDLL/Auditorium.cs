﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        /// <summary>
        /// The constructor with parameters
        /// </summary>
        /// <param name="id">The Id of the auditorium in the database</param>
        /// <param name="isActive">If the auditorium is active in the database</param>
        /// <param name="name">The auditorium name</param>
        public Auditorium(int id, bool isActive, string name)
        {
            this.Id = id;
            this.IsActive = isActive;
            this.Name = name;
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
