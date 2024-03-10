using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGestion.Classes
{
    public class SellLine
    {
        #region Properties

        /// <summary>
        /// The number of tickets sold
        /// </summary>
        public int TicketsSold { get; set; }

        /// <summary>
        /// The of the representation
        /// </summary>
        public DateTime RepresentationDate { get; set; }

        /// <summary>
        /// The name of the show
        /// </summary>
        public string ShowName { get; set; }

        /// <summary>
        /// The total amount of the sell before taxes
        /// </summary>
        public double TotalAmountBeforeTaxe { get; set; }

        /// <summary>
        /// The TPS amount
        /// </summary>
        public double TPS { get; set; }

        /// <summary>
        /// The TVQ amount
        /// </summary>
        public double TVQ { get; set; }

        /// <summary>
        /// The total amount of the sell after taxes
        /// </summary>
        public double TotalAmountAfterTaxe { get; set; }

        #endregion


        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public SellLine()
        {
            TicketsSold = default;
            RepresentationDate = default;
            ShowName = string.Empty;
            TotalAmountBeforeTaxe = default;
            TPS = default;
            TVQ = default;
            TotalAmountAfterTaxe = default;
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="ticketsSold">The number of tickets sold</param>
        /// <param name="representationDate">The of the representation</param>
        /// <param name="showName">The name of the show</param>
        /// <param name="totalAmountBeforeTaxe">The total amount of the sell before taxes</param>
        public SellLine(int ticketsSold, DateTime representationDate, string showName, double totalAmountBeforeTaxe)
        {
            this.TicketsSold = ticketsSold;
            this.RepresentationDate = representationDate;
            this.ShowName = showName;
            this.TotalAmountBeforeTaxe = totalAmountBeforeTaxe;
            this.TPS = GetTPS(totalAmountBeforeTaxe);
            this.TVQ = GetTVQ(totalAmountBeforeTaxe);
            this.TotalAmountAfterTaxe = GetTotalAmountAfterTaxe(totalAmountBeforeTaxe, TPS, TVQ);
        }

        #endregion


        #region Methods

        /// <summary>
        /// Get the TPS amount using the total amount before taxes
        /// </summary>
        /// <returns>The TPS amount</returns>
        /// <param name="totalAmountBeforeTaxe">The total amount before taxes</param>
        public double GetTPS(double totalAmountBeforeTaxe)
        {
            return totalAmountBeforeTaxe * 0.05;
        }

        /// <summary>
        /// Get the TVQ amount using the total amount before taxes
        /// </summary>
        /// <returns>The TVQ amount</returns>
        /// <param name="totalAmountBeforeTaxe">The total amount before taxes</param>
        public double GetTVQ(double totalAmountBeforeTaxe)
        {
            return totalAmountBeforeTaxe * 0.09975;
        }

        /// <summary>
        /// Get the total amount after taxes using the total amount before taxes
        /// </summary>
        /// <returns>The total amount after taxes</returns>
        /// <param name="totalAmountBeforeTaxe">The total amount before taxes</param>
        /// <param name="tps">The TPS amount</param>
        /// <param name="tvq">The TVQ amount</param>
        public double GetTotalAmountAfterTaxe(double totalAmountBeforeTaxe, double tps, double tvq)
        {
            return totalAmountBeforeTaxe + tps + tvq;
        }

        #endregion

    }
}
