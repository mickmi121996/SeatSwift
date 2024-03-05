using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGestion.DataAccessLayer
{
    public class DAL
    {
        #region Properties

        public static string? ConnectionString { get; set; }

        #endregion


        #region Backing Fields



        #endregion


        #region Singleton

        protected static DAL? _instance;

        /// <summary>
        /// Singleton instance of the DAL
        /// </summary>
        public static DAL Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DAL();
                }
                return _instance;
            }
        }

        #endregion

        #region Constructor

        protected DAL() { }

        #endregion
    }
}
