using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppGestion.DataAccessLayer.Factories;

namespace AppGestion.DataAccessLayer
{
    public class DAL
    {
        #region Properties

        public static string? ConnectionString { get; set; }

        #endregion


        #region Backing Fields

        /// <summary>
        /// Backing field for the Auditorium factory
        /// </summary>
        protected AuditoriumFactory? _auditoriumFactory;

        /// <summary>
        /// Backing field for the Client factory
        /// </summary>
        protected ClientFactory? _clientFactory;

        /// <summary>
        /// Backing field for the Order factory
        /// </summary>
        protected OrderFactory? _orderFactory;

        /// <summary>
        /// Backing field for the representation factory
        /// </summary>
        protected RepresentationFactory? _representationFactory;

        /// <summary>
        /// Backing field for the Seat factory
        /// </summary>
        protected SeatFactory? _seatFactory;

        /// <summary>
        /// Backing field for the Show factory
        /// </summary>
        protected ShowFactory? _showFactory;

        /// <summary>
        /// Backing field for the User factory
        /// </summary>
        protected UserFactory? _userFactory;

        /// <summary>
        /// Backing field for the ticket factory
        /// </summary>
        protected TicketFactory? _ticketFactory;

        #endregion


        #region Singleton

        protected static DAL? _instance;

        /// <summary>
        /// Singleton instance of the DAL
        /// </summary>
        protected static DAL Instance
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

        /// <summary>
        /// Singleton for the Auditorium factory
        /// </summary>
        public static AuditoriumFactory AuditoriumFactory
        {
            get
            {
                if (Instance._auditoriumFactory == null)
                {
                    Instance._auditoriumFactory = new AuditoriumFactory();
                }

                return Instance._auditoriumFactory;
            }
        }

        /// <summary>
        /// Singleton for the Client factory
        /// </summary>
        public static ClientFactory ClientFactory
        {
            get
            {
                if (Instance._clientFactory == null)
                {
                    Instance._clientFactory = new ClientFactory();
                }

                return Instance._clientFactory;
            }
        }

        /// <summary>
        /// Singleton for the Order factory
        /// </summary>
        public static OrderFactory OrderFactory
        {
            get
            {
                if (Instance._orderFactory == null)
                {
                    Instance._orderFactory = new OrderFactory();
                }

                return Instance._orderFactory;
            }
        }

        /// <summary>
        /// Singleton for the Representation factory
        /// </summary>
        public static RepresentationFactory RepresentationFactory
        {
            get
            {
                if (Instance._representationFactory == null)
                {
                    Instance._representationFactory = new RepresentationFactory();
                }

                return Instance._representationFactory;
            }
        }

        /// <summary>
        /// Singleton for the Seat factory
        /// </summary>
        public static SeatFactory SeatFactory
        {
            get
            {
                if (Instance._seatFactory == null)
                {
                    Instance._seatFactory = new SeatFactory();
                }

                return Instance._seatFactory;
            }
        }

        /// <summary>
        /// Singleton for the Show factory
        /// </summary>
        public static ShowFactory ShowFactory
        {
            get
            {
                if (Instance._showFactory == null)
                {
                    Instance._showFactory = new ShowFactory();
                }

                return Instance._showFactory;
            }
        }

        /// <summary>
        /// Singleton for the User factory
        /// </summary>
        public static UserFactory UserFactory
        {
            get
            {
                if (Instance._userFactory == null)
                {
                    Instance._userFactory = new UserFactory();
                }

                return Instance._userFactory;
            }
        }

        /// <summary>
        /// Singleton for the Ticket factory
        /// </summary>
        public static TicketFactory TicketFactory
        {
            get
            {
                if (Instance._ticketFactory == null)
                {
                    Instance._ticketFactory = new TicketFactory();
                }

                return Instance._ticketFactory;
            }
        }
        
        #endregion

        #region Constructor

        protected DAL() { }

        #endregion
    }
}
