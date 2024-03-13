using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatSwiftDLL
{
    /// <summary>
    /// The order class
    /// </summary>
    /// <remarks>
    /// This class is used to store the order data
    /// </remarks>
    public class Order : ICloneable
    {
        # region Properties

        /// <summary>
        /// The Id of the order in the database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// If the order is active in the database
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The number of the order
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// The date of the order
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// The total price of the order
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// The TPS amount
        /// </summary>
        public decimal TPS { get; set; }

        /// <summary>
        /// The TVQ amount
        /// </summary>
        public decimal TVQ { get; set; }

        /// <summary>
        /// The total amount of the transaction after taxes
        /// </summary>
        public decimal TotalAmountAfterTaxe { get; set; }

        /// <summary>
        /// The client of the order
        /// </summary>
        public Client? Client { get; set; }
        

        # endregion


        #region Constructor

        /// <summary>
        /// The default constructor of the order
        /// </summary>
        public Order()
        {
            Id = default;
            IsActive = true;
            OrderNumber = string.Empty;
            OrderDate = DateTime.Now;
            TotalPrice = default;
            TPS = default;
            TVQ = default;
            TotalAmountAfterTaxe = default;
            Client = null;
        }

        /// <summary>
        /// The constructor of the order
        /// </summary>
        /// <param name="id">The Id of the order in the database</param>
        /// <param name="isActive">If the order is active in the database</param>
        /// <param name="orderNumber">The number of the order</param>
        /// <param name="orderDate">The date of the order</param>
        /// <param name="totalPrice">The total price of the order</param>
        /// <param name="client">The client of the order</param>
        public Order(int id, bool isActive, string orderNumber, DateTime orderDate, decimal totalPrice, Client client)
        {
            if(client == null) { throw new ArgumentNullException(nameof(client)); }

            this.Id = id;
            this.IsActive = isActive;
            this.OrderNumber = orderNumber;
            this.OrderDate = orderDate;
            this.TotalPrice = totalPrice;
            TPS = TpsCalculation(TotalPrice);
            TVQ = TvqCalculation(TotalPrice);
            TotalAmountAfterTaxe = TotalAmountAfterTaxeCalculation(TotalPrice, TPS, TVQ);
            this.Client = client;
        }

        #endregion


        # region Interface methods

        public object Clone()
        {
            throw new NotImplementedException();
        }

        # endregion


        #region Methods

        /// <summary>
        /// The tps calculation
        /// </summary>
        /// <param name="totalAmountBeforeTaxe">The total amount before taxes</param>
        /// <returns>The tps amount</returns>
        public decimal TpsCalculation(decimal totalAmountBeforeTaxe)
        {
            return totalAmountBeforeTaxe * 0.05m;
        }

        /// <summary>
        /// The tvq calculation
        /// </summary>
        /// <param name="totalAmountBeforeTaxe">The total amount before taxes</param>
        /// <returns>The tvq amount</returns>
        public decimal TvqCalculation(decimal totalAmountBeforeTaxe)
        {
            return totalAmountBeforeTaxe * 0.09975m;
        }

        /// <summary>
        /// The total amount after taxes calculation
        /// </summary>
        /// <param name="totalAmountBeforeTaxe">The total amount before taxes</param>
        /// <param name="tps">The tps amount</param>
        /// <param name="tvq">The tvq amount</param>
        /// <returns>The total amount after taxes</returns>
        public decimal TotalAmountAfterTaxeCalculation(decimal totalAmountBeforeTaxe, decimal tps, decimal tvq)
        {
            return totalAmountBeforeTaxe + tps + tvq;
        }

        #endregion
    }
}
