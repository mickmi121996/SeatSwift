using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatSwiftDLL
{
    /// <summary>
    /// The configuration class
    /// </summary>
    /// <remarks>
    /// This class is used to store the configuration data
    /// </remarks>
    public class Configuration : ICloneable
    {
        # region Properties

        /// <summary>
        /// The Id of the configuration in the database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// If the configuration is active in the database
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The configuration name
        /// </summary>
        public string ConfigurationName { get; set; }

        /// <summary>
        /// The auditorium of the configuration
        /// </summary>
        public Auditorium Auditorium { get; set; }

        # endregion


        #region Constructor

        /// <summary>
        /// The default constructor
        /// </summary>
        public Configuration()
        {
            Id = default;
            IsActive = true;
            ConfigurationName = string.Empty;
            Auditorium = new Auditorium();
        }

        /// <summary>
        /// The constructor with parameters 
        /// </summary>
        /// <param name="id">The Id of the configuration in the database</param>
        /// <param name="isActive">If the configuration is active in the database</param>
        /// <param name="configurationName">The configuration name</param>
        /// <param name="auditorium">The auditorium of the configuration</param>
        public Configuration(int id, bool isActive, string configurationName, Auditorium auditorium)
        {
            this.Id = id;
            this.IsActive = isActive;
            this.ConfigurationName = configurationName;
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
