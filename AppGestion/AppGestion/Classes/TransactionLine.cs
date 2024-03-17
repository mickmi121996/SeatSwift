using System;

namespace AppGestion.Classes
{
    public class TransactionLine
    {
        #region Properties

        /// <summary>
        /// The order number
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// The date of the transaction
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// The total amount of the transaction before taxes
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
        /// The total amount of the transaction after taxes
        /// </summary>
        public double TotalAmountAfterTaxe { get; set; }

        /// <summary>
        /// The client email
        /// </summary>
        public string ClientEmail { get; set; }

        /// <summary>
        /// Number of tickets sold
        /// </summary>
        public int TicketsSold { get; set; }

        #endregion


        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionLine()
        {
            OrderNumber = string.Empty;
            TransactionDate = default;
            TotalAmountBeforeTaxe = default;
            TPS = default;
            TVQ = default;
            TotalAmountAfterTaxe = default;
            ClientEmail = string.Empty;
            TicketsSold = default;
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="orderNumber">The order number</param>
        /// <param name="transactionDate">The date of the transaction</param>
        /// <param name="totalAmountBeforeTaxe">The total amount of the transaction before taxes</param>
        /// <param name="clientEmail">The client email</param>
        /// <param name="ticketsSold">The number of tickets sold</param>
        public TransactionLine(
            string orderNumber,
            DateTime transactionDate,
            double totalAmountBeforeTaxe,
            string clientEmail,
            int ticketsSold
        )
        {
            OrderNumber = orderNumber;
            TransactionDate = transactionDate;
            TotalAmountBeforeTaxe = totalAmountBeforeTaxe;
            TPS = TpsCalculation(totalAmountBeforeTaxe);
            TVQ = TvqCalculation(totalAmountBeforeTaxe);
            TotalAmountAfterTaxe = TotalAmountAfterTaxeCalculation(totalAmountBeforeTaxe, TPS, TVQ);
            ClientEmail = clientEmail;
            TicketsSold = ticketsSold;
        }

        #endregion


        #region Methods

        /// <summary>
        /// The tps calculation
        /// </summary>
        /// <param name="totalAmountBeforeTaxe">The total amount before taxes</param>
        /// <returns>The tps amount</returns>
        public double TpsCalculation(double totalAmountBeforeTaxe)
        {
            return totalAmountBeforeTaxe * 0.05;
        }

        /// <summary>
        /// The tvq calculation
        /// </summary>
        /// <param name="totalAmountBeforeTaxe">The total amount before taxes</param>
        /// <returns>The tvq amount</returns>
        public double TvqCalculation(double totalAmountBeforeTaxe)
        {
            return totalAmountBeforeTaxe * 0.09975;
        }

        /// <summary>
        /// The total amount after taxes calculation
        /// </summary>
        /// <param name="totalAmountBeforeTaxe">The total amount before taxes</param>
        /// <param name="tps">The tps amount</param>
        /// <param name="tvq">The tvq amount</param>
        /// <returns>The total amount after taxes</returns>
        public double TotalAmountAfterTaxeCalculation(
            double totalAmountBeforeTaxe,
            double tps,
            double tvq
        )
        {
            return totalAmountBeforeTaxe + tps + tvq;
        }

        #endregion
    }
}
