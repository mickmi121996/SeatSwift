using CommunityToolkit.Mvvm.ComponentModel;
using SeatSwiftDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuichetAutonome.ViewModels.UserControls
{
    public partial class VMOrderTemplate : ObservableObject
    {
        #region Properties

        /// <summary>
        /// The order
        /// </summary>
        [ObservableProperty]
        private Order _order;

        /// <summary>
        /// The show of the order
        /// </summary>
        [ObservableProperty]
        private Show _show;

        /// <summary>
        /// The total price of the order
        /// </summary>
        [ObservableProperty]
        private decimal _totalPrice;

        /// <summary>
        /// The TPS amount
        /// </summary>
        [ObservableProperty]
        private decimal _tps;

        /// <summary>
        /// The TVQ amount
        /// </summary>
        [ObservableProperty]
        private decimal _tvq;

        /// <summary>
        /// The total amount of the transaction after taxes
        /// </summary>
        [ObservableProperty]
        private decimal _totalAmountAfterTaxe;

        /// <summary>
        /// Number of tickets
        /// </summary>
        [ObservableProperty]
        private int _numberOfTickets;

        #endregion


        #region Constructor

        /// <summary>
        /// The constructor of the VMOrderTemplate
        /// </summary>
        public VMOrderTemplate(Order order)
        {
            // Initialize the view model
            _order = order;
            _show = new Show();
            _totalPrice = 0;
            _tps = 0;
            _tvq = 0;
            _totalAmountAfterTaxe = 0;
            _numberOfTickets = 0;
        }

        #endregion


        #region Command



        #endregion


        #region Methods



        #endregion
    }
}
